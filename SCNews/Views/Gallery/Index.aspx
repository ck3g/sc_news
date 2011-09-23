<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SCNews.Helpers.PaginatedList<SCNews.Models.Gallery>>" %>
<%@ Import Namespace="SCNews.Helpers"%>


<asp:Content ID="Content2" ContentPlaceHolderID="_headContent" runat="server">
    <title>StarCraft Галлерея</title>

    <script src="/Scripts/jquery.fancybox-1.3.1.pack.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.easing-1.3.pack.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.mousewheel-3.0.2.pack.js" type="text/javascript"></script>
    <link href="/Content/jquery.fancybox-1.3.1.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                    Галерея
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
            <fieldset>
    <legend>Добавить изображение</legend>
    <% using ( Html.BeginForm( "Create", "Gallery", FormMethod.Post, new { enctype="multipart/form-data" }) ) { %>

    <table class="add-table">
        <tr>
            <th>Файл:</th>
            <td><input type="file" name="picture" id="picture" /></td>
        </tr>
        <tr>
            <th>Название:</th>
            <td>
                <span class="hint-text">Название может помочь при поиске изображений в галерее</span><br />
                <input name="name" id="name" />
            </td>
        </tr>
        <tr>
            <th></th>
            <td><input type="submit" name="submit"  class="button button-confirm" value="Загрузить" /></td>
        </tr>
    </table>
    <% } %>
</fieldset>


    <div class="fancy">
    <p>
    <% foreach (var item in Model) { %>    
        <a rel="gallery" href="/Content/Gallery/<%=item.filename%>" >
            <img src="/Content/Gallery/<%=item.filename%>" alt="<%= Html.Encode( item.name ) %>" /></a>
    <% } %>
     </p>
    </div>
        <%= Html.Paging( "Gallery", Model.PageIndex, Model.TotalPages, Model.HasPreviousPage, Model.HasNextPage ) %>
        </div>
    </div>


<script type="text/javascript">
    $(function() {
        $("a[rel=gallery]").fancybox();
    });
</script>
</asp:Content>


