<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SCNews.Models.Content>" %>


<asp:Content ID="Content2" ContentPlaceHolderID="_headContent" runat="server">
	<script src="/Scripts/ckeditor/ckeditor.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                        Редактировать контент
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
            <% using (Html. BeginForm())
               {%>
            <%= Html.ValidationSummary(true) %>
            <fieldset class="fieldset">
                <legend>
                    <%= Html.Encode( Model.ContentType.name ) %></legend>
                <div><%= Html.TextAreaFor(model => model.text) %></div>
                <p>
                    <input type="submit" value="Сохранить" class="button button-confirm" />
                    <a href="/ContentManager" class="button">Вернуться к списку</a>
                </p>
            </fieldset>
            <% } %>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            CKEDITOR.replace('text');
        });
    </script>
</asp:Content>
