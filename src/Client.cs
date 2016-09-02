using System;
using System.Runtime.InteropServices;
namespace ROS2Sharp
{
	/*
	 * T is the request type
	 * U is the response type
	 */
	public class Client<T,U>
		where T :struct
		where U :struct
	
	{
		public event EventHandler<ClientRecievedResponseEventArgs<U>> RecievedResponse;
		public Client (Node _Node, string _ServiceName)
		{
		}
	}
	public class rcl_client
	{
		private rcl_client_t native_handle;
		private rcl_node_t native_node;
		private string service_name;
		private rcl_client_options_t options;
		private rosidl_service_type_support_t typesupport;



		public rcl_client(rcl_node_t _node, rosidl_service_type_support_t _typesupport,  string _service_name, rcl_client_options_t _options)
		{
			this.native_node = _node;
			this.service_name = _service_name;
			this.options = _options;
			this.typesupport = _typesupport;

			native_handle = rcl_get_zero_initialized_client ();
			rcl_client_init (ref native_handle, ref native_node, ref typesupport, service_name, ref options);
		}
		~rcl_client()
		{
			rcl_client_fini(ref native_handle, ref native_node);
		}
		public static rcl_client_options_t get_default_options()
		{
			return rcl_client_get_default_options ();
		}

		[DllImport("librcl.so")]
		extern static rcl_client_t rcl_get_zero_initialized_client();

		[DllImport("librcl.so")]
		extern static int rcl_client_init(ref rcl_client_t client, ref rcl_node_t node, ref rosidl_service_type_support_t type_support, string service_name, ref rcl_client_options_t options);

		[DllImport("librcl.so")]
		extern static int rcl_client_fini(ref rcl_client_t client, ref rcl_node_t node);

		[DllImport("librcl.so")]
		extern static rcl_client_options_t rcl_client_get_default_options();

		[DllImport("librcl.so")]
		extern static int rcl_send_request(ref rcl_client_t client, IntPtr ros_request, ref Int64 sequence_number);

		[DllImport("librcl.so")]
		extern static int rcl_take_response(ref rcl_client_t client, ref rmw_request_id_t request_header, IntPtr ros_response);

		[DllImport("librcl.so")]
		extern static string rcl_client_get_service_name(ref rcl_client_t client);

		[DllImport("librcl.so")]
		extern static IntPtr rcl_client_get_options(ref rcl_client_t client);

	}
	public struct rcl_client_t
	{
		IntPtr impl;
	}
	public struct rcl_client_options_t
	{
		rmw_qos_profile_t qos;
		rcl_allocator_t allocator;
	}
}

