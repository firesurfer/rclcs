using System;
using System.Runtime.InteropServices;
namespace rclcs
{
	/*
	 * T is the request type
	 * U is the response type
	 */
	public class Service<T,U>:Executable
		where T: MessageWrapper,new()
		where U: MessageWrapper,new()
	{
		private  bool disposed = false;
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
			Type wrapperType = typeof(T);

			foreach (var item in wrapperType.GetMethods()) {
				if (item.IsStatic) {
					if (item.Name.Contains ("GetMessageType")) {
						ServiceType = (Type)item.Invoke (null, null);
					}
				}
			}
			foreach (var item in ServiceType.GetMethods()) {

				if (item.IsStatic ) {
					if (item.Name.Contains ("rosidl_typesupport_introspection_c_get_message")) {
						TypeSupport = (rosidl_service_type_support_t)Marshal.PtrToStructure((IntPtr)item.Invoke (null, null),typeof(rosidl_service_type_support_t));
					}
				}
			}
			if (TypeSupport.data == IntPtr.Zero)
				throw new Exception ("Couldn't get typesupport");
			ServiceOptions = rcl_service.get_default_options ();
			InternalService = new rcl_service (RosNode.NativeNode, TypeSupport, ServiceName, ServiceOptions);


		}
		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return; 

			if (disposing) {
				
				// Free any other managed objects here.
				//
				InternalService.Dispose();
			}

			// Free any unmanaged objects here.
			//

			disposed = true;
			// Call base class implementation.
			base.Dispose(disposing);
		}

		public override void Execute ()
		{
			bool TakeSuccess = false;
			T Request = InternalService.TakeRequest<T> (ref TakeSuccess);
			if (TakeSuccess) {
				if(RequestRecieved != null)
					RequestRecieved(this, new ServiceRecievedRequestEventArgs<T>(Request));
			}
		}
		/*TODO Make sure this can only be called ones after recieving a RequestRecieved event*/
		public void SendResponse(U response)
		{
			InternalService.SendResponse<U> (ref response);
		}
		public rcl_service_t NativeService
		{
			get{ return InternalService.NativeService;}
		}
		internal rcl_service NativeServiceWrapper
		{
			get{ return NativeServiceWrapper; }
		}
	}
	internal class rcl_service:IDisposable
	{
		private bool disposed = false;
		private rcl_node_t native_node;
		private rcl_service_t native_handle;
		private rmw_request_id_t last_request_header;
		public rcl_service(rcl_node_t _node, rosidl_service_type_support_t typesupport, string service_name, rcl_service_options_t options)
		{
			native_node = _node;
			native_handle = rcl_get_zero_initialized_service ();
			int ret = rcl_service_init (ref native_handle,ref native_node, ref typesupport, service_name, ref options);
			RCLReturnValues retVal = (RCLReturnValues)ret;
			switch (retVal) {
			case RCLReturnValues.RCL_RET_OK:
				
				break;
			case RCLReturnValues.RCL_RET_NODE_INVALID:
				throw new RCLNodeInvalidException ();
				break;
			case RCLReturnValues.RCL_RET_INVALID_ARGUMENT:
				throw new RCLInvalidArgumentException();
				break;
			case RCLReturnValues.RCL_RET_SERVICE_INVALID:
				throw new RCLServiceInvalidException ();
				break;
			case RCLReturnValues.RCL_RET_BAD_ALLOC:
				throw new RCLBadAllocException ();
				break;
			case RCLReturnValues.RCL_RET_ERROR:
				
				break;
			default:
				break;
			}
		}
		~rcl_service()
		{
			Dispose (false);
		}
		public void Dispose()
		{ 
			Dispose(true);
			GC.SuppressFinalize(this);           
		}
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return; 

			if (disposing) {
				
				// Free any other managed objects here.
				//
			}
			rcl_service_fini (ref native_handle, ref native_node);
			// Free any unmanaged objects here.
			//
			disposed = true;
		}
		public rcl_service_t NativeService
		{
			get{return native_handle;}
		}
		public T TakeRequest<T> (ref bool success)
			where T:MessageWrapper,new()
		{
			
			success = false;
		    last_request_header = new rmw_request_id_t ();
			T request = new T ();
			ValueType msg;
			request.GetData (out msg);
			int ret = rcl_take_request (ref native_handle, ref last_request_header, msg);
			RCLReturnValues retVal = (RCLReturnValues)ret;
			switch (retVal) {
			case RCLReturnValues.RCL_RET_OK:
				success = true;
				request.SetData (ref msg);
				break;
			case RCLReturnValues.RCL_RET_INVALID_ARGUMENT:
				//throw new RCLInvalidArgumentException();
				break;
			case RCLReturnValues.RCL_RET_SERVICE_INVALID:
				throw new RCLServiceInvalidException ();
				break;
			case RCLReturnValues.RCL_RET_BAD_ALLOC:
				throw new RCLBadAllocException ();
				break;
			case RCLReturnValues.RCL_RET_ERROR:
				success = false;
				break;
			default:
				break;
			}
			return request;
		}
		public void SendResponse<T>(ref T response)
			where T: MessageWrapper,new()
		{
			ValueType msg;
			response.GetData (out msg);
			int ret = rcl_send_response (ref native_handle, ref last_request_header, msg);
			RCLReturnValues retVal = (RCLReturnValues)ret;
			switch (retVal) {
			case RCLReturnValues.RCL_RET_OK:
				return;
				break;
			case RCLReturnValues.RCL_RET_INVALID_ARGUMENT:
				throw new RCLInvalidArgumentException();
				break;
			case RCLReturnValues.RCL_RET_SERVICE_INVALID:
				throw new RCLServiceInvalidException ();
				break;
			case RCLReturnValues.RCL_RET_ERROR:
				throw new RCLErrorException ();
				break;
			default:
				break;
			}
		}
		public static rcl_service_options_t get_default_options()
		{
			return rcl_service_get_default_options ();
		}
		[DllImport("librcl.so")]
		extern static rcl_service_t rcl_get_zero_initialized_service();

		[DllImport("librcl.so")]
		extern static int rcl_service_init(ref rcl_service_t service, ref rcl_node_t node, ref rosidl_service_type_support_t type_support, string topic_name, ref rcl_service_options_t options);

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

