
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MainBoldReportsAPI.Web.Services
{
    public interface IViewRenderService
    {
        Task<string> RenderViewToStringAsync(string viewName, object model = null);
        Task<string> RenderPartialToStringAsync(string partialName, object model = null);
    }

    public class ViewRenderService : IViewRenderService
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;

        public ViewRenderService(
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        public Task<string> RenderViewToStringAsync(string viewName, object model = null) =>
            RenderToStringInternalAsync(viewName, model, isPartial: false);

        public Task<string> RenderPartialToStringAsync(string partialName, object model = null) =>
            RenderToStringInternalAsync(partialName, model, isPartial: true);

        private async Task<string> RenderToStringInternalAsync(
            string viewName, object model, bool isPartial)
        {
            // Create a stand-in ActionContext
            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            // Locate the view
            var viewResult = isPartial
                ? _viewEngine.FindView(actionContext, viewName, isMainPage: false)
                : _viewEngine.FindView(actionContext, viewName, isMainPage: true);

            if (viewResult.View == null)
            {
                // Optionally try GetView (absolute path), or throw with searched locations
                var searched = string.Join(Environment.NewLine, viewResult.SearchedLocations ?? Array.Empty<string>());
                throw new InvalidOperationException($"View '{viewName}' not found. Searched:{Environment.NewLine}{searched}");
            }

            // Prepare view data & temp data
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            var tempData = new TempDataDictionary(httpContext, _tempDataProvider);

            await using var sw = new StringWriter();

            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewData,
                tempData,
                sw,
                new HtmlHelperOptions());

            await viewResult.View.RenderAsync(viewContext);

            return sw.ToString();
        }
    }
}
