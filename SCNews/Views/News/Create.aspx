<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SCNews.Models.News>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_headContent" runat="server">
	<title>Добавить новость</title>
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
                    Добавить новость
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
            <font class="news_short">
                <% Html.RenderPartial( "NewsmakerMenu" ); %>
                
               <% using (Html.BeginForm()) {%>
                    <%= Html.ValidationSummary(true) %>
                        <p>
    
                        
                        <div class="editor-label">
                            Заголовок:
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
                        <div>
                            <input type="hidden" name="all_tags" id="all_tags" value="<%= ViewData["AllTags"] ?? "" %>" />

                            <% Html.RenderPartial( "ManageTagsPanel" ); %>
                        </div>
                        <p>
                        <% Html.RenderPartial( "Instructions" ); %>
                        </p>
                        
                        <p>
                            <input type="submit" value="Добавить" class="button button-confirm" />
                        </p>
                    

                <% } %>

	            </font>
        </div>
    </div>


</asp:Content>


