<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SCNews.Helpers.PaginatedList<SCNews.Models.Vote>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="miniblock">
    <div class="title-middle">
        <div class="title-left">
            <div class="title-right">
                <div class="title-text">
                Новое голосование
                </div>
            </div>
        </div>
    </div>
    <div class="content">
        <%= Html.ActionLink( "Создать голосование", "Create", new { controller="Vote" }, new{@class="button", title="Создать голосование" } ) %>
        <div class="spacer"></div>
        <% if ( (bool)ViewData["NoPinned"] || (bool)ViewData["NoActive"] ) { %>        
        <div class="message-warning">
            Блок голосований не будет отображаться до тех пор, пока не будет прикреплено одно из активных голосований.
        </div>
        <div class="spacer"></div>	
        <% } %>
        
        <% if ( Model.TotalCount > 0 ) { %>        
        <table class="users-list">
            <thead>
                <tr>
                    <th>Название</th>
                    <th>Состояние</th>
                    <th>Автор</th>
                    <th>Дата</th>
                    <th>Прикреплено</th>
                </tr>
            </thead>
            <tbody>
                <% foreach (var vote in Model) { %>
                <tr>
                    <td><a href="/Vote/Edit/<%= vote.id %>"><%= Html.Encode( vote.question_name ) %></a></td>
                    <% string[] statusImg = { "vote_inactive.png", "vote_active.png", "vote_closed.png" }; %>
                    <%
                        string[] statusText = {
                                "Неактивно: Нельзя участвовать в голосованиях и блок голосований скрыт.",
                                "Активно: Можно участвовать в голосованиях",
                                "Завершено: Нельзя участвовать в голосованиях. Блок голосований отображает результаты голосов."
                            }; %>
                    <td>
                        <img src="/Content/img/<%= statusImg[vote.status] %>" alt="" title="<%= statusText[vote.status] %>" />
                    </td>
                    <td><a href="/Profile/<%= vote.VoteUser.UserName %>" class="username"><%= vote.VoteUser.UserName %></a></td>
                    <td><%= String.Format( "{0:dd MMMM yyyy}", vote.created_at.AddHours( 3 ) )%></td>
                    <td>
                        <% if (vote.is_pinned) { %>
                            <img src="/Content/img/current.png" alt="" title="Текущее голосование" />
                        <% } else { %>
                            <a href="/Vote/Pin/<%= vote.id %>" onclick="if (!confirm( 'Только одно голосование может быть текущим. \nСделать выбранное голосование текущим?' )){ return false;}"><img src="/Content/img/not_current.png" alt="" title="Сделать текущим" /></a>
                        <% } %>
                    </td>
                </tr>        
                <% } %>
            </tbody>
        </table>
        <% } else {%>
            <div class="message-warning">Не создано ни одного голосования</div>
        <% } %>
    </div>
</div>

<div class="pager fl">
<% if (Model.HasPreviousPage) {%>
    <a href="/Vote/Page/<%= Model.PageIndex - 1 %>"><span class="page-numbers">&lt; Пред.</span></a>
<% } %>

<% 
if (Model.TotalPages > 1)
{
    for (var page = 1; page < Model.TotalPages; page++)
    {
        if (Model.PageIndex == page)
        {
            %><span class="page-numbers current"><%= page%></span><%
        }
        else
        {
            %><a href="/Vote/Page/<%= page %>"><span class="page-numbers"><%= page%></span></a><%
        }
    }
}
%>

<% if (Model.HasNextPage) { %>
    <a href="/Vote/Page/<%= Model.PageIndex + 1 %>"><span class="page-numbers">След. &gt;</span></a>
<% } %>

</div>
    
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="_headContent" runat="server">
    <title>Список голосований</title>
</asp:Content>

