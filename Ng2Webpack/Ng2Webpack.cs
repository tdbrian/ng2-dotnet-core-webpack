using System;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Ng2Webpack
{
    public class Ng2Webpack : INg2Webpack
    {
        private const string WEBPACK = "webpack";
        private const string WEBPACK_DEV_SERVER = "webpack-dev-server";

        private readonly ILoggerFactory _loggerFactory;
        private readonly string _contentRootPath;

        public Ng2Webpack(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            _contentRootPath = env.ContentRootPath;
            _loggerFactory = loggerFactory;
        }

        public Ng2WebpackMiddlewareOptions Invoke(string configFile, Ng2WebpackOptions options)
        {
            var enableHotLoading = options.DevServerOptions != null;
            var toolToExecute = enableHotLoading ? WEBPACK_DEV_SERVER : WEBPACK;
            var logger = _loggerFactory.CreateLogger(toolToExecute);

            logger.LogInformation($"Verifying required tools are installed");
            EnsuereNodeModluesInstalled(enableHotLoading, logger);
            logger.LogInformation($"All node modules are properly installed");

            logger.LogInformation($"{toolToExecute} Execution started");
            var hotModuleReplcementTag = options.EnableHotModuleReplacement && enableHotLoading ? "--hot" : string.Empty;
            var arguments = $"--config {configFile} {hotModuleReplcementTag}";

            logger.LogInformation($"{toolToExecute} is called with these arguments: {arguments}");
            new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = GetWebpackToolToExeceute(toolToExecute),
                    Arguments = arguments,
                    UseShellExecute = false
                }
            }.Start();
            logger.LogInformation($"{toolToExecute} started successfully");

            var middleWareOptions = new Ng2WebpackMiddlewareOptions
            {
                OutputFileNames = options.GetBundlesList(),
                EnableHotLoading = enableHotLoading
            };

            if (!enableHotLoading) return middleWareOptions;

            middleWareOptions.Host = options.DevServerOptions.Host;
            middleWareOptions.Port = options.DevServerOptions.Port;
            return middleWareOptions;
        }

        private void EnsuereNodeModluesInstalled(bool enableHotLoading, ILogger logger)
        {
            if (!File.Exists(GetWebpackToolToExeceute(WEBPACK)))
            {
                logger.LogError("webpack is not installed. Please install it by executing npm i webpack");
            }
            if (enableHotLoading && !File.Exists(GetWebpackToolToExeceute(WEBPACK_DEV_SERVER)))
            {
                logger.LogError("webpack-dev-server is not installed. Please install it by executing npm i webpack-dev-server");
            }
        }

        /// <summary>
		/// Helper method that based on the supplied <paramref name="tool"/> returns a full path to the
		/// appropriate tool (webpack or webpack-dev-server) taking into account the current OS
		/// </summary>
		private string GetWebpackToolToExeceute(string tool)
        {
            var executable = Path.Combine(_contentRootPath, "node_modules", ".bin", tool);
            var osEnVariable = Environment.GetEnvironmentVariable("OS");
            if (!string.IsNullOrEmpty(osEnVariable) && string.Equals(osEnVariable, "Windows_NT", StringComparison.OrdinalIgnoreCase))
            {
                executable += ".cmd";
            }
            return executable;
        }
    }
}
