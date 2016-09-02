using System;

namespace rclcs
{
	public struct rmw_qos_profile_t
	{
		int history;
		UIntPtr depth;
		int reliability;
		int durability;
	}
}

