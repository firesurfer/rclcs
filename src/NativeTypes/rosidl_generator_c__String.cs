using System;
using System.Runtime.InteropServices;
namespace ROS2Sharp
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct rosidl_generator_c__String
	{

		public rosidl_generator_c__String(string _data)
		{
			data = _data;
			size = (UIntPtr)_data.Length;
			capacity = (UIntPtr)size +1;

		}
		/*~rosidl_generator_c__String()
		{
			Marshal.FreeHGlobal (data);
			data = IntPtr.Zero;
			size = UIntPtr.Zero;
			capacity = UIntPtr.Zero;
		}*/
		public string Data{
			get{return data;}
		}
		[MarshalAs(UnmanagedType.LPStr)]
		string data ;

		UIntPtr size;

		UIntPtr capacity;

		 
	}
}

