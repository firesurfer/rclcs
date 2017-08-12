using System;
using System.Runtime.InteropServices;
namespace rclcs
{
	internal abstract class RCLBase:IDisposable
	{
		protected bool disposed = false;
		public  abstract void Init (String[] args);
		public  abstract void Init(String[] args, rcl_allocator_t custom_allocator);
		public  abstract bool IsInit{ get;}

		/// <summary>
		/// Releases all resource used by the <see cref="rclcs.RCL"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="rclcs.RCL"/>. The <see cref="Dispose"/>
		/// method leaves the <see cref="rclcs.RCL"/> in an unusable state. After calling <see cref="Dispose"/>, you must
		/// release all references to the <see cref="rclcs.RCL"/> so the garbage collector can reclaim the memory that the
		/// <see cref="rclcs.RCL"/> was occupying.</remarks>
		public void Dispose()
		{
			// Dispose of unmanaged resources.
			Dispose(true);
			// Suppress finalization.
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
		}
        ~RCLBase()
        {
            Dispose(false);
        }
	}
	public class RCL:IDisposable
	{
		bool disposed = false;
		//Check if compiled on windows or linux, unfortunatly we can't do a runtime check for the import statements 
        // !!This has changed. We just need the dynamic paths for the rcl errorhandling functions
		//TODO implement different codepaths for windows and linux. This has the advantage we can do different calls for windows and linux
        //Infrastructure for different codepaths is already implemented
		#if __MonoCS__
		#warning Compiling on linux: path is now: librcl.so
		//On linux the files start with lib and end with .so
		public const string LibRCLPath = "librcl.so";
		public const string LibRMWPath = "librmw.so";
		public const string LibRCUtilsPATH = "librcutils.so";
		#else
		#warning Compiling on windows: path is now: rcl.dll
		//On windows they end with .dll
		public const string LibRCUtilsPATH = @"rcutils.dll";
		public const string LibRCLPath = @"rcl.dll";
		public const string LibRMWPath = @"rmw.dll";
		#endif
		RCLBase Impl;
		public RCL ()
		{
			if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                Impl = new RCLWindows();
			} else if (Environment.OSVersion.Platform == PlatformID.Unix) {
				Impl = new RCLLinux ();
			} else {
				throw new Exception("Operating system: " +Environment.OSVersion.Platform.ToString() + " not supported");
			}
		}
        ~RCL()
        {
            Dispose(false);
        }

		public void Init (String[] args)
		{
			Impl.Init (args);
		}
		public void Init(String[] args, rcl_allocator_t custom_allocator)
		{
			Impl.Init (args, custom_allocator);
		}
		public bool IsInit
		{
			get{ return Impl.IsInit; }
		}

		public void Dispose()
		{
			// Dispose of unmanaged resources.
			Dispose(true);
			// Suppress finalization.
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return;
			if (disposing) {

				// Free any other managed objects here.
				Impl.Dispose();
			}


			disposed = true;
		}

	}

			
}

