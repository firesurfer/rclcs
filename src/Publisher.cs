using System;
using System.Runtime.InteropServices;
using System.Reflection;
namespace ROS2Sharp
{
	public class Publisher<T>
	{
		private IntPtr TypeSupport;
		private rcl_publisher InternalPublisher;
		public Node RosNode{ get; private set;}
		public string TopicName { get; private set; }
		public Publisher (Node _node, string _topicName)
		{
			RosNode = _node;
			TopicName = _topicName;

		 	TypeSupport = IntPtr.Zero;
			Type messsageType = typeof(T);
			foreach (var item in messsageType.GetMethods()) {
				
				if (item.IsStatic ) {
					if (item.Name.Contains ("rosidl_typesupport_introspection_c_get_message")) {
						TypeSupport = (IntPtr)item.Invoke (null, null);
					}
				}
			}

			//InternalPublisher = new rcl_publisher ();

		}
	}
	public class rcl_publisher
	{
		public rcl_publisher(Node _node, IntPtr _type_support, string _topic_name, IntPtr _options)
		{
			publisher = rcl_get_zero_initialized_publisher ();

		}
		private rcl_publisher_t publisher;

		[DllImport("librcl.so")]
		extern static rcl_publisher_t rcl_get_zero_initialized_publisher();

		[DllImport("librcl.so")]
		extern static int rcl_publisher_init(ref rcl_node_t node, ref rosidl_message_type_support_t type_support, string topic_name, IntPtr options);
	}
	public struct rcl_publisher_t
	{
		IntPtr impl;
	}
	public struct rcl_publisher_options_t
	{
		//TODO - How am I supposed to create all these options....
	}
}

