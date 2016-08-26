using System;
using System.Runtime.InteropServices;
namespace ROS2Sharp
{
	public class Service<T>:Executable
		where T: struct
	{
		public Service ()
		{
		}
	}
	public class rcl_service
	{
		rcl_service_t native_handle;

		[DllImport("librcl.so")]
		extern static rcl_service_t rcl_get_zero_initialized_service();

		[DllImport("librcl.so")]
		extern static int rcl_service_init(ref rcl_node_t node, ref rosidl_service_type_support_t type_support, string topic_name, ref rcl_service_options_t options);

		[DllImport("librcl.so")]
		extern static int rcl_service_fini(ref rcl_service_t service, ref rcl_node_t node);

		[DllImport("librcl.so")]
		extern static rcl_service_options_t rcl_service_get_default_options();




	}
	public struct rcl_service_t
	{
		IntPtr impl;
	}
	public struct rcl_service_options_t
	{
		rmw_qos_profile_t qos;
		rcl_allocator_t allocator;
	}
}

