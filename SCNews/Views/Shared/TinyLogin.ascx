<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<% using (Html.BeginForm( "LogOn", "Account", new RouteValueDictionary( new { returnUrl = Request.RawUrl } ) )) { %>

    <div class="tiny_login">
        <div>
            <%= Html.TextBox( "UserName", "", new { placeholder = "        Пользователь" } )%><br />
            <%= Html.ValidationMessage( "Account", "UserName" ) %>
        </div>
        <div class="inputs-spacer"></div>
        <div>
            <%= Html.Password( "Password", "", new { placeholder = "            Пароль" } )%><br />
            <%= Html.ValidationMessage( "Account", "Password" ) %>
        </div>
        <div class="inputs-spacer"></div>
        <div>
            <label><%= Html.CheckBox( "RememberMe" ) %>Не выходить из системы</label>
        </div>
        <div class="inputs-spacer"></div>
        <div>
            <input type="submit" value="Вход" class="button button-confirm" />
            <%= Html.ActionLink("Регистрация", "Register", new { controller = "Account" }, new { @class="button" }) %>
        </div>

        <div><%= Html.ActionLink("Забыли пароль?", "PasswordReset", "Account") %></div>
    </div>

<% } %>

