<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SCNews.Models.MenuType>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

      <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                    Настройка меню
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
            <% using (Html.BeginForm( "Edit", "Menu", FormMethod.Post )) { %>
                <p>Тип меню: 
                <select name="type" id="menu_type">
                    <% foreach (var type in Model) { %>
                    <option value="<%= type.id %>"><%= Html.Encode( type.type_name ) %></option> 
                    <% } %>
                </select>
                </p>
                <div id="menus"></div>
                <p>
                    <input type="button" id="add" class="button" value="Добавить" />
                    <input type="submit" class="button button-confirm" value="Сохранить" />
                </p>
            <% } %>
        </div>
    </div>
    <script type="text/javascript">
        function ChangeContent() {
            var typeId = $("#menu_type").val();
            $("#menus").load("/Menu/Menus/", { "typeId": typeId });
        }

        $(function() {
            ChangeContent();

            $("#add").click(function() {
                var menuItem = '<div class="menu-item">\
                                     <input type="text" name="menuNames" class="short" />\
                                     <input type="text" name="menuUrls" class="long" />\
                                     <a href="javascript://" class="remove-item"><img src="/Content/img/delete.png" /></a>\
                                 </div>';
                $("#menus").append(menuItem);
            });

            $("#menus a.remove-item").live("click", function() {
                var item = $(this).closest("div.menu-item");
                item.fadeOut("slow", function() {
                    item.remove();
                });
                return false;
            });

            $("#menu_type").change(function() {
                ChangeContent();
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="_headContent" runat="server">
</asp:Content>

