using System;
using System.Collections.Concurrent;
using System.Threading;
namespace rclcs
{
	public abstract class Executor:IDisposable
	{
		//TODO allow any executables
		protected ConcurrentBag<Node> Nodes = new ConcurrentBag<Node> ();
		bool disposed = false;

		public Executor ()
		{
		}
		~Executor()
		{
			Dispose (false);
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
		//TODO add a datatype that represents a intervall in Hertz
		public virtual void Spin(System.TimeSpan Intervall)
		{
		}
		public virtual void SpinSome()
		{
		}
		public virtual void Cancel()
		{
			
		}
		public void Dispose()
		{ 
			Dispose(true);
			GC.SuppressFinalize(this);           
		}
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return; 

			if (disposing) {
				
				// Free any other managed objects here.
				//
			}

			// Free any unmanaged objects here.
			//
			disposed = true;
		}
	}
}

