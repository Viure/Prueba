<%@ Page Language="VB" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Prueba
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var apiKey = '<%=ViewData("apiKey") %>';
        var sessionId = '<%=ViewData("sessionId") %>';
        var token = '<%=ViewData("token") %>';

        TB.setLogLevel(TB.DEBUG); // Set this for helpful debugging messages in console

        var session = TB.initSession(sessionId);
        session.addEventListener('sessionConnected', sessionConnectedHandler);
        session.connect(apiKey, token);

        var publisher;

        function sessionConnectedHandler(event) {
            // Publish my webcam stream and put it in a div
            alert('Hello world. I am connected to OpenTok :).');
            publisher = session.publish('myPublisherDiv');
        }
       
  </script>
  <script>
      $(function () {
          $("#myPublisherDivConten").draggable();
      });
	</script>
    
    <h2><%: ViewData("Message") %></h2>
    <p>
        Para obtener más información sobre ASP.NET MVC, visite el <a href="http://asp.net/mvc" title="sitio web de ASP.NET MVC">http://asp.net/mvc</a>.
    </p>

    <div id="myPublisherDivConten" ><div id="myPublisherDiv"></div></div>
    

</asp:Content>
