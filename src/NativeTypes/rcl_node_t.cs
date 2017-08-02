
using System;
using System.Runtime.InteropServices;

namespace rclcs
{
	/// <summary>
	/// Wrapper for the pointer on the node
	/// This struct is a managed implementation of the rcl_node_t of ros2
	/// </summary>
	public struct rcl_node_t
	{
		public IntPtr impl;
	}

	/// <summary>
	/// Managed implementation of the rcl_node_options_t in ros2
	/// </summary>
	public struct rcl_node_options_t
	{
		public UIntPtr domain_id;
		public rcl_allocator_t allocator;
	}
}
