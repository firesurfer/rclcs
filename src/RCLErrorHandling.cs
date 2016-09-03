using System;
using System.Runtime.InteropServices;
namespace rclcs
{
	public static class RCLErrorHandling
	{
		//TODO Do I have to call the corresponding functions in the rcl ?
		[DllImport("librmw.so")]
		extern static bool rmw_error_is_set();

		[DllImport("librmw.so")]
		[return : MarshalAs(UnmanagedType.LPStruct)]
		extern static rmw_error_state_t rmw_get_error_state();

		[DllImport("librmw.so")]
		extern static IntPtr rmw_get_error_string ();

		[DllImport("librmw.so")]
		extern static IntPtr rmw_get_error_string_safe();

		[DllImport("librmw.so")]
		extern static void rmw_reset_error();

		public static bool IsErrorSet()
		{
			return rmw_error_is_set ();
		}
		public static RMWErrorState GetRMWErrorState()
		{
			if (IsErrorSet ()) {
				return new RMWErrorState (rmw_get_error_state ());
			} else
				return null;
		}
	}
	public struct rmw_error_state_t
	{
		public IntPtr message;
		public IntPtr file;
		public UIntPtr line_number;
	}
	public class RMWErrorState
	{
		public string Message{ get; private set; }
		public string File { get; private set; }
		public UInt64 LineNumber { get; private set; }
		public RMWErrorState(string message, string file, UInt64 line_number)
		{
			Message = message;
			File = file;
			LineNumber = line_number;
		}
		public RMWErrorState(rmw_error_state_t state)
		{
			Message = Marshal.PtrToStringAnsi(state.message);
			File = Marshal.PtrToStringAnsi(state.file);
			LineNumber = (UInt64)state.line_number;
		}
		public override string ToString ()
		{
			return string.Format ("[RMWErrorState: Message={0}, File={1}, LineNumber={2}]", Message, File, LineNumber);
		}
	}
}

