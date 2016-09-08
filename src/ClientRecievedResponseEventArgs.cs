using System;

namespace rclcs
{
	public class ClientRecievedResponseEventArgs<T>:EventArgs
		where T: MessageWrapper,new()
	{
		//TODO Provide rmw_service_info_t ?
		private T response;
		public ClientRecievedResponseEventArgs (T res)
		{
			response = res;
		}
		public T Response
		{
			get { return response; }
		}
	}
}

