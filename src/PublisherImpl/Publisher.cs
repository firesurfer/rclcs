using System;
using System.Runtime.InteropServices;
using System.Reflection;

namespace rclcs
{

	/// <summary>
	/// Publisher.
	/// This class represents a ROS publisher which is used to publish messages of the type T.
	/// <typeparam name="T">Message type</typeparam>
	/// </summary>
	public class Publisher<T>:Executable
		where T: MessageWrapper, new()
	{
		//The c-message-type-support that is obtained for the given message type
		private rosidl_message_type_support_t TypeSupport;
		//The instance of the wrapper object around the native methods
		private rcl_publisher InternalPublisher;
		//For the IDisposable pattern
		private bool disposed = false;
		//The options the publisher was created with
		private rcl_publisher_options_t PublisherOptions;

		/// <summary>
		/// Gets the QOS profile.
		/// </summary>
		/// <value>The QOS profile.</value>
		public rmw_qos_profile_t QOSProfile{ get; private set; }

		/// <summary>
		/// Gets the ros node.
		/// </summary>
		/// <value>The ros node.</value>
		public Node RosNode{ get; private set; }

		/// <summary>
		/// Gets the name of the topic.
		/// </summary>
		/// <value>The name of the topic.</value>
		public string TopicName { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="rclcs.Publisher`1"/> class.
		/// With the default qos profile
		/// </summary>
		/// <param name="_Node">Node.</param>
		/// <param name="_TopicName">Topic name.</param>
		public Publisher (Node _Node, string _TopicName) : this (_Node, _TopicName, rmw_qos_profile_t.rmw_qos_profile_default)
		{
			

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="rclcs.Publisher`1"/> class.
		/// with a custom qos profile. <see cref="rclcs.rmw_qos_profile_t"/> for possible preconfigured profiles. 
		/// </summary>
		/// <param name="_Node">Node.</param>
		/// <param name="_TopicName">Topic name.</param>
		/// <param name="_QOS">Custom qos profile.</param>
		/// 
		public Publisher (Node _Node, string _TopicName, rmw_qos_profile_t _QOS)
		{
			QOSProfile = _QOS;
			RosNode = _Node;
			TopicName = _TopicName;

			//This is some reflection magic that is used in order to obtain the correct C typesupport.
			//Every generated message contains a method with a name similiar to rosidl_typesupport_introspection_c_get_message. 
			//This is a interop method declaration that does a call in the corresponding C library and that returns a pointer to a rosidl_message_type_support_t struct.
			//This pointer is marshalled to a managed struct of the type rosidl_message_type_support_t.

			//This is the type of the wrapper class around the message struct
			Type wrapperType = typeof(T);
			//This variable will store the type of the message struct 
			Type messageType = typeof(T);
			//Go through all methods of the wrapper class
			foreach (var item in wrapperType.GetMethods()) {
				if (item.IsStatic) {
					//If its a method called GetMessageType
					if (item.Name.Contains ("GetMessageType")) {
						//We call it and cast the returned object to a Type.
						messageType = (Type)item.Invoke (null, null);
					}
				}
			}
            
			//Now we do the same for the message struct
			bool foundMethod = false;
			foreach (var item in messageType.GetMethods()) {
				if (item.IsStatic) {
					//We search for the method that does the native call
					if (item.Name.Contains ("rosidl_typesupport_introspection_c__get_message_type_support_handle")) {
						foundMethod = true;
						//We call it and marshal the returned IntPtr (a managed wrapper around a pointer) to the managed typesupport struct
						TypeSupport = (rosidl_message_type_support_t)Marshal.PtrToStructure ((IntPtr)item.Invoke (null, null), typeof(rosidl_message_type_support_t));
					}

				}
			}
			if (!foundMethod)
				throw new MissingMethodException ("Could not find typesupport method");
			//The case that the data pointer inside the type support struct is 0 is a strong indicator that the call failed somewhere 
			//Or that we got a wrong typesupport object at least
			if (TypeSupport.data == IntPtr.Zero)
				throw new Exception ("Couldn't get typesupport");
			//Get the default options for the rcl publisher
			PublisherOptions = rcl_publisher.get_default_options ();
			//Set the custom qos profile
			PublisherOptions.qos = QOSProfile;
			//And create an new publisher 
			InternalPublisher = new rcl_publisher (RosNode, TypeSupport, TopicName, PublisherOptions);

		}

		/// <summary>
		/// Gets the native publisher which is simply a struct that contains a pointer to the publisher inside the unmanaged ros code.
		/// Don't mess around with it!
		/// </summary>
		/// <value>The native publisher.</value>
		public rcl_publisher_t NativePublisher {
			get{ return InternalPublisher.NativePublisher; }
		}

		/// <summary>
		/// Gets the native wrapper that is used in this managed publisher instance
		/// </summary>
		/// <value>The native wrapper.</value>
		public rcl_publisher NativeWrapper {
			get{ return InternalPublisher; }
		}

		/// <summary>
		/// Publish the specified msg.
		/// Call this method in order to publish a new message of the type T
		/// </summary>
		/// <param name="msg">Message.</param>
		public bool Publish (T msg)
		{
			if (msg == null)
				throw new ArgumentNullException ("msg may not be null");
			//This is needed for nested types, mostly because we can't store a reference to a value type in C#
			msg.SyncDataOut ();
			ValueType temp;
			//We get the data out of the message wrapper
			msg.GetData (out temp);


			/*Console.WriteLine ("##############################################");
			Console.WriteLine ("Debug in publish method:");
			//I still don't like this solution which is needed for nested types...

			foreach (var item in temp.GetType().GetFields()) {
				if (item.GetValue(temp).ToString().Contains ("Time")) {
					Console.WriteLine ("!!!!!Found time!!!!!");
					foreach (var item2 in item.FieldType.GetFields()) {
						Console.WriteLine (item2.Name + " " + item2.GetValue (item.GetValue(temp)));
					}
				}
				Console.WriteLine (item.Name + " " + item.GetValue (temp));
			}
			Console.WriteLine ("##############################################");*/

			//And the the native wrapper to publish the message struct
			return InternalPublisher.PublishMessage (temp);
		}

		/// <summary>
		/// Implementation of the IDisposable pattern
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose (bool disposing)
		{
			if (disposed)
				return; 

			if (disposing) {
				
				// Free any other managed objects here.
				//
				InternalPublisher.Dispose ();
			}

			// Free any unmanaged objects here.
			//

			disposed = true;
			// Call base class implementation.
			base.Dispose (disposing);
		}
	}
}

