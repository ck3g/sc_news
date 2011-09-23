<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div class="newsmacer-menu">
    <%= Html.ActionLink( "Галерея", "Index", new { controller="Gallery" }, new { @class="button" } ) %>
    <%= Html.ActionLink("К списку Новостей", "Index", new { controller="News" }, new { @class="button" }) %>
</div>
