<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="passwordResetTitle" ContentPlaceHolderID="_headContent" runat="server">
    <title>Смена пароля</title>
</asp:Content>

<asp:Content ID="passwordResetContent" ContentPlaceHolderID="MainContent" runat="server">    <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                    Восстановление пароля
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
        <% using (Html.BeginForm()) { %>
            <p>
                <input type="text" id="username" placeholder="Введите имя пользователя" style="width: 300px;" name="userName" />
                <input type="submit" value="Восстановить" class="button button-confirm" />
            </p>
        <% } %>
        </div>
    </div>
</asp:Content>


