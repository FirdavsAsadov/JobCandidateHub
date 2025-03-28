namespace JobCandidateHub.Api.Configurations
{
    public static partial class HostConfiguration
    {
        public static ValueTask<WebApplicationBuilder> ConfigureAsync(this WebApplicationBuilder builder)
        {
            return new(builder);
        }

        public static ValueTask<WebApplication> ConfigureAsync(this WebApplication app)
        {
            return new(app);
        }
    }
}
