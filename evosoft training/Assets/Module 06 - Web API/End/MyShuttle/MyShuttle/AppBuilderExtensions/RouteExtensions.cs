namespace MyShuttle.Web.AppBuilderExtensions
{
	
    using Microsoft.AspNetCore.Builder;

	public static class RouteExtensions
	{

		public static IApplicationBuilder ConfigureRoutes(this IApplicationBuilder app)
		{
			return app.UseMvc(endpoints =>
			{
				endpoints.MapRoute(
					name: "default",
					template: "{controller}/{action}/{id?}",
					defaults: new { controller = "Home", action = "Index" });
			});
		}
	}
}