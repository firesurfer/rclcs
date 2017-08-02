using System;
using System.Runtime.InteropServices;
namespace rclcs
{
	internal abstract class RCLBase
	{
		public  abstract void Init (String[] args);
		public  abstract void Init(String[] args, rcl_allocator_t custom_allocator);
		public  abstract bool IsInit{ get;}
	}
	public class RCL
	{

		//Check if compiled on windows or linux, unfortunatly we can't do a runtime check for the import statements
		//TODO implement different codepaths for windows and linux. This has the advantage we can do different calls for windows and linux
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
				//TODO codepath for windows
			} else if (Environment.OSVersion.Platform == PlatformID.Unix) {
				Impl = new RCLLinux ();
			} else {
				throw new Exception("Operating system: " +Environment.OSVersion.Platform.ToString() + " not supported");
			}
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
	}

			
}

