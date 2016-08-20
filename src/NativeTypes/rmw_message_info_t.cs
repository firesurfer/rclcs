using System;

namespace ROS2Sharp
{
	public struct rmw_message_info_t
	{
		rmw_gid_t publisher_gid;
		bool from_intra_process;
	}
}

