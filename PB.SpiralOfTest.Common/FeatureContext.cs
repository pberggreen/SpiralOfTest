using System;
using System.Runtime.Remoting.Messaging;

namespace PB.SpiralOfTest.Common
{
    /// <summary>
    /// Example of an AmbientContext
    /// Source: http://blogs.msdn.com/b/ploeh/archive/2007/07/23/ambientcontext.aspx
    /// May be used to create a CallContext specific ServiceLocator
    /// </summary>
    [Serializable]
    public abstract class FeatureContext
    {
        private const string contextSlotName_ = "FeatureContext";

        protected FeatureContext()
        {
        }

        public static FeatureContext Current
        {
            get
            {
                FeatureContext ctx = CallContext.LogicalGetData(
                    FeatureContext.contextSlotName_) as FeatureContext;
                if (ctx == null)
                {
                    ctx = new BasicContext();
                    CallContext.LogicalSetData(
                        FeatureContext.contextSlotName_, ctx);
                }
                return ctx;
            }
            set
            {
                CallContext.LogicalSetData(
                    FeatureContext.contextSlotName_, value);
            }
        }

        //public abstract void Demand(FeatureLevel requestedLevel);
    }

    [Serializable]
    public class BasicContext : FeatureContext
    {
    }

    [Serializable]
    public class SilverContext : FeatureContext
    {
    }

    [Serializable]
    public class GoldContext : FeatureContext
    {
    }
}
