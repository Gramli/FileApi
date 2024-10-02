namespace File.API.Configuration
{
    public static class ContainerConfigurationExtension
    {
        public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
        {
            builder.Logging
                .ClearProviders()
                .AddConsole();

            return builder;
        }

        public static IServiceCollection AddCustomCors(this IServiceCollection services, string myAllowSpecificOrigins)
        => services.AddCors(options =>
        {
            options.AddPolicy(name: myAllowSpecificOrigins, policy =>
            {
                policy.WithOrigins("http://127.0.0.1:4200", "http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });

    }
}
