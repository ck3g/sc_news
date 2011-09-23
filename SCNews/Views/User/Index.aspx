<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SCNews.Helpers.PaginatedList<SCNews.Models.User>>" %>
<%@ Import Namespace="SCNews.Helpers"%>
<%@ Import Namespace="System.Security.Policy"%> 

<asp:Content ID="Content1" ContentPlaceHolderID="_headContent" runat="server">
	<title>Список пользователей</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="miniblock">
    <div class="title-middle">
        <div class="title-left">
            <div class="title-right">
                <div class="title-text">
                Список пользователей
                </div>
            </div>
        </div>
    </div>
    <div class="content">
        <div id="message" style="display: none;"></div>
        <div id="preloader" class="preloader" style="display: none;">
            <img src="/Content/img/preloaders/preloader1.gif" alt=""/>
            <div id="preloader_text"></div>
        </div>
	    <div class="spacer"></div>
	    <div class="newsmaker-menu">
            <input type="button" id="reload" class="button" value="Обновить Статистику" />
	    </div>
        <table class="users-list">
            <thead>
                <tr>
                    <th>Имя пользователя</th>
                    <th>Действия</th>
                </tr>
            </thead>
            <tbody>
            <% List<String> bnetUsers = new List<string>(); %>
            <% foreach (var item in Model) { %>
                <tr>
                    <%
                        if (item.UsersProfile != null && !String.IsNullOrEmpty(item.UsersProfile.profile_url))
                        {
                            bnetUsers.Add( "\"" + item.UserName + "\"" );
                        }
                    %>
                    <td><a href="/Profile/<%= Html.Encode( item.UserName ) %>"> <%= Html.Encode( item.UserName ) %></a></td>
                    <td class="table-actions">
                        <a href="/User/ManageRoles/<%= Html.Encode( item.UserName ) %>">
                            <img src="/Content/img/design/UsersRoles.png" alt="Управление ролями" title="Управление ролями" /></a>
                        <% if (item.UsersProfile != null && !String.IsNullOrEmpty(item.UsersProfile.profile_url)) { %>
                        <a href="javascript://" class="bnet-refresh" rel="<%= Html.Encode( item.UserName ) %>">
                            <img src="/Content/img/design/bnet_info_refresh.png" style="height: 24px;" alt="" title="Обновить Battle.Net статистику" /></a>
                        <% } %>
                    </td>
                </tr>
            <% } %>
            
            </tbody>
        </table>
        <% var jsBnetUsers = String.Join( ", ", bnetUsers.ToArray() ); %>
    </div>
</div>

<%= Html.Paging( "User", Model.PageIndex, Model.TotalPages, Model.HasPreviousPage, Model.HasNextPage ) %>

<script type="text/javascript">

    $(function() {
        $("a.bnet-refresh").click(function() {
            var a = $(this);
            var username = a.attr("rel");
            $("#message").removeClass().hide();
            $("#preloader").show();
            $.getJSON("/User/ReloadBnetData", { "username": username }, function(data) {
                $("#preloader").hide();
                if (data.result == "ok") {
                    var msg = "Данные успешно обновлены";
                    $("#message").addClass("message-info").html(msg).show();
                }
                else {
                    var msg = "Ошибка при обновлении данных";
                    $("#message").addClass("message-error").html(msg).show();
                }
            });
        });

        $("#reload").click(function() {
            $("#message").removeClass().hide();
            $("#preloader").show();
            $.getJSON("/User/GetBnetUsers", function(users) {
                var total = users.length;
                var current = 0;
                var fails = 0;
                for (var i in users) {
                    $("#preloader_text").html("Получение данных " + (current + 1) + " из " + total);
                    $("#preloader").show();
                    $.getJSON("/User/ReloadBnetData", { "username": users[i] }, function(data) {
                        if (data.result != "ok")
                            fails++;

                        current++;
                        $("#preloader_text").html("Получение данных " + (current + 1) + " из " + total);
                        if (current >= total) {
                            $("#preloader").hide();
                            if (total > fails) {
                                var msg = "Данные успешно обновлены для " + (total - fails) + " из " + total + " профайлов";
                                $("#message").addClass("message-info").html(msg).show();
                            }
                            else {
                                var msg = "Ошибка при обновлении данных";
                                $("#message").addClass("message-error").html(msg).show();
                            }

                        }
                    });
                }
            });


        });
    });
</script>

</asp:Content>

