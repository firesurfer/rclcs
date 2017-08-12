using System;
using System.Runtime.InteropServices;
namespace rclcs
{
	internal abstract class rcl_node_base:IDisposable
	{
        protected bool disposed = false;
		protected rcl_node_t native_handle;
		protected string name_space;
		/// <summary>
		/// Gets the native handle (Which is wrapped in a rcl_node_t.
		/// </summary>
		/// <value>The native node.</value>
		public  rcl_node_t NativeNode{ 
			get{ return native_handle;} 
		}
		public string NameSpace{ 
			get { return name_space; } 
		}
		public rcl_node_base (string name, string namespace_ = "")
		{
			name_space = namespace_;
		}
		public abstract string get_node_name ();
		public abstract bool node_is_valid ();
		public void Dispose (){
			Dispose (true);
			GC.SuppressFinalize (this);       
		}
		protected virtual void Dispose (bool disposing)
		{

            if (disposed)
                return;
            if (disposing) {

                // Free any other managed objects here.
              
            }


            disposed = true;
		}
	}
	internal class rcl_node:IDisposable
	{
        private bool disposed = false;
		private rcl_node_base Impl;
		public rcl_node(string name, string namespace_ = "")
		{
			if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                Impl = new rcl_node_windows(name, namespace_);
			} else if (Environment.OSVersion.Platform == PlatformID.Unix) {
				Impl = new rcl_node_linux (name,namespace_);
			} else {
				throw new Exception("Operating system: " +Environment.OSVersion.Platform.ToString() + " not supported");
			}
		}
		public  string get_node_name ()
		{
			return Impl.get_node_name ();
		}
		public  bool node_is_valid ()
		{
			return Impl.node_is_valid ();
		}
		public string NameSpace {
			get{ return Impl.NameSpace; }
		}
		public  rcl_node_t NativeNode{ 
			get{ return Impl.NativeNode;} 
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return;
			if (disposing)
			{

				// Free any other managed objects here.
				Impl.Dispose();
			}


			disposed = true;
		}
        ~rcl_node()
        {
            Dispose(false);
        }

	}
}
