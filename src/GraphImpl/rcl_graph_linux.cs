using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
namespace rclcs
{
   /// <summary>
    /// This class wraps the functions defined in the rcl/graph.h in the rcl package.
    /// The Graph class provides meta information about the ros2 connection structure, like the amount of publishers on a certain topic.
    /// </summary>
    public class GraphLinux:GraphBase
    {
        
        public GraphLinux ()
        {
        }

        /// <summary>
        /// Counts the publishers to a given topic name 
        /// </summary>
        /// <returns>The publishers.</returns>
        /// <param name="RosNode">Ros node.</param>
        /// <param name="TopicName">Topic name.</param>
        public override UInt64 CountPublishers(Node RosNode, string TopicName)
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
        public  override UInt64 CountSubscriptions(Node RosNode, string TopicName)
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
        public override RCLTopicNamesAndTypes GetTopicNamesAndTypes(Node RosNode)
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
        public override bool ServiceServerAvailable<T,U>(Node RosNode, Client<T,U> ServiceClient)
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

    
        protected override void Dispose(bool disposing)
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
}
