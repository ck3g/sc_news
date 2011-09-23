<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<PaginatedList<SCNews.Models.Replay>>" %>
<%@ Import Namespace="SCNews.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                        Реплеи
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
            <% if (Page.User.IsInRole( "Administrators" ) || Page.User.IsInRole( "Replays" )) {%>
                <div class="newsmaker-menu">
                    <a href="/Replays/Create" class="button">Загрузить</a>
                </div>
            <% } %>

            <% foreach ( var replay in Model ) { %>
                <div class="replay-block">
                    <div class="left">
                        <a href="/Content/Replays/<%= replay.file_name %>"><%= Html.Encode( replay.name ) %></a> 
                        (<%= String.Format( "{0:0.##}", Convert.ToSingle( replay.file_size ) / 1024 / 1024) %> Мб)
                        <% if (Page.User.IsInRole( "Administrators" ) || Page.User.IsInRole( "Replays" )) {%>
                            <a href="javascript://" class="delete-replay" rel="<%= replay.id %>"><img src="/Content/img/design/delete.png" alt="Удалить" title="Удалить" style="height: 16px;" /></a>
                        <% } %>
                    </div>
                    <div class="right">
                        Загрузил: <a href="/Profile/<%= replay.ReplayUser.UserName %>" class="username"><%= replay.ReplayUser.UserName %></a> @ <%= String.Format( "{0:dd MMMM yyyy}", replay.uploaded_at.AddHours( 3 ) ) %>
                    </div>
                    <div class="clear"></div>
                    <div class="description"><%= Html.Encode( replay.description ) %></div>
                    <hr class="hr"/>
                </div>
            <% } %>
            
            <%= Html.Paging( "Replays", Model.PageIndex, Model.TotalPages, Model.HasPreviousPage, Model.HasNextPage ) %>
        </div>
    </div>
    <% if (Page.User.IsInRole( "Administrators" ) || Page.User.IsInRole( "Replays" )) {%>
        <script type="text/javascript">
            $(function () {
                $(".delete-replay").click(function () {
                    var $a = $(this);
                    var id = $a.attr("rel");
                    if (confirm("Удаленный реплей восстановлению не подлежит. Продолжить удаление?")) {
                        $.getJSON("/Replays/Delete", { "id": id }, function (data) {
                            if (data.result) {
                                $a.closest("div.replay-block").fadeOut("slow", function () {
                                    $(this).remove();
                                });
                            }
                        });
                    }

                    return false;
                });
            });
        </script>
    <% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headContent" runat="server">
</asp:Content>
