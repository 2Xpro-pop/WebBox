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
    private readonly HttpListener _listener;

    public WebBoxedServer(HttpListener? listener = null)
    {
        _listener = listener ?? new HttpListener();
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

    #endregion

    public virtual void Start()
    {
        _listener.Start();
    }
}
