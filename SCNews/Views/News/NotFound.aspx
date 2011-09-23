<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SCNews.Models.News>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_headContent" runat="server">
	<title>Ошибка</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                    Ошибка
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
            <p>
               <h3> Новость не найдена</h3>
            </p>
            <p>
             <%= Html.ActionLink( "Вернуться к списку новостей", "Index", new { controller="News" }, new { @class="button" } ) %>
            </p>
        </div>
    </div>

</asp:Content>
