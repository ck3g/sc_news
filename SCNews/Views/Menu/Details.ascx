<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<SCNews.Models.Menu>>" %>

<ul class="menu">
<% foreach (var menu in Model) { %>
    <li><a href="<%= menu.url %>"><%= Html.Encode( menu.name ) %></a></li>
<% } %>
</ul>

