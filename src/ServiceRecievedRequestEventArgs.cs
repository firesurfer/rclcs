using System;

namespace rclcs
{

	public class ServiceRecievedRequestEventArgs<T>:EventArgs
		where T: struct
	{
		//TODO Provide rmw_service_info_t ?
		private T request;

		public ServiceRecievedRequestEventArgs (T req)
		{
			request = req;
		}
		public T Request
		{
			get { return request; }
		}
	}
}

