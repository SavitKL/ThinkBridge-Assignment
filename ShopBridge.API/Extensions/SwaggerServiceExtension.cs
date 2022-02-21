using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using ShopBridge.Models.AppSettings;

namespace ShopBridge.Extensions
{
    public static class SwaggerServiceExtension
    {
        public static IServiceCollection AddUserSwagger(this IServiceCollection services, AppSettings appSetting)
        {
            if (appSetting.SwaggerEnabled)
            {

                services.AddSwaggerGen(c =>
                {
                   
                    var version = "V1";
                    c.SwaggerDoc(version, new OpenApiInfo
                    {
                        Version = version,
                        Title = "ShopBridge Service"
                    });

                });
            }

            return services;
        }
        public static IApplicationBuilder UseUserSwaggerUi(this IApplicationBuilder app, AppSettings appSetting)
        {

            if (appSetting.SwaggerEnabled)
            {
                var routePrefix = appSetting.BaseRoute;
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = BuildPath(includeStartingSlash: false, routePrefix, "swagger/{documentname}/swagger.json");
                });

                app.UseSwaggerUI(c =>
                {
                    var swaggerEndpointUrl = string.Empty;
                    var name = "V1";

                    swaggerEndpointUrl = BuildPath(includeStartingSlash: true, "swagger", name, "swagger.json");
                    c.SwaggerEndpoint(swaggerEndpointUrl, $"ShopBridge Service API {name}");
                    c.DefaultModelExpandDepth(3);
                    c.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                    c.EnableDeepLinking();
                    c.DisplayOperationId();
                });
            }
            return app;
        }
        private static string BuildPath(bool includeStartingSlash, params string[] paths)
        {
            const string delimiter = "/";
            var path = string.Join(delimiter, paths);
            return includeStartingSlash
                ? delimiter + path
                : path;
        }
    }
}
