using System.Net;
using System.Threading;
using System.Diagnostics;
using System;

namespace cfalck.Web
{
    class RequestHandler
    {
        public void HandleRequest(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            // Get a string of all current threads.
            string threadsString = "";
            var currentThreads = Process.GetCurrentProcess().Threads;
            foreach (ProcessThread thread in currentThreads)
            {
                threadsString += thread.Id + ",";
            }

            // Construct a response.
            string responseString =
            "<html><body> " +
                "Hello world :)" +
                "<p>" +
                $"Date: {System.DateTime.Now.ToLongTimeString()}" +
                "</p>" +

                "<h3>" +
                "Request Info:" +
                "</h3>" +

                "<p>" +
                $"Headers: {request.Headers}" +
                "</p>" +
                "<p>" +
                $"Method : {request.HttpMethod}" +
                "</p>" +
                "<p>" +
                $"RawUrl: {request.RawUrl}" +
                "</p>" +
                "<p>" +
                $"Url: {request.Url}" +
                "</p>" +
                "<p>" +
                $"UserHostAddress: {request.UserHostAddress}" +
                "</p>" +
                "<p>" +
                $"UserHostName: {request.UserHostName}" +
                "</p>" +
                "<p>" +
                $"UserLanguages: {request.UserLanguages}" +
                "</p>" +

                "<h3>" +
                "Threading Info:" +
                "</h3>" +

                "<p>" +
                $"Thread Ids: {threadsString}" +
                "</p>" +

           "</body></html>";

            // Create a buffer from the response string.
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

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
