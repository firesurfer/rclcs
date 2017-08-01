using System;

namespace rclcs
{
	/// <summary>
	/// Managed implementation of the rmw_qos_profile_t defined in the rmw ros2 package.
	/// It furthermores implementes all default/preset qos profiles that are defined in ros2.
	/// </summary>
	public struct rmw_qos_profile_t
	{
		public rmw_qos_profile_t(rmw_qos_history_policy_t _history, Int64 _depth,rmw_qos_reliability_policy_t _reliability, rmw_qos_durability_policy_t _durability)
		{
			history = _history;
			reliability = _reliability;
			depth = (UIntPtr)_depth;
			durability = _durability;
			avoid_ros_namespace_conventions = 0;
		}
		public static rmw_qos_profile_t rmw_qos_profile_sensor_data
		{
			get{ return new rmw_qos_profile_t (rmw_qos_history_policy_t.RMW_QOS_POLICY_HISTORY_KEEP_LAST, 5, rmw_qos_reliability_policy_t.RMW_QOS_POLICY_RELIABILITY_BEST_EFFORT, rmw_qos_durability_policy_t.RMW_QOS_POLICY_DURABILITY_SYSTEM_DEFAULT); }
		}
		public static rmw_qos_profile_t rmw_qos_profile_parameters
		{
			get{ return new rmw_qos_profile_t (rmw_qos_history_policy_t.RMW_QOS_POLICY_HISTORY_KEEP_LAST, 1000, rmw_qos_reliability_policy_t.RMW_QOS_POLICY_RELIABILITY_RELIABLE, rmw_qos_durability_policy_t.RMW_QOS_POLICY_DURABILITY_SYSTEM_DEFAULT); }
		}
		public static rmw_qos_profile_t rmw_qos_profile_default
		{
			get{ return new rmw_qos_profile_t (rmw_qos_history_policy_t.RMW_QOS_POLICY_HISTORY_KEEP_ALL, 10, rmw_qos_reliability_policy_t.RMW_QOS_POLICY_RELIABILITY_RELIABLE, rmw_qos_durability_policy_t.RMW_QOS_POLICY_DURABILITY_SYSTEM_DEFAULT); }
		}
		public static rmw_qos_profile_t rmw_qos_profile_services_default
		{
			get{ return new rmw_qos_profile_t (rmw_qos_history_policy_t.RMW_QOS_POLICY_HISTORY_KEEP_LAST, 10, rmw_qos_reliability_policy_t.RMW_QOS_POLICY_RELIABILITY_RELIABLE, rmw_qos_durability_policy_t.RMW_QOS_POLICY_DURABILITY_TRANSIENT_LOCAL); }
		}
		public static rmw_qos_profile_t rmw_qos_profile_parameter_events
		{
			get{ return new rmw_qos_profile_t (rmw_qos_history_policy_t.RMW_QOS_POLICY_HISTORY_KEEP_LAST, 1000, rmw_qos_reliability_policy_t.RMW_QOS_POLICY_RELIABILITY_RELIABLE, rmw_qos_durability_policy_t.RMW_QOS_POLICY_DURABILITY_SYSTEM_DEFAULT); }
		}
		public static rmw_qos_profile_t rmw_qos_profile_system_default
		{
			//TODO does this have the right depth value?
			get{ return new rmw_qos_profile_t (rmw_qos_history_policy_t.RMW_QOS_POLICY_HISTORY_SYSTEM_DEFAULT, 0, rmw_qos_reliability_policy_t.RMW_QOS_POLICY_RELIABILITY_SYSTEM_DEFAULT, rmw_qos_durability_policy_t.RMW_QOS_POLICY_DURABILITY_SYSTEM_DEFAULT); }
		}
		rmw_qos_history_policy_t history;
		UIntPtr depth;
		rmw_qos_reliability_policy_t reliability;
		rmw_qos_durability_policy_t durability;
		byte avoid_ros_namespace_conventions;

		public override string ToString ()
		{
			return "[rmw_qos_profile_t: history: " + history.ToString() + " depth: " + depth.ToString() + " reliability: " +reliability.ToString() + " durability: " + durability.ToString() + " ]" ;
		}
	}
	/// <summary>
	/// See rmw package for more documentation
	/// </summary>
	public enum rmw_qos_history_policy_t
	{
		RMW_QOS_POLICY_HISTORY_SYSTEM_DEFAULT,
		RMW_QOS_POLICY_HISTORY_KEEP_LAST,
		RMW_QOS_POLICY_HISTORY_KEEP_ALL
	}
	/// <summary>
	///  See rmw package for more documentation
	/// </summary>
	public enum rmw_qos_reliability_policy_t
	{
		RMW_QOS_POLICY_RELIABILITY_SYSTEM_DEFAULT,
		RMW_QOS_POLICY_RELIABILITY_RELIABLE,
		RMW_QOS_POLICY_RELIABILITY_BEST_EFFORT
	}
	/// <summary>
	///  See rmw package for more documentation
	/// </summary>
	public enum rmw_qos_durability_policy_t
	{
		RMW_QOS_POLICY_DURABILITY_SYSTEM_DEFAULT,
		RMW_QOS_POLICY_DURABILITY_TRANSIENT_LOCAL,
		RMW_QOS_POLICY_DURABILITY_VOLATILE
	}
}

