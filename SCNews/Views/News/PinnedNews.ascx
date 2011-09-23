<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<SCNews.Models.News>>" %>

<% if (Model.Count() > 0) { %>
   <% MvcHtmlString parsedBody; %>

    <% foreach (var item in Model) { %>
        <div class="news-block">
            <div class="news-title-middle">
                <div class="news-title-left">
                    <div class="news-title-right">
                        <div class="news-title">
                            <h3>
                                <a href="/News/Details/<%= item.id %>"><%= Html.Encode( item.title ) %></a>
                                <% if( Page.User.IsInRole( "Administrators" ) || Page.User.IsInRole( "News" ) ) {%>
                                <div class="wrap">
                                    <span class="down-arrow"></span>
                                    <div class="admin-menu-popup hidden">
                                        <ul>
                                            <li><%= Html.ActionLink( "Редактировать", "Edit", new { id = item.id } )%></li>
                                            <li><%= Html.ActionLink( "Удалить", "Delete", new { id = item.id } )%></li>
                                        </ul>
                                    </div>
                                </div>
                                <% } %>
                                </h3>
                        </div>
                    </div>
                </div>
            </div>
            
            <div class="news-body">
                <div class="news-details">
                    <ul>
                        <li>Опубликовал: <a href="/Profile/<%= item.User.UserName %>" class="username"><%= item.User.UserName %></a></li>
                        <li>@ <%= String.Format( "{0:dd MMMM yyyy}", item.created_at.Value.AddHours( 3 ) ) %></li>
                    </ul>
                    
                </div>
                <div class="clear"></div>
	            <% parsedBody = Html.Action( "ParseNewsBody", new {item.body} );%>
	            <%= parsedBody.ToString().Split( new string[] { "&lt;cut&gt;" }, StringSplitOptions.RemoveEmptyEntries )[0]  %>
	            <div class="link-more">
	            <%= Html.ActionLink("далее...", "Details", new { id=item.id })%>
	            </div>
	            <div class="comments-panel">
	                <ul>
	                    <li>Комментариев: <strong><a href="/News/Details/<%= item.id %>"><%= Html.Action( "GetCommentsCount", new { newsId = item.id } )  %></a></strong> |</li>
	                    <li>
	                        Просмотров: 
	                        <strong>
	                            <%= item.hits%>
	                            <% if( Page.User.IsInRole( "Administrators" ) || Page.User.IsInRole( "News" ) ) {%>
	                                &nbsp;(<%=item.views%>)
	                            <% } %>
	                        </strong>
	                    </li>
	                    <%--<li>Просмотров: <%= item.hits %> / <%= item.views %></li>--%>
	                </ul>
	            </div>
	            <div class="clear"></div>
            </div>
            <div class="news-panel">
                <div class="news-tags">
                <% var tags = (Dictionary<Int64, String>)ViewData["Tags"]; %>
                <%= tags[item.id] %>
                </div>
                <div class="like-buttons">
        		    <ul>
		                <li><iframe src="http://www.facebook.com/plugins/like.php?href=http://starcraft.md/News/Details/<%= item.id %>&amp;layout=button_count&amp;show_faces=true&amp;width=100&amp;action=like&amp;font=lucida+grande&amp;colorscheme=light&amp;height=21" scrolling="no" frameborder="0" style="border:none; overflow:hidden; width:100px; height:23px;" allowTransparency="true"></iframe></li>
		                <li><a href="http://twitter.com/share" class="twitter-share-button" data-url="http://starcraft.md/News/Details/<%= item.id %>" data-text="<%= Html.Encode( item.title ) %>" data-count="none">Tweet</a><script type="text/javascript" src="http://platform.twitter.com/widgets.js"></script></li>
		                <%--<li><div class="my_rank"><a href="" class="rank_up" rel="<%= item.id %>"><span id="Span1"><%= Html.Encode( item.voted_for ) %></span></a></div></li>--%>
		            </ul>
                </div>
                <div class="clear"></div>
            </div>
        </div>
    <% } %>

<% } %>