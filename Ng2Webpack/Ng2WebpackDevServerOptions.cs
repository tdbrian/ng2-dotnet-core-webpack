namespace Ng2Webpack
{
    public class Ng2WebpackDevServerOptions
    {
        public Ng2WebpackDevServerOptions(string host = "localhost", int port = 4000)
        {
            Host = host;
            Port = port;
        }

        /// <summary>
        /// The ip address of the server
        /// Default value if not specified is localhost
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// The port of the server
        /// Default value if not specified is 4000
        /// </summary>
        public int Port { get; set; }
    }
}
