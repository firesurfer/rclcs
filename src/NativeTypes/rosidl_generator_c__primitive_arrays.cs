using System;
using System.Runtime.InteropServices;
namespace ROS2Sharp
{
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_bool:IRosTransportItem
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
		public void Free()
		{
			if(Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);
			
		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_byte:IRosTransportItem
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
		public void Free()
		{
			if(Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_char:IRosTransportItem
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
		public void Free()
		{
			if(Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_string:IRosTransportItem
	{	
		IntPtr Data;
		UIntPtr Size;
		UIntPtr Capacity;

		public rosidl_generator_c__primitive_array_string(string[] _Data)
		{
			Data = Marshal.AllocHGlobal (Marshal.SizeOf (_Data));
			Size = (UIntPtr)Marshal.SizeOf (_Data);
			Capacity = Size;
			Marshal.StructureToPtr (_Data, Data, true);  
		}
		public string[] Array
		{
			get{ return Marshal.PtrToStructure<string[]> (Data);}
		}
		public void Free()
		{
			if(Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_float32:IRosTransportItem
	{	
		 //[MarshalAs(UnmanagedType.LPArray)]
		 public IntPtr Data;
		 UIntPtr Size;
		 UIntPtr Capacity;


		public rosidl_generator_c__primitive_array_float32(float[] _Data)
		{

			int size = Marshal.SizeOf (typeof(float)) * (_Data.Length);
			Console.WriteLine ("Allocating: " + size + " For: " + _Data);
			Data = Marshal.AllocHGlobal (size);

			//Data =_Data;
			Size = (UIntPtr)(_Data.Length);
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
		public void Free()
		{
			if(Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_float64:IRosTransportItem
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
		public void Free()
		{
			if(Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_int8:IRosTransportItem
	{	
		public IntPtr Data ;
		UIntPtr Size;
		UIntPtr Capacity;


		public rosidl_generator_c__primitive_array_int8(byte[] _Data)
		{
			
			Data = Marshal.AllocHGlobal (_Data.Length);//Marshal.SizeOf<byte> ()*_Data.Length);
			Size = (UIntPtr) _Data.Length;
			Capacity = Size;
			Marshal.Copy (_Data, 0, Data, _Data.Length);
		}
		public byte[] Array
		{
			get{ byte[] tempArray = new byte[(int)Size]; 
				Marshal.Copy (Data, tempArray, 0, (int)Size);
				return tempArray;}
		}
		public int ArraySize
		{
			get{return (int)Size;}
		}
		public int ArrayCapacity
		{
			get{ return (int)Capacity;}
		}
		public void Free()
		{
			if(Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_uint8:IRosTransportItem
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
		public void Free()
		{
			if(Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_int16:IRosTransportItem
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
		public void Free()
		{
			if(Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_uint16:IRosTransportItem
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
		public void Free()
		{
			if(Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_int32:IRosTransportItem
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
	public struct rosidl_generator_c__primitive_array_uint32:IRosTransportItem
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
		public void Free()
		{
			if(Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_int64:IRosTransportItem
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
		public void Free()
		{
			if(Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_uint64:IRosTransportItem
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
		public void Free()
		{
			if(Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}
	}
}

