using System;

namespace rclcs
{
	public class Message<T>:IDisposable
		where T: struct, IRosTransportItem
	{
		private bool disposed = false;
		public T Data = new T();

		/*public ref T Data
		{
			get{ return ref data; }
			set{ data = value; }
		}*/
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
				IRosTransportItem item = (IRosTransportItem)Data;
				item.Free ();
			}

			// Free any unmanaged objects here.
			//
			disposed = true;

	
		}

		~Message()
		{
			Dispose(false);
		}

	}
}

