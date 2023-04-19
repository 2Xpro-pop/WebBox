using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBox.Ioc;
public class WebBoxedServiceProvider: IServiceProvider
{
    private readonly WebBoxedServer _webBoxed;
    public WebBoxedServiceProvider(WebBoxedServer webBoxed)
    {
        _webBoxed = webBoxed;
    }

    public object? GetService(Type type)
    {
        var serviceDescriptor = _webBoxed.Services.FirstOrDefault(x => x.ServiceType == type);
        
        if (serviceDescriptor == null)
        {
            throw new Exception($"Service {type} not found");
        }
        
        object? service = null;

        if (serviceDescriptor.Instance == null)
        {
            service = Activator.CreateInstance(serviceDescriptor.ImplementationType);

            if (service is IService providable)
            {
                providable.ProvideService(new RecursiveServiceProviderProtection(this, serviceDescriptor));
            }
        }

        if (serviceDescriptor.Lifetime.Equals(ServiceScope.Singleton))
        {
            serviceDescriptor.Instance = service;
        }

        return service;
    }

    public IServiceProvider CreateLifeTimedScope(Enum scope)
    {
        return new LifeTimedServiceProvider((ServiceDescriptors)_webBoxed.Services.Clone(), scope);
    }
}
