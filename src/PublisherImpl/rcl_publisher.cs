using System;
using System.Runtime.InteropServices;

namespace rclcs
{
	internal abstract class rcl_publisher_base:IDisposable
	{
		protected rcl_publisher_t native_handle;
		protected rcl_node_t native_node;
		protected string topic_name;
		protected rcl_publisher_options_t options;
		protected rosidl_message_type_support_t type_support;
		protected bool disposed = false;

		public rcl_publisher_base (Node _node, rosidl_message_type_support_t _type_support, string _topic_name, rcl_publisher_options_t _options)
		{
			this.native_node = _node.NativeNode;
			this.topic_name = _topic_name;
			this.type_support = _type_support;
			this.options = _options;
		}

		public void Dispose ()
		{ 
			Dispose (true);
			GC.SuppressFinalize (this);           
		}

		protected virtual void Dispose (bool disposing)
		{
			if (disposed)
				return; 

			if (disposing) {
				// Free any other managed objects here.
				//
			}
			//Clean up unmanaged resources

			// Free any unmanaged objects here.
			//
			disposed = true;
		}

		/// <summary>
		/// Gets the name of the topic.
		/// </summary>
		/// <value>The name of the topic.</value>
		public string TopicName {
			get{ return topic_name; }
		}

		/// <summary>
		/// Gets the native publisher handle
		/// </summary>
		/// <value>The native publisher.</value>
		public rcl_publisher_t NativePublisher {
			get{ return native_handle; }
		}

		public abstract bool PublishMessage (ValueType msg);



	}
	public class rcl_publisher:IDisposable
	{
		protected bool disposed = false;

		rcl_publisher_base Impl;
		public rcl_publisher(Node _node, rosidl_message_type_support_t _type_support, string _topic_name, rcl_publisher_options_t _options)
		{
			if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                Impl = new rcl_publisher_windows(_node, _type_support, _topic_name, _options);
			} else if (Environment.OSVersion.Platform == PlatformID.Unix) {
				Impl = new rcl_publisher_linux (_node, _type_support,_topic_name,_options);
			} else {
				throw new Exception("Operating system: " +Environment.OSVersion.Platform.ToString() + " not supported");
			}
		}


		/// <summary>
		/// Gets the name of the topic.
		/// </summary>
		/// <value>The name of the topic.</value>
		public string TopicName {
			get{ return Impl.TopicName; }
		}

		/// <summary>
		/// Gets the native publisher handle
		/// </summary>
		/// <value>The native publisher.</value>
		public rcl_publisher_t NativePublisher {
			get{ return Impl.NativePublisher; }
		}
		public void Dispose ()
		{ 
			Dispose (true);
			GC.SuppressFinalize (this);           
		}

		protected virtual void Dispose (bool disposing)
		{
			if (disposed)
				return; 

			if (disposing) {
				// Free any other managed objects here.
				//
				Impl.Dispose();
			}
			//Clean up unmanaged resources

			// Free any unmanaged objects here.
			//
			disposed = true;
		}


		/// <summary>
		/// Gets the default options from the rcl
		/// //TODO rename  to GetDefaultOptions
		/// </summary>
		/// <returns>The default options.</returns>
		public static rcl_publisher_options_t get_default_options ()
		{
			if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
				//TODO codepath for windows
				//for now we use the linux codepath
				return rcl_publisher_windows.rcl_publisher_get_default_options ();
			} else if (Environment.OSVersion.Platform == PlatformID.Unix) {
				return rcl_publisher_linux.rcl_publisher_get_default_options ();
			} else {
				throw new Exception("Operating system: " +Environment.OSVersion.Platform.ToString() + " not supported");
			}
		}
		public bool PublishMessage (ValueType msg)
		{
			return Impl.PublishMessage (msg);
		}

	}

}
