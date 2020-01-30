using System;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Threading.Tasks;
using GliwickiDzik.API.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace GliwickiDzik.API.Helpers
{
    public class ActionFilter : IAsyncActionFilter
    {
        private readonly IUnitOfWork _unitOfWork;
        public ActionFilter(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            var repository = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();

            var userId = int.Parse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var user = await repository.GetOneUserAsync(userId);

            user.LastActive = DateTime.Now;

            await _unitOfWork.SaveAllAsync();
        }
    }
}