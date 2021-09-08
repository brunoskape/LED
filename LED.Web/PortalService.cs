using System;
using System.IO;
using System.Net;
using System.Text;

namespace LED.Web
{
    public static class PortalService
    {
        public static string DotNetImport(string url)
        {
            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Timeout = 50000;
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

            Stream dataStream = response.GetResponseStream();
            Encoding encoding = Encoding.GetEncoding("UTF-8");
            StreamReader reader = new StreamReader(dataStream, encoding);
            String responseFromServer = reader.ReadToEnd();

            response.Close();
            reader.Close();

            return responseFromServer;
        }
    }
}