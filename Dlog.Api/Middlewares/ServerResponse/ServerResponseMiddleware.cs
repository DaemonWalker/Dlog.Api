using Dlog.Api.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dlog.Api.Middlewares.ServerResponse
{
    public class ServerResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly DeployModel deployModel;
        public ServerResponseMiddleware(RequestDelegate next, DeployModel deployModel)
        {
            this._next = next;
            this.deployModel = deployModel;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var responseModel = new ResponseModel();
                var serverResponseInfo = new ServerResponseInfoModel();

                serverResponseInfo.HttpStatusCode = HttpStatusCode.InternalServerError;
                if (this.deployModel == DeployModel.Develop) 
                {
                    serverResponseInfo.Message = $"{e.Message}\n{e.StackTrace}";
                }
                else
                {
                    serverResponseInfo.Message = e.Message;
                }

                responseModel.ServerResponse = serverResponseInfo;
                var serializeOption = new JsonSerializerOptions(JsonSerializerDefaults.Web);
                serializeOption.IgnoreNullValues = true;
                await context.Response.WriteAsJsonAsync(responseModel, serializeOption);
            }
        }
    }
}
