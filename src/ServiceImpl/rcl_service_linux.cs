using System;
using System.Runtime.InteropServices;
namespace rclcs
{
	internal class rcl_service_linux:rcl_service_base,IDisposable
	{

		public rcl_service_linux(rcl_node_t _node, rosidl_service_type_support_t typesupport, string service_name, rcl_service_options_t options):base(_node,typesupport, service_name, options)
		{

			native_handle = rcl_get_zero_initialized_service ();
			int ret = rcl_service_init (ref native_handle,ref native_node, ref typesupport, service_name, ref options);
			RCLReturnValues retVal = (RCLReturnValues)ret;
			switch (retVal) {
			case RCLReturnValues.RCL_RET_OK:

				break;
			case RCLReturnValues.RCL_RET_NODE_INVALID:
				throw new RCLNodeInvalidException ();
			case RCLReturnValues.RCL_RET_INVALID_ARGUMENT:
				throw new RCLInvalidArgumentException();
			case RCLReturnValues.RCL_RET_SERVICE_INVALID:
				throw new RCLServiceInvalidException ();
			case RCLReturnValues.RCL_RET_BAD_ALLOC:
				throw new RCLBadAllocException ();
			case RCLReturnValues.RCL_RET_ERROR:

				break;
			default:
				break;
			}
		}
		~rcl_service_linux()
		{
			Dispose (false);
		}

		protected override void Dispose(bool disposing)
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

		public override T TakeRequest<T> (ref bool success)
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

			case RCLReturnValues.RCL_RET_BAD_ALLOC:
				throw new RCLBadAllocException ();

			case RCLReturnValues.RCL_RET_ERROR:
				success = false;
				break;
			default:
				break;
			}
			return request;
		}

		public override void SendResponse<T>( T response)
		{
			ValueType msg;
			response.GetData (out msg);
			int ret = rcl_send_response (ref native_handle, ref last_request_header, msg);
			RCLReturnValues retVal = (RCLReturnValues)ret;
			switch (retVal) {
			case RCLReturnValues.RCL_RET_OK:
				return;

			case RCLReturnValues.RCL_RET_INVALID_ARGUMENT:
				throw new RCLInvalidArgumentException();

			case RCLReturnValues.RCL_RET_SERVICE_INVALID:
				throw new RCLServiceInvalidException ();

			case RCLReturnValues.RCL_RET_ERROR:
				throw new RCLErrorException ();

			default:
				break;
			}
		}
		public static rcl_service_options_t get_default_options()
		{
			return rcl_service_get_default_options ();
		}
		[DllImport(RCL.LibRCLPath)]
		extern static rcl_service_t rcl_get_zero_initialized_service();

		[DllImport(RCL.LibRCLPath)]
		extern static int rcl_service_init(ref rcl_service_t service, ref rcl_node_t node, ref rosidl_service_type_support_t type_support, string topic_name, ref rcl_service_options_t options);

		[DllImport(RCL.LibRCLPath)]
		extern static int rcl_service_fini(ref rcl_service_t service, ref rcl_node_t node);

		[DllImport(RCL.LibRCLPath)]
		extern static rcl_service_options_t rcl_service_get_default_options();

		[DllImport(RCL.LibRCLPath)]
		extern static int rcl_take_request(ref rcl_service_t service, ref rmw_request_id_t request_header, [In,Out] ValueType ros_request);

		[DllImport(RCL.LibRCLPath)]
		extern static int rcl_send_response(ref rcl_service_t service, ref rmw_request_id_t request_header, [In,Out] ValueType ros_response);

		[DllImport(RCL.LibRCLPath)]
		extern static string rcl_service_get_service_name(ref rcl_service_t service);

		[DllImport(RCL.LibRCLPath)]
		extern static IntPtr rcl_service_get_options(ref rcl_service_t service);


	}
}