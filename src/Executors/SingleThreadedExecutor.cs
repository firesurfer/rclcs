using System;
using System.Threading;
namespace ROS2Sharp
{
	public class SingleThreadedExecutor:Executor
	{
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
			foreach (var item in Nodes) {
				item.Execute ();
			}
		}
		public  override void Cancel()
		{
			AbortSpin = true;
		}

		private void InternalSpinMethod(object Intervall)
		{
			
			while (!AbortSpin) {
				SpinSome ();
				Thread.Sleep (((System.TimeSpan)Intervall).Milliseconds);

			}
		}
	}
}

