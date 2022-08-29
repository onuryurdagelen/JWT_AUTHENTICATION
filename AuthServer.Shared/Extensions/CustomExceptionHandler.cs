using AuthServer.Shared.DTO;
using AuthServer.Shared.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthServer.Shared.Extensions
{
    public static class CustomExceptionHandler
    {

        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
            {
                config.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if(errorFeature != null)
                    {
                        var ex = errorFeature.Error;

                        ErrorDto errorDto = null;

                        if(ex is CustomException)
                        {
                            errorDto = new ErrorDto(ex.Message, true); 
                            //Bizim gönderdiğimiz exception ise bunu çalıştır ve kullanıcıya göster
                        }
                        else
                        {
                            //Kullanıcıya göstermeyeceğiz.
                            errorDto = new ErrorDto(ex.Message, false);

                        }

                        var response = Response<NoDataDto>.Failure(errorDto, 500);


                        await context.Response.WriteAsync(response.ToString());
                    }
                });

            }); //Tüm hataları yakalayan middleware'dır.
        }
    }
}
