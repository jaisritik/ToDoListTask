using System.Net;

namespace ToDoListTask.Middleware
{
    public class CustomErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomErrorHandlerMiddleware> _logger;

        public CustomErrorHandlerMiddleware(RequestDelegate next, ILogger<CustomErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Proceed with the request pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "An unhandled exception occurred");

                // Handle the error and return a custom response
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "An error occurred while processing your request.",
                Detailed = ex.Message // You can remove this in production to avoid exposing sensitive info
            };

            return context.Response.WriteAsJsonAsync(result);
        }
    }

}
