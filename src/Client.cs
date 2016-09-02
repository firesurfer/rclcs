using System;
using System.Runtime.InteropServices;
namespace rclcs
{
	/*
	 * T is the request type
	 * U is the response type
	 */
	public class Client<T,U>:Executable
		where T :struct
		where U :struct
	
	{
		private rosidl_service_type_support_t TypeSupport;
		private rcl_client InternalClient;
		public Node RosNode{ get; private set; }
		public string ServiceName{ get;private set;}
		public rcl_client_options_t ClientOptions { get; private set; }

		public event EventHandler<ClientRecievedResponseEventArgs<U>> RecievedResponse;
		public Client (Node _Node, string _ServiceName)
		{
			RosNode = _Node;
			ServiceName = _ServiceName;
			Type ServiceType = typeof(T);

			foreach (var item in ServiceType.GetMethods()) {

				if (item.IsStatic ) {
					if (item.Name.Contains ("rosidl_typesupport_introspection_c_get_message")) {
						
						TypeSupport = (rosidl_service_type_support_t)Marshal.PtrToStructure((IntPtr)item.Invoke (null, null),typeof(rosidl_service_type_support_t));
					}
				}
			}
			ClientOptions = rcl_client.get_default_options ();
			InternalClient = new rcl_client (RosNode.NativeNode, TypeSupport, ServiceName, ClientOptions);
		}
		public override void Execute ()
		{
			bool success = false;
			U Response = InternalClient.TakeResponse<U> (ref success);
			if (success) {
				if (RecievedResponse != null)
					RecievedResponse (this, new ClientRecievedResponseEventArgs<U> (Response));
			}
		}
		public void SendRequest(T Request)
		{
			InternalClient.SendRequest<T> (Request);
		}
		public rcl_client_t NativeClient
		{
			get{return InternalClient.NativeClient; }
		}
		internal rcl_client NativeWrapper
		{
			get{return InternalClient; }
		}
	}
	internal class rcl_client
	{
		private rcl_client_t native_handle;
		private rcl_node_t native_node;
		private string service_name;
		private rcl_client_options_t options;
		private rosidl_service_type_support_t typesupport;

		private Int64 last_sequence_number = 0;

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
		public rcl_client_t NativeClient
		{
			get{return native_handle;}
		}
		public T TakeResponse<T>(ref bool success)
			where T: struct
		{
			success = false;
			rmw_request_id_t request_header = new rmw_request_id_t ();
			T response = new T ();
			int ret = rcl_take_response (ref native_handle, ref request_header, response);
			RCLReturnValues retVal = (RCLReturnValues)ret;
			switch (retVal) {
			case RCLReturnValues.RCL_RET_OK:
				success = true;
				break;
			case RCLReturnValues.RCL_RET_INVALID_ARGUMENT:
				throw new RCLInvalidArgumentException();
				break;
			case RCLReturnValues.RCL_RET_CLIENT_INVALID:
				throw new RCLClientInvalidException ();
				break;
			case RCLReturnValues.RCL_RET_ERROR:
				success = false;
				break;
			default:
				break;
			}
			return response;
		}
		public void SendRequest<T>(T request)
			where T :struct
		{
			int ret = rcl_send_request (ref native_handle,  request, ref last_sequence_number);
			RCLReturnValues retVal = (RCLReturnValues)ret;
			switch (retVal) {
			case RCLReturnValues.RCL_RET_OK:
				break;
			case RCLReturnValues.RCL_RET_INVALID_ARGUMENT:
				throw new RCLInvalidArgumentException();
				break;
			case RCLReturnValues.RCL_RET_CLIENT_INVALID:
				throw new RCLClientInvalidException ();
				break;
			case RCLReturnValues.RCL_RET_ERROR:
				throw new RCLErrorException ();
				break;
			default:
				break;
			}
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
		extern static int rcl_send_request(ref rcl_client_t client, [In,Out] ValueType ros_request, ref Int64 sequence_number);

		[DllImport("librcl.so")]
		extern static int rcl_take_response(ref rcl_client_t client, ref rmw_request_id_t request_header, [In,Out] ValueType ros_response);

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

