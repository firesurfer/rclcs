using System;
using System.Runtime.InteropServices;
namespace rclcs
{
    public class rcl_error_handling_linux:rcl_error_handling_base
    {
        public rcl_error_handling_linux()
        {
        }

		//TODO Do I have to call the corresponding functions in the rcl ?
		[DllImport("librmw.so")]
		extern static bool rmw_error_is_set();

		[DllImport("librmw.so")]
		[return: MarshalAs(UnmanagedType.LPStruct)]
		extern static rmw_error_state_t rmw_get_error_state();

		[DllImport("librmw.so")]
		extern static IntPtr rmw_get_error_string();

		[DllImport("librmw.so")]
		extern static IntPtr rmw_get_error_string_safe();

		[DllImport("librmw.so")]
		extern static void rmw_reset_error();

        public override  RMWErrorState get_rmw_error_state()
        {
            if (is_error_set())
			{
				return new RMWErrorState(rmw_get_error_state());
			}
			else
				return null;
        }
		public  override bool is_error_set()
        {
            return rmw_error_is_set();
        }
        protected override void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
                // Free any other managed objects here.

              

			}
			rmw_reset_error();
			//Clean up unmanaged resources

			// Free any unmanaged objects here.
			//
			disposed = true;
		}
        ~rcl_error_handling_linux()
        {
            Dispose(false);
        }

	}
}
