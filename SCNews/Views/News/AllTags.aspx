<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                        Облако тегов
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
            <% Html.RenderAction( "TagListAll", "News" ); %>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headContent" runat="server">
</asp:Content>
