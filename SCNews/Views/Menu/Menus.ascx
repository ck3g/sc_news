<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<SCNews.Models.Menu>>" %>

<% foreach (var menu in Model) { %>
    <div class="menu-item">
        <input type="text" name="menuNames" class="short" value="<%= Html.Encode( menu.name ) %>" />
        <input type="text" name="menuUrls" class="long" value="<%= Html.Encode( menu.url ) %>" />
        <a href="javascript://" class="remove-item"><img src="/Content/img/delete.png" /></a>
    </div>
<% } %>


