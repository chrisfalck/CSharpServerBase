using System;
using System.Net;
using System.Threading;
using System.Diagnostics;
using cfalck.FileSystem;

namespace cfalck.Web
{
    class RequestHandler
    {
        public void HandleRequest(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            var fsHelper = new FileSystemHelper();

            // Create a buffer from the response string.
            byte[] buffer = fsHelper.GetFileAsBytes("index.html");

            // Tell the response how many bytes are in the body.
            response.ContentLength64 = buffer.Length;

            // Create a new OutputStream pointed into the response.
            System.IO.Stream output = response.OutputStream;

            try
            {
                // Write the bytes into the response buffer, sending them to the client.
                output.Write(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                output.Close();
            }
        }
    }

    class WebServer
    {
        string[] uriPrefixes = { "http://localhost:8080/" };
        HttpListener listener = null;

        public WebServer()
        {
            this.listener = new HttpListener();
            foreach (string prefix in this.uriPrefixes)
            {
                listener.Prefixes.Add(prefix);
            }

            listener.Start();
        }

        public void Run()
        {
            while (true)
            {
                // GetContext blocks until a request is received.
                var context = listener.GetContext();

                // Start a handler thread.
                var handler = new RequestHandler();
                var handlerThread = new Thread(() => handler.HandleRequest(context));
                handlerThread.Start();
            }
        }
    }
}
