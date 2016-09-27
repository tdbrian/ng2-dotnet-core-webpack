using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ng2Webpack
{
    public static class Ng2WebpackExtenstions
    {
        /// <summary>
        /// Adds webpack functionality to the application. It should be used only in development environment.
        /// This method requires an external configuration file and covers specific configurations based on an external config file.
        /// Based on the provided <paramref name="webpackOptions"/> the underlying middleware will add the required script tags
        /// so there is not need to add them manually.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configFile">The path to the external configuration file e.g. webpack/webpack.development.js</param>
        /// <param name="webpackOptions">Configuration options which sets flags on the webpack command and adds the required script tags.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseWebpack(this IApplicationBuilder app, string configFile, Ng2WebpackOptions webpackOptions)
        {
            var webpack = app.ApplicationServices.GetService<INg2Webpack>();
            var options = webpack.Invoke(configFile, webpackOptions);
            app.UseMiddleware<Ng2WebpackMiddleware>(options);
            return app;
        }
    }
}
