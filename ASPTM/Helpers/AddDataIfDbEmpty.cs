using ASPTM.Middleware.Initializers;

namespace ASPTM.Helpers
{
    public static class AddDataIfDbEmpty
    {
        public static IApplicationBuilder UseAddData(this IApplicationBuilder app)
        {
            return app.UseMiddleware<DatabaseInitializerMiddleware>();
        }
    }
}
