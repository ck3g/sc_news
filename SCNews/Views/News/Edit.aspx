<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SCNews.Models.News>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_headContent" runat="server">
    <title>Редактировать новость</title>
	<script src="/Scripts/ckeditor/ckeditor.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-1.8.1.custom.min.js" type="text/javascript"></script>
    <link href="/Content/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" />
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function() {
            CKEDITOR.replace('body');
        });
    </script>

     <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                    Редактировать новость
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
            <% Html.RenderPartial( "NewsmakerMenu" ); %>

            <% using (Html.BeginForm()) {%>
                <%= Html.ValidationSummary(true) %>

                    <p>
                    <div class="editor-label">
                        Залоговок:
                    </div>
                    <div class="editor-field">
                        <%= Html.TextBoxFor( model => model.title, new { @class = "wide_input" } )%>
                        <%= Html.ValidationMessageFor(model => model.title) %>
                    </div>
                    </p>
                    <p>
                    <div class="editor-label">
                        Новость:
                    </div>
                    <div class="editor-field">
                        <%= Html.TextAreaFor(model => model.body) %>
                        <%= Html.ValidationMessageFor(model => model.body) %>
                    </div>
                    </p>
                    <p>
                    <div class="editor-label">
                        <%= Html.LabelFor(model => model.type) %>
                    </div>
                    <div class="editor-field">
                        <%= Html.DropDownList( "types" ) %>
                        <%= Html.ValidationMessageFor(model => model.type) %>
                    </div>
                    </p>
                    <div>
                        <input type="hidden" name="all_tags" id="all_tags" value="<%= ViewData["AllTags"] ?? "" %>" />
                        <input type="hidden" name="news_tags" id="news_tags" value="<%= ViewData["NewsTags"] ?? "" %>" />
                        <% Html.RenderPartial( "ManageTagsPanel" ); %>
                    </div>
                    <p>
                        <% Html.RenderPartial( "Instructions" ); %>
                    </p>
                                            
                    <p>
                        <input type="submit" value="Сохранить" class="button button-confirm" />
                    </p>

            <% } %>

        </div>
    </div>

</asp:Content>

