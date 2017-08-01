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
		where T: MessageWrapper,new()
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
		public Node RosNode{ get; private set;}

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
		public Publisher (Node _Node, string _TopicName, rmw_qos_profile_t _QOS )
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
				if (item.IsStatic ) {
					//We search for the method that does the native call
					if (item.Name.Contains ("rosidl_typesupport_introspection_c__get_message_type_support_handle")) {
						foundMethod = true;
						//We call it and marshal the returned IntPtr (a managed wrapper around a pointer) to the managed typesupport struct
						TypeSupport = (rosidl_message_type_support_t)Marshal.PtrToStructure((IntPtr)item.Invoke (null, null), typeof(rosidl_message_type_support_t));
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
			InternalPublisher = new rcl_publisher (RosNode, TypeSupport, TopicName,PublisherOptions);

		}
		/// <summary>
		/// Gets the native publisher which is simply a struct that contains a pointer to the publisher inside the unmanaged ros code.
		/// Don't mess around with it!
		/// </summary>
		/// <value>The native publisher.</value>
		public rcl_publisher_t NativePublisher
		{
			get{ return InternalPublisher.NativePublisher;}
		}
		/// <summary>
		/// Gets the native wrapper that is used in this managed publisher instance
		/// </summary>
		/// <value>The native wrapper.</value>
		public rcl_publisher NativeWrapper
		{
			get{ return InternalPublisher; }
		}
		/// <summary>
		/// Publish the specified msg.
		/// Call this method in order to publish a new message of the type T
		/// </summary>
		/// <param name="msg">Message.</param>
		public bool Publish(T msg)
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
			return InternalPublisher.PublishMessage ( temp);
		}
		/// <summary>
		/// Implementation of the IDisposable pattern
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return; 

			if (disposing) {
				
				// Free any other managed objects here.
				//
				InternalPublisher.Dispose();
			}

			// Free any unmanaged objects here.
			//

			disposed = true;
			// Call base class implementation.
			base.Dispose(disposing);
		}
	}
	/// <summary>
	/// This class wraps the native methods and makes sure the Publisher<T> can only make save calls.
	/// It does furthermore the correct memory handling for memory allocated in the native code, like finishing the native publisher
	/// </summary>
	public class rcl_publisher:IDisposable
	{
		private rcl_publisher_t native_handle;
		private rcl_node_t native_node;
		private string topic_name;
		private rcl_publisher_options_t options;
		private rosidl_message_type_support_t type_support;
		private bool disposed = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="rclcs.rcl_publisher"/> class.
		/// </summary>
		/// <param name="_node">Node.</param>
		/// <param name="_type_support">Type support.</param>
		/// <param name="_topic_name">Topic name.</param>
		/// <param name="_options">Options.</param>
		public rcl_publisher(Node _node, rosidl_message_type_support_t _type_support, string _topic_name, rcl_publisher_options_t _options)
		{
			this.native_node = _node.NativeNode;
			this.topic_name = _topic_name;
			this.type_support = _type_support;
			this.options = _options;

			native_handle = rcl_get_zero_initialized_publisher ();
			rcl_publisher_init (ref native_handle,ref native_node, ref type_support, topic_name, ref options);

		}
		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the <see cref="rclcs.rcl_publisher"/> is
		/// reclaimed by garbage collection.
		/// </summary>
		~rcl_publisher()
		{
			Dispose (false);	
		}
		/// <summary>
		/// Releases all resource used by the <see cref="rclcs.rcl_publisher"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="rclcs.rcl_publisher"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="rclcs.rcl_publisher"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the <see cref="rclcs.rcl_publisher"/> so the garbage
		/// collector can reclaim the memory that the <see cref="rclcs.rcl_publisher"/> was occupying.</remarks>
		public void Dispose()
		{ 
			Dispose(true);
			GC.SuppressFinalize(this);           
		}
		/// <summary>
		/// Implementation of the IDisposable pattern
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return; 

			if (disposing) {
				// Free any other managed objects here.
				//
			}
			//Clean up unmanaged resources
			rcl_publisher_fini (ref native_handle, ref native_node);
			// Free any unmanaged objects here.
			//
			disposed = true;
		}
		/// <summary>
		/// Gets the name of the topic.
		/// </summary>
		/// <value>The name of the topic.</value>
		public string TopicName
		{
			get{ return topic_name; }
		}

		/// <summary>
		/// Gets the native publisher handle
		/// </summary>
		/// <value>The native publisher.</value>
		public rcl_publisher_t NativePublisher
		{
			get{ return native_handle;}
		}
		/// <summary>
		/// Gets the default options from the rcl
		/// //TODO rename  to GetDefaultOptions
		/// </summary>
		/// <returns>The default options.</returns>
		public static rcl_publisher_options_t get_default_options()
		{
			return rcl_publisher_get_default_options ();
		}
		/// <summary>
		/// This method gets called from the Publisher<T> class in order to publish a message. Have in mind this method does not perform any type checks
		/// </summary>
		/// <returns><c>true</c>, if message was published, <c>false</c> otherwise.</returns>
		/// <param name="msg">Message.</param>
		public bool PublishMessage( ValueType msg)
		{
			int ret = rcl_publish (ref native_handle,  msg);
			//Handle the return types
			RCLReturnValues ret_val = (RCLReturnValues)ret;

			bool publish_message_success = false;
			switch (ret_val) {
			case RCLReturnValues.RCL_RET_OK:
				publish_message_success = true;
				break;
			case RCLReturnValues.RCL_RET_INVALID_ARGUMENT:
				throw new RCLInvalidArgumentException ();

			case RCLReturnValues.RCL_RET_PUBLISHER_INVALID:
				throw new RCLPublisherInvalidException ();

			case RCLReturnValues.RCL_RET_ERROR:
				throw new RCLInvalidArgumentException ();

			default:
				break;
			}
			return publish_message_success;
		}
		//Native methods. See the rcl/publisher.h file for detailed documentation.

		[DllImport(RCL.LibRCLPath)]
		extern static rcl_publisher_t rcl_get_zero_initialized_publisher();

		[DllImport(RCL.LibRCLPath)]
		extern static int rcl_publisher_init(ref rcl_publisher_t publisher,ref rcl_node_t node, ref rosidl_message_type_support_t type_support, string topic_name, ref rcl_publisher_options_t options);

		[DllImport(RCL.LibRCLPath)]
		extern static int rcl_publisher_fini (ref rcl_publisher_t publisher, ref rcl_node_t node);

		[DllImport(RCL.LibRCLPath)]
		extern static rcl_publisher_options_t rcl_publisher_get_default_options();

		[DllImport(RCL.LibRCLPath)]
		extern static int rcl_publish(ref rcl_publisher_t publisher,  [In] ValueType ros_message);

		[DllImport(RCL.LibRCLPath)]
		extern static string rcl_publisher_get_topic_name(ref rcl_publisher_t publisher);

		[DllImport(RCL.LibRCLPath)]
		extern static rcl_publisher_options_t rcl_publisher_get_options(ref rcl_publisher_t publisher);
	}
	/// <summary>
	/// This struct wraps the pointer on the "real" publisher that ros uses
	/// </summary>
	public struct rcl_publisher_t
	{
		IntPtr impl;
	}
	/// <summary>
	/// Implementation of the rcl_publisher_options_t struct with the same name and same definition in ros.
	/// </summary>
	public struct rcl_publisher_options_t
	{
		public rmw_qos_profile_t qos;
		public rcl_allocator_t allocator;
	}
}

