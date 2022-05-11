namespace MyShuttle.Web.AppBuilderExtensions
{
	using Microsoft.AspNetCore.Builder;

	public static class RouteExtensions
	{

		public static IApplicationBuilder ConfigureRoutes(this IApplicationBuilder app)
		{
            return app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
	}
}