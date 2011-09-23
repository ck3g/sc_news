<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SCNews.Controllers.Voting>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="miniblock">
    <div class="title-middle">
        <div class="title-left">
            <div class="title-right">
                <div class="title-text">
                Редактирование голосования
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
            <td><input type="input" name="question_name" class="wide_input" value="<%= Html.Encode( Model.Vote.question_name ) %>" /></td>
        </tr>
        <tr>
            <th>Варианты ответов:</th>
            <td>
                <div id="answers">
                <% foreach (var answer in Model.Answers) { %>
                    <div class="single-answer">
                        <input type="text" name="answers" class="wide_input" value="<%= answer.answer_text %>" style="width: 93%" />
                        <a href="" class="remove_anwers"><img src="/Content/img/answer_delete.png" alt="X" title="Удалить" /></a>
                        <div style="margin-bottom: 2px;"></div>
                    </div>
                <% } %>
                </div>
                <a href="" id="add_answer"><img src="/Content/img/answer_add.png" alt="+" title="Добавить ответ" /></a>
            </td>
        </tr>
        <tr>
            <th>Статус:</th>
            <td>
                <select name="status" class="vote-status">
                    <option value="0" <%= Model.Vote.status == 0 ? " selected='selected'" : "" %> style="background-image: url('/Content/img/vote_inactive.png');">Неактивно</option>
                    <option value="1" <%= Model.Vote.status == 1 ? " selected='selected'" : "" %> style="background-image: url('/Content/img/vote_active.png');">Активно</option>
                    <option value="2" <%= Model.Vote.status == 2 ? " selected='selected'" : "" %> style="background-image: url('/Content/img/vote_closed.png');">Завершено</option>
                </select>
            </td>
        </tr>
        <tr>
            <th></th>
            <td>
                <input type="submit" class="button button-confirm" value="Сохранить"/>
                <%= Html.ActionLink("Отмена", "Index", new { controller="Vote" }, new { @class="button" }) %>
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
<title>Редактировать голосование</title>
</asp:Content>

