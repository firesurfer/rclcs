using System;

namespace rclcs
{
	public class MessageWrapper<T>:IDisposable
		where T :struct
	{
		private bool disposed = false;
		protected T data;
		public MessageWrapper()
		{

		}
		public MessageWrapper(T init_data)
		{
			data = init_data;
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
				
			}
			disposed = true;
		}
		~MessageWrapper()
		{
			Dispose (false);
		}
		public virtual T Data
		{
			get{ return data; }
		}
	}
}

