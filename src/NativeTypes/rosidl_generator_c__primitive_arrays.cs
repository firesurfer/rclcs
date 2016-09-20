using System;
using System.Runtime.InteropServices;
using System.Text;

namespace rclcs
{
	[StructLayout (LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_bool:IRosTransportItem
	{
		IntPtr Data;
		UIntPtr Size;
		UIntPtr Capacity;


		public rosidl_generator_c__primitive_array_bool (bool[] _Data)
		{
			//TODO Performance
			byte[] ConvertedData = new byte[_Data.Length];
			for (int i = 0; i < _Data.Length; i++) {
				if (_Data [i])
					ConvertedData [i] = 255;
				else
					ConvertedData [i] = 0;
			}
			Data = Marshal.AllocHGlobal (Marshal.SizeOf<byte> () * _Data.Length);
			Size = (UIntPtr)_Data.Length;
			Capacity = Size;
			Marshal.Copy (ConvertedData, 0, Data, _Data.Length);

		}

		public bool[] Array {
			get { 
				byte[] tempArray = new byte[(int)Size]; 
				if (Data != IntPtr.Zero)
					Marshal.Copy (Data, tempArray, 0, (int)Size);
				bool[] resultArray = new bool[(int)Size];
				for (int i = 0; i < tempArray.Length; i++) {
					if (tempArray [i] != 0)
						resultArray [i] = true;
					else
						resultArray [i] = false;
				}
				return resultArray;
			}
		}

		public void Free ()
		{
			if (Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);
			
		}
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_byte:IRosTransportItem
	{
		IntPtr Data;
		UIntPtr Size;
		UIntPtr Capacity;


		public rosidl_generator_c__primitive_array_byte (byte[] _Data)
		{
			Data = Marshal.AllocHGlobal (Marshal.SizeOf<byte> () * _Data.Length);
			Size = (UIntPtr)_Data.Length;
			Capacity = Size;
			Marshal.Copy (_Data, 0, Data, _Data.Length);

		}

		public byte[] Array {
			get { 
				byte[] tempArray = new byte[(int)Size]; 
				if (Data != IntPtr.Zero)
					Marshal.Copy (Data, tempArray, 0, (int)Size);
				return tempArray;
			}
		}

		public void Free ()
		{
			if (Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_char:IRosTransportItem
	{
		IntPtr Data;
		UIntPtr Size;
		UIntPtr Capacity;

		public rosidl_generator_c__primitive_array_char (char[] _Data)
		{
			Data = Marshal.AllocHGlobal (Marshal.SizeOf<byte> () * _Data.Length);
			byte[] ConvertedData = Encoding.ASCII.GetBytes (_Data);
			Size = (UIntPtr)_Data.Length;
			Capacity = Size;
			Marshal.Copy (ConvertedData, 0, Data, _Data.Length);

		}

		public char[] Array {
			get { 
				byte[] tempArray = new byte[(int)Size]; 
				if (Data != IntPtr.Zero)
					Marshal.Copy (Data, tempArray, 0, (int)Size);
				char[] resultArray = new char[(int)Size];
				Encoding.ASCII.GetChars (tempArray);
				return resultArray;
			}
		}

		public void Free ()
		{
			if (Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_string:IRosTransportItem
	{
		IntPtr Data;
		UIntPtr Size;
		UIntPtr Capacity;

		public rosidl_generator_c__primitive_array_string (string[] _Data)
		{
			//TODO Check for native type, do marshalling
			Data = Marshal.AllocHGlobal (Marshal.SizeOf<rosidl_generator_c__String> () * _Data.Length);
			Size = (UIntPtr)_Data.Length;
			Capacity = Size;
			rosidl_generator_c__String[] ConvertedData = new rosidl_generator_c__String[_Data.Length];
			IntPtr mem = Data;
			for (int i = 0; i < _Data.Length; i++) {
				ConvertedData [i] = new rosidl_generator_c__String (_Data [i]);
				Marshal.StructureToPtr (ConvertedData [i], mem, true);
				mem = new IntPtr((long)mem + Marshal.SizeOf<rosidl_generator_c__String> ());
			}



		}

		public string[] Array {
			//TODO
			get{ 
				int size = (int)Size;
				IntPtr mem = Data;
				rosidl_generator_c__String[] tempData = new rosidl_generator_c__String[size];
				for (int i = 0; i < size; i++) {
					tempData[i] = 	Marshal.PtrToStructure<rosidl_generator_c__String> (mem);
					mem = new IntPtr((long)mem + Marshal.SizeOf<rosidl_generator_c__String> ());
				}
			
				string[] tempArray = new string[tempData.Length];
				for (int i = 0; i < tempData.Length; i++) {
					tempArray [i] = tempData [i].Data;
				}
				return tempArray;}
		}

		public void Free ()
		{
			if (Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_float32:IRosTransportItem
	{
		 
		IntPtr Data;
		UIntPtr Size;
		UIntPtr Capacity;


		public rosidl_generator_c__primitive_array_float32 (float[] _Data)
		{

			Data = Marshal.AllocHGlobal (Marshal.SizeOf<float> () * _Data.Length);

			Size = (UIntPtr)_Data.Length;
			Capacity = Size;
			Marshal.Copy (_Data, 0, Data, _Data.Length);

		}

		public float[] Array {
			get { 
				float[] tempArray = new float[(int)Size]; 
				if (Data != IntPtr.Zero)
					Marshal.Copy (Data, tempArray, 0, (int)Size);
				return tempArray;
			}

		}

		public int ArraySize {
			get{ return (int)Size; }
		}

		public int ArrayCapacity {
			get{ return (int)Capacity; }
		}

		public void Free ()
		{
			if (Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}

		public override string ToString ()
		{
			if (Array.Length == 0)
				return "";
			StringBuilder temp = new StringBuilder ();
			foreach (var item in Array) {
				temp.Append (item + " ; ");
			}
			temp.Remove (temp.Length - 1, 1);
			return temp.ToString ();
		}
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_float64:IRosTransportItem
	{
		IntPtr Data;
		UIntPtr Size;
		UIntPtr Capacity;


		public rosidl_generator_c__primitive_array_float64 (double[] _Data)
		{
			Data = Marshal.AllocHGlobal (Marshal.SizeOf<double> () * _Data.Length);
			Size = (UIntPtr)_Data.Length;
			Capacity = Size;
			Marshal.Copy (_Data, 0, Data, _Data.Length);

		}

		public double[] Array {
			get { 
				double[] tempArray = new double[(int)Size]; 
				if (Data != IntPtr.Zero)
					Marshal.Copy (Data, tempArray, 0, (int)Size);
				return tempArray;
			}
		}

		public void Free ()
		{
			if (Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}

		public override string ToString ()
		{
			if (Array.Length == 0)
				return "";
			StringBuilder temp = new StringBuilder ();
			foreach (var item in Array) {
				temp.Append (item + " ; ");
			}
			temp.Remove (temp.Length - 1, 1);
			return temp.ToString ();
		}
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_int8:IRosTransportItem
	{
		public IntPtr Data;
		UIntPtr Size;
		UIntPtr Capacity;


		public rosidl_generator_c__primitive_array_int8 (byte[] _Data)
		{
			
			Data = Marshal.AllocHGlobal (Marshal.SizeOf<byte> () * _Data.Length);
			Size = (UIntPtr)_Data.Length;
			Capacity = Size;
			Marshal.Copy (_Data, 0, Data, _Data.Length);

		}

		public byte[] Array {
			get { 
				byte[] tempArray = new byte[(int)Size]; 
				if (Data != IntPtr.Zero)
					Marshal.Copy (Data, tempArray, 0, (int)Size);
				return tempArray;
			}
		}

		public int ArraySize {
			get{ return (int)Size; }
		}

		public int ArrayCapacity {
			get{ return (int)Capacity; }
		}

		public void Free ()
		{
			if (Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}

		public override string ToString ()
		{
			if (Array.Length == 0)
				return "";
			StringBuilder temp = new StringBuilder ();
			foreach (var item in Array) {
				temp.Append (item + " ; ");
			}
			temp.Remove (temp.Length - 1, 1);
			return temp.ToString ();
		}
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_uint8:IRosTransportItem
	{
		IntPtr Data;
		UIntPtr Size;
		UIntPtr Capacity;


		public rosidl_generator_c__primitive_array_uint8 (SByte[] _Data)
		{
			Data = Marshal.AllocHGlobal (Marshal.SizeOf<sbyte> () * _Data.Length);
			Size = (UIntPtr)_Data.Length;
			Capacity = Size;
			Marshal.Copy ((byte[])(Array)_Data, 0, Data, _Data.Length);

		}

		public sbyte[] Array {
			get { 
				byte[] tempArray = new byte[(int)Size]; 
				if (Data != IntPtr.Zero)
					Marshal.Copy (Data, tempArray, 0, (int)Size);
				return (sbyte[])(Array)tempArray;
			}
		}

		public void Free ()
		{
			if (Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}

		public override string ToString ()
		{
			if (Array.Length == 0)
				return "";
			StringBuilder temp = new StringBuilder ();
			foreach (var item in Array) {
				temp.Append (item + " ; ");
			}
			temp.Remove (temp.Length - 1, 1);
			return temp.ToString ();
		}
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_int16:IRosTransportItem
	{
		IntPtr Data;
		UIntPtr Size;
		UIntPtr Capacity;


		public rosidl_generator_c__primitive_array_int16 (Int16[] _Data)
		{
			Data = Marshal.AllocHGlobal (Marshal.SizeOf<Int16> () * _Data.Length);
			Size = (UIntPtr)_Data.Length;
			Capacity = Size;
			Marshal.Copy (_Data, 0, Data, _Data.Length);

		}

		public Int16[] Array {

			get { 
				Int16[] tempArray = new Int16[(int)Size]; 
				if (Data != IntPtr.Zero)
					Marshal.Copy (Data, tempArray, 0, (int)Size);
				return tempArray;
			}
		}

		public void Free ()
		{
			if (Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}

		public override string ToString ()
		{
			if (Array.Length == 0)
				return "";
			StringBuilder temp = new StringBuilder ();
			foreach (var item in Array) {
				temp.Append (item + " ; ");
			}
			temp.Remove (temp.Length - 1, 1);
			return temp.ToString ();
		}
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_uint16:IRosTransportItem
	{
		IntPtr Data;
		UIntPtr Size;
		UIntPtr Capacity;

		public rosidl_generator_c__primitive_array_uint16 (UInt16[] _Data)
		{
			Data = Marshal.AllocHGlobal (Marshal.SizeOf<UInt16> () * _Data.Length);
			Size = (UIntPtr)_Data.Length;
			Capacity = Size;
			Marshal.Copy ((Int16[])(Array)_Data, 0, Data, _Data.Length);

		}

		public UInt16[] Array {
			get { 
				Int16[] tempArray = new Int16[(int)Size]; 
				if (Data != IntPtr.Zero)
					Marshal.Copy (Data, tempArray, 0, (int)Size);
				return (UInt16[])(Array)tempArray;
			}
		}

		public void Free ()
		{
			if (Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}

		public override string ToString ()
		{
			if (Array.Length == 0)
				return "";
			StringBuilder temp = new StringBuilder ();
			foreach (var item in Array) {
				temp.Append (item + " ; ");
			}
			temp.Remove (temp.Length - 1, 1);
			return temp.ToString ();
		}
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_int32:IRosTransportItem
	{
		IntPtr Data;
		UIntPtr Size;
		UIntPtr Capacity;


		public rosidl_generator_c__primitive_array_int32 (Int32[] _Data)
		{
			Data = Marshal.AllocHGlobal (Marshal.SizeOf<Int32> () * _Data.Length);
			Size = (UIntPtr)_Data.Length;
			Capacity = Size;
			Marshal.Copy (_Data, 0, Data, _Data.Length);

		}

		public Int32[] Array {

			get { 
				Int32[] tempArray = new Int32[(int)Size]; 
				if (Data != IntPtr.Zero)
					Marshal.Copy (Data, tempArray, 0, (int)Size);
				return tempArray;
			}
		}

		public void Free ()
		{
			if (Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}

		public override string ToString ()
		{
			if (Array.Length == 0)
				return "";
			StringBuilder temp = new StringBuilder ();
			foreach (var item in Array) {
				temp.Append (item + " ; ");
			}
			temp.Remove (temp.Length - 1, 1);
			return temp.ToString ();
		}
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_uint32:IRosTransportItem
	{
		IntPtr Data;
		UIntPtr Size;
		UIntPtr Capacity;


		public rosidl_generator_c__primitive_array_uint32 (UInt32[] _Data)
		{
			Data = Marshal.AllocHGlobal (Marshal.SizeOf<UInt32> () * _Data.Length);
			Size = (UIntPtr)_Data.Length;
			Capacity = Size;
			Marshal.Copy ((Int32[])(Array)_Data, 0, Data, _Data.Length);
		}

		public UInt32[] Array {
			get { 
				Int32[] tempArray = new Int32[(int)Size]; 
				if (Data != IntPtr.Zero)
					Marshal.Copy (Data, tempArray, 0, (int)Size);
				return (UInt32[])(Array)tempArray;
			}
		}

		public void Free ()
		{
			if (Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}

		public override string ToString ()
		{
			if (Array.Length == 0)
				return "";
			StringBuilder temp = new StringBuilder ();
			foreach (var item in Array) {
				temp.Append (item + " ; ");
			}
			temp.Remove (temp.Length - 1, 1);
			return temp.ToString ();
		}
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_int64:IRosTransportItem
	{
		IntPtr Data;
		UIntPtr Size;
		UIntPtr Capacity;

		public rosidl_generator_c__primitive_array_int64 (Int64[] _Data)
		{
			Data = Marshal.AllocHGlobal (Marshal.SizeOf<Int64> () * _Data.Length);
			Size = (UIntPtr)_Data.Length;
			Capacity = Size;
			Marshal.Copy (_Data, 0, Data, _Data.Length);

		}

		public Int64[] Array {
			get { 
				Int64[] tempArray = new Int64[(int)Size]; 
				if (Data != IntPtr.Zero)
					Marshal.Copy (Data, tempArray, 0, (int)Size);
				return tempArray;
			}
		}

		public void Free ()
		{
			if (Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}

		public override string ToString ()
		{
			if (Array.Length == 0)
				return "";
			StringBuilder temp = new StringBuilder ();
			foreach (var item in Array) {
				temp.Append (item + " ; ");
			}
			temp.Remove (temp.Length - 1, 1);
			return temp.ToString ();
		}
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct rosidl_generator_c__primitive_array_uint64:IRosTransportItem
	{
		IntPtr Data;
		UIntPtr Size;
		UIntPtr Capacity;

		public rosidl_generator_c__primitive_array_uint64 (UInt64[] _Data)
		{
			Data = Marshal.AllocHGlobal (Marshal.SizeOf<UInt64> () * _Data.Length);
			Size = (UIntPtr)_Data.Length;
			Capacity = Size;
			Marshal.Copy ((Int64[])(Array)_Data, 0, Data, _Data.Length);

		}

		public UInt64[] Array {
			get { 
				Int64[] tempArray = new Int64[(int)Size]; 
				if (Data != IntPtr.Zero)
					Marshal.Copy (Data, tempArray, 0, (int)Size);
				return (UInt64[])(Array)tempArray;
			}
		}

		public void Free ()
		{
			if (Data != IntPtr.Zero)
				Marshal.FreeHGlobal (Data);

		}

		public override string ToString ()
		{
			if (Array.Length == 0)
				return "";
			StringBuilder temp = new StringBuilder ();
			foreach (var item in Array) {
				temp.Append (item + " ; ");
			}
			temp.Remove (temp.Length - 1, 1);
			return temp.ToString ();
		}
	}
}

