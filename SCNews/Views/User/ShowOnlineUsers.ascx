<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<SCNews.Models.User>>" %>

    <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                    Сейчас на сайте
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
            <div>
            <% foreach (var user in Model) { %>
                <a href="/Profile/<%= user.UserName %>" class="username"><%= user.UserName%></a>&nbsp;
            <% } %>
            </div>
            <div class="inputs-spacer"></div>
            <div>
                Пользователей:
                <%= Application["UsersCount"].ToString() %>
            </div>
            <div class="inputs-spacer"></div>
            <div>
                Из них гостей:
                <%= ( (int)Application["UsersCount"] - Model.Count() ) > 0 ? ( (int)Application["UsersCount"] - Model.Count() ).ToString() : "0" %>
            </div>
        </div>
    </div>




