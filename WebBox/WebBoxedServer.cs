using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebBox.Controller;
using WebBox.Ioc;
using WebBox.Router;

namespace WebBox;
public class WebBoxedServer
{
    protected bool _isRunning = false;
    protected CancellationToken _cancellation = default;
    
    protected readonly HttpListener _listener;

    public WebBoxedServer(HttpListener? listener = null)
    {
        _listener = listener ?? new HttpListener();
        ServiceProvider = new WebBoxedServiceProvider(this);
        Route = new RouteCollection(this);
    }

    #region Options

    public virtual ControllerDesctiptors Controllers
    {
        get;
    } = new();

    public virtual ServiceDescriptors Services
    {
        get;
    } = new();

    public virtual WebBoxedServiceProvider ServiceProvider
    {
        get;
    }

    public RouteCollection Route
    {
        get;
    }

    #endregion

    public virtual Task Start(CancellationToken cancellationToken = default)
    {
        
        foreach (var controller in Controllers)
        {
            Route.AddRoutes(controller);
        }
        
        _isRunning = true;
        _listener.Start();

        return HandleConnection(cancellationToken);
    }

    public virtual void Stop()
    {
        _isRunning = false;
    }

    protected async virtual Task HandleConnection(CancellationToken cancellationToken = default)
    {
        while (!_cancellation.IsCancellationRequested && _isRunning)
        {
            var ctx = await _listener.GetContextAsync();

            await Middleware(ctx);
        }

        _listener.Stop();
    }

    protected async virtual Task Middleware(HttpListenerContext context)
    {
        var req = context.Request;
        var resp = context.Response;

        var scopedServices = ServiceProvider.CreateLifeTimedScope(ServiceScope.Request);

        foreach(var controller in Controllers)
        {
            controller.Instance.ProvideService(scopedServices);
        }

        var url = req.Url;
        var route = url.PathAndQuery + url.Fragment;

        var routeInfo = Route.MapRoute(Controllers,route);

        if (routeInfo != null)
        {
            var result = routeInfo.Method();
            if (result != null)
            {
                routeInfo.Controller.ResultHandler(result, req, resp);
            }
            else
            {
                resp.StatusCode = 200;
            }
        }
    }
}
