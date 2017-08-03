using System;
using System.Runtime.InteropServices;
namespace rclcs
{
	public struct rcl_service_t
	{
		IntPtr impl;
	}
	public struct rcl_service_options_t
	{
		public rmw_qos_profile_t qos;
		public rcl_allocator_t allocator;
	}
}

