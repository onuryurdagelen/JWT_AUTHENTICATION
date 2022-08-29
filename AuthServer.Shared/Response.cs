using AuthServer.Shared.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AuthServer.Shared
{
    public class Response<T> where T:class
    {
        public T Data { get; private set; }
        public int StatusCode { get; private set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public bool IsSuccessful { get; private set; } //Client'ta gozukmesin ama ic API'lerde gozukmesini istiyoruz.

        public ErrorDto Error { get; private set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Response<T> Success(T data,int statusCode)
        {
            return new Response<T> 
            { 
                Data = data, 
                StatusCode = statusCode,
                IsSuccessful =true 
            };
        }
        // public static Response<T> Success()
        // {
        //      return new Response<T> 
        //     { 
      
        //     };
        // }
        //bazen basarili oldugumuzda data gondermek istemeyiz.Ornegin urun ekledigimizde,urun sildigimizde,urun guncelledigimizde kullailir.Bunun icin;
        public static Response<T> Success(int statusCode)
        {
            return new Response<T> 
            { 
                Data = default, 
                StatusCode = statusCode, 
                IsSuccessful = true 
            };
        }
        public static Response<T> Failure(ErrorDto errorDto,int statusCode)
        {
            return new Response<T> 
            { 
                Error = errorDto, 
                StatusCode = statusCode, 
                IsSuccessful = false 
            };
        }
        public static Response<T> Failure(string errorMessage,int statusCode,bool isShow)
        {
            var errorDto = new ErrorDto(errorMessage, isShow);
            return new Response<T> 
            { 
                Error = errorDto,
                StatusCode = statusCode, 
                IsSuccessful = false 
            };
        }

    }
}
