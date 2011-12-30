<%@ Control Language="VB" Inherits="System.Web.Mvc.ViewUserControl" %>
<%-- The following line works around an ASP.NET compiler warning --%>
<%: ""%>
<%
    If Request.IsAuthenticated Then
    %>
        ¡Hola <strong><%: Page.User.Identity.Name %></strong>!
        [ <%: Html.ActionLink("Cerrar sesión", "LogOff", "Account")%> ]
    <%
    Else
    %>
        [ <%: Html.ActionLink("Iniciar sesión", "LogOn", "Account")%> ]
    <%        
    End If
%>