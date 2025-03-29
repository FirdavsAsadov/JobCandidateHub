namespace JobCandidateHub.Api.Configurations
{
    public static partial class HostConfiguration
    {
        public static ValueTask<WebApplicationBuilder> ConfigureAsync(this WebApplicationBuilder builder)
        {
            builder
                .AddMappers()
                .AddJobCandidateHubDbContext()
                .AddJobCandidateHubServices()
                .AddJobCandidateHubRepositories()
                .AddDevTools()
                .AddExposers();

            return new(builder);
        }

        public static ValueTask<WebApplication> ConfigureAsync(this WebApplication app)
        {
            app
                .UseExposers()
                .UseDevTools();

            return new(app);
        }
    }
}
