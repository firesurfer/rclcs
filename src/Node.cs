using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace rclcs
{
	public class Node:Executable
	{
		private bool disposed = false;
		private rcl_node InternalNode;
		private ConcurrentBag<Executable> ManagedExecutables = new ConcurrentBag<Executable> ();

		public string Name{ get; private set; }

		public Node (string _Name)
		{
			InternalNode = rcl_node.create_native_node (_Name);
			Name = _Name;
		}

		/**
		 * This isn't supported yet
		 */
		public Node (string _Name, UInt64 DomainId)
		{
			throw  new NotImplementedException ();
		}

		public rcl_node_t NativeNode {
			get{ return InternalNode.NativeNode; }
		}

		internal rcl_node NativeNodeWrapper {
			get{ return InternalNode; }
		}

		public  Publisher<T> CreatePublisher<T> (string TopicName, bool AddToExecutables = true)
			where T: MessageWrapper, new()
		{
			return CreatePublisher<T> (TopicName, rmw_qos_profile_t.rmw_qos_profile_default, AddToExecutables);
		}

		public  Publisher<T> CreatePublisher<T> (string TopicName, rmw_qos_profile_t QOSProfile, bool AddToExecutables = true)
			where T: MessageWrapper, new()
		{
			Publisher<T> NewPublisher = new Publisher<T> (this, TopicName, QOSProfile);
			if (AddToExecutables)
				ManagedExecutables.Add (NewPublisher);
			return NewPublisher;
		}

		public Subscription<T> CreateSubscription<T> (string TopicName, bool AddToExecutables = true)
			where T: MessageWrapper, new()
		{
			return CreateSubscription<T> (TopicName, rmw_qos_profile_t.rmw_qos_profile_default, AddToExecutables);
		}

		public Subscription<T> CreateSubscription<T> (string TopicName, rmw_qos_profile_t QOSProfile, bool AddToExecutables = true)
			where T: MessageWrapper, new()
		{
			Subscription<T> NewSubscription = new Subscription<T> (this, TopicName, QOSProfile);
			if (AddToExecutables)
				ManagedExecutables.Add (NewSubscription);
			return NewSubscription;
		}
		public Service<T,U> CreateService<T,U> (string ServiceName, bool AddToExecutables = true)
			where T: MessageWrapper, new()
			where U: MessageWrapper, new()
		{
			return CreateService<T,U> (ServiceName, rmw_qos_profile_t.rmw_qos_profile_default, AddToExecutables);
		}
		public Service<T,U> CreateService<T,U> (string ServiceName,rmw_qos_profile_t QOSProfile, bool AddToExecutables = true)
			where T: MessageWrapper, new()
			where U: MessageWrapper, new()
		{
			Service<T,U> NewService = new Service<T,U> (this, ServiceName,QOSProfile);
			if (AddToExecutables)
				ManagedExecutables.Add (NewService);
			return NewService;
		}

		public Client<T,U> CreateClient<T,U> (string ServiceName, bool AddToExecutables = true)
			where T: MessageWrapper, new()
			where U: MessageWrapper, new()
		{
			return CreateClient<T,U> (ServiceName, rmw_qos_profile_t.rmw_qos_profile_default, AddToExecutables);
		}
		public Client<T,U> CreateClient<T,U> (string ServiceName,rmw_qos_profile_t QOSProfile, bool AddToExecutables = true)
			where T: MessageWrapper, new()
			where U: MessageWrapper, new()
		{
			Client<T,U> NewClient = new Client<T,U> (this, ServiceName,QOSProfile);
			if (AddToExecutables)
				ManagedExecutables.Add (NewClient);
			return NewClient;
		}

		public override void Execute ()
		{
			foreach (var item in ManagedExecutables) {
				item.Execute ();
			}
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

	internal class rcl_node:IDisposable
	{
		private bool disposed = false;
		private rcl_node_t nativ_handle;

		public rcl_node_t NativeNode {
			get{ return nativ_handle; }
		}

		public rcl_node (rcl_node_t _node)
		{
			nativ_handle = _node;
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
			if (rcl_node_is_valid (ref nativ_handle))
				rcl_node_fini (ref nativ_handle);
			// Free any unmanaged objects here.
			//
			disposed = true;
		}

		~rcl_node ()
		{
			Dispose (false);
		}

		public string get_node_name ()
		{	
			IntPtr handle =  rcl_node_get_name (ref nativ_handle);
			return Marshal.PtrToStringAnsi (handle);
		}

		public bool node_is_valid ()
		{
			return rcl_node_is_valid (ref nativ_handle);
		}

		public static rcl_node create_native_node (string name)
		{
			rcl_node_t node = rcl_get_zero_initialized_node ();
			rcl_node_options_t default_options = rcl_node_get_default_options ();
			int ret = rcl_node_init (ref node, name,ref default_options);
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
		static extern int rcl_node_init (ref rcl_node_t node, [MarshalAs (UnmanagedType.LPStr)]string name, ref rcl_node_options_t options);

		[DllImport(RCL.LibRCLPath)]
		static extern int rcl_node_fini (ref rcl_node_t node);

		[DllImport(RCL.LibRCLPath)]
		static extern bool rcl_node_is_valid (ref rcl_node_t node);

		[DllImport(RCL.LibRCLPath)]
		static extern IntPtr rcl_node_get_name (ref rcl_node_t node);





	}

	public struct rcl_node_t
	{
		public IntPtr impl;
	}


	public struct rcl_node_options_t
	{
		public UIntPtr domain_id;
		public rcl_allocator_t allocator;
	}
}

