<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    if (Request.IsAuthenticated) {
%>

    <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                    Личный Кабинет
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
            <div class="userpic"><%= Html.Action( "GetAvatarHtml", new { controller = "Profile", username = Page.User.Identity.Name } )%></div>
            Добро пожаловать, <%=  Html.ActionLink( Html.Encode(Page.User.Identity.Name), "", new{ controller="Profile" }, new{ @class = "username" } )  %>!
            
            <ul class="menu">
	            <% if (Page.User.IsInRole( "Administrators" )) {%>
	            <li><a href="/User">Пользователи</a></li>
                <% } %>
                <% if (Page.User.IsInRole( "Administrators" ) || Page.User.IsInRole( "Tags" )) {%>
	            <li><a href="/News/ManageTags">Управление тегами</a></li>
	            <% } %>
	            
	            <% if (Page.User.IsInRole( "Administrators" ) || Page.User.IsInRole( "Gallery" )) {%>
	            <li><a href="/Gallery">Галерея</a></li>
                <% } %>
                <% if (Page.User.IsInRole( "Administrators" ) || Page.User.IsInRole( "Vote" )) {%>
	            <li><a href="/Vote">Голосования</a></li>
                <% } %>
                <% if (Page.User.IsInRole( "Administrators" ) || Page.User.IsInRole( "Menu" )) {%>
	            <li><a href="/Menu">Настройка Меню</a></li>
                <% } %>
                <% if (Page.User.IsInRole( "Administrators" ) || Page.User.IsInRole( "Content" )) {%>
	            <li><a href="/ContentManager">Управление контентом</a></li>
	            <% } %>

	            <hr class="hr" />

	            <li><a href="/User/Profile">Профиль</a></li>
	            <li><a href="/Account/ChangePassword">Смена пароля</a></li>
            </ul>
            <%= Html.ActionLink("Выход", "LogOff", new { controller = "Account" }, new { @class="button" })%>
        </div>
    </div>

<%
    }
    else {
%> 
    <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                    Личный Кабинет
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
            <% Html.RenderPartial( "TinyLogin" ); %>
        </div>
    </div>
<%
    }
%>
