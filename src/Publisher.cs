using System;
using System.Runtime.InteropServices;
using System.Reflection;
namespace rclcs
{
	public class Publisher<T>:Executable
		where T: struct
	{
		private rosidl_message_type_support_t TypeSupport;
		private rcl_publisher InternalPublisher;

		public Node RosNode{ get; private set;}
		public string TopicName { get; private set; }
		public rcl_publisher_options_t PublisherOptions{ get; private set; }


		public Publisher (Node _Node, string _TopicName)
		{
			RosNode = _Node;
			TopicName = _TopicName;
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
		internal rcl_publisher NativeWrapper
		{
			get{ return InternalPublisher; }
		}
		public bool Publish(ValueType msg)
		{
			return InternalPublisher.PublishMessage<T> (ref msg);
		}
	}
	internal class rcl_publisher
	{
		private rcl_publisher_t native_handle;
		private rcl_node_t native_node;
		private string topic_name;
		private rcl_publisher_options_t options;
		private rosidl_message_type_support_t type_support;

		public rcl_publisher(Node _node, rosidl_message_type_support_t _type_support, string _topic_name, rcl_publisher_options_t _options)
		{
			this.native_node = _node.NativeNode;
			this.topic_name = _topic_name;
			this.type_support = _type_support;
			this.options = _options;

			native_handle = rcl_get_zero_initialized_publisher ();
			rcl_publisher_init (ref native_handle,ref native_node, ref type_support, topic_name, ref options);

		}
		~rcl_publisher()
		{
			rcl_publisher_fini (ref native_handle, ref native_node);
		}
		public string TopicName
		{
			get{ return topic_name; }
		}
		public rcl_publisher_t NativePublisher
		{
			get{ return native_handle;}
		}
		public static rcl_publisher_options_t get_default_options()
		{
			return rcl_publisher_get_default_options ();
		}
		public bool PublishMessage<T>(ref ValueType msg)
			where T : struct
		{
			int ret = rcl_publish (ref native_handle,  msg);
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
		extern static int rcl_publish(ref rcl_publisher_t publisher,  [In] ValueType ros_message);

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

