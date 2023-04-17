using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebBox.Controller;
public class ControllerDescriptor
{
    public bool AsSingletone { get; set; } = false;
    public Type ControllerType { get; set; } = null!;
    public Action<object, HttpListenerRequest, HttpListenerResponse> ResultHandler { get; set; } 
}
