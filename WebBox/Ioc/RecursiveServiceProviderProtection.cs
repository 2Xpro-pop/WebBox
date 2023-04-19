using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBox.Ioc;
internal class RecursiveServiceProviderProtection : IServiceProvider
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ServiceDescriptor[] _descriptors;

    public RecursiveServiceProviderProtection(IServiceProvider serviceProvider, params ServiceDescriptor[] descriptors)
    {
        _serviceProvider = serviceProvider;
        _descriptors = descriptors;
    }
    public object? GetService(Type serviceType)
    {
        if (_descriptors.Any(d => d.ServiceType == serviceType))
        {
            throw new Exception($"Recursive dependency detected for {serviceType}");
        }

        return _serviceProvider.GetService(serviceType);
    }
}
