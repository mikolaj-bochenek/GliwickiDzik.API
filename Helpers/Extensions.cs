using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace GliwickiDzik.API.Helpers
{
    public static class Extensions
    {        
        public static void AddApplicationError(this HttpResponse response, string message)
        {
           response.Headers.Add("Application-Error", message);
           response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
           response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static int CalculateAge(this DateTime dateOfBirth)
        {
            var age = DateTime.Now.Year - dateOfBirth.Year;

            if (dateOfBirth.AddYears(age) > DateTime.Now)
                age --;
            
            return age;
        }

        public static void AddPagination(this HttpResponse response, int currentPage, int pageSize, int totalCount, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, pageSize, totalCount, totalPages);

            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}