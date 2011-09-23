<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SCNews.Models.RegisterModel>" %>
<%@ Import Namespace="SCNews.Helpers"%>

<asp:Content ID="registerTitle" ContentPlaceHolderID="_headContent" runat="server">
    <title>Регистрация</title>
</asp:Content>
<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                        Введите ваши данные для регистрации на сайте
                    </div>
                </div>
            </div>
        </div>
        <div class="content register">
            <div class="login-view">
                <% using (Html.BeginForm()) { %>

                    <%= Html.ValidationSummary( true, "Создание учетной записи не удалось. Пожалуйста, исправьте ошибки и повторите попытку." )%>
                        
                    <%= Html.LabelFor( m => m.UserName ) %><br />
                    <%= Html.TextBoxFor(m => m.UserName) %><br />
                    <%= Html.ValidationMessageFor(m => m.UserName) %><br />
                    <br />
                    <%= Html.LabelFor( m => m.Password ) %><br />
                    <%= Html.PasswordFor(m => m.Password) %><br />
                    <%= Html.ValidationMessageFor(m => m.Password) %><br />
                    <br />
                    <%= Html.LabelFor( m => m.ConfirmPassword ) %><br />
                    <%= Html.PasswordFor(m => m.ConfirmPassword) %><br />
                    <%= Html.ValidationMessageFor(m => m.ConfirmPassword) %><br />
                    <br />
                    <%= Html.LabelFor( m => m.Email ) %><br />
                    <%= Html.TextBoxFor(m => m.Email) %><br />
                    <%= Html.ValidationMessageFor(m => m.Email) %><br />
                    <br />
                    <%= Html.GenerateCaptcha() %>
                    <br />
                    
                    <br />
                    <input type="submit" value="Зарегистрироваться" class="button button-confirm" />
                    <br />
                <% } %>
            </div>
             
        </div>
    </div>
    
    <div class="spacer"></div>
    
    <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                        Информация
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
        <%= Html.Action( "Details", new { controller = "Content", id = 3 } ) %>
        </div>
    </div>
    
</asp:Content>
