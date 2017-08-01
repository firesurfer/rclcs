using System;
using System.Runtime.InteropServices;
namespace ROS2Sharp
{
	[StructLayout(LayoutKind.Sequential)]
	public class rosidl_generator_c__primitive_array_bool
	{	
		IntPtr Data;
		UIntPtr Size;
		UIntPtr Capacity;

		public rosidl_generator_c__primitive_array_bool(bool[] _Data)
		{
			Data = Marshal.AllocHGlobal (Marshal.SizeOf (_Data));
			Size = (UIntPtr)Marshal.SizeOf (_Data);
			Capacity = Size;
			Marshal.StructureToPtr (_Data, Data, true);  
		}
	}
}

