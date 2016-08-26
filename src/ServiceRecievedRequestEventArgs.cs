using System;

namespace ROS2Sharp
{
	public class ServiceRecievedRequestEventArgs<T>:EventArgs
		where T: struct
	{
		//TODO Provide rmw_service_info_t ?
		private T message;
		public ServiceRecievedRequestEventArgs (T msg)
		{
			message = msg;
		}
		public T Message
		{
			get { return message; }
		}
	}
}

