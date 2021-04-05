using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Models;

namespace WebApplication1.Helper
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultcontext = await next();
            int userid = int.Parse(resultcontext.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("UserID", StringComparison.InvariantCultureIgnoreCase)).Value);
            var repo = resultcontext.HttpContext.RequestServices.GetService<IDatingRepository>();
            User user = await repo.GetUser(userid);
            user.LastActive = DateTime.Now;
            await repo.SaveAll();
        }
    }
}
