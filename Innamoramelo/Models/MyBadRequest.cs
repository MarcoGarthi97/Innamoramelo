using Microsoft.AspNetCore.Mvc;

namespace Innamoramelo.Models
{
    public class MyBadRequest
    {
        internal BadRequestObjectResult CreateBadRequest(string title, string details, int statusCode)
        {
            var badRequest = new ProblemDetails
            {
                Title = title,
                Detail = details,
                Status = statusCode
            };

            return new BadRequestObjectResult(badRequest);
        }
    }
}
