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
		public rcl_subscription_options_t SubscriptionOptions{ get; private set; }
		public event EventHandler<MessageRecievedEventArgs<T>> MessageRecieved;
		public Subscription (Node _node, string _topicName)
		{
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
			foreach (var item in messageType.GetMethods()) {

				if (item.IsStatic ) {
					if (item.Name.Contains ("rosidl_typesupport_introspection_c_get_message")) {
						TypeSupport = (rosidl_message_type_support_t)Marshal.PtrToStructure((IntPtr)item.Invoke (null, null),typeof(rosidl_message_type_support_t));
					}
				}
			}
			if (TypeSupport.data == IntPtr.Zero)
				throw new Exception ("Couldn't get typesupport");
			SubscriptionOptions = rcl_subscription.get_default_options ();
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
	internal class rcl_subscription:IDisposable
	{
		private bool disposed = false;
		private rcl_subscription_t subscription;
		private rcl_node_t native_node;
		public rcl_subscription(Node _node, rosidl_message_type_support_t _type_support, string _topic_name, rcl_subscription_options_t _options)
		{
			subscription = rcl_get_zero_initialized_subscription ();
			native_node = _node.NativeNode;
			rcl_subscription_init (ref subscription,ref native_node,ref _type_support, _topic_name,ref _options);
		}
		~rcl_subscription()
		{
			Dispose (false);
		}
		public rcl_subscription_t NativeSubscription
		{
			get{ return subscription;}
		}

		public static rcl_subscription_options_t get_default_options()
		{
			return rcl_subscription_get_default_options ();
		}
		public T TakeMessage<T>(ref bool success)
			where T: MessageWrapper, new()
		{
			rmw_message_info_t message_info = new rmw_message_info_t ();
			return TakeMessage<T> (ref success, ref message_info);
		}
		public T TakeMessage<T>(ref bool success, ref rmw_message_info_t _message_info )
			where T: MessageWrapper, new()
		{
			MessageWrapper ret_msg = new T();
			ValueType msg;
			ret_msg.GetData (out msg);
			rmw_message_info_t message_info = _message_info;

			int ret = rcl_take (ref subscription,  msg, message_info);

			RCLReturnValues ret_val = (RCLReturnValues)ret;
			//Console.WriteLine (ret_val);
			/*return RCL_RET_OK if the message was published, or
			*         RCL_RET_INVALID_ARGUMENT if any arguments are invalid, or
			*         RCL_RET_SUBSCRIPTION_INVALID if the subscription is invalid, or
			*         RCL_RET_BAD_ALLOC if allocating memory failed, or
			*         RCL_RET_SUBSCRIPTION_TAKE_FAILED if take failed but no error
				*         occurred in the middleware, or
			*         RCL_RET_ERROR if an unspecified error occurs.*/
			bool take_message_success = false;
			switch (ret_val) {
			case RCLReturnValues.RCL_RET_INVALID_ARGUMENT:
				throw new RCLInvalidArgumentException ();
				break;
			case RCLReturnValues.RCL_RET_SUBSCRIPTION_INVALID:
				throw new RCLSubscriptionInvalidException ();
				break;
			case RCLReturnValues.RCL_RET_BAD_ALLOC:
				throw new RCLBadAllocException ();
				break;
			case RCLReturnValues.RCL_RET_SUBSCRIPTION_TAKE_FAILED:
				//throw new RCLSubscriptonTakeFailedException ();
				take_message_success = false;
				//Marshal.FreeHGlobal (msg_ptr);
				break;
			case RCLReturnValues.RCL_RET_ERROR:
				throw new RCLErrorException ();
				break;
			case RCLReturnValues.RCL_RET_OK:
				{
					take_message_success = true;
					//Bring the data back into the message wrapper
					ret_msg.SetData (ref msg);
					//And do a sync for nested types. This is in my opinion a hack because I can't store references on value types in C#
					ret_msg.SyncDataIn ();

				}
				break;
			default:
				break;
			}
			success = take_message_success;
			//Marshal.FreeHGlobal (message_info_ptr);
		
			return (T)ret_msg;
		}
		// Public implementation of Dispose pattern callable by consumers.
		public void Dispose()
		{ 
			Dispose(true);
			GC.SuppressFinalize(this);           
		}

		// Protected implementation of Dispose pattern.
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return; 

			if (disposing) {
				// Free any other managed objects here.
				//
			}
			rcl_subscription_fini (ref subscription, ref native_node);
			// Free any unmanaged objects here.
			//
			disposed = true;
		}

		[DllImport("librcl.so")]
		extern static rcl_subscription_t rcl_get_zero_initialized_subscription();

		[DllImport("librcl.so")]
		extern static int rcl_subscription_init(ref rcl_subscription_t subscription, ref rcl_node_t node, ref rosidl_message_type_support_t typesupport, string topic_name, ref rcl_subscription_options_t options);

		[DllImport("librcl.so")]
		extern static int rcl_subscription_fini (ref rcl_subscription_t subscription, ref rcl_node_t node);

		[DllImport("librcl.so")]
	    extern static rcl_subscription_options_t rcl_subscription_get_default_options ();

		[DllImport("librcl.so")]
		extern static int rcl_take(ref rcl_subscription_t subscription,[Out] ValueType ros_message,[In,Out] rmw_message_info_t message_info);

		[DllImport("librcl.so")]
		extern static string rcl_subscription_get_topic_name(ref rcl_subscription_t subscription);

		[DllImport("librcl.so")]
		extern static IntPtr rcl_subscription_get_options(ref rcl_subscription_t subscription);
			
	}
	public struct rcl_subscription_t
	{
		IntPtr impl;
	}
	public struct rcl_subscription_options_t
	{
		rmw_qos_profile_t qos;
		bool ignore_local_publications;
		rcl_allocator_t allocator;
	}
}

