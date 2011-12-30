Imports System.Diagnostics.CodeAnalysis
Imports System.Security.Principal
Imports System.Web.Routing

Public Class AccountController
    Inherits System.Web.Mvc.Controller

    '
    ' GET: /Account/LogOn

    Public Function LogOn() As ActionResult
        Return View()
    End Function

    '
    ' POST: /Account/LogOn

    <HttpPost()> _
    Public Function LogOn(ByVal model As LogOnModel, ByVal returnUrl As String) As ActionResult
        If ModelState.IsValid Then
            If Membership.ValidateUser(model.UserName, model.Password) Then
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe)
                If Url.IsLocalUrl(returnUrl) AndAlso returnUrl.Length > 1 AndAlso returnUrl.StartsWith("/") _
                   AndAlso Not returnUrl.StartsWith("//") AndAlso Not returnUrl.StartsWith("/\\") Then
                    Return Redirect(returnUrl)
                Else
                    Return RedirectToAction("Index", "Home")
                End If
            Else
                ModelState.AddModelError("", "El nombre de usuario o la contraseña especificados son incorrectos.")
            End If
        End If

        ' Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
        Return View(model)
    End Function

    '
    ' GET: /Account/LogOff

    Public Function LogOff() As ActionResult
        FormsAuthentication.SignOut()

        Return RedirectToAction("Index", "Home")
    End Function

    '
    ' GET: /Account/Register

    Public Function Register() As ActionResult
        Return View()
    End Function

    '
    ' POST: /Account/Register

    <HttpPost()> _
    Public Function Register(ByVal model As RegisterModel) As ActionResult
        If ModelState.IsValid Then
            ' Intento de registrar al usuario
            Dim createStatus As MembershipCreateStatus
            Membership.CreateUser(model.UserName, model.Password, model.Email, Nothing, Nothing, True, Nothing, createStatus)

            If createStatus = MembershipCreateStatus.Success Then
                FormsAuthentication.SetAuthCookie(model.UserName, False)
                Return RedirectToAction("Index", "Home")
            Else
                ModelState.AddModelError("", ErrorCodeToString(createStatus))
            End If
        End If

        ' Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
        Return View(model)
    End Function

    '
    ' GET: /Account/ChangePassword

    <Authorize()> _
    Public Function ChangePassword() As ActionResult
        Return View()
    End Function

    '
    ' POST: /Account/ChangePassword

    <Authorize()> _
    <HttpPost()> _
    Public Function ChangePassword(ByVal model As ChangePasswordModel) As ActionResult
        If ModelState.IsValid Then
            ' ChangePassword iniciará una excepción en lugar de
            ' devolver false en determinados escenarios de error.
            Dim changePasswordSucceeded As Boolean

            Try
                Dim currentUser As MembershipUser = Membership.GetUser(User.Identity.Name, True)
                changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword)
            Catch ex As Exception
                changePasswordSucceeded = False
            End Try

            If changePasswordSucceeded Then
                Return RedirectToAction("ChangePasswordSuccess")
            Else
                ModelState.AddModelError("", "La contraseña actual es incorrecta o la nueva contraseña no es válida.")
            End If
        End If

        ' Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
        Return View(model)
    End Function

    '
    ' GET: /Account/ChangePasswordSuccess

    Public Function ChangePasswordSuccess() As ActionResult
        Return View()
    End Function

#Region "Status Code"
    Public Function ErrorCodeToString(ByVal createStatus As MembershipCreateStatus) As String
        ' Vaya a http://go.microsoft.com/fwlink/?LinkID=177550 para
        ' obtener una lista completa de códigos de estado.
        Select Case createStatus
            Case MembershipCreateStatus.DuplicateUserName
                Return "El nombre de usuario ya existe. Escriba un nombre de usuario diferente."

            Case MembershipCreateStatus.DuplicateEmail
                Return "Ya existe un nombre de usuario para esa dirección de correo electrónico. Escriba una dirección de correo electrónico diferente."

            Case MembershipCreateStatus.InvalidPassword
                Return "La contraseña especificada no es válida. Escriba un valor de contraseña válido."

            Case MembershipCreateStatus.InvalidEmail
                Return "La dirección de correo electrónico especificada no es válida. Compruebe el valor e inténtelo de nuevo."

            Case MembershipCreateStatus.InvalidAnswer
                Return "La respuesta de recuperación de la contraseña especificada no es válida. Compruebe el valor e inténtelo de nuevo."

            Case MembershipCreateStatus.InvalidQuestion
                Return "La pregunta de recuperación de la contraseña especificada no es válida. Compruebe el valor e inténtelo de nuevo."

            Case MembershipCreateStatus.InvalidUserName
                Return "El nombre de usuario especificado no es válido. Compruebe el valor e inténtelo de nuevo."

            Case MembershipCreateStatus.ProviderError
                Return "El proveedor de autenticación devolvió un error. Compruebe los datos especificados e inténtelo de nuevo. Si el problema continúa, póngase en contacto con el administrador del sistema."

            Case MembershipCreateStatus.UserRejected
                Return "La solicitud de creación de usuario se ha cancelado. Compruebe los datos especificados e inténtelo de nuevo. Si el problema continúa, póngase en contacto con el administrador del sistema."

            Case Else
                Return "Error desconocido. Compruebe los datos especificados e inténtelo de nuevo. Si el problema continúa, póngase en contacto con el administrador del sistema."
        End Select
    End Function
#End Region

End Class
