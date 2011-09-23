<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="_headContent" runat="server">
	
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Index</h2>

    <p><%= Encoding.ASCII.GetChars((byte[])ViewData["GameInfo"])%></p>
    <p><%= ViewData["Exists"] %></p>
    <p><%= ViewData["GameSpeed"] %></p>
    <p><%= ViewData["Map"] %></p>
    <p><%= ViewData["ImageSize"] %></p>
    <p><%= ViewData["PlayersCount"] %></p>
    <p><%= ViewData["Players"] %></p>

</asp:Content>

