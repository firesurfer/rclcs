using System;
using System.Runtime.InteropServices;
namespace ROS2Sharp
{
	
	public static class RCL
	{
		[DllImport("librcl.so")]
		public static extern int rcl_init(int argc, [In, Out] String[] argv, rcl_allocator_t allocator);

		[DllImport("librcl.so")]
		public static extern int rcl_shutdown ();

		[DllImport("librcl.so")]
		public static extern UInt64 rcl_get_instance_id ();

		[DllImport("librcl.so")]
		public static extern bool rcl_ok ();


	}
}

