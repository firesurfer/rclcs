using System;
using System.Runtime.InteropServices;
namespace rclcs
{
	/// <summary>
	/// The RCL class handles the initialisation of the ros client librarie and wraps the functions defined in the rcl/rcl.h.
	/// It furthermore defines the paths to the rcl and rmw libs that are used in the DllImport statement for native interop.
	/// This class implements IDisposable.
	/// </summary>
	public class RCL:IDisposable
	{
		bool disposed = false;

		//Check if compiled on windows or linux, unfortunatly we can't do a runtime check for the import statements
		#if __MonoCS__
		#warning Compiling on linux: path is now: librcl.so
		//On linux the files start with lib and end with .so
		public const string LibRCLPath = "librcl.so";
		public const string LibRMWPath = "librmw.so";
		#else
		#warning Compiling on windows: path is now: rcl.dll
		//On windows they end with .dll
		public const string LibRCLPath = "rcl.dll";
		public const string LibRMWPath = "rmw.dll";
		#endif

		/// <summary>
		/// This method does the initilisation of the ros client lib.
		/// <remarks>Call this method before you do any other calls to ros</remarks>
		/// </summary>
		/// <param name="args">Commandline arguments</param>
		public void Init(String[] args)
		{
			RCLReturnValues retVal = (RCLReturnValues)rcl_init (args.Length, args, Allocator.rcl_get_default_allocator ());
			switch (retVal) {
			case RCLReturnValues.RCL_RET_OK:
				break;
			case RCLReturnValues.RCL_RET_ALREADY_INIT:
				throw new RCLAlreadyInitExcption ();
			case RCLReturnValues.RCL_RET_BAD_ALLOC:
				throw new RCLBadAllocException ();
			case RCLReturnValues.RCL_RET_ERROR:
				throw new RCLErrorException (RCLErrorHandling.GetRMWErrorState());
			default:
				break;
			}

		}
		/// <summary>
		/// This method does the initilisation of the ros client lib
		/// <remarks>Call this method before you do any other calls to ros
		/// You can specify a custom memory allocator for ros but I wouldn't recommend doing this at the moment. </remarks>
		/// </summary>
		/// <param name="args">Arguments.</param>
		/// <param name="custom_allocator">Custom allocator.</param>
		public void Init(String[] args, rcl_allocator_t custom_allocator)
		{
			if (args == null)
				throw new ArgumentNullException ();
			RCLReturnValues retVal = (RCLReturnValues)rcl_init (args.Length, args, custom_allocator);
			switch (retVal) {
			case RCLReturnValues.RCL_RET_OK:
				break;
			case RCLReturnValues.RCL_RET_ALREADY_INIT:
				throw new RCLAlreadyInitExcption ();
			case RCLReturnValues.RCL_RET_BAD_ALLOC:
				throw new RCLBadAllocException ();
			case RCLReturnValues.RCL_RET_ERROR:
				throw new RCLErrorException (RCLErrorHandling.GetRMWErrorState());
			default:
				break;
			}

		}

		
		/// <summary>
		/// Releases all resource used by the <see cref="rclcs.RCL"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="rclcs.RCL"/>. The <see cref="Dispose"/>
		/// method leaves the <see cref="rclcs.RCL"/> in an unusable state. After calling <see cref="Dispose"/>, you must
		/// release all references to the <see cref="rclcs.RCL"/> so the garbage collector can reclaim the memory that the
		/// <see cref="rclcs.RCL"/> was occupying.</remarks>
		public void Dispose()
		{
			// Dispose of unmanaged resources.
			Dispose(true);
			// Suppress finalization.
			GC.SuppressFinalize(this);
		}
		/// <summary>
		/// Implementation of IDisposable
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return;
			if (disposing) {
				
				// Free any other managed objects here.
			}
			// Free any unmanaged objects here.
			RCLReturnValues retVal = (RCLReturnValues)rcl_shutdown ();
			switch (retVal) {
			case RCLReturnValues.RCL_RET_OK:
				break;
			case RCLReturnValues.RCL_RET_NOT_INIT:
				//throw new RCLNotInitException ();
				break;
			case RCLReturnValues.RCL_RET_ERROR:
				//throw new RCLErrorException (RCLErrorHandling.GetRMWErrorState());
				break;
			default:
				break;
			}
			disposed = true;
		}
		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the <see cref="rclcs.RCL"/> is reclaimed
		/// by garbage collection.
		/// </summary>
		~RCL()
		{
			Dispose (false);
		}
		//Native methods

		[DllImport(LibRCLPath)]
		static extern int rcl_init(int argc, [In, Out] String[] argv, rcl_allocator_t allocator);

		[DllImport(LibRCLPath)]
		static extern int rcl_shutdown ();

		[DllImport(LibRCLPath)]
		static extern UInt64 rcl_get_instance_id ();

		[DllImport(LibRCLPath)]
		static extern bool rcl_ok ();


	}
	/// <summary>
	/// Managed implementation of the rcl_ret_t enum which specifies return values for the rcl functions.
	/// </summary>
	public enum RCLReturnValues
	{
		RCL_RET_OK = 0,
		RCL_RET_ERROR = 1,
		RCL_RET_TIMEOUT = 2,
		// rcl specific ret codes start at 100
		RCL_RET_ALREADY_INIT = 100,
		RCL_RET_NOT_INIT = 101,
		RCL_RET_BAD_ALLOC = 102,
		RCL_RET_INVALID_ARGUMENT = 103,
		RCL_REG_MISMATCHED_RMW_ID = 104,
		// rcl node specific ret codes in 2XX
		RCL_RET_NODE_INVALID = 200,
		// rcl publisher specific ret codes in 3XX
		RCL_RET_PUBLISHER_INVALID =  300,
		// rcl subscription specific ret codes in 4XX
		RCL_RET_SUBSCRIPTION_INVALID = 400,
		RCL_RET_SUBSCRIPTION_TAKE_FAILED = 401,
		// rcl service client specific ret codes in 5XX
		RCL_RET_CLIENT_INVALID = 500,
		RCL_RET_CLIENT_TAKE_FAILED = 501,
		// rcl service server specific ret codes in 6XX
		RCL_RET_SERVICE_INVALID =600,
		RCL_RET_SERIVCE_TAKE_FAILD = 601,
		// rcl guard condition specific ret codes in 7XX
		// rcl timer specific ret codes in 8XX
		RCL_RET_TIMER_INVALID = 800,
		RCL_RET_TIMER_CANCELED = 801,
		// rcl wait and wait set specific ret codes in 9XX
		RCL_RET_WAIT_SET_INVALID = 900,
		RCL_RET_WAIT_SET_EMPTY = 901,
		RCL_RET_WAIT_SET_FULL = 902
	}
			
}

