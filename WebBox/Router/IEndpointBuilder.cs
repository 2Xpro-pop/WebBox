using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebBox.Controller;

namespace WebBox.Router;
public interface IEndpointsBuilder
{
    void Map<T>(T t,string pattern, string method, Expression<Action> methodSelector) where T: ControllerBase;
}
