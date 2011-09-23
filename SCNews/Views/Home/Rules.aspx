<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_headContent" runat="server">
	<title>Правила пользования</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                    Правила
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
            <%= Html.Action( "Details", new { controller = "Content", id = 2 } ) %>
        </div>
    </div>

</asp:Content>
