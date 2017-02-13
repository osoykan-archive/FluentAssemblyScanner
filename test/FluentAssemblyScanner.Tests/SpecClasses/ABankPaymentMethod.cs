namespace FluentAssemblyScanner.Tests.SpecClasses
{
    public class ABankPaymentMethod : IPaymentMethod
    {
        public void PayMe()
        {
        }
    }

    namespace subPaymentMethodNamespace
    {
        public class DBankPaymentMethod : IPaymentMethod
        {
            public void PayMe()
            {
            }
        }
    }
}
