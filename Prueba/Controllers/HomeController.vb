Imports Prueba.OpenTok

Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Function Index() As ActionResult
        ViewData("Message") = "ASP.NET MVC"

        Dim opentok As OpenTokSDK = New OpenTokSDK()
        Dim sessionId As String = opentok.CreateSession(Request.ServerVariables("REMOTE_ADDR"))

        Dim tokenOptions As New Dictionary(Of String, Object)
        tokenOptions.Add(TokenPropertyConstants.ROLE, RoleConstants.MODERATOR)
        Dim token As String = opentok.GenerateToken(sessionId, tokenOptions)

        'Dim token As String = devtoken

        ViewData("apiKey") = "10819292"
        ViewData("sessionId") = sessionId
        ViewData("token") = token


        Return View()
    End Function

    Function About() As ActionResult
        Return View()
    End Function
End Class
