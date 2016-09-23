using System;

namespace rclcs
{
	/// <summary>
	/// See rmw_message_info_t in the ros2 rmw package.
	/// The byte from_intra_process has to be interpreted as a bool.
	/// </summary>
	public struct rmw_message_info_t
	{
		public rmw_gid_t publisher_gid;
		public byte from_intra_process;
	}
}

