<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="passwordResetFinalTitle" ContentPlaceHolderID="_headContent" runat="server">
    <title>Смена пароля</title>
</asp:Content>

<asp:Content ID="passwordResetFinalContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                    Пароль восстановлен
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
            Сгенерированный пароль был выслан на Email указанный при регистрации.
        </div>
    </div>
</asp:Content>