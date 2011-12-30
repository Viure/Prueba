<%@ Page Language="VB" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage(Of Prueba.ChangePasswordModel)" %>

<asp:Content ID="changePasswordTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Cambiar contraseña
</asp:Content>

<asp:Content ID="changePasswordContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Cambiar contraseña</h2>
    <p>
        Use el formulario siguiente para cambiar la contraseña. 
    </p>
    <p>
        Es necesario que las nuevas contraseñas tengan al menos <%: Membership.MinRequiredPasswordLength %> caracteres.
    </p>

    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>

    <% Using Html.BeginForm() %>
        <%: Html.ValidationSummary(True, "No se realizó el cambio de contraseña. Corrija los errores e inténtelo de nuevo.")%>
        <div>
            <fieldset>
                <legend>Información de cuenta</legend>
                
                <div class="editor-label">
                    <%: Html.LabelFor(Function(m) m.OldPassword) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(Function(m) m.OldPassword) %>
                    <%: Html.ValidationMessageFor(Function(m) m.OldPassword) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(Function(m) m.NewPassword) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(Function(m) m.NewPassword) %>
                    <%: Html.ValidationMessageFor(Function(m) m.NewPassword) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(Function(m) m.ConfirmPassword) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(Function(m) m.ConfirmPassword) %>
                    <%: Html.ValidationMessageFor(Function(m) m.ConfirmPassword) %>
                </div>
                
                <p>
                    <input type="submit" value="Cambiar contraseña" />
                </p>
            </fieldset>
        </div>
    <% End Using %>
</asp:Content>
