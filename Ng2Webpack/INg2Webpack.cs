namespace Ng2Webpack
{
    public interface INg2Webpack
    {
        Ng2WebpackMiddlewareOptions Invoke(string configFile, Ng2WebpackOptions options);
    }
}