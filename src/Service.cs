using System;
using System.Runtime.InteropServices;
namespace ROS2Sharp
{
	/*
	 * T is the request type
	 * U is the response type
	 */
	public class Service<T,U>:Executable
		where T: struct
		where U: struct
	{
		private rosidl_service_type_support_t TypeSupport;
		private rcl_service InternalService;
		public Node RosNode{ get; private set; }
		public string ServiceName{ get;private set;}
		public rcl_service_options_t ServiceOptions { get; private set; }
		public event EventHandler<ServiceRecievedRequestEventArgs<T>> RequestRecieved;

		public Service (Node _Node, string _ServiceName)
		{
			RosNode = _Node;
			ServiceName = _ServiceName;
			Type ServiceType = typeof(T);

			foreach (var item in ServiceType.GetMethods()) {

				if (item.IsStatic ) {
					if (item.Name.Contains ("rosidl_typesupport_introspection_c_get_service")) {
						TypeSupport = (rosidl_service_type_support_t)Marshal.PtrToStructure((IntPtr)item.Invoke (null, null),typeof(rosidl_service_type_support_t));
					}
				}
			}
			ServiceOptions = rcl_service.get_default_options ();
			InternalService = new rcl_service (RosNode.NativeNode, TypeSupport, ServiceName, ServiceOptions);


		}
		public override void Execute ()
		{
			
		}
		public void SendResponse(U response)
		{
			InternalService.SendResponse<U> (ref response);
		}
		public rcl_service_t NativeService
		{
			get{ return InternalService.NativeService;}
		}
		public rcl_service NativeServiceWrapper
		{
			get{ return NativeServiceWrapper; }
		}
	}
	public class rcl_service
	{
		private rcl_node_t native_node;
		private rcl_service_t native_handle;

		public rcl_service(rcl_node_t _node, rosidl_service_type_support_t typesupport, string service_name, rcl_service_options_t options)
		{
			native_node = _node;
			native_handle = rcl_get_zero_initialized_service ();
			rcl_service_init (ref native_node, ref typesupport, service_name, ref options);
		}
		~rcl_service()
		{
			rcl_service_fini (ref native_handle, ref native_node);
		}
		public rcl_service_t NativeService
		{
			get{return native_handle;}
		}
		public T TakeRequest<T> (ref bool success)
			where T:struct
		{
			success = false;
			return new T ();
		}
		public void SendResponse<T>(ref T response)
			where T: struct
		{

		}
		public static rcl_service_options_t get_default_options()
		{
			return rcl_service_get_default_options ();
		}
		[DllImport("librcl.so")]
		extern static rcl_service_t rcl_get_zero_initialized_service();

		[DllImport("librcl.so")]
		extern static int rcl_service_init(ref rcl_node_t node, ref rosidl_service_type_support_t type_support, string topic_name, ref rcl_service_options_t options);

		[DllImport("librcl.so")]
		extern static int rcl_service_fini(ref rcl_service_t service, ref rcl_node_t node);

		[DllImport("librcl.so")]
		extern static rcl_service_options_t rcl_service_get_default_options();

		[DllImport("librcl.so")]
		extern static int rcl_take_request(ref rcl_service_t service, ref rmw_request_id_t request_header, [In,Out] ValueType ros_request);

		[DllImport("librcl.so")]
		extern static int rcl_send_response(ref rcl_service_t service, ref rmw_request_id_t request_header, [In,Out] ValueType ros_response);

		[DllImport("librcl.so")]
		extern static string rcl_service_get_service_name(ref rcl_service_t service);

		[DllImport("librcl.so")]
		extern static IntPtr rcl_service_get_options(ref rcl_service_t service);


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

