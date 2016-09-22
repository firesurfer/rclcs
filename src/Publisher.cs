using System;
using System.Runtime.InteropServices;
using System.Reflection;
namespace rclcs
{
	public class Publisher<T>:Executable
		where T: MessageWrapper,new()
	{
		private rosidl_message_type_support_t TypeSupport;
		private rcl_publisher InternalPublisher;
		private bool disposed = false;
		public Node RosNode{ get; private set;}
		public string TopicName { get; private set; }
		private rcl_publisher_options_t PublisherOptions;
		public rmw_qos_profile_t QOSProfile{ get; private set; }
		public Publisher (Node _Node, string _TopicName) : this (_Node, _TopicName, rmw_qos_profile_t.rmw_qos_profile_default)
		{
			

		}
		public Publisher (Node _Node, string _TopicName, rmw_qos_profile_t _QOS )
		{
			QOSProfile = _QOS;
			RosNode = _Node;
			TopicName = _TopicName;


			Type wrapperType = typeof(T);
			Type messageType = typeof(T);
			foreach (var item in wrapperType.GetMethods()) {
				if (item.IsStatic) {
					if (item.Name.Contains ("GetMessageType")) {
						messageType = (Type)item.Invoke (null, null);
					}
				}
			}

			foreach (var item in messageType.GetMethods()) {
				
				if (item.IsStatic ) {
					
					if (item.Name.Contains ("rosidl_typesupport_introspection_c_get_message")) {
						TypeSupport = (rosidl_message_type_support_t)Marshal.PtrToStructure((IntPtr)item.Invoke (null, null),typeof(rosidl_message_type_support_t));
					}
				}
			}
			if (TypeSupport.data == IntPtr.Zero)
				throw new Exception ("Couldn't get typesupport");
			PublisherOptions = rcl_publisher.get_default_options ();
			PublisherOptions.qos = QOSProfile;
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
		public bool Publish(T msg)
		{
			msg.SyncDataOut ();
			ValueType temp;
			msg.GetData (out temp);
			/*Console.WriteLine ("##############################################");
			Console.WriteLine ("Debug in publish method:");
			//I still don't like this solution which is needed for nested types...

			foreach (var item in temp.GetType().GetFields()) {
				if (item.GetValue(temp).ToString().Contains ("Time")) {
					Console.WriteLine ("!!!!!Found time!!!!!");
					foreach (var item2 in item.FieldType.GetFields()) {
						Console.WriteLine (item2.Name + " " + item2.GetValue (item.GetValue(temp)));
					}
				}
				Console.WriteLine (item.Name + " " + item.GetValue (temp));
			}
			Console.WriteLine ("##############################################");*/
			return InternalPublisher.PublishMessage ( temp);
		}
		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return; 

			if (disposing) {
				
				// Free any other managed objects here.
				//
				InternalPublisher.Dispose();
			}

			// Free any unmanaged objects here.
			//

			disposed = true;
			// Call base class implementation.
			base.Dispose(disposing);
		}
	}
	internal class rcl_publisher:IDisposable
	{
		private rcl_publisher_t native_handle;
		private rcl_node_t native_node;
		private string topic_name;
		private rcl_publisher_options_t options;
		private rosidl_message_type_support_t type_support;
		private bool disposed = false;

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
			Dispose (false);	
		}
		public void Dispose()
		{ 
			Dispose(true);
			GC.SuppressFinalize(this);           
		}
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return; 

			if (disposing) {
				// Free any other managed objects here.
				//
			}
			rcl_publisher_fini (ref native_handle, ref native_node);
			// Free any unmanaged objects here.
			//
			disposed = true;
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
		public bool PublishMessage( ValueType msg)
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
		[DllImport(RCL.LibRCLPath)]
		extern static rcl_publisher_t rcl_get_zero_initialized_publisher();

		[DllImport(RCL.LibRCLPath)]
		extern static int rcl_publisher_init(ref rcl_publisher_t publisher,ref rcl_node_t node, ref rosidl_message_type_support_t type_support, string topic_name, ref rcl_publisher_options_t options);

		[DllImport(RCL.LibRCLPath)]
		extern static int rcl_publisher_fini (ref rcl_publisher_t publisher, ref rcl_node_t node);

		[DllImport(RCL.LibRCLPath)]
		extern static rcl_publisher_options_t rcl_publisher_get_default_options();

		[DllImport(RCL.LibRCLPath)]
		extern static int rcl_publish(ref rcl_publisher_t publisher,  [In] ValueType ros_message);

		[DllImport(RCL.LibRCLPath)]
		extern static string rcl_publisher_get_topic_name(ref rcl_publisher_t publisher);

		[DllImport(RCL.LibRCLPath)]
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

