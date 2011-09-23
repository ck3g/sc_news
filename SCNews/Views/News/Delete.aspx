<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SCNews.Models.News>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_headContent" runat="server">
    <title>Удалить</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                    Подтверждение удаления
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
           <p>
                Пожалуйста, подтвердите удаление новости: <i><%= Model.title %></i>
            </p>
            <p>
            <% using (Html.BeginForm()) { %>
                <input type="submit" name="confirmButton" value="Удалить" class="button button-confirm" />
            <% } %>
            </p> 
        </div>
    </div>
 
</asp:Content>
