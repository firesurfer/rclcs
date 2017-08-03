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
		private rcl_service_options_t ServiceOptions;
		public event EventHandler<ServiceRecievedRequestEventArgs<T,U>> RequestRecieved;
		public rmw_qos_profile_t QOSProfile{ get; private set; }
		public Service (Node _Node, string _ServiceName):this (_Node, _ServiceName, rmw_qos_profile_t.rmw_qos_profile_default)
		{
		}
		public Service (Node _Node, string _ServiceName, rmw_qos_profile_t _QOS)
		{
			QOSProfile = _QOS;
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
			bool foundMethod = false;
			foreach (var item in ServiceType.GetMethods()) {

				if (item.IsStatic ) {
					if (item.Name.Contains ("rosidl_typesupport_introspection_c__get_service_type_support_handle__")) {
						foundMethod = true;
						TypeSupport = (rosidl_service_type_support_t)Marshal.PtrToStructure((IntPtr)item.Invoke (null, null),typeof(rosidl_service_type_support_t));
					}
				}
			}
			if (!foundMethod)
				throw new MissingMethodException ("Could not find typesupport method");
			if (TypeSupport.data == IntPtr.Zero)
				throw new Exception ("Couldn't get typesupport");
			ServiceOptions = rcl_service.get_default_options ();
			ServiceOptions.qos = QOSProfile;
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
					RequestRecieved(this, new ServiceRecievedRequestEventArgs<T,U>(Request,new SendResponseDelegate<U>(InternalService.SendResponse<U>)));
			}
		}
		/* Use delegate provided by ServiceRecievedRequest event
		public void SendResponse(U response)
		{
			InternalService.SendResponse<U> (ref response);
		}*/
		public rcl_service_t NativeService
		{
			get{ return InternalService.NativeService;}
		}
		internal rcl_service NativeServiceWrapper
		{
			get{ return NativeServiceWrapper; }
		}
	}



}

