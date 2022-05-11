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
                    name: "DriverCountWithFilter",
                    pattern: "api/Drivers/{filter}",
                    defaults: new { controller = "Drivers", action = "count" });

                endpoints.MapControllerRoute(
                    name: "defaultWithPrefix",
                    pattern: "api/{controller}/{action}/{id?}"
                    );

                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
		}
	}
}