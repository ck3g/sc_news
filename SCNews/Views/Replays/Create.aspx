<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SCNews.Models.Replay>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                        Добавить реплей
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
            <form action="/Replays/Create" method="post" enctype="multipart/form-data">
            <table class="table-editor">
                <tr>
                    <th>
                        Название:
                    </th>
                    <td>
                        <input type="text" name="name" class="wide_input" value="<%= ViewData["name"] %>" />
                    </td>
                </tr>
                <tr>
                    <th>
                        Файл:
                    </th>
                    <td>
                        <input type="file" name="file" id="replay_file" />
                    </td>
                </tr>
                <tr>
                    <th>
                        Описание:
                    </th>
                    <td>
                        <div id="len">
                            Осталось символов: <span id="chars">500</span></div>
                        <textarea id="description" name="description" class="wide_textarea"><%= ViewData["Description"] %></textarea>
                    </td>
                </tr>
                <tr>
                    <th>
                    </th>
                    <td>
                        <input type="submit" class="button button-confirm" value="Загрузить" />
                        <%= Html.ActionLink("К списку реплеев", "Index", new{ controller="Replays" }, new{ @class="button" }) %>
                    </td>
                </tr>
            </table>
            </form>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            var max_len = 500;
            $("#description").keyup(function () {
                var len = this.value.length;
                if (len >= max_len)
                    this.value = this.value.substring(0, max_len);

                var remaining = max_len - len;
                remaining = remaining >= 0 ? remaining : 0;
                $('#chars').text(remaining);
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headContent" runat="server">
</asp:Content>
