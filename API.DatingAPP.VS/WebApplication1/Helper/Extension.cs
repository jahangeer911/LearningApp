using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Helper
{
    public static class Extension
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
        public static void AddPageinationHeaders(this HttpResponse response,int currentPage,int itemsperPage,int totalitems,int totalpages)
        {
            var paginationHeader = new PaginationHeader(currentPage,  itemsperPage,  totalitems,  totalpages);
            var camlcaseformatter = new JsonSerializerSettings();
            camlcaseformatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination", JsonConvert.SerializeObject( paginationHeader,camlcaseformatter));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
        public static int CalculateAge(this DateTime dateTime)
        {
            return DateTime.Now.Year - dateTime.Year;
        }
    }
}
