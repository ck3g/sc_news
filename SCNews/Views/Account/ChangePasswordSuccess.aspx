<%@Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="changePasswordTitle" ContentPlaceHolderID="_headContent" runat="server">
    <title>Смена пароля</title>
</asp:Content>

<asp:Content ID="changePasswordSuccessContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                    Изменение пароля
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
        <p>
            <h3>Ваш пароль успешно изменен.</h3>
        </p>    
        </div>
    </div>
    
</asp:Content>
