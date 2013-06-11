﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18046
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StackExchange.Exceptional.Pages
{
    
    #line 2 "..\..\Pages\ErrorInfo.cshtml"
    using System;
    
    #line default
    #line hidden
    
    #line 3 "..\..\Pages\ErrorInfo.cshtml"
    using System.Collections.Generic;
    
    #line default
    #line hidden
    
    #line 4 "..\..\Pages\ErrorInfo.cshtml"
    using System.Collections.Specialized;
    
    #line default
    #line hidden
    
    #line 5 "..\..\Pages\ErrorInfo.cshtml"
    using System.Linq;
    
    #line default
    #line hidden
    
    #line 6 "..\..\Pages\ErrorInfo.cshtml"
    using System.Text;
    
    #line default
    #line hidden
    
    #line 7 "..\..\Pages\ErrorInfo.cshtml"
    using System.Text.RegularExpressions;
    
    #line default
    #line hidden
    
    #line 8 "..\..\Pages\ErrorInfo.cshtml"
    using System.Web;
    
    #line default
    #line hidden
    
    #line 9 "..\..\Pages\ErrorInfo.cshtml"
    using StackExchange.Exceptional;
    
    #line default
    #line hidden
    
    #line 11 "..\..\Pages\ErrorInfo.cshtml"
    using StackExchange.Exceptional.Extensions;
    
    #line default
    #line hidden
    
    #line 10 "..\..\Pages\ErrorInfo.cshtml"
    using StackExchange.Exceptional.Pages;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    internal partial class ErrorInfo : RazorPageBase
    {
#line hidden

        #line 19 "..\..\Pages\ErrorInfo.cshtml"

    public Guid Guid { get; set; }
    private static HashSet<string> hiddenHttpKeys = new HashSet<string>
                                                        {
                                                            "ALL_HTTP",
                                                            "ALL_RAW",
                                                            "HTTP_CONTENT_LENGTH",
                                                            "HTTP_CONTENT_TYPE",
                                                            "HTTP_COOKIE",
                                                            "QUERY_STRING"
                                                        };

    private static HashSet<string> defaultHttpKeys = new HashSet<string>
                                                     {
                                                         "APPL_MD_PATH",
                                                         "APPL_PHYSICAL_PATH",
                                                         "GATEWAY_INTERFACE",
                                                         "HTTP_ACCEPT",
                                                         "HTTP_ACCEPT_CHARSET",
                                                         "HTTP_ACCEPT_ENCODING",
                                                         "HTTP_ACCEPT_LANGUAGE",
                                                         "HTTP_CONNECTION",
                                                         "HTTP_HOST",
                                                         "HTTP_KEEP_ALIVE",
                                                         "HTTPS",
                                                         "INSTANCE_ID",
                                                         "INSTANCE_META_PATH",
                                                         "PATH_INFO",
                                                         "PATH_TRANSLATED",
                                                         "REMOTE_PORT",
                                                         "SCRIPT_NAME",
                                                         "SERVER_NAME",
                                                         "SERVER_PORT",
                                                         "SERVER_PORT_SECURE",
                                                         "SERVER_PROTOCOL",
                                                         "SERVER_SOFTWARE"
                                                     };

    public IHtmlString RenderVariableTable(string title, string className, NameValueCollection vars)
    {
        if (vars == null || vars.Count == 0) return Html("");

        var result = new StringBuilder();
        var hiddenRows = new StringBuilder();

        var fetchError = vars[Error.CollectionErrorKey];
        var errored = fetchError.HasValue();
        var keys = vars.AllKeys.Where(key => !hiddenHttpKeys.Contains(key) && key != Error.CollectionErrorKey).OrderBy(k => k);

        result.AppendFormat("    <div class=\"{0}\">", className);
        result.AppendFormat("      <h3 class=\"kv-title{1}\">{0}{2}</h3>", title, errored ? " title-error" : "", errored ? " - Error while gathering data" : "");
        if(keys.Any())
        {
            result.AppendFormat("      <div class=\"side-scroll\">");
            result.AppendFormat("        <table class=\"kv-table\">");
            foreach (var k in keys)
            {
                // If this has no value, skip it
                if (vars[k].IsNullOrEmpty())
                {
                    continue;
                }
                // If this is a hidden row, buffer it up, since CSS has no clean mechanism for :visible:nth-row(odd) type styling behavior
                var hidden = defaultHttpKeys.Contains(k);
                var sb = hidden ? hiddenRows : result;
                sb.AppendFormat("          <tr{2}><td class=\"key\">{0}</td><td class=\"value\">{1}</td></tr>", k, Linkify(vars[k]), hidden ? " class=\"hidden\"" : "");
            }
            if (vars["HTTP_HOST"].HasValue() && vars["URL"].HasValue())
            {
                var url = string.Format("http://{0}{1}{2}", vars["HTTP_HOST"], vars["URL"], vars["QUERY_STRING"].HasValue() ? "?" + vars["QUERY_STRING"] : "");
                result.AppendFormat("          <tr><td class=\"key\">URL and Query</td><td class=\"value\">{0}</td></tr>", vars["REQUEST_METHOD"] == "GET" ? Linkify(url).ToString() : Server.HtmlEncode(url));
            }
            result.Append(hiddenRows);
            result.AppendFormat("        </table>");
            result.AppendFormat("      </div>");
        }
        if(errored)
        {
            result.AppendFormat("<span class=\"custom-error-label\">Get {0} threw an exception:</span>", title);
            result.AppendFormat("<pre class=\"error-detail\">{0}</pre>", Server.HtmlEncode(fetchError));
        }
        result.AppendFormat("    </div>");
        return Html(result.ToString());
    }


    private IHtmlString Linkify(string s)
    {
        if (s.IsNullOrEmpty()) return Html("");
        
        if (Regex.IsMatch(s, @"%[A-Z0-9][A-Z0-9]"))
        {
            s = Server.UrlDecode(s);
        }

        if (Regex.IsMatch(s, "^(https?|ftp|file)://"))
        {
            //@* || (Regex.IsMatch(s, "/[^ /,]+/") && !s.Contains("/LM"))*@ // block special case of "/LM/W3SVC/1"
            var sane = SanitizeUrl(s);
            if (sane == s) // only link if it's not suspicious
                return Html(string.Format(@"<a href=""{0}"">{1}</a>", sane, Server.HtmlEncode(s)));
        }

        return Html(Server.HtmlEncode(s));
    }

    private static readonly Regex _sanitizeUrl = new Regex(@"[^-a-z0-9+&@#/%?=~_|!:,.;\(\)\{\}]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    public static string SanitizeUrl(string url)
    {
        return url.IsNullOrEmpty() ? url : _sanitizeUrl.Replace(url, "");
    }


        #line default
        #line hidden

        public override void Execute()
        {


WriteLiteral("\r\n");













            
            #line 13 "..\..\Pages\ErrorInfo.cshtml"
  
    var log = ErrorStore.Default;
    var error = log.Get(Guid);
    Layout = new Master { PageTitle = "Error Info - " + error, Error = error };


            
            #line default
            #line hidden

WriteLiteral("\r\n<div id=\"ErrorInfo\">\r\n");


            
            #line 133 "..\..\Pages\ErrorInfo.cshtml"
 if (error == null)
{

            
            #line default
            #line hidden
WriteLiteral("    <h1>Error not found.</h1>\r\n");


            
            #line 136 "..\..\Pages\ErrorInfo.cshtml"
}
else
{

            
            #line default
            #line hidden
WriteLiteral("    <h1 class=\"error-title\">");


            
            #line 139 "..\..\Pages\ErrorInfo.cshtml"
                       Write(error.Message);

            
            #line default
            #line hidden
WriteLiteral("</h1>\r\n");



WriteLiteral("    <div class=\"error-type\">");


            
            #line 140 "..\..\Pages\ErrorInfo.cshtml"
                       Write(error.Type);

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n");



WriteLiteral("    <pre class=\"error-detail\">");


            
            #line 141 "..\..\Pages\ErrorInfo.cshtml"
                         Write(error.Detail);

            
            #line default
            #line hidden
WriteLiteral("\r\n    </pre>\r\n");



WriteLiteral("    <p class=\"error-time\">occurred <b title=\"");


            
            #line 143 "..\..\Pages\ErrorInfo.cshtml"
                                        Write(error.CreationDate.ToLongDateString());

            
            #line default
            #line hidden
WriteLiteral(" at ");


            
            #line 143 "..\..\Pages\ErrorInfo.cshtml"
                                                                                  Write(error.CreationDate.ToLongTimeString());

            
            #line default
            #line hidden
WriteLiteral("\">");


            
            #line 143 "..\..\Pages\ErrorInfo.cshtml"
                                                                                                                          Write(error.CreationDate.ToRelativeTime());

            
            #line default
            #line hidden
WriteLiteral("</b> on ");


            
            #line 143 "..\..\Pages\ErrorInfo.cshtml"
                                                                                                                                                                      Write(error.MachineName);

            
            #line default
            #line hidden
WriteLiteral(" <span class=\"info-delete-link\">(<a class=\"info-link\" href=\"delete?guid=");


            
            #line 143 "..\..\Pages\ErrorInfo.cshtml"
                                                                                                                                                                                                                                                                Write(error.GUID);

            
            #line default
            #line hidden
WriteLiteral("\">delete</a>)</span></p>\r\n");


            
            #line 144 "..\..\Pages\ErrorInfo.cshtml"
    if (!string.IsNullOrEmpty(error.SQL))
    {

            
            #line default
            #line hidden
WriteLiteral("        <h3>SQL</h3>\r\n");



WriteLiteral("        <pre class=\"sql-detail\">");


            
            #line 147 "..\..\Pages\ErrorInfo.cshtml"
                           Write(error.SQL);

            
            #line default
            #line hidden
WriteLiteral("</pre>\r\n");



WriteLiteral("        <br/>\r\n");


            
            #line 149 "..\..\Pages\ErrorInfo.cshtml"
    }
    
            
            #line default
            #line hidden
            
            #line 150 "..\..\Pages\ErrorInfo.cshtml"
Write(RenderVariableTable("Server Variables", "server-variables", error.ServerVariables));

            
            #line default
            #line hidden
            
            #line 150 "..\..\Pages\ErrorInfo.cshtml"
                                                                                       
    if (error.CustomData != null && error.CustomData.Count > 0)
    {
        var errored = error.CustomData.ContainsKey(ErrorStore.CustomDataErrorKey);
        var cdKeys = error.CustomData.Keys.Where(k => k != ErrorStore.CustomDataErrorKey);

            
            #line default
            #line hidden
WriteLiteral("        <div class=\"custom-data\">\r\n");


            
            #line 156 "..\..\Pages\ErrorInfo.cshtml"
             if (errored)
            {

            
            #line default
            #line hidden
WriteLiteral("                <h3 class=\"kv-title title-error\">Custom - Error while gathering c" +
"ustom data</h3>\r\n");


            
            #line 159 "..\..\Pages\ErrorInfo.cshtml"
            } else
            {

            
            #line default
            #line hidden
WriteLiteral("                <h3 class=\"kv-title\">Custom</h3>\r\n");


            
            #line 162 "..\..\Pages\ErrorInfo.cshtml"
            }

            
            #line default
            #line hidden

            
            #line 163 "..\..\Pages\ErrorInfo.cshtml"
             if(cdKeys.Any(k => k != ErrorStore.CustomDataErrorKey))
            {

            
            #line default
            #line hidden
WriteLiteral("                <div class=\"side-scroll\">\r\n                    <table class=\"kv-t" +
"able\">\r\n");


            
            #line 167 "..\..\Pages\ErrorInfo.cshtml"
                         foreach (var cd in cdKeys)
                        {

            
            #line default
            #line hidden
WriteLiteral("                            <tr>\r\n                                <td class=\"key\"" +
">");


            
            #line 170 "..\..\Pages\ErrorInfo.cshtml"
                                           Write(cd);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                                <td class=\"value\">");


            
            #line 171 "..\..\Pages\ErrorInfo.cshtml"
                                             Write(Linkify(error.CustomData[cd]));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                            </tr>\r\n");


            
            #line 173 "..\..\Pages\ErrorInfo.cshtml"
                        }

            
            #line default
            #line hidden
WriteLiteral("                    </table>\r\n                </div>\r\n");


            
            #line 176 "..\..\Pages\ErrorInfo.cshtml"
            }

            
            #line default
            #line hidden

            
            #line 177 "..\..\Pages\ErrorInfo.cshtml"
             if(errored)
            {

            
            #line default
            #line hidden
WriteLiteral("                <span class=\"custom-error-label\">GetCustomData threw an exception" +
":</span>\r\n");



WriteLiteral("                <pre class=\"error-detail\">");


            
            #line 180 "..\..\Pages\ErrorInfo.cshtml"
                                     Write(error.CustomData[ErrorStore.CustomDataErrorKey]);

            
            #line default
            #line hidden
WriteLiteral("</pre>\r\n");


            
            #line 181 "..\..\Pages\ErrorInfo.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </div>\r\n");


            
            #line 183 "..\..\Pages\ErrorInfo.cshtml"
    }
    
            
            #line default
            #line hidden
            
            #line 184 "..\..\Pages\ErrorInfo.cshtml"
Write(RenderVariableTable("QueryString", "querystring", error.QueryString));

            
            #line default
            #line hidden
            
            #line 184 "..\..\Pages\ErrorInfo.cshtml"
                                                                         
    
            
            #line default
            #line hidden
            
            #line 185 "..\..\Pages\ErrorInfo.cshtml"
Write(RenderVariableTable("Form", "form", error.Form));

            
            #line default
            #line hidden
            
            #line 185 "..\..\Pages\ErrorInfo.cshtml"
                                                    
    
            
            #line default
            #line hidden
            
            #line 186 "..\..\Pages\ErrorInfo.cshtml"
Write(RenderVariableTable("Cookies", "cookies", error.Cookies));

            
            #line default
            #line hidden
            
            #line 186 "..\..\Pages\ErrorInfo.cshtml"
                                                             
}

            
            #line default
            #line hidden
WriteLiteral("</div>");


        }
    }
}
#pragma warning restore 1591
