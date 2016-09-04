using System;

namespace rclcs
{
	public class Executable:IDisposable
	{
		bool disposed = false;
		public Executable ()
		{
		}
		public virtual void Execute()
		{
		}
		public void Dispose()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
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
		public virtual bool IsDisposed()
		{
			return disposed;
		}
		~Executable()
		{
			Dispose (false);
		}
	}
}

