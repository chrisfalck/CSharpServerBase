using cfalck.FileSystem;

namespace cfalck.Web
{
    class MyWeb
    {
        static int Main(string[] args)
        {
            WebServer web = new WebServer();
            web.Run();
            System.Threading.Thread.Sleep(10000);
            return 0;
        }
    }
}
