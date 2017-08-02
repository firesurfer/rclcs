using System;
using System.Runtime.InteropServices;
namespace rclcs
{
	public struct rcl_subscription_t
	{
		IntPtr impl;
	}
	public struct rcl_subscription_options_t
	{
		public rmw_qos_profile_t qos;
		public byte ignore_local_publications;
		public rcl_allocator_t allocator;
	}
}