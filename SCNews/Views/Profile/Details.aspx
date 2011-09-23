<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SCNews.Models.UsersProfile>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_headContent" runat="server">
    <title>Профайл пользователя - <%= Html.Encode(Model.User.UserName) %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                        Профайл пользователя - <%= Html.Encode(Model.User.UserName) %>
                    </div>
                </div>
            </div>
        </div>
        <div class="content profile-content">
            <fieldset>
                <legend>Общая информация</legend>
    	        <strong>Никнейм на сайте:</strong> <%= Html.Encode(Model.User.UserName) %><br />				
                <strong>Реальное имя:</strong> <%= Html.Encode(Model.first_name) %> <%= Html.Encode(Model.last_name) %><br />				
        		
                <strong>Страна проживания:</strong> <%= Html.Encode( ViewData["Country"].ToString() ) %><br />		
                <br />
                <strong>Дата регистрации:</strong> <%= String.Format( "{0:dd MMMM yyyy (HH:mm)}", Model.User.UsersMembership.CreateDate.AddHours( 3 ) )%><br />				
                <strong>Последний раз заходил:</strong> <%= String.Format( "{0:dd MMMM yyyy (HH:mm)}", Model.User.LastActivityDate.AddHours( 3 ) )%><br />
            </fieldset>
            <% if (!String.IsNullOrEmpty( Model.profile_url )) { %>
            <table class="bnet-table">
                <tr>
                    <td>
                        <fieldset>
                            <legend>Battle.Net информация</legend>
                            <table class="bnet-profile">
                                <tr>
                                    <td class="bnet-userpic">
                                        <%= Html.Action( "GetAvatarHtml", new { controller = "Profile", username = Model.User.UserName } )%>
                                        <% if (User.Identity.Name.ToLower() == Model.User.LoweredUserName) { %>
                                        <div id="reload_button"><input type="button" class="button" style="width: 90px; margin-top: 3px;" id="reload" value="Обновить" rel="<%= User.Identity.Name %>" /></div>
                                        <div id="preloader" style="display: none; margin-top: 5px;"><img src="/Content/img/preloaders/preloader2.gif" alt="" /></div>
                                        <% } %>
                                    </td>
                                    <td>
                                        <ul>
                                            <li><strong>Никнейм в сети Battle.net:</strong> <a href="<%= Model.profile_url %>"><%= Html.Encode( Model.bnet_name ) %></a></li>
                                            <li><strong>Сервер:</strong> <img src="/Content/img/flags/<%= Html.Encode( Model.bnet_server ) %>.gif" alt="" style="vertical-align: middle;"/><%= Html.Action( "ServerToText", new { server = Model.bnet_server } ) %></li>
                                            <li><strong>Очки достижений:</strong> <span class="achievement"><%= Html.Encode( Model.achievements ) %></span></li>
                                            <% if (!String.IsNullOrEmpty( Model.race )) { %>
                                            <li><strong>Любимая раса:</strong> <img src="/Content/img/races/<%= Html.Encode( Model.race ) %>.png" alt="" style="vertical-align: middle;"/><%= Html.Action( "RaceToText", new { race = Model.race } )%></li>
                                            <% } %>
                                            <li><strong>Последнее обновление:</strong> <%= String.Format( "{0:dd MMMM yyyy (HH:mm)}", Model.synchronized_at.Value.AddHours( 3 ) )%></li>
                                        </ul>
                                    </td>

                                </tr>
                            </table>
                            
                        </fieldset>
                    </td>
                    <td style="width: 200px;">
                        <fieldset>
                            <legend>Статистика матчей</legend>
                            <table class="bnet-profile">
                                <tr>
                                    <% if (!String.IsNullOrEmpty( Model.race )) { %>
                                    <td class="bnet-league">
                                        <% var leagueParts = Model.league.Split('_'); %>
                                        <% if (leagueParts.Count() == 2) { %>
                                        <span class="<%= "badge badge-" + leagueParts[0] + " badge-medium-" + leagueParts[1] %>"></span>
                                        <% } %>
                                    </td>
                                    <td class="bnet-stats">
                                        <ul>
                                            <li><strong>Тип игры:</strong> 1х1</li>
                                            <li><strong>Место:</strong> <%= Html.Encode( Model.rank ) %></li>
                                            <li><strong>Очки:</strong> <%= Html.Encode( Model.points ) %></li>
                                            <li><strong>Побед:</strong> <%= Html.Encode( Model.wins ) %></li>
                                        </ul>
                                    </td>
                                    <% } else { %>
                                    <td class="bnet-stats">
                                        <ul>
                                            <li><strong>Нет сыграных матчей</strong></li>
                                        </ul>
                                    </td>
                                    <% } %>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
            </table>
            <% } %>
            
            <% if (!String.IsNullOrEmpty( Model.details )) { %>
            <fieldset>
                <legend>Дополнительно</legend>
                <strong>Дополнительная информация:</strong><br /> <%= Html.Encode( Model.details ).Replace( "\r\n", "<br />" ) %>
            </fieldset>
            <% } %>
            
            <% if (User.Identity.Name.ToLower() == Model.User.LoweredUserName) { %>
            <p>
                <%= Html.ActionLink( "Редактировать", "Edit", new { controller="Profile" }, new { @class="button" } )%>
            </p>
            <% } %>
                        
        </div>
    </div>
   
<script type="text/javascript">
    $(function() {
        $("#reload").click(function() {
            var username = $(this).attr( "rel" );
            $("#reload_button").hide();
            $("#preloader").show();
            $.getJSON("/User/ReloadBnetData", { "username":username }, function(data) {
                window.location.href = "/Profile";
            });

        });
    });
</script>
   
</asp:Content>
