using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace FloydPink.Flickr.Downloadr.OAuth
{
    public class OAuthCallbackManager : IOAuthCallbackManager
    {
        static Random random = new Random();

        public string ListenerAddress { get; private set; }

        public event EventHandler<OAuthCallbackEventArgs> OAuthCallbackEvent;

        public OAuthCallbackManager()
        {
        }

        public IAsyncResult SetupCallback()
        {
            this.ListenerAddress = GetNewHttpListenerAddress();
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(ListenerAddress);
            listener.Start();
            return listener.BeginGetContext(HttpListener_Callback, listener);
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

        private void HttpListener_Callback(IAsyncResult result)
        {
            HttpListener listener = (HttpListener)result.AsyncState;
            HttpListenerContext context = listener.EndGetContext(result);
            HttpListenerRequest request = context.Request;
            string token = request.QueryString["oauth_token"];
            string verifier = request.QueryString["oauth_verifier"];

            HttpListenerResponse response = context.Response;

            string responseString = "<HTML><BODY>You have been authenticated. Please return to the application. <script language=\"javascript\">window.close();</script></BODY></HTML>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();

            response.Close();
            listener.Close();

            OAuthCallbackEvent(this, new OAuthCallbackEventArgs(token, verifier));
        }
    }
}
