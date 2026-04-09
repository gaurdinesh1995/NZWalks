namespace NZWalks.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvolkeAsync(HttpContext context)
        {
            try
            {
           await next(context);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();
                // Log the exception
                logger.LogError(ex,$"${errorId} : {ex.Message}");
              
                //return custom response
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var errorResponse = new
                {
                    ErrorId = errorId,
                    Message = "An unexpected error occurred. Please try again later."
                };

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}
