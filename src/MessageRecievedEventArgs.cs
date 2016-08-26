using System;
namespace ROS2Sharp
{
	public class MessageRecievedEventArgs<T>:EventArgs
		where T : struct
	{
		//TODO Provide rmw_message_info_t ?
		private T message;
		public MessageRecievedEventArgs(T msg)
		{
			message = msg;
		}
		public T Message
		{
			get { return message; }
		}

	}
}
