using System;
using System.Runtime.InteropServices;
namespace rclcs
{
	/// <summary>
	/// Managed implementation of the rosidl_generator_c__String type. See rosidl_generator_c package for more information.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__String:IRosTransportItem
	{

		public rosidl_generator_c__String(string _data)
		{
			size = (UIntPtr)_data.Length;
			capacity = (UIntPtr)size +1;
			data = Marshal.StringToHGlobalAnsi (_data);
		}
		public string Data{
			get{return Marshal.PtrToStringAnsi(data);}
		}
		public int Size{
			get{ return (int)size;}
		}
		public int Capacity{
			get{ return (int)capacity; }
		}
		public void Free()
		{
			Marshal.FreeHGlobal (data);
		}
		public override string ToString ()
		{
			return Data;
		}

		IntPtr data;
		
		UIntPtr size;

		UIntPtr capacity;

		 
	}
}

