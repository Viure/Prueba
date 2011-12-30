<%@ Page Language="VB" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage(Of Prueba.RegisterModel)" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Registrarse
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Crear una nueva cuenta</h2>
    <p>
        Use el formulario siguiente para crear una cuenta nueva. 
    </p>
    <p>
        Es necesario que las contraseñas tengan al menos <%: Membership.MinRequiredPasswordLength %> caracteres.
    </p>

    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>

    <% Using Html.BeginForm() %>
        <%: Html.ValidationSummary(True, "No se creó la cuenta. Corrija los errores e inténtelo de nuevo.")%>
        <div>
            <fieldset>
                <legend>Información de cuenta</legend>
                
                <div class="editor-label">
                    <%: Html.LabelFor(Function(m) m.UserName) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(Function(m) m.UserName) %>
                    <%: Html.ValidationMessageFor(Function(m) m.UserName) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(Function(m) m.Email) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(Function(m) m.Email) %>
                    <%: Html.ValidationMessageFor(Function(m) m.Email) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(Function(m) m.Password) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(Function(m) m.Password) %>
                    <%: Html.ValidationMessageFor(Function(m) m.Password) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(Function(m) m.ConfirmPassword) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(Function(m) m.ConfirmPassword) %>
                    <%: Html.ValidationMessageFor(Function(m) m.ConfirmPassword) %>
                </div>
                
                <p>
                    <input type="submit" value="Registrarse" />
                </p>
            </fieldset>
        </div>
    <% End Using %>
</asp:Content>
