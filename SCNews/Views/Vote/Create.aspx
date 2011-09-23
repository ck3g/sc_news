<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SCNews.Models.Vote>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="miniblock">
    <div class="title-middle">
        <div class="title-left">
            <div class="title-right">
                <div class="title-text">
                Новое голосование
                </div>
            </div>
        </div>
    </div>
    <div class="content">

    <% using (Html.BeginForm()) {%>
        <%= Html.ValidationSummary(true) %>

        <table class="table-editor">
            <tr>
                <th>Название:</th>
                <td><input type="text" name="question_name" class="wide_input" /></td>
            </tr>
            <tr>
                <th>Варианты ответов:</th>
                <td>
                    <div id="answers">
                    </div>
                    <a href="" id="add_answer"><img src="/Content/img/answer_add.png" alt="+" title="Добавить ответ" /></a>
                </td>
            </tr>
            <tr>
                <th></th>
                <td>
                    <input type="submit" class="button button-confirm" value="Создать"/>
                    <%= Html.ActionLink("К списку голосований", "Index", new{ controller="Vote" }, new{ @class="button" }) %>
                </td>
            </tr>
        </table>

    <% } %>

    <script type="text/javascript">
        $(function() {
            $("#add_answer").click(function() {
                $("#answers").append(
                    '<div class="single-answer">' +
                        '<input type="text" name="answers" class="wide_input" style="width: 93%" /> ' +
                        '<a href="" class="remove_answer"><img src="/Content/img/answer_delete.png" alt="X" title="Удалить" /></a>' +
                        '<div style="margin-bottom: 2px;"></div>' +
                    '</div>'
                );
                return false;
            });

            $("a.remove_answer").live("click", function() {
                var answer = $(this).closest("div.single-answer");
                answer.fadeOut("slow", function() {
                    answer.remove();
                });
                return false;
            });


        });
    </script>
    </div>
</div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="_headContent" runat="server">
<title>Новое голосование</title>
</asp:Content>

