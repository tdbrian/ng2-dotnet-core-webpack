using Microsoft.Extensions.DependencyInjection;

namespace Ng2Webpack
{
    public static class Ng2WebpackServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all services required for Webpack
        /// </summary>
        public static IServiceCollection AddWebpack(this IServiceCollection services)
        {
            services.AddSingleton<INg2Webpack, Ng2Webpack>();
            return services;
        }
    }
}
