using System;

namespace rclcs
{
	public delegate void SendResponseDelegate<U>( U res);
	public class ServiceRecievedRequestEventArgs<T,U>:EventArgs
		where T: MessageWrapper,new()
		where U: MessageWrapper,new()
	{
		//TODO Provide rmw_service_info_t ?
		private T request;
		public SendResponseDelegate<U> SendResponseFunc{ get; private set;}
		public ServiceRecievedRequestEventArgs (T req, SendResponseDelegate<U> sendResponseFunc)
		{
			SendResponseFunc = sendResponseFunc;
			request = req;
		}
		public T Request
		{
			get { return request; }
		}
	}
}

