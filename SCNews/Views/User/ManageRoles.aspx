<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SCNews.Models.Role>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_headContent" runat="server">
	<title>Управление ролями</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="miniblock">
    <div class="title-middle">
        <div class="title-left">
            <div class="title-right">
                <div class="title-text">
                    Список ролей <%= ViewData["UserName"].ToString() %>
                </div>
            </div>
        </div>
    </div>
    <div class="content">
        <% foreach (var item in Model) { %>
	        <p>
                <input type="checkbox" id="role_<%= item.LoweredRoleName %>" class="role_check" rel="<%= item.RoleId %>" value="<%= ViewData["userId"].ToString() %>" <%= Html.Action( "IsInRole", new { roleId = item.RoleId, userId = new Guid( ViewData["userId"].ToString() ) } ) %> />
                <label for="role_<%= item.LoweredRoleName %>"><%= Html.Encode(item.RoleName) %></label>
            </p>
            <% } %>
            <p>
                <%= Html.ActionLink( "Назад к списку пользователей", "", new { controller="User" }, new { @class="button" } ) %>
            </p>
    </div>
</div>


<script type="text/javascript">
    $(function() {
        $("input.role_check").click(function() {
            var role_check = $(this);
            var role_id = role_check.attr("rel");
            var user_id = role_check.val();
            if (role_check.is(":checked")) {
                $.getJSON("/User/AddRole/" + role_id + "/" + user_id);
            }
            else {
                $.getJSON("/User/RemoveRole/" + role_id + "/" + user_id);
            }
        });
    });
</script>

</asp:Content>


