using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dlog.Api.Middlewares.ServerResponse
{
    public static class Extenssions
    {
        public static IApplicationBuilder UseServerResponse(
            this IApplicationBuilder app,
            DeployModel deployModel)
        {
            app.UseMiddleware<ServerResponseMiddleware>(deployModel);
            return app;
        } 
    }
}
