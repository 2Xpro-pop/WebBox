using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebBox.Controller;
public class ControllerDescriptor
{
    public string Path { get; set; } = null!;
    
    private Type controllerType = null!;
    public Type ControllerType
    {
        get => controllerType; 
        set => controllerType = value.IsAssignableFrom(typeof(ControllerBase)) ? value : throw new NotSupportedException();
    }
    
    public Action<object, HttpListenerRequest, HttpListenerResponse> ResultHandler
    {
        get; set;
    }
    public ControllerBase Instance { get; set; }
}
