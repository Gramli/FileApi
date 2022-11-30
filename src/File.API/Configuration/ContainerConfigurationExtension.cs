namespace File.API.Configuration
{
    public static class ContainerConfigurationExtension
    {
        public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            return builder;
        }
    }
}
