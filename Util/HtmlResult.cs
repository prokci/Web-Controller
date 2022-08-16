﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControllersApp.Util
{
    public class HtmlResult : IActionResult
    {
        string htmlCode;
        public HtmlResult(string html)
        {
            htmlCode = html;
        }
        public async Task ExecuteResultAsync(ActionContext context)
        {
            string fullHtmlCode = "<!DOCTYPE html><html><head>";
            fullHtmlCode += "<title>Главная страница</title>";
            fullHtmlCode += "<meta charset=utf-8 />";
            fullHtmlCode += "</head> <body>";
            fullHtmlCode += htmlCode;
            fullHtmlCode += "</body></html>";
            await context.HttpContext.Response.WriteAsync(fullHtmlCode);
        }
    }
}
