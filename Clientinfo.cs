using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DNWS
{
  class Clientinfo : IPlugin
  {
    public Clientinfo()
    {
    }

    public void PreProcessing(HTTPRequest request)
    {
    }

    public HTTPResponse GetResponse(HTTPRequest request)
    {
      //Name:Pongsakorn Thongaram ID:620615026
      HTTPResponse response = null;
      StringBuilder sb = new StringBuilder();
      String[] showip = request.getPropertyByKey("RemoteEndPoint").Split(":");
      String Browse = request.getPropertyByKey("user-agent");
      String Language = request.getPropertyByKey("accept-language");
      String encode = request.getPropertyByKey("accept-encoding");
      sb.Append("<html><body style=font-family:Courier New>");
      sb.Append("<p> Client ip: "+ showip[0] + "</p>" );
      sb.Append("<p> Client port: "+ showip[1] + "</p>");
      sb.Append("<p> Browser Information: "+ Browse + "</p>");
      sb.Append("<p> Accept Language: "+ Language + "</p>");
      sb.Append("<p> Accept Encoding: "+ encode + "</p>");
      sb.Append("</body></html>");
      response = new HTTPResponse(200);
      response.body = Encoding.UTF8.GetBytes(sb.ToString());
      return response;
    }

    public HTTPResponse PostProcessing (HTTPResponse response)  
    {
      throw new NotImplementedException();
    }
  }
}