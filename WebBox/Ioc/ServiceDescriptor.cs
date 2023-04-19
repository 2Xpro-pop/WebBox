using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBox.Ioc;
public class ServiceDescriptor: ICloneable
{
    public ServiceDescriptor(Type serviceType, Type implementationType, Enum lifetime)
    {
        ServiceType = serviceType;
        ImplementationType = implementationType;
        Lifetime = lifetime;

    }

    public Type ServiceType
    {
        get;
    }
    public Type ImplementationType
    {
        get;
    }
    public Enum Lifetime
    {
        get;
    }

    public object? Instance { get; set; }

    public object Clone() => Lifetime.Equals(ServiceScope.Singleton) ? this : MemberwiseClone();
}
