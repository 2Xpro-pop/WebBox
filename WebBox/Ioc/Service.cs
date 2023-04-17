using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBox.Ioc;
public interface IService
{
    public virtual void ProvideService(IServiceProvider serviceProvider){ }
}
