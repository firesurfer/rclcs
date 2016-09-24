using System;
using System.Threading;
using System.Collections.Generic;
namespace rclcs
{
	public class SingleThreadedExecutor:Executor
	{
		private  bool disposed = false;
		private bool AbortSpin = false;
		private Mutex SpinMutex = new Mutex();
		public SingleThreadedExecutor ()
		{
			SpinThread = new Thread(new ParameterizedThreadStart(InternalSpinMethod));
		}
		private Thread SpinThread;
		public  override void SpinOnce(System.TimeSpan Span)
		{
			
		}
		public  override void Spin(System.TimeSpan Intervall)
		{
			AbortSpin = false;
			SpinThread.Start (Intervall);
			Thread.Sleep (10);
		}
		public  override void SpinSome()
		{
			List<Node> ToRemove = new List<Node> ();
			foreach (var item in Nodes) {
				if (!item.IsDisposed ()) {
					item.Execute ();
				} else {
					ToRemove.Add (item);
				}
			}
			if (ToRemove.Count > 0) {
				foreach (var item in ToRemove) {
					RemoveNode (item);
				}
			}
		}
		public  override void Cancel()
		{
			
			AbortSpin = true;
			if(SpinThread.IsAlive)
				SpinThread.Join ();
			
		}

		private void InternalSpinMethod(object Intervall)
		{
			
			while (!AbortSpin) 
			{
				lock (SpinMutex) 
				{
					SpinSome ();
				}
				Thread.Sleep (((System.TimeSpan)Intervall).Milliseconds);

			}
		}
		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return; 

			if (disposing) {
				// Free any other managed objects here.
				//
			}
			//Stop the spin
			Cancel ();
			// Free any unmanaged objects here.
			//

			disposed = true;
			// Call base class implementation.
			base.Dispose(disposing);
		}
	}
}

