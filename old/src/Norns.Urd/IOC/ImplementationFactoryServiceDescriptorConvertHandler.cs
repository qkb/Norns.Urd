﻿using Microsoft.Extensions.DependencyInjection;
using Norns.Urd.Extensions;
using Norns.Urd.Proxy;
using Norns.Urd.Utils;
using System.Reflection;

namespace Norns.Urd.IOC
{
    public class ImplementationFactoryServiceDescriptorConvertHandler : IServiceDescriptorConvertHandler
    {
        private readonly IProxyCreator creator;

        public ImplementationFactoryServiceDescriptorConvertHandler(IProxyCreator creator)
        {
            this.creator = creator;
        }

        public bool CanConvert(ServiceDescriptor descriptor)
        {
            return descriptor.ImplementationFactory != null
                || descriptor.ImplementationInstance != null
                || (descriptor.ImplementationType != null && (descriptor.ImplementationType.IsSealed || !descriptor.ImplementationType.GetTypeInfo().IsVisible()));
        }

        public ServiceDescriptor Convert(ServiceDescriptor descriptor)
        {
            var type = creator.CreateProxyType(descriptor.ServiceType, ProxyTypes.Facade);
            return ServiceDescriptor.Describe(descriptor.ServiceType, i =>
            {
                var proxy = ActivatorUtilities.CreateInstance(i, type);
                var f = proxy.GetType().GetField(ConstantInfo.Instance, BindingFlags.NonPublic | BindingFlags.Instance); // todo: 性能优化
                f.SetValue(proxy, descriptor.ImplementationFactory != null
                    ? descriptor.ImplementationFactory(i)
                    : (descriptor.ImplementationInstance ?? ActivatorUtilities.CreateInstance(i, descriptor.ImplementationType)));
                return proxy;
            }, descriptor.Lifetime);
        }
    }
}