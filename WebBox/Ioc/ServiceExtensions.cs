using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBox.Ioc;

namespace System;
public static class ServiceExtensions
{
    public static T GetService<T>(this IServiceProvider provider, Type serviceType)
    {
        return (T)provider.GetService(serviceType);
    }
}
