namespace PB.SpiralOfTest.Infrastructure.Service
{
    public class IntranetServiceBehaviorAttribute : CustomServiceBehaviorAttribute
    {
        public IntranetServiceBehaviorAttribute()
        {
            OperationBehavior = typeof(IntranetOperationBehaviorAttribute);
        }
    }
}
