using System;
using System.Runtime.InteropServices;

namespace rclcs
{
	/// <summary>
	/// Implementation of the rmw_error_state_t struct
	/// </summary>
	public struct rmw_error_state_t
	{
		public IntPtr message;
		public IntPtr file;
		public UIntPtr line_number;
	}
}