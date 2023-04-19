using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebBox.Controller;
using WebBox.Ioc;

namespace WebBox;
public class WebBoxedServer
{
    private bool _isRunning = false;
    private CancellationToken _cancellation = default;
    
    private readonly HttpListener _listener;

    public WebBoxedServer(HttpListener? listener = null)
    {
        _listener = listener ?? new HttpListener();
        ServiceProvider = new WebBoxedServiceProvider(this);
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

    #endregion

    public virtual Task Start(CancellationToken cancellationToken = default)
    {
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

        
    }
}
