namespace Medlab_MVC_Uİ.Middlewares
{
    
        public class CheckNotFound
        {
            private readonly RequestDelegate _next;

            public CheckNotFound(RequestDelegate next)
            {
                _next = next;
            }

            public async Task Invoke(HttpContext httpContext)
            {
                await _next(httpContext);
                if (httpContext.Response.StatusCode == 404)
                {
                    httpContext.Response.Redirect("/home/error");
                }
            }
        }

        // Extension method used to add the middleware to the HTTP request pipeline.
        public static class MiddlewareExtensions
        {
            public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder)
            {
                return builder.UseMiddleware<CheckNotFound>();
            }
       }
    
}
