<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SCNews.Models.Comment>" %>

<%  if (Request.IsAuthenticated)
    { %>

    <form action="/News/CreateComment" method="post" id="comment_add">
        <%= Html.ValidationSummary( true )%>
    
        <div class="block-content">
            <%= Html.TextAreaFor( model => model.body, new { @class = "wide_textarea" } )%>
            <%= Html.ValidationMessageFor( model => model.body )%>
            <%= Html.Hidden( "newsId", (Int64)Session["newsId"] )%>
        </div>
        <div class="block-content">
            <input type="submit" class="button button-confirm" value="Оставить комментарий" />
        </div>
    </form>

<% } else { %>
	<div class="block-content-notice">
        <p>
            Чтобы оставить комментарий, вы должны <a href="<%= Url.Action( "LogOn", "Account", new RouteValueDictionary( new { returnUrl = Request.RawUrl }) ) %>">активировать</a> вашу учетную запись или <%= Html.ActionLink( "зарегистрироваться", "Register", "Account" ) %>.
            
        </p>
    </div>
    
<% } %>

