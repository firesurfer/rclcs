using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace ROS2Sharp
{
	public class Node:Executable
	{
		private rcl_node InternalNode;
		private ConcurrentBag<Executable> ManagedExecutables = new ConcurrentBag<Executable>();

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

		public rcl_node_t NativeNode
		{
			get{return InternalNode.NativeNode; }
		}
		public rcl_node NativeNodeWrapper
		{
			get{ return InternalNode; }
		}
		public  Publisher<T> CreatePublisher<T>(string TopicName, bool AddToExecutables = true)
			where T: struct
		{
			Publisher<T> NewPublisher = new Publisher<T> (this, TopicName);
			if(AddToExecutables)
				ManagedExecutables.Add (NewPublisher);
			return NewPublisher;
		}
		public Subscription<T> CreateSubscription<T>(string TopicName, bool AddToExecutables = true)
			where T: struct
		{
			Subscription<T> NewSubscription = new Subscription<T> (this, TopicName);
			if(AddToExecutables)
				ManagedExecutables.Add (NewSubscription);
			return NewSubscription;
		}
		public Service<T,U> CreateService<T,U>(string ServiceName, bool AddToExecutables = true)
			where T: struct
			where U: struct
		{
			//TODO -> Add parameters to constructor when serivce is implemented
			Service<T,U> NewService = new Service<T,U> (this, ServiceName);
			if(AddToExecutables)
				ManagedExecutables.Add (NewService);
			return NewService;
		}
		public override void Execute ()
		{
			foreach (var item in ManagedExecutables) {
				item.Execute ();
			}
		}

	}
	public class rcl_node
	{
		private rcl_node_t node;
		public rcl_node_t NativeNode
		{
			get{return node; }
		}

		public rcl_node(rcl_node_t _node)
		{
			node = _node;
		}
		~rcl_node()
		{
			rcl_node_fini (ref node);
		}
		public string get_node_name()
		{	
			return rcl_node_get_name(ref node);
		}
		public bool node_is_valid()
		{
			return rcl_node_is_valid (ref node);
		}
		public static rcl_node create_native_node(string name)
		{
			rcl_node_t node = rcl_get_zero_initialized_node ();
			IntPtr default_options = rcl_node_get_default_options ();
			int ret = rcl_node_init(ref node,name, default_options);
			rcl_node local_node = new rcl_node(node);
			return local_node;
		}

		[DllImport ("librcl.so")]
		static extern rcl_node_t rcl_get_zero_initialized_node ();
	
		[DllImport ("librcl.so")]
		static extern IntPtr rcl_node_get_default_options ();

		public rcl_node_options_t marshal_node_get_default_options()
		{
			return Marshal.PtrToStructure<rcl_node_options_t> (rcl_node_get_default_options ());
		}

		[DllImport("librcl.so")]
		static extern int rcl_node_init(ref rcl_node_t node,[MarshalAs(UnmanagedType.LPStr)]string name, IntPtr options);

		[DllImport("librcl.so")]
		static extern int rcl_node_fini (ref rcl_node_t node);

		[DllImport("librcl.so")]
		static extern bool rcl_node_is_valid(ref rcl_node_t node);

		[DllImport("librcl.so")]
		static extern string rcl_node_get_name(ref rcl_node_t node);





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

