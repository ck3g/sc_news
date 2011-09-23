<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                    Ошибка 404
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
            Запрошенная Вами страница не найдена. <a href="<%= Url.Action( "", "" ) %>">Перейти к списку новостей.</a>
        </div>
    </div>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="_headContent" runat="server">
<title>Страница не найдена</title>
</asp:Content>
