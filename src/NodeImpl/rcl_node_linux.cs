using System;
using System.Runtime.InteropServices;
namespace rclcs
{
	/// <summary>
	/// Wrapper for the native methods
	/// </summary>
	internal class rcl_node_linux:rcl_node_base
	{
		

		/// <summary>
		/// Initializes a new instance of the <see cref="rclcs.rcl_node"/> class.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="namespace_">Namespace.</param>
		public rcl_node_linux (string name, string namespace_ = ""):base(name,namespace_)
		{
			rcl_node_t node = rcl_get_zero_initialized_node ();
			rcl_node_options_t default_options = rcl_node_get_default_options ();
			int ret = rcl_node_init (ref node, name, namespace_, ref default_options);
			native_handle = node;
		}
		/// <summary>
		/// Releases all resource used by the <see cref="rclcs.rcl_node"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="rclcs.rcl_node"/>. The <see cref="Dispose"/>
		/// method leaves the <see cref="rclcs.rcl_node"/> in an unusable state. After calling <see cref="Dispose"/>, you must
		/// release all references to the <see cref="rclcs.rcl_node"/> so the garbage collector can reclaim the memory that
		/// the <see cref="rclcs.rcl_node"/> was occupying.</remarks>
		protected override void Dispose (bool disposing)
		{
			if (disposed)
				return; 

			if (disposing) {
				// Free any other managed objects here.
				//
			}
			if (rcl_node_is_valid (ref native_handle))
				rcl_node_fini (ref native_handle);
			// Free any unmanaged objects here.
			//
			disposed = true;
		}

		~rcl_node_linux ()
		{
			Dispose (false);
		}
		/// <summary>
		/// Gets the name of the node over the rcl interface
		/// </summary>
		/// <returns>The node name.</returns>
		public override string get_node_name ()
		{	
			IntPtr handle =  rcl_node_get_name (ref native_handle);
			return Marshal.PtrToStringAnsi (handle);
		}
		/// <summary>
		/// Checks if the stored native_handle is valid
		/// </summary>
		/// <returns><c>true</c>, if native_handle is valid, <c>false</c> otherwise.</returns>
		public override bool node_is_valid ()
		{
			//TODO bool should be marshalled ?
			return rcl_node_is_valid (ref native_handle);
		}


		[DllImport("librcl.so")]
		static extern rcl_node_t rcl_get_zero_initialized_node ();

		[DllImport("librcl.so")]
		static extern rcl_node_options_t rcl_node_get_default_options ();

		[DllImport("librcl.so")]
		static extern int rcl_node_get_domain_id( ref rcl_node_t  node, ref UIntPtr  domain_id);

		[DllImport("librcl.so")]
		static extern int rcl_node_init (ref rcl_node_t node, [MarshalAs (UnmanagedType.LPStr)]string name, [MarshalAs(UnmanagedType.LPStr)]string namespace_, ref rcl_node_options_t options);

		[DllImport("librcl.so")]
		static extern int rcl_node_fini (ref rcl_node_t node);

		[DllImport("librcl.so")]
		static extern bool rcl_node_is_valid (ref rcl_node_t node);

		[DllImport("librcl.so")]
		static extern IntPtr rcl_node_get_name (ref rcl_node_t node);





	}

}