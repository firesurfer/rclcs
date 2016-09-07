using System;
namespace rclcs
{
	public class MessageRecievedEventArgs<T>:EventArgs
		where T : struct
	{
		//TODO Provide rmw_message_info_t ?
		private MessageWrapper<T> message;
		public MessageRecievedEventArgs(MessageWrapper<T> msg)
		{
			message = msg;
		}
		public MessageWrapper<T> Message
		{
			get { return message; }
		}

	}
}
