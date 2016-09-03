using System;

namespace rclcs
{
	public class RCLErrorException:Exception
	{
		public RCLReturnValues RCLReturnValue{ get; private set;}
		public RMWErrorState ErrorState{ get; private set;}

		public RCLErrorException()
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_ERROR;
		}
		public RCLErrorException(RMWErrorState _ErrorState):this()
		{
			ErrorState = _ErrorState;

		}
		public RCLErrorException(string message):base(message)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_ERROR;
		}
		public RCLErrorException(string message,Exception inner):base(message,inner)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_ERROR;
		}
	}
	public class RCLTimeoutException:Exception
	{
		public RCLReturnValues RCLReturnValue{ get; private set;}
		public RCLTimeoutException()
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_TIMEOUT;
		}
		public RCLTimeoutException(string message):base(message)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_TIMEOUT;
		}
		public RCLTimeoutException(string message,Exception inner):base(message,inner)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_TIMEOUT;
		}
	}
	public class RCLNotInitException:Exception
	{
		public RCLReturnValues RCLReturnValue{ get; private set;}
		public RCLNotInitException()
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_NOT_INIT;
		}
		public RCLNotInitException(string message):base(message)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_NOT_INIT;
		}
		public RCLNotInitException(string message,Exception inner):base(message,inner)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_NOT_INIT;
		}
	}
	public class RCLBadAllocException:Exception
	{
		public RCLReturnValues RCLReturnValue{ get; private set;}
		public RCLBadAllocException()
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_BAD_ALLOC;
		}
		public RCLBadAllocException(string message):base(message)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_BAD_ALLOC;
		}
		public RCLBadAllocException(string message,Exception inner):base(message,inner)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_BAD_ALLOC;
		}
	}
	public class RCLInvalidArgumentException:Exception
	{
		public RCLReturnValues RCLReturnValue{ get; private set;}
		public RCLInvalidArgumentException()
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_INVALID_ARGUMENT;
		}
		public RCLInvalidArgumentException(string message):base(message)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_INVALID_ARGUMENT;
		}
		public RCLInvalidArgumentException(string message,Exception inner):base(message,inner)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_INVALID_ARGUMENT;
		}
	}
	public class RCLPublisherInvalidException:Exception
	{
		public RCLReturnValues RCLReturnValue{ get; private set;}
		public RCLPublisherInvalidException()
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_PUBLISHER_INVALID;
		}
		public RCLPublisherInvalidException(string message):base(message)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_PUBLISHER_INVALID;
		}
		public RCLPublisherInvalidException(string message,Exception inner):base(message,inner)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_PUBLISHER_INVALID;
		}
	}
	public class RCLSubscriptionInvalidException:Exception
	{
		public RCLReturnValues RCLReturnValue{ get; private set;}
		public RCLSubscriptionInvalidException()
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_SUBSCRIPTION_INVALID;
		}
		public RCLSubscriptionInvalidException(string message):base(message)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_SUBSCRIPTION_INVALID;
		}
		public RCLSubscriptionInvalidException(string message,Exception inner):base(message,inner)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_SUBSCRIPTION_INVALID;
		}
	}
	public class RCLSubscriptonTakeFailedException:Exception
	{
		public RCLReturnValues RCLReturnValue{ get; private set;}
		public RCLSubscriptonTakeFailedException()
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_SUBSCRIPTION_TAKE_FAILED;
		}
		public RCLSubscriptonTakeFailedException(string message):base(message)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_SUBSCRIPTION_TAKE_FAILED;
		}
		public RCLSubscriptonTakeFailedException(string message,Exception inner):base(message,inner)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_SUBSCRIPTION_TAKE_FAILED;
		}
	}
	public class RCLServiceInvalidException:Exception
	{
		public RCLReturnValues RCLReturnValue{ get; private set;}
		public RCLServiceInvalidException()
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_SERVICE_INVALID;
		}
		public RCLServiceInvalidException(string message):base(message)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_SERVICE_INVALID;
		}
		public RCLServiceInvalidException(string message,Exception inner):base(message,inner)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_SERVICE_INVALID;
		}
	}
	public class RCLClientInvalidException:Exception
	{
		public RCLReturnValues RCLReturnValue{ get; private set;}
		public RCLClientInvalidException()
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_CLIENT_INVALID;
		}
		public RCLClientInvalidException(string message):base(message)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_CLIENT_INVALID;
		}
		public RCLClientInvalidException(string message,Exception inner):base(message,inner)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_CLIENT_INVALID;
		}
	}
	public class RCLNodeInvalidException:Exception
	{
		public RCLReturnValues RCLReturnValue{ get; private set;}
		public RCLNodeInvalidException()
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_NODE_INVALID;
		}
		public RCLNodeInvalidException(string message):base(message)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_NODE_INVALID;
		}
		public RCLNodeInvalidException(string message,Exception inner):base(message,inner)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_NODE_INVALID;
		}
	}
	public class RCLAlreadyInitExcption:Exception
	{
		public RCLReturnValues RCLReturnValue{ get; private set;}
		public RCLAlreadyInitExcption()
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_ALREADY_INIT;
		}
		public RCLAlreadyInitExcption(string message):base(message)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_ALREADY_INIT;
		}
		public RCLAlreadyInitExcption(string message,Exception inner):base(message,inner)
		{
			RCLReturnValue = RCLReturnValues.RCL_RET_ALREADY_INIT;
		}
	}
}

