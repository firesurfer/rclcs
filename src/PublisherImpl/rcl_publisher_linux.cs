using System;
using System.Runtime.InteropServices;

namespace rclcs
{
	/// <summary>
	/// This class wraps the native methods and makes sure the Publisher<T> can only make save calls.
	/// It does furthermore the correct memory handling for memory allocated in the native code, like finishing the native publisher
	/// </summary>
	internal class rcl_publisher_linux:rcl_publisher_base
	{


		/// <summary>
		/// Initializes a new instance of the <see cref="rclcs.rcl_publisher"/> class.
		/// </summary>
		/// <param name="_node">Node.</param>
		/// <param name="_type_support">Type support.</param>
		/// <param name="_topic_name">Topic name.</param>
		/// <param name="_options">Options.</param>
		public rcl_publisher_linux (Node _node, rosidl_message_type_support_t _type_support, string _topic_name, rcl_publisher_options_t _options):base(_node,_type_support,_topic_name,_options)
		{
			native_handle = rcl_get_zero_initialized_publisher ();
			rcl_publisher_init (ref native_handle, ref native_node, ref type_support, topic_name, ref options);
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the <see cref="rclcs.rcl_publisher"/> is
		/// reclaimed by garbage collection.
		/// </summary>
		~rcl_publisher_linux ()
		{
			Dispose (false);	
		}



		/// <summary>
		/// Implementation of the IDisposable pattern
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose (bool disposing)
		{
			if (disposed)
				return; 

			if (disposing) {
				// Free any other managed objects here.
				//
			}
			//Clean up unmanaged resources
			rcl_publisher_fini (ref native_handle, ref native_node);
			// Free any unmanaged objects here.
			//
			disposed = true;
		}




		/// <summary>
		/// This method gets called from the Publisher<T> class in order to publish a message. Have in mind this method does not perform any type checks
		/// </summary>
		/// <returns><c>true</c>, if message was published, <c>false</c> otherwise.</returns>
		/// <param name="msg">Message.</param>
		public override bool PublishMessage (ValueType msg)
		{
			int ret = rcl_publish (ref native_handle, msg);
			//Handle the return types
			RCLReturnValues ret_val = (RCLReturnValues)ret;

			bool publish_message_success = false;
			switch (ret_val) {
			case RCLReturnValues.RCL_RET_OK:
				publish_message_success = true;
				break;
			case RCLReturnValues.RCL_RET_INVALID_ARGUMENT:
				throw new RCLInvalidArgumentException ();

			case RCLReturnValues.RCL_RET_PUBLISHER_INVALID:
				throw new RCLPublisherInvalidException ();

			case RCLReturnValues.RCL_RET_ERROR:
				throw new RCLInvalidArgumentException ();

			default:
				break;
			}
			return publish_message_success;
		}
		//Native methods. See the rcl/publisher.h file for detailed documentation.

		[DllImport ("librcl.so")]
		extern static rcl_publisher_t rcl_get_zero_initialized_publisher ();

		[DllImport ("librcl.so")]
		extern static int rcl_publisher_init (ref rcl_publisher_t publisher, ref rcl_node_t node, ref rosidl_message_type_support_t type_support, string topic_name, ref rcl_publisher_options_t options);

		[DllImport ("librcl.so")]
		extern static int rcl_publisher_fini (ref rcl_publisher_t publisher, ref rcl_node_t node);

		[DllImport ("librcl.so")]
		internal extern static rcl_publisher_options_t rcl_publisher_get_default_options ();

		[DllImport ("librcl.so")]
		extern static int rcl_publish (ref rcl_publisher_t publisher, [In] ValueType ros_message);

		[DllImport ("librcl.so")]
		extern static string rcl_publisher_get_topic_name (ref rcl_publisher_t publisher);

		[DllImport ("librcl.so")]
		extern static rcl_publisher_options_t rcl_publisher_get_options (ref rcl_publisher_t publisher);
	}

}
