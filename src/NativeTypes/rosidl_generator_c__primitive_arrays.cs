using System;
using System.Runtime.InteropServices;
namespace ROS2Sharp
{
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_bool
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
		public bool[] Array
		{
			get{ return Marshal.PtrToStructure<bool[]> (Data);}
		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_byte
	{	
		IntPtr Data;
		UIntPtr Size;
		UIntPtr Capacity;


		public rosidl_generator_c__primitive_array_byte(byte[] _Data)
		{
			Data = Marshal.AllocHGlobal (Marshal.SizeOf (_Data));
			Size = (UIntPtr)Marshal.SizeOf (_Data);
			Capacity = Size;
			Marshal.StructureToPtr (_Data, Data, true);  
		}
		public byte[] Array
		{
			get{ return Marshal.PtrToStructure<byte[]> (Data);}
		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_char
	{	
		IntPtr Data;
		UIntPtr Size;
		UIntPtr Capacity;

		public rosidl_generator_c__primitive_array_char(char[] _Data)
		{
			Data = Marshal.AllocHGlobal (Marshal.SizeOf (_Data));
			Size = (UIntPtr)Marshal.SizeOf (_Data);
			Capacity = Size;
			Marshal.StructureToPtr (_Data, Data, true);  
		}
		public char[] Array
		{
			get{ return Marshal.PtrToStructure<char[]> (Data);}
		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_float32
	{	
		 //[MarshalAs(UnmanagedType.LPArray)]
		 public IntPtr Data;
		 UIntPtr Size;
		 UIntPtr Capacity;


		public rosidl_generator_c__primitive_array_float32(float[] _Data)
		{
			
			Data = Marshal.AllocHGlobal (Marshal.SizeOf(typeof(float))*(_Data.Length));
			//Data =_Data;
			Size = (UIntPtr)(_Data.Length-1);
			Capacity = Size;
		
			Marshal.Copy (_Data, 0, Data,_Data.Length);

   			//Marshal.StructureToPtr<float[] >(_Data, Data, true);  
		}
		public float[] Array
		{
			get{ 
				float[] tempArray = new float[(int)Size]; 
				Marshal.Copy (Data, tempArray, 0, (int)Size);
				return tempArray;
			}
			//get{return Data;}
		}
		public int ArraySize
		{
			get{return (int)Size;}
		}
		public int ArrayCapacity
		{
			get{ return (int)Capacity;}
		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_float64
	{	
		IntPtr Data;
		UIntPtr Size ;
		UIntPtr Capacity;


		public rosidl_generator_c__primitive_array_float64(double[] _Data)
		{
			Data = Marshal.AllocHGlobal (Marshal.SizeOf (_Data));
			Size = (UIntPtr)Marshal.SizeOf (_Data);
			Capacity = Size;
			Marshal.StructureToPtr (_Data, Data, true);  
		}
		public double[] Array
		{
			get{ return Marshal.PtrToStructure<double[]> (Data);}
		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_int8
	{	
		IntPtr Data ;
		UIntPtr Size;
		UIntPtr Capacity;


		public rosidl_generator_c__primitive_array_int8(byte[] _Data)
		{
			Data = Marshal.AllocHGlobal (Marshal.SizeOf<byte> ()*_Data.Length);
			Size = (UIntPtr) _Data.Length;
			Capacity = Size;
			Marshal.Copy (_Data, 0, Data, _Data.Length);
			//Marshal.StructureToPtr (_Data, Data, true);  
		}
		public byte[] Array
		{
			get{ return Marshal.PtrToStructure<byte[]> (Data);}
		}
		public int ArraySize
		{
			get{return (int)Size;}
		}
		public int ArrayCapacity
		{
			get{ return (int)Capacity;}
		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_uint8
	{	
		IntPtr Data ;
		UIntPtr Size ;
		UIntPtr Capacity;


		public rosidl_generator_c__primitive_array_uint8(SByte[] _Data)
		{
			Data = Marshal.AllocHGlobal (Marshal.SizeOf (_Data));
			Size = (UIntPtr)Marshal.SizeOf (_Data);
			Capacity = Size;
			Marshal.StructureToPtr (_Data, Data, true);  
		}
		public sbyte[] Array
		{
			get{ return Marshal.PtrToStructure<sbyte[]> (Data);}
		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_int16
	{	
		IntPtr Data ;
		UIntPtr Size ;
		UIntPtr Capacity;


		public rosidl_generator_c__primitive_array_int16(Int16[] _Data)
		{
			Data = Marshal.AllocHGlobal (Marshal.SizeOf (_Data));
			Size = (UIntPtr)Marshal.SizeOf (_Data);
			Capacity = Size;
			Marshal.StructureToPtr (_Data, Data, true);  
		}
		public Int16[] Array
		{
			get{ return Marshal.PtrToStructure<Int16[]> (Data);}
		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_uint16
	{	
		IntPtr Data ;
		UIntPtr Size ;
		UIntPtr Capacity;

		public rosidl_generator_c__primitive_array_uint16(UInt16[] _Data)
		{
			Data = Marshal.AllocHGlobal (Marshal.SizeOf (_Data));
			Size = (UIntPtr)Marshal.SizeOf (_Data);
			Capacity = Size;
			Marshal.StructureToPtr (_Data, Data, true);  
		}
		public UInt16[] Array
		{
			get{ return Marshal.PtrToStructure<UInt16[]> (Data);}
		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_int32
	{	
		IntPtr Data ;
		UIntPtr Size;
		UIntPtr Capacity;


		public rosidl_generator_c__primitive_array_int32(Int32[] _Data)
		{
			Data = Marshal.AllocHGlobal (Marshal.SizeOf (_Data));
			Size = (UIntPtr)Marshal.SizeOf (_Data);
			Capacity = Size;
			Marshal.StructureToPtr (_Data, Data, true);  
		}
		public Int32[] Array
		{
			get{ return Marshal.PtrToStructure<Int32[]> (Data);}
		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_uint32
	{	
		IntPtr Data;
		UIntPtr Size ;
		UIntPtr Capacity;


		public rosidl_generator_c__primitive_array_uint32(UInt32[] _Data)
		{
			Data = Marshal.AllocHGlobal (Marshal.SizeOf (_Data));
			Size = (UIntPtr)Marshal.SizeOf (_Data);
			Capacity = Size;
			Marshal.StructureToPtr (_Data, Data, true);  
		}
		public UInt32[] Array
		{
			get{ return Marshal.PtrToStructure<UInt32[]> (Data);}
		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_int64
	{	
		IntPtr Data;
		UIntPtr Size;
		UIntPtr Capacity;

		public rosidl_generator_c__primitive_array_int64(Int64[] _Data)
		{
			Data = Marshal.AllocHGlobal (Marshal.SizeOf (_Data));
			Size = (UIntPtr)Marshal.SizeOf (_Data);
			Capacity = Size;
			Marshal.StructureToPtr (_Data, Data, true);  
		}
		public Int64[] Array
		{
			get{ return Marshal.PtrToStructure<Int64[]> (Data);}
		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_uint64
	{	
		IntPtr Data;
		UIntPtr Size;
		UIntPtr Capacity;

		public rosidl_generator_c__primitive_array_uint64(UInt64[] _Data)
		{
			Data = Marshal.AllocHGlobal (Marshal.SizeOf (_Data));
			Size = (UIntPtr)Marshal.SizeOf (_Data);
			Capacity = Size;
			Marshal.StructureToPtr (_Data, Data, true);  
		}
		public UInt64[] Array
		{
			get{ return Marshal.PtrToStructure<UInt64[]> (Data);}
		}
	}
}

