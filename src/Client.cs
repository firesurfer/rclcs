using System;
using System.Runtime.InteropServices;
namespace rclcs
{
	/// <summary>
	/// Client class for a ROS2 Service.
	/// <typeparam name="T">Request type</typeparam>
	/// <typeparam name="U">Response type</typeparam>
	/// </summary>
	public class Client<T,U>:Executable
		where T :MessageWrapper,new()
		where U :MessageWrapper,new()
	
	{
		private bool disposed = false;
		private rosidl_service_type_support_t TypeSupport;
		private rcl_client InternalClient;
		public Node RosNode{ get; private set; }
		public string ServiceName{ get;private set;}
		private rcl_client_options_t ClientOptions;
		public rmw_qos_profile_t QOSProfile{ get; private set; }
		public event EventHandler<ClientRecievedResponseEventArgs<U>> RecievedResponse;
		public Client (Node _Node, string _ServiceName):this(_Node,_ServiceName, rmw_qos_profile_t.rmw_qos_profile_default)
		{
			
		}
		public Client (Node _Node, string _ServiceName, rmw_qos_profile_t _QOS)
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
				throw new MissingMethodException ("Could not find typesupprt method");
			if (TypeSupport.data == IntPtr.Zero)
				throw new Exception ("Couldn't get typesupport");
			ClientOptions = rcl_client.get_default_options ();
			ClientOptions.qos = QOSProfile;
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
		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return; 

			if (disposing) {
				// Free any other managed objects here.
				//
				InternalClient.Dispose();
			}

			// Free any unmanaged objects here.
			//
			disposed = true;

			// Call the base class implementation.
			base.Dispose(disposing);
		}

	}

}

