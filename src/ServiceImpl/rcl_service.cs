using System;
using System.Runtime.InteropServices;
namespace rclcs
{
	internal abstract class rcl_service_base:IDisposable
	{
		protected bool disposed = false;
		protected rcl_node_t native_node;
		protected rcl_service_t native_handle;
		protected rmw_request_id_t last_request_header;
		protected rcl_service_options_t service_options;

		public rcl_service_base(rcl_node_t _node, rosidl_service_type_support_t typesupport, string service_name, rcl_service_options_t options)
		{
			this.native_node = _node;
			this.service_options = options;
		}

		public void Dispose()
		{ 
			Dispose(true);
			GC.SuppressFinalize(this);           
		}
        ~rcl_service_base()
        {
            Dispose(false);
        }
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return; 

			if (disposing) {

				// Free any other managed objects here.
				//
			}

			// Free any unmanaged objects here.
			//
			disposed = true;
		}
		public rcl_service_t NativeService
		{
			get{return native_handle;}
		}

		public abstract T TakeRequest<T> (ref bool success)
			where T:MessageWrapper, new();

		public abstract void SendResponse<T> (T response)
			where T: MessageWrapper, new();
	}


	public class rcl_service:IDisposable
	{
		private bool disposed = false;
		private rcl_service_linux Impl;

		public rcl_service(rcl_node_t _node, rosidl_service_type_support_t typesupport, string service_name, rcl_service_options_t options)
		{
			if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
				//TODO codepath for windows
			} else if (Environment.OSVersion.Platform == PlatformID.Unix) {
				Impl = new rcl_service_linux (_node, typesupport, service_name, options);
			} else {
				throw new Exception("Operating system: " +Environment.OSVersion.Platform.ToString() + " not supported");
			}
		}
		public void Dispose()
		{ 
			Dispose(true);
			GC.SuppressFinalize(this);           
		}
        ~rcl_service()
        {
            Dispose(false);
        }

		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return; 

			if (disposing) {

				// Free any other managed objects here.
				//
			}
			Impl.Dispose ();
			// Free any unmanaged objects here.
			//
			disposed = true;
		}
		public rcl_service_t NativeService
		{
			get{return Impl.NativeService;}
		}

		public  T TakeRequest<T> (ref bool success)
			where T:MessageWrapper, new()
		{
			return Impl.TakeRequest<T> (ref success);
		}

		public  void SendResponse<T> (T response)
			where T: MessageWrapper, new()
		{
			Impl.SendResponse<T> (response);
		}

		public static rcl_service_options_t get_default_options()
		{
			if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
				//TODO codepath for windows
				return rcl_service_linux.get_default_options();
			} else if (Environment.OSVersion.Platform == PlatformID.Unix) {
				return rcl_service_linux.get_default_options();
			} else {
				throw new Exception("Operating system: " +Environment.OSVersion.Platform.ToString() + " not supported");
			}
		}
	}
}