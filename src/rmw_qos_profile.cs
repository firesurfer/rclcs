using System;

namespace ROS2Sharp
{
	public struct rmw_qos_profile_t
	{
		int history;
		UIntPtr depth;
		int reliability;
		int durability;
	}
}

