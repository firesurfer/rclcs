using System;
using System.Runtime.InteropServices;
namespace rclcs
{
	/// <summary>
	/// Implements error handling for the rcl and rmw (ros middle ware)
	/// </summary>
	public static class RCLErrorHandling
	{
		//TODO Do I have to call the corresponding functions in the rcl ?
		[DllImport(RCL.LibRMWPath)]
		extern static bool rmw_error_is_set();

		[DllImport(RCL.LibRMWPath)]
		[return : MarshalAs(UnmanagedType.LPStruct)]
		extern static rmw_error_state_t rmw_get_error_state();

		[DllImport(RCL.LibRMWPath)]
		extern static IntPtr rmw_get_error_string ();

		[DllImport(RCL.LibRMWPath)]
		extern static IntPtr rmw_get_error_string_safe();

		[DllImport(RCL.LibRMWPath)]
		extern static void rmw_reset_error();

		/// <summary>
		/// Checks if the error flag is set in the rmw
		/// </summary>
		/// <returns><c>true</c> if is error set; otherwise, <c>false</c>.</returns>
		public static bool IsErrorSet()
		{
			return rmw_error_is_set ();
		}
		/// <summary>
		/// Gets the rmw error state which basically contains a message ,a line and a file
		/// </summary>
		/// <returns>The RMW error state.</returns>
		public static RMWErrorState GetRMWErrorState()
		{
			if (IsErrorSet ()) {
				return new RMWErrorState (rmw_get_error_state ());
			} else
				return null;
		}
	}
	/// <summary>
	/// Implementation of the rmw_error_state_t struct
	/// </summary>
	public struct rmw_error_state_t
	{
		public IntPtr message;
		public IntPtr file;
		public UIntPtr line_number;
	}
	/// <summary>
	/// Managed wrapper for the rmw_error_state_t struct that does the marshalling for the char pointers.
	/// </summary>
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

