<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>

<asp:Content ID="errorTitle" ContentPlaceHolderID="_headContent" runat="server">
    <title>Ошибка</title>
</asp:Content>

<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                    Ошибка
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
            <h3>Произошла ошибка при обработке вашего запроса.</h3>
        </div>
    </div>
    
</asp:Content>
