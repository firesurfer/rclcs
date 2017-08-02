using System;
using System.Runtime.InteropServices;
namespace rclcs
{
	/// <summary>
	/// This struct wraps the pointer on the "real" publisher that ros uses
	/// </summary>
	public struct rcl_publisher_t
	{
		IntPtr impl;
	}

	/// <summary>
	/// Implementation of the rcl_publisher_options_t struct with the same name and same definition in ros.
	/// </summary>
	public struct rcl_publisher_options_t
	{
		public rmw_qos_profile_t qos;
		public rcl_allocator_t allocator;
	}
}

