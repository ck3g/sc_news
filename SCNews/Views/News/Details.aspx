<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SCNews.Controllers.NewsComments>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_headContent" runat="server">
    <title>
        <%= Html.Encode( Model.News.title ) %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% if (Request.IsAuthenticated) { %>

    <script type="text/javascript">
        $(function() {
            $("#vote_for").click(function() {
                var newsId = $(this).attr("rel");
                $.getJSON("/News/VoteFor/" + newsId, function(data) {
                    $("#vote").text(data.voteFor);
                });
                return false;
            });
        });
    </script>

    <% }
       else { %>

    <script type="text/javascript">
        $(function() {
            $("#vote_for").click(function() {
                return false;
            });
        });
    </script>

    <% } %>
        <% MvcHtmlString parsedBody; %>
        <div class="news-block">
            <div class="news-title-middle">
                <div class="news-title-left">
                    <div class="news-title-right">
                        <div class="news-title">
                            <h3>
                                <a href="/News/Details/<%= Model.News.id %>"><%= Html.Encode( Model.News.title )%></a>
                                <% if( Page.User.IsInRole( "Administrators" ) || Page.User.IsInRole( "News" ) ) {%>
                                <div class="wrap">
                                    <span class="down-arrow"></span>
                                    <div class="admin-menu-popup hidden">
                                        <ul>
                                            <li><%= Html.ActionLink( "Редактировать", "Edit", new { id = Model.News.id } )%></li>
                                            <li><%= Html.ActionLink( "Удалить", "Delete", new { id = Model.News.id } )%></li>
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
                        <li>Опубликовал: <a href="/Profile/<%= Model.News.User.UserName %>" class="username"><%= Model.News.User.UserName%></a></li>
                        <li>@ <%= String.Format( "{0:dd MMMM yyyy}", Model.News.created_at.Value.AddHours( 3 ) )%></li>
                    </ul>
                    
                </div>
                <div class="clear"></div>
                    <% parsedBody = Html.Action( "ParseNewsBody", new { Model.News.body } );%>
                    <%= parsedBody.ToString().Replace( "&lt;cut&gt;", "" ) %>
	            <div class="comments-panel">
	                <ul>
	                    <li>Комментариев: <strong><a href="/News/Details/<%= Model.News.id %>"><%= Html.Action( "GetCommentsCount", new { newsId = Model.News.id } )%></a></strong> |</li>
	                    <li>
	                        Просмотров: 
	                        <strong>
	                            <%= Model.News.hits%>
	                            <% if( Page.User.IsInRole( "Administrators" ) || Page.User.IsInRole( "News" ) ) {%>
	                                &nbsp;(<%=Model.News.views%>)
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
                <%= ViewData["Tags"].ToString() %>
                </div>
                <div class="like-buttons">
        		    <ul>
		                <li><iframe src="http://www.facebook.com/plugins/like.php?href=http://starcraft.md/News/Details/<%= Model.News.id %>&amp;layout=button_count&amp;show_faces=true&amp;width=100&amp;action=like&amp;font=lucida+grande&amp;colorscheme=light&amp;height=21" scrolling="no" frameborder="0" style="border:none; overflow:hidden; width:100px; height:23px;" allowTransparency="true"></iframe></li>
		                <li><a href="http://twitter.com/share" class="twitter-share-button" data-url="http://starcraft.md/News/Details/<%= Model.News.id %>" data-text="<%= Html.Encode( Model.News.title ) %>" data-count="none">Tweet</a><script type="text/javascript" src="http://platform.twitter.com/widgets.js"></script></li>
		                <%--<li><div class="my_rank"><a href="" class="rank_up" rel="<%= item.id %>"><span id="Span1"><%= Html.Encode( item.voted_for ) %></span></a></div></li>--%>
		            </ul>
                </div>
                <div class="clear"></div>
            </div>
        </div>

        

        <div class="spacer"></div>    

        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                    Комментарии к новости
                    </div>
                </div>
            </div>
        </div>

        <% if (Model.Comments.Count() != 0) { %>
        <% foreach (var comment in Model.Comments) {%>
            <div class="comment-block" id="comment_<%= comment.id %>">
                <div class="comment-header">
                    <div class="comment-userpic" >
                    <a href="/Profile/<%= comment.User.UserName %>"><%= Html.Action( "GetAvatarHtml", new { controller = "Profile", username = comment.User.UserName } )%></a>
                    </div>
                    <div class="comment-userinfo">
                        <ul>
                            <li><a class="username" href="/Profile/<%= comment.User.UserName %>"><%= comment.User.UserName%></a></li>
                            <li><strong>Команда:</strong> - </li>
                            <li><strong>Любимая раса:</strong><img src="/Content/img/races/<%=comment.User.UsersProfile.race%>.png" alt="" /><%= Html.Action( "RaceToText", new { controller="Profile", race = comment.User.UsersProfile.race } )%></li>
                            <li><strong>Звание:</strong> Рядовой</li>
                            <li><strong>Опыт:</strong> <a href="#"><%= comment.User.UsersProfile.experience%></a></li>
                        </ul>
                    </div>
                    <div class="comment-rewards" style="display: none;">
                        <span>Участие в турнирах сайта:</span>
                        <ul class="comment-medals">
                            <li>Медаль 1</li>
                            <li>Медаль 2</li>
                            <li>Медаль 3</li>
                        </ul>
                        <ul class="comment-winner">
                            <li>Победитель 1</li>
                            <li>Победитель 2</li>
                        </ul>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="comment-body">
                    <%= Html.Encode( comment.body ).Replace( "\r\n", "<br />" )%>
                </div>
                <div class="comment-footer">
                    <div class="comment-panel">
                        Комментарий написан: <%= String.Format( "{0:dd MMMM yyyy | HH:mm}", comment.created_at.Value.AddHours( 3 ) )%>
                    </div>
                    <% if( Page.User.IsInRole( "Administrators" ) || Page.User.IsInRole( "News" ) ) {%>
                    <div class="comment-admin">
                        <div class="wrap">
                            <span class="comment-ip"><%= Page.User.IsInRole( "Administrators" ) ? comment.ip_address : ""%> <img class="tool-menu" src="/Content/img/design/tools26.png" alt="" /></span>
                            <div class="comment-admin-menu-popup hidden">
                                <ul>
                                    <li><a href="javascript://" class="remove-comment" ref="<%= Url.Action( "CommentRemove", new {id = comment.id} ) %>">Удалить</a></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                    <% } %>
                    
                    <div class="clear"></div>
                </div>
            </div>
        <% } %>
    <% } %>
    <% else {%>
        <div class="block-content-notice">
            <p>Для этой новости еще нет комментариев, но вы можете стать первым.</p>
        </div>
        <div class="spacer">
        </div>
    <% } %>
    
    <% Session["newsId"] = Model.News.id; %>
    <% Html.RenderPartial( "CreateComment", Model.Comment ); %>    
    
    
<% if (Page.User.IsInRole( "Administrators" ) || Page.User.IsInRole( "News" ) ) {%>
<script type="text/javascript">
    $(function() {
        $("a.remove-comment").click(function() {
            var comment = $(this);
            if (confirm("Удаленный комментарий восстановлению не подлежит. Продолжить удаление?")) {
                $.getJSON(comment.attr("ref"), function(data) {
                    if (data.result) {
                        comment.closest("div.comment-block").fadeOut("slow", function() {
                            $(this).remove()
                        });
                        return false;
                    }
                });
            }
            return false;
        });

        $("span.down-arrow").click(function() {
            $(this).next("div").toggleClass("hidden");
        });

        $("img.tool-menu").click(function() {
            $(this).parent().next("div").toggleClass("hidden");
        });

        $(".news-body, .comment-header, .comment-body").click(function() {
            $(".admin-menu-popup").addClass("hidden");
            $(".comment-admin-menu-popup").addClass( "hidden" );
        });
    });
</script>    
<% } %>
    
</asp:Content>
