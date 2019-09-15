using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceSite
{
    public class MHHandler : IHttpModule
    {
        public void Init(HttpApplication application)
        {
            application.BeginRequest +=
            (new EventHandler(this.Application_BeginRequest));
            application.EndRequest +=
            (new EventHandler(this.Application_EndRequest));
        }

        /*
         * 对直接请求比如http://w.com/12.html .aspx等处理直接转html页面
         * 对资源.ico等不处理
         * 对http://w.com/act/login.do等走cshtml分发处理 act调用方法
         * 对http://w.com/actAj/login.aj走loginService ajax请求 actAj调用方法
         * 注意/Views/Home/Login.html 和Views/Home/Login.html区别
         */
        /// <summary>
        /// 页面处理,
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void Application_BeginRequest(Object source,EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;
            int p = context.Request.RawUrl.LastIndexOf('?');
            String baseUrl = (p > 0) ? context.Request.RawUrl.Substring(0, p) : context.Request.RawUrl;
            string filePath = context.Request.FilePath;
            string fileExtension = VirtualPathUtility.GetExtension(filePath);
            switch (fileExtension.ToLower())
            {
                case ".PHP":
                case ".php":
                    baseUrl = baseUrl.Replace(".php", ".aspx");
                    baseUrl = baseUrl.Replace(".PHP", ".aspx");
                    context.RewritePath(baseUrl);
                    break;
            }
        }

        private void Application_EndRequest(Object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;
            string filePath = context.Request.FilePath;
            string fileExtension =
            VirtualPathUtility.GetExtension(filePath);
            if(fileExtension.Equals(".html"))
            {
                context.Response.Write("<hr><h1><font color=red>" +
                "HelloWorldModule: End of Request</font></h1>");
            }
        }
        public void Dispose() { }
    } 
}