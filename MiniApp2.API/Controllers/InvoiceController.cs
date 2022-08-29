using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MiniApp2.API.Controllers
{  
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        [HttpGet]
         public IActionResult GetInvoices()
        {
            var userName = HttpContext.User.Identity.Name; //userName
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier); //userId
            //veri tabanında userId veya ıserName alanları üzerinden gerekli dataları çek.

            return Ok($"Invoice ==> UserName: {userName} -- userId: {userIdClaim.Value}");
        }
    }
}