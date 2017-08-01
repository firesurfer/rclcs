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
			InternalNode = rcl_node.create_native_node (_Name, _Namespace);
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
	/// <summary>
	/// Wrapper for the native methods
	/// </summary>
	internal class rcl_node:IDisposable
	{
		private bool disposed = false;
		private rcl_node_t native_handle;
		/// <summary>
		/// Gets the native handle (Which is wrapped in a rcl_node_t.
		/// </summary>
		/// <value>The native node.</value>
		public rcl_node_t NativeNode {
			get{ return native_handle; }
		}

		public rcl_node (rcl_node_t _node)
		{
			native_handle = _node;
		}
		/// <summary>
		/// Releases all resource used by the <see cref="rclcs.rcl_node"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="rclcs.rcl_node"/>. The <see cref="Dispose"/>
		/// method leaves the <see cref="rclcs.rcl_node"/> in an unusable state. After calling <see cref="Dispose"/>, you must
		/// release all references to the <see cref="rclcs.rcl_node"/> so the garbage collector can reclaim the memory that
		/// the <see cref="rclcs.rcl_node"/> was occupying.</remarks>
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
			if (rcl_node_is_valid (ref native_handle))
				rcl_node_fini (ref native_handle);
			// Free any unmanaged objects here.
			//
			disposed = true;
		}

		~rcl_node ()
		{
			Dispose (false);
		}
		/// <summary>
		/// Gets the name of the node over the rcl interface
		/// </summary>
		/// <returns>The node name.</returns>
		public string get_node_name ()
		{	
			IntPtr handle =  rcl_node_get_name (ref native_handle);
			return Marshal.PtrToStringAnsi (handle);
		}
		/// <summary>
		/// Checks if the stored native_handle is valid
		/// </summary>
		/// <returns><c>true</c>, if native_handle is valid, <c>false</c> otherwise.</returns>
		public bool node_is_valid ()
		{
			//TODO bool should be marshalled ?
			return rcl_node_is_valid (ref native_handle);
		}
		/// <summary>
		/// Creates the native node.
		/// Moving this method into the constructor resulted into an error
		/// </summary>
		/// <returns>The native node.</returns>
		/// <param name="name">Name.</param>
		/// <param name="namespace">Namespace.</param>
		public static rcl_node create_native_node (string name, string namespace_ = "")
		{
			Console.WriteLine ("Test");
			rcl_node_t node = rcl_get_zero_initialized_node ();
			Console.WriteLine ("Test2");
			rcl_node_options_t default_options = rcl_node_get_default_options ();
			int ret = rcl_node_init (ref node, name, namespace_, ref default_options);
			rcl_node local_node = new rcl_node (node);
			return local_node;
		}

		[DllImport(RCL.LibRCLPath)]
		static extern rcl_node_t rcl_get_zero_initialized_node ();

		[DllImport(RCL.LibRCLPath)]
		static extern rcl_node_options_t rcl_node_get_default_options ();

		[DllImport(RCL.LibRCLPath)]
		static extern int rcl_node_get_domain_id( ref rcl_node_t  node, ref UIntPtr  domain_id);

		[DllImport(RCL.LibRCLPath)]
		static extern int rcl_node_init (ref rcl_node_t node, [MarshalAs (UnmanagedType.LPStr)]string name, [MarshalAs(UnmanagedType.LPStr)]string namespace_, ref rcl_node_options_t options);

		[DllImport(RCL.LibRCLPath)]
		static extern int rcl_node_fini (ref rcl_node_t node);

		[DllImport(RCL.LibRCLPath)]
		static extern bool rcl_node_is_valid (ref rcl_node_t node);

		[DllImport(RCL.LibRCLPath)]
		static extern IntPtr rcl_node_get_name (ref rcl_node_t node);





	}
	/// <summary>
	/// Wrapper for the pointer on the node
	/// This struct is a managed implementation of the rcl_node_t of ros2
	/// </summary>
	public struct rcl_node_t
	{
		public IntPtr impl;
	}

	/// <summary>
	/// Managed implementation of the rcl_node_options_t in ros2
	/// </summary>
	public struct rcl_node_options_t
	{
		public UIntPtr domain_id;
		public rcl_allocator_t allocator;
	}
}

