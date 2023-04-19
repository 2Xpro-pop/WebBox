using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBox.Ioc;
internal class LifeTimedServiceProvider : IDisposable, IServiceProvider
{
    private readonly ServiceDescriptors _descriptors = new();
    private readonly IEnumerable<ServiceDescriptor> _scopedService;
    private readonly Enum[] _scopes;

    public LifeTimedServiceProvider(ServiceDescriptors serviceDescriptors, params Enum[] scopes)
    {   
        _descriptors = serviceDescriptors;
        _scopedService = serviceDescriptors.Where(d => scopes.Any(s => s == d.Lifetime));
        _scopes = scopes;
    }
    
    public void Dispose()
    {
        foreach (var descriptor in _scopedService.Where(f => f.Lifetime.Equals(_scopes.Last())))
        {
            if (descriptor.Instance is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }

    public object? GetService(Type type)
    {
        var serviceDescriptor = _scopedService.FirstOrDefault(x => x.ServiceType == type) ?? 
            throw new Exception($"Service {type} not found");
        
        if (serviceDescriptor.Instance == null)
        {
            var instance = Activator.CreateInstance(serviceDescriptor.ImplementationType);

            if (instance is IService service)
            {
                service.ProvideService(this);
            }

            serviceDescriptor.Instance = instance;
        }

        return serviceDescriptor.Instance;
    }

    public IServiceProvider CreateSubScope(Enum scope)
    {
        return new LifeTimedServiceProvider(_descriptors, _scopes.Append(scope).ToArray());
    }
}
