using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
namespace rclcs
{
	public class Graph:IDisposable
	{
		bool disposed = false;
		public Graph ()
		{
		}

		/// <summary>
		/// Counts the publishers to a given topic name 
		/// </summary>
		/// <returns>The publishers.</returns>
		/// <param name="RosNode">Ros node.</param>
		/// <param name="TopicName">Topic name.</param>
		public UInt64 CountPublishers(Node RosNode, string TopicName)
		{
			IntPtr count = new IntPtr ();
			rcl_node_t native_handle = RosNode.NativeNode;
			int ret = rcl_count_publishers (ref native_handle, TopicName, ref count);
			//TODO Evaluate return value
			UInt64 publisherCount = (UInt64)count;
			return publisherCount;
		}
		/// <summary>
		/// Counts the subscriptions to a given topic name
		/// </summary>
		/// <returns>The subscriptions.</returns>
		/// <param name="RosNode">Ros node.</param>
		/// <param name="TopicName">Topic name.</param>
		public UInt64 CountSubscriptions(Node RosNode, string TopicName)
		{
			IntPtr count = new IntPtr ();
			rcl_node_t native_handle = RosNode.NativeNode;
			int ret = rcl_count_subscribers (ref native_handle, TopicName, ref count);
			//TODO Evaluate return value
			UInt64 subscriptionCount = (UInt64)count;
			return subscriptionCount;
		}
		/// <summary>
		/// Gets the topic names and their types in the system
		/// </summary>
		/// <returns>The topic names and types.</returns>
		/// <param name="RosNode">Ros node.</param>
		public RCLTopicNamesAndTypes GetTopicNamesAndTypes(Node RosNode)
		{
			rcl_node_t native_handle = RosNode.NativeNode;
			rcl_topic_names_and_types_t data_handle = new rcl_topic_names_and_types_t ();
			int ret = rcl_get_topic_names_and_types (ref native_handle, ref data_handle);
			//TODO Evaluate return value
			RCLTopicNamesAndTypes names_and_types = new RCLTopicNamesAndTypes (data_handle);
			ret = rcl_destroy_topic_names_and_types (ref  data_handle);
			return names_and_types;
		}
		/// <summary>
		/// Determines if a server to the given client is available
		/// </summary>
		/// <returns><c>true</c>, if a service to the given client is avilable, <c>false</c> otherwise.</returns>
		/// <param name="RosNode">Ros node.</param>
		/// <param name="ServiceClient">Service client.</param>
		/// <typeparam name="T">Request type</typeparam>
		/// <typeparam name="U">Response type</typeparam>
		public bool ServiceServerAvailable<T,U>(Node RosNode, Client<T,U> ServiceClient)
			where T: MessageWrapper, new()
			where U: MessageWrapper, new()
		{
			byte available = 0;
			rcl_node_t native_node_handle = RosNode.NativeNode;
			rcl_client_t native_client_handle = ServiceClient.NativeClient;
			int ret = rcl_service_server_is_available (ref native_node_handle, ref native_client_handle, ref available);
			//TODO Evaluate return value
			if (available > 0)
				return true;
			return false;
		}

		[DllImport("librcl.so")]
		static extern int rcl_get_topic_names_and_types(ref rcl_node_t  node, ref rcl_topic_names_and_types_t  topic_names_and_types);

		[DllImport("librcl.so")]
		static extern int rcl_destroy_topic_names_and_types(ref rcl_topic_names_and_types_t  topic_names_and_types);

		[DllImport("librcl.so")]
		static extern int rcl_count_publishers(ref rcl_node_t  node,string topic_name,ref IntPtr  count);

		[DllImport("librcl.so")]
		static extern int rcl_count_subscribers(ref rcl_node_t  node,string topic_name,ref IntPtr  count);

		[DllImport("librcl.so")]
		static extern int rcl_service_server_is_available(ref rcl_node_t node,ref rcl_client_t  client,ref byte is_available);

		public void Dispose()
		{
			// Dispose of unmanaged resources.
			Dispose(true);
			// Suppress finalization.
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return;
			if (disposing) {

				// Free any other managed objects here.
			}
			// Free any unmanaged objects here.


			disposed = true;
		}
	}
	/// <summary>
	/// Rcl topic names and types t.
	/// Native version 
	/// </summary>
	internal struct rcl_topic_names_and_types_t
	{
		/*
		 *  size_t topic_count;
  			char ** topic_names;
  			char ** type_names;
 		 */
		public IntPtr topic_count;
		public IntPtr topic_names;
		public IntPtr type_names;
	}
	/// <summary>
	/// RCL topic names and types.
	/// Managed version of the rcl_topic_names_and_types_t
	/// </summary>
	public class RCLTopicNamesAndTypes
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="rclcs.RCLTopicNamesAndTypes"/> class.
		/// Takes the native_struct and marshalls the char** for topic_names and type_names
		/// </summary>
		/// <param name="native_struct">Native struct.</param>
		internal RCLTopicNamesAndTypes(rcl_topic_names_and_types_t native_struct)
		{
			string[] topics = MarshallingHelpers.PtrToStringArray (native_struct.topic_names);
			string[] types = MarshallingHelpers.PtrToStringArray (native_struct.type_names);
			TopicNames = new List<string> (topics);
			TypeNames = new List<string> (types);

		}
		public List<string> TopicNames{ get; private set; }
		public List<string> TypeNames{ get; private set; }



	}

}

