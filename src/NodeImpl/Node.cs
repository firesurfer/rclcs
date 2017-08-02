using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace rclcs
{
	/// <summary>
	/// Represents a managed ROS node
	/// </summary>
	public class Node:Executable
	{
		private bool disposed = false;
		private rcl_node InternalNode;
		private ConcurrentBag<Executable> ManagedExecutables = new ConcurrentBag<Executable> ();
		/// <summary>
		/// The ROS node name
		/// </summary>
		/// <value>The name.</value>
		public string Name{ get; private set; }
		/// <summary>
		/// Initializes a new instance of the <see cref="rclcs.Node"/> class.
		/// Takes the node name
		/// </summary>
		/// <param name="_Name">Name.</param>
		/// <param name="_Namespace">Namespace.</param>
		public Node (string _Name, string _Namespace = "")
		{
			//Create a rcl_node which is a wrapper of the native methods
			InternalNode = new rcl_node(_Name,_Namespace);
			Name = _Name;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="rclcs.Node"/> class.
		/// This method is not supported at the moment
		/// </summary>
		/// <param name="_Name">Name.</param>
		/// <param name="DomainId">Domain identifier.</param>
		public Node (string _Name, UInt64 DomainId)
		{
			throw  new NotImplementedException ();
		}
		/// <summary>
		/// Gets the native node handle
		/// </summary>
		/// <value>The native node.</value>
		public rcl_node_t NativeNode {
			get{ return InternalNode.NativeNode; }
		}
		/// <summary>
		/// Gets the native node wrapper.
		/// </summary>
		/// <value>The native node wrapper.</value>
		internal rcl_node NativeNodeWrapper {
			get{ return InternalNode; }
		}

		/// <summary>
		/// Creates a publisher.
		/// </summary>
		/// <returns>The publisher.</returns>
		/// <param name="TopicName">Topic name.</param>
		/// <param name="AddToExecutables">If set to <c>true</c> add to executables.</param>
		/// <typeparam name="T">Message type</typeparam>
		public  Publisher<T> CreatePublisher<T> (string TopicName, bool AddToExecutables = true)
			where T: MessageWrapper, new()
		{
			return CreatePublisher<T> (TopicName, rmw_qos_profile_t.rmw_qos_profile_default, AddToExecutables);
		}
		/// <summary>
		/// Creates a publisher.
		/// With specific QOS Options
		/// </summary>
		/// <returns>The publisher.</returns>
		/// <param name="TopicName">Topic name.</param>
		/// <param name="QOSProfile">QOS profile.</param>
		/// <param name="AddToExecutables">If set to <c>true</c> add to executables.</param>
		/// <typeparam name="T">Message type</typeparam>
		public  Publisher<T> CreatePublisher<T> (string TopicName, rmw_qos_profile_t QOSProfile, bool AddToExecutables = true)
			where T: MessageWrapper, new()
		{
			Publisher<T> NewPublisher = new Publisher<T> (this, TopicName, QOSProfile);
			if (AddToExecutables)
				ManagedExecutables.Add (NewPublisher);
			return NewPublisher;
		}
		/// <summary>
		/// Creates a subscription.
		/// </summary>
		/// <returns>The subscription.</returns>
		/// <param name="TopicName">Topic name.</param>
		/// <param name="AddToExecutables">If set to <c>true</c> add to executables.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public Subscription<T> CreateSubscription<T> (string TopicName, bool AddToExecutables = true)
			where T: MessageWrapper, new()
		{
			return CreateSubscription<T> (TopicName, rmw_qos_profile_t.rmw_qos_profile_default, AddToExecutables);
		}
		/// <summary>
		/// Creates a subscription.
		/// with a specific QOSProfile
		/// </summary>
		/// <returns>The subscription.</returns>
		/// <param name="TopicName">Topic name.</param>
		/// <param name="QOSProfile">QOS profile.</param>
		/// <param name="AddToExecutables">If set to <c>true</c> add to executables.</param>
		/// <typeparam name="T">Message type</typeparam>
		public Subscription<T> CreateSubscription<T> (string TopicName, rmw_qos_profile_t QOSProfile, bool AddToExecutables = true)
			where T: MessageWrapper, new()
		{
			Subscription<T> NewSubscription = new Subscription<T> (this, TopicName, QOSProfile);
			if (AddToExecutables)
				ManagedExecutables.Add (NewSubscription);
			return NewSubscription;
		}
		/// <summary>
		/// Creates a service.
		/// </summary>
		/// <returns>The service.</returns>
		/// <param name="ServiceName">Service name.</param>
		/// <param name="AddToExecutables">If set to <c>true</c> add to executables.</param>
		/// <typeparam name="T">Request message type</typeparam>
		/// <typeparam name="U">Response message type</typeparam>
		public Service<T,U> CreateService<T,U> (string ServiceName, bool AddToExecutables = true)
			where T: MessageWrapper, new()
			where U: MessageWrapper, new()
		{
			return CreateService<T,U> (ServiceName, rmw_qos_profile_t.rmw_qos_profile_default, AddToExecutables);
		}
		/// <summary>
		/// Creates a service.
		/// With a specific QOS Profile
		/// </summary>
		/// <returns>The service.</returns>
		/// <param name="ServiceName">Service name.</param>
		/// <param name="QOSProfile">QOS profile.</param>
		/// <param name="AddToExecutables">If set to <c>true</c> add to executables.</param>
		/// <typeparam name="T">Request message type</typeparam>
		/// <typeparam name="U">Response message type</typeparam>
		public Service<T,U> CreateService<T,U> (string ServiceName,rmw_qos_profile_t QOSProfile, bool AddToExecutables = true)
			where T: MessageWrapper, new()
			where U: MessageWrapper, new()
		{
			Service<T,U> NewService = new Service<T,U> (this, ServiceName,QOSProfile);
			if (AddToExecutables)
				ManagedExecutables.Add (NewService);
			return NewService;
		}
		/// <summary>
		/// Creates the client.
		/// </summary>
		/// <returns>The client.</returns>
		/// <param name="ServiceName">Service name.</param>
		/// <param name="AddToExecutables">If set to <c>true</c> add to executables.</param>
		/// <typeparam name="T">Request message type</typeparam>
		/// <typeparam name="U">Response message type</typeparam>
		public Client<T,U> CreateClient<T,U> (string ServiceName, bool AddToExecutables = true)
			where T: MessageWrapper, new()
			where U: MessageWrapper, new()
		{
			return CreateClient<T,U> (ServiceName, rmw_qos_profile_t.rmw_qos_profile_default, AddToExecutables);
		}
		/// <summary>
		/// Creates a client
		/// with a specific QOS Profile
		/// </summary>
		/// <returns>The client.</returns>
		/// <param name="ServiceName">Service name.</param>
		/// <param name="QOSProfile">QOS profile.</param>
		/// <param name="AddToExecutables">If set to <c>true</c> add to executables.</param>
		/// <typeparam name="T">Request message type</typeparam>
		/// <typeparam name="U">Response message type</typeparam>
		public Client<T,U> CreateClient<T,U> (string ServiceName,rmw_qos_profile_t QOSProfile, bool AddToExecutables = true)
			where T: MessageWrapper, new()
			where U: MessageWrapper, new()
		{
			Client<T,U> NewClient = new Client<T,U> (this, ServiceName,QOSProfile);
			if (AddToExecutables)
				ManagedExecutables.Add (NewClient);
			return NewClient;
		}
		/// <summary>
		/// Execute all given executables like publishers, services and so on
		/// </summary>
		public override void Execute ()
		{
			foreach (var item in ManagedExecutables) {
				item.Execute ();
			}
		}
		public void AddExecutable(Executable _Executable)
		{
			this.ManagedExecutables.Add (_Executable);
		}
		protected override void Dispose (bool disposing)
		{
			if (disposed)
				return; 

			if (disposing) {
				// Free any other managed objects here.
				//
				InternalNode.Dispose ();
			}

			// Free any unmanaged objects here.
			//

			disposed = true;
			// Call base class implementation.
			base.Dispose (disposing);
		}

		public override bool IsDisposed ()
		{
			return disposed;
		}
	


	}


}

