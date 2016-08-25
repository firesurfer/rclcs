using System;

namespace ROS2Sharp
{
	public struct rmw_message_info_t
	{
		public rmw_gid_t publisher_gid;
		public bool from_intra_process;
	}
}

