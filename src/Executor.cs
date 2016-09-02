using System;
using System.Collections.Concurrent;
using System.Threading;
namespace rclcs
{
	public class Executor
	{
		protected ConcurrentBag<Node> Nodes = new ConcurrentBag<Node> ();
	
		public Executor ()
		{
		}
		public virtual void AddNode(Node _node)
		{
			Nodes.Add (_node);
		}
		public virtual void RemoveNode(Node _node)
		{
			foreach (var item in Nodes) {
				if (item.Name == _node.Name) {
					Node tempNode;
					while (Nodes.TryTake (out tempNode)) {
						Thread.Sleep (10);
					}
				}
			}
		}
		public virtual void SpinOnce(System.TimeSpan Span)
		{
			
		}
		public virtual void Spin(System.TimeSpan Intervall)
		{
		}
		public virtual void SpinSome()
		{
		}
		public virtual void Cancel()
		{
		}
	}
}

