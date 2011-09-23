<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="_headContent" runat="server">
    <title>О Нас</title>
</asp:Content>
<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                    О Нас
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
            <%= Html.Action( "Details", new { controller = "Content", id = 1 } ) %>
        </div>
    </div>
</asp:Content>
