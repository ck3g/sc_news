<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Linq.IQueryable<SCNews.Models.Tag>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                    Управление тегами
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
            <div class="manage-tags">
                <% foreach (var item in Model) { %>
                <div class="tag">
                    <div class="name-panel">
                        <span class="name-text"><%= Html.Encode( item.name ) %></span>
                        <input type="text" class="name-input" value="<%= Html.Encode( item.name ) %>" />
                    </div>
                    <div class="edit-panel">
                        <a href="javascript://" class="save-edit" rel="<%= item.id %>"><img src="/Content/img/design/accept.png" alt="Сохранить" title="Сохранить" /></a>
                        <a href="javascript://" class="cancel-edit"><img src="/Content/img/design/pencil_delete.png" alt="Отменить" title="Отменить" /></a>
                    </div>
                    <div class="actions-panel">
                        <a href="javascript://" class="edit-tag"><img src="/Content/img/design/tag_blue_edit.png" alt="Редактировать" title="Редактировать" /></a>
                        <a href="javascript://" class="remove-tag" rel="<%= item.id %>"><img src="/Content/img/design/tag_blue_delete.png" alt="Удалить" title="Удалить" /></a>
                    </div>
                    <div class="clear"></div>
                </div>
                <% } %>
            </div>
        </div>
    </div>
    
    <script type="text/javascript">
        function HideEditPanel( $tag ) {
            $tag.find(".name-text").show();
            $tag.find(".name-input").hide();
            $tag.find(".edit-panel").hide();
            $tag.find(".actions-panel").show();
        }

        $(function () {
            var canShowActionsPanel = true;

            $(".tag").mouseover(function () {
                if (canShowActionsPanel)
                    $(this).find(".actions-panel").show();
            });

            $(".tag").mouseout(function () {
                $(this).find(".actions-panel").hide();
            });

            $(".edit-tag").click(function () {
                canShowActionsPanel = false;
                var $tag = $(this).closest(".tag");
                $tag.find(".name-text").hide();
                $tag.find(".name-input").show();
                $tag.find(".actions-panel").hide();
                $tag.find(".edit-panel").show();
            });

            $(".cancel-edit").click(function () {
                HideEditPanel($(this).closest(".tag"));
                canShowActionsPanel = true;
            });

            $(".save-edit").click(function () {
                var tagId = $(this).attr("rel");
                $tag = $(this).closest(".tag");
                var tagName = $tag.find(".name-input").val();
                $.post("/News/RenameTag", { id: tagId, name: tagName }, function () {
                    HideEditPanel($tag);
                    $tag.find(".name-text").html(tagName);
                    canShowActionsPanel = true;
                });
            });

            $(".remove-tag").click(function () {
                var $cancelLink = $(this);
                var tagId = $cancelLink.attr("rel");
                var $tag = $cancelLink.closest(".tag");
                if (confirm("Удаленный тег восстановлению не подлежит. Удалить?")) {
                    $.post("/News/DeleteTag", { id: tagId }, function () {
                        $tag.fadeOut( "slow", function () {
                            $tag.remove();
                        });
                    })
                }
            });
        });
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="_headContent" runat="server">
</asp:Content>

