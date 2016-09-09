using System;

namespace rclcs
{
	public struct rmw_qos_profile_t
	{
		public rmw_qos_profile_t(rmw_qos_history_policy_t _history, Int64 _depth,rmw_qos_reliability_policy_t _reliability, rmw_qos_durability_policy_t _durability)
		{
			history = _history;
			reliability = _reliability;
			depth = (UIntPtr)_depth;
			durability = _durability;
		}
		public static rmw_qos_profile_t rmw_qos_profile_sensor_data
		{
			get{ return new rmw_qos_profile_t (rmw_qos_history_policy_t.RMW_QOS_POLICY_KEEP_LAST_HISTORY, 5, rmw_qos_reliability_policy_t.RMW_QOS_POLICY_BEST_EFFORT, rmw_qos_durability_policy_t.RMW_QOS_POLICY_DURABILITY_SYSTEM_DEFAULT); }
		}
		public static rmw_qos_profile_t rmw_qos_profile_parameters
		{
			get{ return new rmw_qos_profile_t (rmw_qos_history_policy_t.RMW_QOS_POLICY_KEEP_LAST_HISTORY, 1000, rmw_qos_reliability_policy_t.RMW_QOS_POLICY_RELIABLE, rmw_qos_durability_policy_t.RMW_QOS_POLICY_DURABILITY_SYSTEM_DEFAULT); }
		}
		public static rmw_qos_profile_t rmw_qos_profile_default
		{
			get{ return new rmw_qos_profile_t (rmw_qos_history_policy_t.RMW_QOS_POLICY_KEEP_ALL_HISTORY, 10, rmw_qos_reliability_policy_t.RMW_QOS_POLICY_RELIABLE, rmw_qos_durability_policy_t.RMW_QOS_POLICY_DURABILITY_SYSTEM_DEFAULT); }
		}
		public static rmw_qos_profile_t rmw_qos_profile_services_default
		{
			get{ return new rmw_qos_profile_t (rmw_qos_history_policy_t.RMW_QOS_POLICY_KEEP_LAST_HISTORY, 10, rmw_qos_reliability_policy_t.RMW_QOS_POLICY_RELIABLE, rmw_qos_durability_policy_t.RMW_QOS_POLICY_TRANSIENT_LOCAL_DURABILITY); }
		}
		public static rmw_qos_profile_t rmw_qos_profile_parameter_events
		{
			get{ return new rmw_qos_profile_t (rmw_qos_history_policy_t.RMW_QOS_POLICY_KEEP_LAST_HISTORY, 1000, rmw_qos_reliability_policy_t.RMW_QOS_POLICY_RELIABLE, rmw_qos_durability_policy_t.RMW_QOS_POLICY_DURABILITY_SYSTEM_DEFAULT); }
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

		public override string ToString ()
		{
			return "[rmw_qos_profile_t: history: " + history.ToString() + " depth: " + depth.ToString() + " reliability: " +reliability.ToString() + " durability: " + durability.ToString() + " ]" ;
		}
	}
	public enum rmw_qos_history_policy_t
	{
		RMW_QOS_POLICY_HISTORY_SYSTEM_DEFAULT,
		RMW_QOS_POLICY_KEEP_LAST_HISTORY,
		RMW_QOS_POLICY_KEEP_ALL_HISTORY
	}
	public enum rmw_qos_reliability_policy_t
	{
		RMW_QOS_POLICY_RELIABILITY_SYSTEM_DEFAULT,
		RMW_QOS_POLICY_RELIABLE,
		RMW_QOS_POLICY_BEST_EFFORT
	}
	public enum rmw_qos_durability_policy_t
	{
		RMW_QOS_POLICY_DURABILITY_SYSTEM_DEFAULT,
		RMW_QOS_POLICY_TRANSIENT_LOCAL_DURABILITY,
		RMW_QOS_POLICY_VOLATILE_DURABILITY
	}
}

