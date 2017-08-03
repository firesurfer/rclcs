using System;
using System.Runtime.InteropServices;

namespace rclcs
{
	public struct rcl_client_t
	{
		IntPtr impl;
	}
	public struct rcl_client_options_t
	{
		public rmw_qos_profile_t qos;
		public rcl_allocator_t allocator;
	}

}