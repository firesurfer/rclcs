using System;
using System.Runtime.InteropServices;


namespace rclcs
{

	internal abstract class rcl_client_base:IDisposable
	{
		protected  bool disposed = false;
		protected rcl_client_t native_handle;
		protected rcl_node_t native_node;
		protected string service_name;
		protected rcl_client_options_t options;
		protected rosidl_service_type_support_t typesupport;
		protected Int64 last_sequence_number = 0;

		public rcl_client_base(rcl_node_t _node, rosidl_service_type_support_t _typesupport,  string _service_name, rcl_client_options_t _options)
		{
			this.native_node = _node;
			this.service_name = _service_name;
			this.options = _options;
			this.typesupport = _typesupport;
		}
        ~rcl_client_base()
        {
            Dispose(false);
        }
		public rcl_client_t NativeClient
		{
			get{return native_handle;}
		}

		public abstract T TakeResponse<T> (ref bool success)
			where T: MessageWrapper, new();

		public abstract void SendRequest<T>(T request)
			where T :MessageWrapper,new();

		public void Dispose()
		{ 
			Dispose(true);
			GC.SuppressFinalize(this);           
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
	}


	public class rcl_client:IDisposable
	{
		rcl_client_base Impl;
		protected  bool disposed = false;
		public rcl_client(rcl_node_t _node, rosidl_service_type_support_t _typesupport,  string _service_name, rcl_client_options_t _options)
		{
			if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
				//TODO codepath for windows
			} else if (Environment.OSVersion.Platform == PlatformID.Unix) {
				Impl = new rcl_client_linux (_node,_typesupport,_service_name,_options);
			} else {
				throw new Exception("Operating system: " +Environment.OSVersion.Platform.ToString() + " not supported");
			}
		}

		public void Dispose()
		{ 
			Dispose(true);
			GC.SuppressFinalize(this);           
		}
        ~rcl_client()
        {
            Dispose(false);
        }

		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return; 

			if (disposing) {
				// Free any other managed objects here.
				Impl.Dispose();
			}

			// Free any unmanaged objects here.
			//
			disposed = true;
		}
		public rcl_client_t NativeClient
		{
			get{return Impl.NativeClient;}
		}

		public T TakeResponse<T> (ref bool success)
			where T: MessageWrapper, new()
		{
			return Impl.TakeResponse<T> (ref success);
		}

		public void SendRequest<T>(T request)
			where T :MessageWrapper,new()
		{
			Impl.SendRequest<T> (request);
		}
		public static rcl_client_options_t get_default_options()
		{
			if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
				//TODO codepath for windows
				return rcl_client_linux.get_default_options ();
			} else if (Environment.OSVersion.Platform == PlatformID.Unix) {
				return rcl_client_linux.get_default_options ();
			} else {
				throw new Exception("Operating system: " +Environment.OSVersion.Platform.ToString() + " not supported");
			}
		}
	}
}