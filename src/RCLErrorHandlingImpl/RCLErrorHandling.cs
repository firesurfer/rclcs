﻿using System;
using System.Runtime.InteropServices;
namespace rclcs
{
	/// <summary>
	/// Implements error handling for the rcl and rmw (ros middle ware)
	/// </summary>
    public class RCLErrorHandling:IDisposable
	{
		bool disposed = false;
        private static volatile RCLErrorHandling instance;
        private static object syncRoot = new object();
        public static RCLErrorHandling Instance
        {
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
                            instance = new RCLErrorHandling();
					}
				}

				return instance;
			}
        }

        private  rcl_error_handling_base Impl;
        private RCLErrorHandling()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                //TODO codepath for windows
            }
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                Impl = new rcl_error_handling_linux();
            }
            else
            {
                throw new Exception("Operating system: " + Environment.OSVersion.Platform.ToString() + " not supported");
            }
        }
        ~RCLErrorHandling()
        {
            Dispose(false);
        }
		/// <summary>
		/// Checks if the error flag is set in the rmw
		/// </summary>
		/// <returns><c>true</c> if is error set; otherwise, <c>false</c>.</returns>
		public  bool IsErrorSet()
		{
            return Impl.is_error_set();
		}
		/// <summary>
		/// Gets the rmw error state which basically contains a message ,a line and a file
		/// </summary>
		/// <returns>The RMW error state.</returns>
		public  RMWErrorState GetRMWErrorState()
		{
            return Impl.get_rmw_error_state();
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

			if (disposing)
			{
                Impl.Dispose();
				// Free any other managed objects here.
				//

			}
			//Clean up unmanaged resources

			// Free any unmanaged objects here.
			//
			disposed = true;
		}
	}
	
	/// <summary>
	/// Managed wrapper for the rmw_error_state_t struct that does the marshalling for the char pointers.
	/// </summary>
	public class RMWErrorState
	{
		public string Message{ get; private set; }
		public string File { get; private set; }
		public UInt64 LineNumber { get; private set; }
		public RMWErrorState(string message, string file, UInt64 line_number)
		{
			Message = message;
			File = file;
			LineNumber = line_number;
		}
		public RMWErrorState(rmw_error_state_t state)
		{
			Message = Marshal.PtrToStringAnsi(state.message);
			File = Marshal.PtrToStringAnsi(state.file);
			LineNumber = (UInt64)state.line_number;
		}
		public override string ToString ()
		{
			return string.Format ("[RMWErrorState: Message={0}, File={1}, LineNumber={2}]", Message, File, LineNumber);
		}
	}
}

