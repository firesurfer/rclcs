using System;
namespace rclcs
{
	/// <summary>
	/// These event args are used in the subscription new message event.
	/// </summary>
	public class MessageRecievedEventArgs<T>:EventArgs
		where T : class
	{
		//TODO Provide rmw_message_info_t ?
		private T message;
		public MessageRecievedEventArgs(T msg)
		{
			message = msg;
		}
		/// <summary>
		/// Gets the recieved message.
		/// </summary>
		/// <value>The message.</value>
		public T Message
		{
			get { return message; }
		}

	}
}
