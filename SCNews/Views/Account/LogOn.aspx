<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SCNews.Models.LogOnModel>" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="_headContent" runat="server">
    <title>Авторизация</title>
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                    Авторизация
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
            <% using (Html.BeginForm()) { %>

                <div class="news_short login-view">
                    <div class="validation-summary-errors">
                    <%= Html.ValidationSummary(true, "Ошибка авторизации. Пожалуйста, попробуйе снова.") %>
                    </div>

                    <p>
                        <%= Html.LabelFor(m => m.UserName) %><br />
                        <%= Html.TextBoxFor(m => m.UserName) %><br />
                        <%= Html.ValidationMessageFor(m => m.UserName) %><br />
                    </p>
                    <p>
                        <%= Html.LabelFor(m => m.Password) %><br />
                        <%= Html.PasswordFor(m => m.Password) %><br />
                        <%= Html.ValidationMessageFor(m => m.Password) %><br />
                    </p>
                    <p>
                        <%= Html.CheckBoxFor(m => m.RememberMe) %>
                        <%= Html.LabelFor(m => m.RememberMe) %>
                    </p>
                    <p>Если у вас все еще нет учетной записи, вы можете <%= Html.ActionLink("зарегистрировать", "Register", new { controller="Account" }, new {  }) %> ее.</p>
                    <p><input type="submit" value="Вход" class="button button-confirm" /></p>
                </div>

            <% } %>
        </div>
    </div>


</asp:Content>
