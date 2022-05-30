using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Shared.DTO
{
    public class ErrorDto
    {
        public List<String> Errors { get; private set; }
        public bool IsShow { get; private set; }
        // bu alani kullaniciya gostermek icin kullanilir.Bazi durumlarda sadece yazilimcinin anlayacagi bir hata olma durumunda false'a set edersek kullanicilar gormez.

        public ErrorDto()
        {
            Errors = new List<string>();
        }
        public ErrorDto(string error,bool isShow=true)
        {
            Errors.Add(error);
            IsShow = isShow;
        }
        public ErrorDto(List<string> errors,bool isShow= true)
        {
            Errors = errors;
            IsShow = isShow;
        }
    }
}
