using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;

namespace PB.SpiralOfTest.Common
{
    /// <summary>
    /// Wrapper of the IoC container
    /// ref: http://stackoverflow.com/questions/871405/why-do-i-need-an-ioc-container-as-opposed-to-straightforward-di-code
    /// For a more advanced solution using MEF to register dependencies: 
    /// http://www.developer.com/net/dependency-injection-best-practices-in-an-n-tier-modular-application.html
    /// AUtomatic registration of types: 
    /// https://msdn.microsoft.com/en-us/library/dn507479(v=pandp.30).aspx
    /// </summary>
    public static class IoC
    {
        private static LightInject.ServiceContainer _container;

        static IoC()
        {
            InitializeIoC();
        }

        static void InitializeIoC()
        {
            _container = new LightInject.ServiceContainer();

            //_container.RegisterTypes(
            //    AllClasses.FromLoadedAssemblies(),
            //    WithMappings.FromMatchingInterface,
            //    WithName.Default,
            //    WithLifetime.ContainerControlled);

        }

        /// <summary>
        /// Register a type mapping
        /// </summary>
        public static void RegisterType<TFrom, TTo>() where TTo : TFrom
        {
            _container.Register<TFrom, TTo>();
        }

        /// <summary>
        /// Register a named type mapping
        /// </summary>
        public static void RegisterType<TFrom, TTo>(string name) where TTo : TFrom
        {
            _container.Register<TFrom, TTo>(name);
        }

        /// <summary>
        /// Register an instance of a type
        /// </summary>
        public static void RegisterInstance<T>(object instance)
        {
            _container.RegisterInstance(typeof(T), instance);
        }

        /// <summary>
        /// Resolve an instance of a type
        /// </summary>
        public static T Resolve<T>()
        {
            return _container.GetInstance<T>();
        }

        /// <summary>
        /// Resolve a named instance of a type
        /// </summary>
        public static T Resolve<T>(string name)
        {
            return _container.GetInstance<T>(name);
        }

        public static Type ResolveType<T>()
        {
            var instance = _container.GetInstance<T>();
            return instance.GetType();
        }

        public static bool IsInstanceRegistered<T>()
        {
            var instance = _container.TryGetInstance<T>();
            if (instance == null)
                return false;

            return instance.GetType() != typeof (T);
        }

        public static bool IsRegistered<T>()
        {
            return _container.CanGetInstance(typeof(T), "");
        }

    }
}

