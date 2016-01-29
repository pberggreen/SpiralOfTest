namespace PB.SpiralOfTest.Infrastructure.Service
{
    public class ServiceBusServiceBehaviorAttribute : CustomServiceBehaviorAttribute
    {
        public ServiceBusServiceBehaviorAttribute()
        {
            OperationBehavior = typeof(ServiceBusOperationBehaviorAttribute);
        }
    }
}
