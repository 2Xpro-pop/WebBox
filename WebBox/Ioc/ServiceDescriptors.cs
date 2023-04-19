using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBox.Ioc;
public class ServiceDescriptors : List<ServiceDescriptor>, ICloneable
{
    public object Clone()
    {
        var clone = new ServiceDescriptors();
        foreach (var serviceDescriptor in this)
        {
            clone.Add((ServiceDescriptor)serviceDescriptor.Clone());
        }

        return clone;
    }
}
}
