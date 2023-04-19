using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBox.Controller;

namespace WebBox.Router;
public record RouteInfo(ControllerDescriptor Controller, Func<object> Method);
