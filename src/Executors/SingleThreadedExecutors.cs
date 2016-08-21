using System;
using System.Threading;
namespace ROS2Sharp
{
	public class SingleThreadedExecutors:Executor
	{
		private bool AbortSpin = false;
		private Mutex SpinMutex = new Mutex();
		public SingleThreadedExecutors ()
		{
			SpinThread = new Thread(new ThreadStart(InternalSpinMethod));
		}
		private Thread SpinThread;
		public virtual void SpinOnce(System.TimeSpan Span)
		{
		}
		public virtual void Spin()
		{
			while (!AbortSpin) {
				
			}
		}
		public virtual void SpinSome()
		{
		}
		public virtual void Cancel()
		{
			AbortSpin = true;
		}

		private void InternalSpinMethod()
		{

		}
	}
}

