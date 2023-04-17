using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebBox;
using WebBox.Controller;

namespace System;
public static class WebBoxExtensions
{
    public static WebBoxedServer AddApiController<T>(this WebBoxedServer boxedServer) where T : ControllerBase
    {
        var descriptor = new ControllerDescriptor
        {
            ControllerType = typeof(T),
            ResultHandler = (result, request, response) =>
            {
                var json = JsonSerializer.Serialize(result);

                var buffer = Encoding.UTF8.GetBytes(json);
                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.Headers.Add("Content-Type", "application/json");
            }
        };
        
        boxedServer.Controllers.Add(descriptor);

        return boxedServer;
    }
}
