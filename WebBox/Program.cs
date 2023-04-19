using System.Net;
using WebBox;
using WebBox.Controller;
using WebBox.Router;

var listener = new HttpListener();
listener.Prefixes.Add("http://localhost:8089/");

var box = new WebBoxedServer(listener);
box.Controllers.AddApiController<ControllerTest>();
await box.Start();


class ControllerTest : ControllerBase
{
    public override void BuildRoutes(IEndpointsBuilder endpoints)
    {
        endpoints.Map(this, "/test", "GET", () => Test());
    }
    public string Test()
    {
        return "Hello World";
    }
}