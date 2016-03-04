using System;
using System.Collections;
using System.Text;
using AspectInjector.Broker;
using Common.Logging;
using PB.SpiralOfTest.Infrastructure.ServiceLocator;

namespace PB.SpiralOfTest.Infrastructure.Aspects
{
    public class MethodLoggingAspect
    {
        [Advice(InjectionPoints.Before, InjectionTargets.Method)]
        public void BeforeMethod([AdviceArgument(AdviceArgumentSource.Type)] Type sourceType,
            [AdviceArgument(AdviceArgumentSource.TargetName)] string methodName,
            [AdviceArgument(AdviceArgumentSource.TargetArguments)] object[] arguments)
        {
            var logger = IoC.Resolve<ILogManager>().GetLogger(sourceType);
            if (logger.IsDebugEnabled)
            {
                logger.Debug($"Calling {methodName}({ToJson(arguments)})");
            }
        }

        [Advice(InjectionPoints.After, InjectionTargets.Method)]
        public void AfterMethod([AdviceArgument(AdviceArgumentSource.Type)] Type sourceType,
            [AdviceArgument(AdviceArgumentSource.TargetName)] string methodName,
            [AdviceArgument(AdviceArgumentSource.TargetValue)] object returnValue)
        {
            var logger = IoC.Resolve<ILogManager>().GetLogger(sourceType);
            if (logger.IsDebugEnabled)
            {
                logger.Debug($"Return value from {methodName}: {ToJson(returnValue)}");
            }
        }

        [Advice(InjectionPoints.Exception, InjectionTargets.Method)]
        public void MethodException([AdviceArgument(AdviceArgumentSource.Type)] Type sourceType,
            [AdviceArgument(AdviceArgumentSource.TargetName)] string methodName,
            [AdviceArgument(AdviceArgumentSource.TargetArguments)] object[] arguments,
            [AdviceArgument(AdviceArgumentSource.TargetException)] Exception ex)
        {
            var logger = IoC.Resolve<ILogManager>().GetLogger(sourceType);
            if (logger.IsWarnEnabled)
            {
                logger.Warn($"Error calling {methodName}({ToJson(arguments)})", ex);
            }
        }

        private static string ToJson(IEnumerable args)
        {
            var str = new StringBuilder();
            var first = true;
            foreach (var argument in args)
            {
                if (!first) str.Append(",");
                first = false;
                str.Append(ToJson(argument));
            }
            return str.ToString();
        }

        private static string ToJson(object argument)
        {
            var str = new StringBuilder();
            try
            {
                var argType = argument.GetType();

                if (argType == typeof(string) || argType.IsPrimitive)
                {
                    str.Append(argument);
                }
                else if (argument is IEnumerable)
                {
                    str.Append("[");
                    str.Append(ToJson(argument as IEnumerable));
                    //foreach (var arg in argument as IEnumerable)
                    //    str.Append(GetValue(arg));
                    str.Append("]");
                }
                else
                {
                    str.Append("{");
                    var first = true;
                    foreach (var property in argType.GetProperties())
                    {
                        if (!first) str.Append(",");
                        first = false;
                        str.AppendFormat("{0}:'{1}'",
                            property.Name, property.GetValue(argument, null));
                    }
                    str.Append("}");
                }
            }
            catch (Exception)
            {
                //Ignore exceptions so that the logging aspect doesn't cause errors in the real code
            }
            return str.ToString();
        }
    }
}
