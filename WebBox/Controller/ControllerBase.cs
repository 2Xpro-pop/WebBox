using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebBox.Controller;
public abstract class ControllerBase
{
    public virtual HttpListenerResponse Response { get; set; } = null!;
    public virtual HttpListenerResponse Request { get; set; } = null!;
    public virtual void ProvideService(IServiceProvider serviceProvider){}
}
