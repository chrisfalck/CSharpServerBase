using cfalck.FileSystem;

namespace cfalck.Web
{
    class MyWeb
    {
        static int Main(string[] args)
        {
            WebServer web = new WebServer();
            web.Run();
            return 0;
        }
    }
}
