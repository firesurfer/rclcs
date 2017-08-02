using System;
using System.Runtime.InteropServices;
using System.Reflection;
namespace rclcs
{
	public class Subscription<T>:Executable
		where T : MessageWrapper, new()
	{
		private  bool disposed = false;
		private rosidl_message_type_support_t TypeSupport;
		private rcl_subscription InternalSubscription;
		public Node RosNode{ get; private set;}
		public string TopicName { get; private set; }
		private rcl_subscription_options_t SubscriptionOptions;
		public rmw_qos_profile_t QOSProfile { get; private set; }
		public event EventHandler<MessageRecievedEventArgs<T>> MessageRecieved;

		/// <summary>
		/// Initializes a new instance of the <see cref="rclcs.Subscription`1"/> class. With default qos profile.
		/// </summary>
		/// <param name="_node">Node.</param>
		/// <param name="_topicName">Topic name.</param>
		public Subscription(Node _node, string _topicName): this(_node, _topicName, rmw_qos_profile_t.rmw_qos_profile_default)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="rclcs.Subscription`1"/> class. With a custom qos profile 
		/// </summary>
		/// <param name="_node">Node.</param>
		/// <param name="_topicName">Topic name.</param>
		/// <param name="_QOS">QO.</param>
		public Subscription (Node _node, string _topicName, rmw_qos_profile_t _QOS)
		{
			QOSProfile = _QOS;
			RosNode = _node;
			TopicName = _topicName;

			Type wrapperType = typeof(T);
			Type messageType = typeof(T);
			foreach (var item in wrapperType.GetMethods()) {
				if (item.IsStatic) {
					if (item.Name.Contains ("GetMessageType")) {
						messageType = (Type)item.Invoke (null, null);
					}
				}
			}
			bool foundMethod = false;
			foreach (var item in messageType.GetMethods()) {

				if (item.IsStatic ) {
					
					if (item.Name.Contains ("rosidl_typesupport_introspection_c__get_message_type_support_handle")) {
						foundMethod = true;
						TypeSupport = (rosidl_message_type_support_t)Marshal.PtrToStructure((IntPtr)item.Invoke (null, null),typeof(rosidl_message_type_support_t));
					}
				}
			}
			if (!foundMethod)
				throw new MissingMethodException ("Could not find typesupport method");
			if (TypeSupport.data == IntPtr.Zero)
				throw new Exception ("Couldn't get typesupport");
			SubscriptionOptions = rcl_subscription.get_default_options ();
			SubscriptionOptions.qos = QOSProfile;
			InternalSubscription = new rcl_subscription (RosNode, TypeSupport, TopicName,SubscriptionOptions);
		}
		public rcl_subscription_t NativeSubscription
		{
			get{return InternalSubscription.NativeSubscription;}
		}
		internal rcl_subscription NativeWrapper
		{
			get{ return InternalSubscription;}
		}

		public override void Execute()
		{
			bool success = false;
			T message = InternalSubscription.TakeMessage<T> (ref success);
			if (success) {
				if (MessageRecieved != null)
					MessageRecieved (this,new MessageRecievedEventArgs<T> (message));
			}
		}

		~Subscription()
		{
			Dispose (false);
		}
		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return; 

			if (disposing) {
				
				// Free any other managed objects here.
				//
				InternalSubscription.Dispose();
			}

			// Free any unmanaged objects here.
			//

			disposed = true;
			// Call base class implementation.
			base.Dispose(disposing);
		}

	}

}

