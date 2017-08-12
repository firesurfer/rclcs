using System;
using System.Runtime.InteropServices;
namespace rclcs
{
	internal class rcl_subscription_windows : rcl_subscription_base, IDisposable
	{

		public rcl_subscription_windows(Node _node, rosidl_message_type_support_t _type_support, string _topic_name, rcl_subscription_options_t _options) : base(_node, _type_support, _topic_name, _options)
		{
			subscription = rcl_get_zero_initialized_subscription();

			rcl_subscription_init(ref subscription, ref native_node, ref _type_support, _topic_name, ref _options);
		}
		~rcl_subscription_windows()
		{
			Dispose(false);
		}

		public static rcl_subscription_options_t get_default_options()
		{
			return rcl_subscription_get_default_options();
		}
		public override T TakeMessage<T>(ref bool success)

		{
			rmw_message_info_t message_info = new rmw_message_info_t();
			return TakeMessage<T>(ref success, ref message_info);
		}
		public override T TakeMessage<T>(ref bool success, ref rmw_message_info_t _message_info)

		{
			MessageWrapper ret_msg = new T();
			ValueType msg;
			ret_msg.GetData(out msg);
			rmw_message_info_t message_info = _message_info;

			int ret = rcl_take(ref subscription, msg, message_info);

			RCLReturnValues ret_val = (RCLReturnValues)ret;
			//Console.WriteLine (ret_val);
			/*return RCL_RET_OK if the message was published, or
            *         RCL_RET_INVALID_ARGUMENT if any arguments are invalid, or
            *         RCL_RET_SUBSCRIPTION_INVALID if the subscription is invalid, or
            *         RCL_RET_BAD_ALLOC if allocating memory failed, or
            *         RCL_RET_SUBSCRIPTION_TAKE_FAILED if take failed but no error
                *         occurred in the middleware, or
            *         RCL_RET_ERROR if an unspecified error occurs.*/
			bool take_message_success = false;
			switch (ret_val)
			{
				case RCLReturnValues.RCL_RET_INVALID_ARGUMENT:
					throw new RCLInvalidArgumentException();

				case RCLReturnValues.RCL_RET_SUBSCRIPTION_INVALID:
					throw new RCLSubscriptionInvalidException();

				case RCLReturnValues.RCL_RET_BAD_ALLOC:
					throw new RCLBadAllocException();

				case RCLReturnValues.RCL_RET_SUBSCRIPTION_TAKE_FAILED:
					//throw new RCLSubscriptonTakeFailedException ();
					take_message_success = false;
					//Marshal.FreeHGlobal (msg_ptr);
					break;
				case RCLReturnValues.RCL_RET_ERROR:
					throw new RCLErrorException();

				case RCLReturnValues.RCL_RET_OK:
					{
						take_message_success = true;
						//Bring the data back into the message wrapper
						ret_msg.SetData(ref msg);
						//And do a sync for nested types. This is in my opinion a hack because I can't store references on value types in C#
						ret_msg.SyncDataIn();

					}
					break;
				default:
					break;
			}
			success = take_message_success;
			//Marshal.FreeHGlobal (message_info_ptr);

			return (T)ret_msg;
		}


		// Protected implementation of Dispose pattern.
		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				// Free any other managed objects here.
				//
			}
			rcl_subscription_fini(ref subscription, ref native_node);
			// Free any unmanaged objects here.
			//
			disposed = true;
		}

		[DllImport(@"rcl.dll")]
		extern static rcl_subscription_t rcl_get_zero_initialized_subscription();

		[DllImport(@"rcl.dll")]
		extern static int rcl_subscription_init(ref rcl_subscription_t subscription, ref rcl_node_t node, ref rosidl_message_type_support_t typesupport, string topic_name, ref rcl_subscription_options_t options);

		[DllImport(@"rcl.dll")]
		extern static int rcl_subscription_fini(ref rcl_subscription_t subscription, ref rcl_node_t node);

		[DllImport(@"rcl.dll")]
		extern static rcl_subscription_options_t rcl_subscription_get_default_options();

		[DllImport(@"rcl.dll")]
		extern static int rcl_take(ref rcl_subscription_t subscription, [Out] ValueType ros_message, [In, Out] rmw_message_info_t message_info);

		[DllImport(@"rcl.dll")]
		extern static string rcl_subscription_get_topic_name(ref rcl_subscription_t subscription);

		[DllImport(@"rcl.dll")]
		extern static IntPtr rcl_subscription_get_options(ref rcl_subscription_t subscription);

	}
}
