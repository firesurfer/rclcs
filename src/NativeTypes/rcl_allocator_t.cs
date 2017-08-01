using System;
using System.Runtime.InteropServices;
namespace rclcs
{
	public static class Allocator
	{
		
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public  delegate void Allocate(UIntPtr size, IntPtr state);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public   delegate void Deallocate(IntPtr ptr, IntPtr state);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public  delegate  void Reallocate(IntPtr ptr, IntPtr state);

		[DllImport(RCL.LibRCUtilsPATH, EntryPoint ="rcutils_get_default_allocator")]
		public static extern rcl_allocator_t rcl_get_default_allocator ();
	}
	public struct rcl_allocator_t
	{
		/*[MarshalAs(UnmanagedType.FunctionPtr)]Allocator.Allocate allocate;
		[MarshalAs(UnmanagedType.FunctionPtr)]Allocator.Deallocate deallocate;
		[MarshalAs(UnmanagedType.FunctionPtr)]Allocator.Reallocate reallocate;*/
		IntPtr allocate;
		IntPtr deallocate;
		IntPtr reallocate;
		IntPtr zero_allocate;
		IntPtr state;
	}


}

