
using System;
using System.Runtime.InteropServices;
namespace rclcs
{
	internal abstract class rcl_subscription_base:IDisposable
	{
		protected bool disposed = false;
		protected rcl_subscription_t subscription;
		protected rcl_node_t native_node;

		public rcl_subscription_base(Node _node, rosidl_message_type_support_t _type_support, string _topic_name, rcl_subscription_options_t _options)
		{
			native_node = _node.NativeNode;
		}
		public rcl_subscription_t NativeSubscription
		{
			get{ return subscription;}
		}
		// Public implementation of Dispose pattern callable by consumers.
		public void Dispose()
		{ 
			Dispose(true);
			GC.SuppressFinalize(this);           
		}

        ~rcl_subscription_base()
        {
            Dispose(false);
        }

		// Protected implementation of Dispose pattern.
		protected abstract void Dispose(bool disposing);

		public abstract T TakeMessage<T> (ref bool success)
			where T: MessageWrapper, new();

		public abstract T TakeMessage<T> (ref bool success, ref rmw_message_info_t _message_info)
			where T: MessageWrapper, new();
	}
	public class rcl_subscription:IDisposable
	{
		private rcl_subscription_base Impl;
		protected bool disposed = false;
		public rcl_subscription(Node _node, rosidl_message_type_support_t _type_support, string _topic_name, rcl_subscription_options_t _options)
		{
			if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                Impl = new rcl_subscription_windows(_node, _type_support, _topic_name, _options);
			} else if (Environment.OSVersion.Platform == PlatformID.Unix) {
				Impl = new rcl_subscription_linux (_node,_type_support,_topic_name,_options);
			} else {
				throw new Exception("Operating system: " +Environment.OSVersion.Platform.ToString() + " not supported");
			}
		}
		public rcl_subscription_t NativeSubscription
		{
			get{ return Impl.NativeSubscription;}
		}
		public T TakeMessage<T>(ref bool success)
			where T: MessageWrapper, new()
		{
			return Impl.TakeMessage<T> (ref success);
		}
		public T TakeMessage<T>(ref bool success, ref rmw_message_info_t _message_info )
			where T: MessageWrapper, new()
		{
			return Impl.TakeMessage<T> (ref success, ref _message_info);
		}
		// Public implementation of Dispose pattern callable by consumers.
		public void Dispose()
		{ 
			Dispose(true);
			GC.SuppressFinalize(this);           
		}
        ~rcl_subscription()
        {
            Dispose(false);
        }

		// Protected implementation of Dispose pattern.
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return; 

			if (disposing) {
				// Free any other managed objects here.
				//
				Impl.Dispose ();
			}

			// Free any unmanaged objects here.
			//
			disposed = true;
		}

		public static rcl_subscription_options_t get_default_options()
		{
			if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
				//TODO codepath for windows
				//At the moment use linux path
				return rcl_subscription_windows.get_default_options();
			} else if (Environment.OSVersion.Platform == PlatformID.Unix) {
				return rcl_subscription_linux.get_default_options();
			} else {
				throw new Exception("Operating system: " +Environment.OSVersion.Platform.ToString() + " not supported");
			}
		}
	}

}