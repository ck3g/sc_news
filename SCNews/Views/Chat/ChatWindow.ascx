<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<SCNews.Models.ChatMessage>>" %>

<% foreach (var message in Model) { %>
    <% if (Page.User.IsInRole( "Administrators" )) { %>
    <a href="" class="chat-delete" ref="<%= message.id %>">[x]</a>&nbsp;
    <% } %>
    <span class="chat-username"><a href="/Profile/<%= message.ChatUser.UserName %>" title="@ <%= String.Format( "{0:dd MMMM yyyy | HH:mm}", message.texted_at.AddHours( 3 ) ) %>"><%= message.ChatUser.UserName %></a>: </span><span class="chat-message"><%= Html.Encode( message.text ) %></span><br />
<% } %>

<% if (Page.User.IsInRole( "Administrators" )) { %>
<script type="text/javascript">
    $(function() {
        $("a.chat-delete").click(function() {
            if (confirm("Удалить?")) {
                $("div.chat-window").load("/Chat/RemoveMessage/" + $(this).attr("ref"), function() {
                    $("#message_text").val("");
                    chatScroll();
                });
            }
            return false;
        });
    });
</script>
<% } %>