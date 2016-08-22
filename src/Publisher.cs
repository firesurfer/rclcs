using System;
using System.Runtime.InteropServices;
using System.Reflection;
namespace ROS2Sharp
{
	public class Publisher<T>:Executable
		where T: struct
	{
		private rosidl_message_type_support_t TypeSupport;
		private rcl_publisher InternalPublisher;
		public Node RosNode{ get; private set;}
		public string TopicName { get; private set; }
		public rcl_publisher_options_t PublisherOptions{ get; private set; }
		public Publisher (Node _node, string _topicName)
		{
			RosNode = _node;
			TopicName = _topicName;

		 	//TypeSupport = IntPtr.Zero;
			Type messsageType = typeof(T);
			foreach (var item in messsageType.GetMethods()) {
				
				if (item.IsStatic ) {
					if (item.Name.Contains ("rosidl_typesupport_introspection_c_get_message")) {
						TypeSupport = (rosidl_message_type_support_t)Marshal.PtrToStructure((IntPtr)item.Invoke (null, null),typeof(rosidl_message_type_support_t));
					}
				}
			}
			PublisherOptions = rcl_publisher.get_default_options ();
			InternalPublisher = new rcl_publisher (RosNode, TypeSupport, TopicName,PublisherOptions);

		}
		public rcl_publisher_t NativePublisher
		{
			get{ return InternalPublisher.NativePublisher;}
		}
		public rcl_publisher NativeWrapper
		{
			get{ return InternalPublisher; }
		}
		public bool Publish(T msg)
		{
			return InternalPublisher.PublishMessage<T> (ref msg);
		}
	}
	public class rcl_publisher
	{
		private rcl_publisher_t publisher;
		private rcl_node_t native_node;
		public rcl_publisher(Node _node, rosidl_message_type_support_t _type_support, string _topic_name, rcl_publisher_options_t _options)
		{
			publisher = rcl_get_zero_initialized_publisher ();
			native_node = _node.NativeNode;
			rcl_publisher_init (ref publisher,ref native_node, ref _type_support, _topic_name, ref _options);

		}
		~rcl_publisher()
		{
			rcl_publisher_fini (ref publisher, ref native_node);
		}
		public rcl_publisher_t NativePublisher
		{
			get{ return publisher;}
		}
		public static rcl_publisher_options_t get_default_options()
		{
			return rcl_publisher_get_default_options ();
		}
		public bool PublishMessage<T>(ref T msg)
			where T : struct
		{
			IntPtr msg_ptr = Marshal.AllocHGlobal (Marshal.SizeOf (typeof(T)));
			Marshal.StructureToPtr (msg, msg_ptr, true);
			int ret = rcl_publish (ref publisher, msg_ptr);
			RCLReturnValues ret_val = (RCLReturnValues)ret;

			/* \return RCL_RET_OK if the message was published successfully, or
			*         RCL_RET_INVALID_ARGUMENT if any arguments are invalid, or
			*         RCL_RET_PUBLISHER_INVALID if the publisher is invalid, or
			*         RCL_RET_ERROR if an unspecified error occurs.
				*/
			bool publish_message_success = false;
			switch (ret_val) {
			case RCLReturnValues.RCL_RET_OK:
				publish_message_success = true;
				break;
			case RCLReturnValues.RCL_RET_INVALID_ARGUMENT:
				throw new RCLInvalidArgumentException ();
				break;
			case RCLReturnValues.RCL_RET_PUBLISHER_INVALID:
				throw new RCLPublisherInvalidException ();
				break;
			case RCLReturnValues.RCL_RET_ERROR:
				throw new RCLInvalidArgumentException ();
				break;
			default:
				break;
			}
			return publish_message_success;
		}
		[DllImport("librcl.so")]
		extern static rcl_publisher_t rcl_get_zero_initialized_publisher();

		[DllImport("librcl.so")]
		extern static int rcl_publisher_init(ref rcl_publisher_t publisher,ref rcl_node_t node, ref rosidl_message_type_support_t type_support, string topic_name, ref rcl_publisher_options_t options);

		[DllImport("librcl.so")]
		extern static int rcl_publisher_fini (ref rcl_publisher_t publisher, ref rcl_node_t node);

		[DllImport("librcl.so")]
		extern static rcl_publisher_options_t rcl_publisher_get_default_options();

		[DllImport("librcl.so")]
		extern static int rcl_publish(ref rcl_publisher_t publisher, IntPtr ros_message);

		[DllImport("librcl.so")]
		extern static string rcl_publisher_get_topic_name(ref rcl_publisher_t publisher);

		[DllImport("librcl.so")]
		extern static rcl_publisher_options_t rcl_publisher_get_options(ref rcl_publisher_t publisher);
	}
	public struct rcl_publisher_t
	{
		IntPtr impl;
	}
	public struct rcl_publisher_options_t
	{
		public rmw_qos_profile_t qos;
		public rcl_allocator_t allocator;
	}
}

