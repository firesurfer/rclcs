using System;
using System.Runtime.InteropServices;
namespace ROS2Sharp
{
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__String
	{

		public rosidl_generator_c__String(string _data)
		{
			//data = _data;
			size = (UIntPtr)_data.Length;
			capacity = (UIntPtr)size +1;
			data = Marshal.StringToHGlobalAnsi (_data);
		}
		/*~rosidl_generator_c__String()
		{
			Marshal.FreeHGlobal (data);
			data = IntPtr.Zero;
			size = UIntPtr.Zero;
			capacity = UIntPtr.Zero;
		}*/
		public string Data{
			//get{return data;}
			get{return Marshal.PtrToStringAnsi(data);}
		}
		public int Size{
			get{ return (int)size;}
		}
		public int Capacity{
			get{ return (int)capacity; }
		}
		//[MarshalAs(UnmanagedType.LPStr)]
		//string data ;
		IntPtr data;
		
		UIntPtr size;

		UIntPtr capacity;

		 
	}
}

