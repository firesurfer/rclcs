using System;
using System.Runtime.InteropServices;
namespace rclcs
{
	
	public class MessageWrapper:IDisposable
	{
		private bool disposed = false;

		public MessageWrapper()
		{
			
		}
		public virtual void GetData(out ValueType _data)
		{
			//This is to suppress an compiler error due to the out keyword that requires the variable to be assigned inside the function
			_data = 0;
		}
		public virtual void SetData(ref ValueType _data)
		{

		}
		public virtual void SyncDataOut()
		{

		}
		public virtual void SyncDataIn()
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
				
			}
			disposed = true;
		}
		~MessageWrapper()
		{
			Dispose (false);
		}


	}
}

