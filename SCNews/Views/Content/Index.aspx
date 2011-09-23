<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SCNews.Models.Content>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                        Управление контентом
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
            <table class="users-list">
                <thead>
                    <tr>
                        <th>Название</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <% foreach (var item in Model) { %>
                    <tr>
                        <td>
                            <%= Html.Encode(item.ContentType.name) %>
                        </td>
                        <td class="table-actions">
                            <%= Html.ActionLink("Редактировать", "Edit", new { id=item.type_id }) %>
                        </td>
                    </tr>
                    <% } %>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headContent" runat="server">
</asp:Content>
