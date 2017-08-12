using System;
namespace rclcs
{
    public abstract class rcl_error_handling_base:IDisposable
    {
        protected bool disposed = false;
        public rcl_error_handling_base()
        {
        }
        public abstract RMWErrorState get_rmw_error_state();
        public abstract bool is_error_set();

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				// Free any other managed objects here.
				//
			
			}
			//Clean up unmanaged resources

			// Free any unmanaged objects here.
			//
			disposed = true;
		}
        ~rcl_error_handling_base()
        {
            Dispose(false);
        }

    }
}
