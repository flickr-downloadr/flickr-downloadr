using System;
using System.Net;
using System.Collections.Specialized;
using FloydPink.Flickr.Downloadr.Extensions;

namespace FloydPink.Flickr.Downloadr.Listener
{
    public class HttpListenerManager : IHttpListenerManager
    {
        static Random random = new Random();

        public string ListenerAddress { get; private set; }

        public string ResponseString { get; set; }

        public event EventHandler<HttpListenerCallbackEventArgs> RequestReceived;

        public HttpListenerManager()
        {
        }

        public IAsyncResult SetupCallback()
        {
            this.ListenerAddress = GetNewHttpListenerAddress();
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(ListenerAddress);
            listener.Start();
            return listener.BeginGetContext(HttpListenerCallback, listener);
        }

        private string GetNewHttpListenerAddress()
        {
            string listenerAddress;
            while (true)
            {
                var listener = new HttpListener();
                int randomPortNumber = random.Next(1025, 65535);
                listenerAddress = string.Format("http://localhost:{0}/", randomPortNumber);
                listener.Prefixes.Add(listenerAddress);
                try
                {
                    listener.Start();
                    listener.Stop();
                    listener.Close();
                }
                catch
                {
                    continue;
                }
                break;
            }
            return listenerAddress;
        }

        private void HttpListenerCallback(IAsyncResult result)
        {
            HttpListener listener = (HttpListener)result.AsyncState;

            HttpListenerContext context = listener.EndGetContext(result);
            HttpListenerRequest request = context.Request;
            NameValueCollection queryStrings = request.QueryString;
            
            HttpListenerResponse response = context.Response;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(ResponseString);
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();

            response.Close();
            listener.Close();

            RequestReceived(this, new HttpListenerCallbackEventArgs(queryStrings));
        }

    }
}
