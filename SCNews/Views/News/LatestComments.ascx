<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<SCNews.Models.Comment>>" %>

<ul class="latest-comments">
<% foreach (var item in Model) { %>
    <li>
        <a class="username" href="/Profile/<%= Html.Encode( item.User.UserName ) %>">
            <%= Html.Encode( item.User.UserName ) %></a> &gt; 
        <a href="/News/Details/<%= item.News.id %>#comment_<%= item.id %>"><%= Html.Encode( item.News.title ) %></a><br />
        <span class="time">(<%= String.Format( "{0:dd MMMM yyyy | HH:mm}", item.created_at.Value.AddHours( 3 ) ) %>)</span><br />
        <div class="body">
        <% if (item.body.Length > 50) { %>
            <%= Html.Encode( item.body.Substring( 0, 49 ) ) + "…" %>
        <% } else { %>
            <%= Html.Encode( item.body ) %>
        <% } %>
        </div>
    </li>
    
<% } %>
</ul>
