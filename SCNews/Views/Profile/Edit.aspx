<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SCNews.Models.UsersProfile>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_headContent" runat="server">
	<title>Редактирование профайла пользователя - <%= Html.Encode(Model.User.UserName) %></title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                    Редактирование профайла пользователя - <%= Html.Encode(Model.User.UserName) %>
                    </div>
                </div>
            </div>
        </div>
        <div class="content edit-profile">
           <% using (Html.BeginForm()) {%>
                <%= Html.ValidationSummary(true) %>
                <table class="table-editor">
                    <tr>
                        <th>Имя:</th>
                        <td><%= Html.TextBoxFor(model => model.first_name) %></td>
                    </tr>
                    <tr>
                        <th>Фамилия:</th>
                        <td><%= Html.TextBoxFor(model => model.last_name) %></td>
                    </tr>
                    <tr>
                        <th>Страна проживания:</th>
                        <td><%= Html.DropDownList( "country_id" ) %></td>
                    </tr>
                    <tr>
                        <th>Ссылка на Battle.Net профайл:</th>
                        <td>
                            <div class="hint">Поможет получить информацию из вашего профиля. Количество игр, достижений и т.п.</div>
                            <div><%= Html.TextBoxFor( model => model.profile_url ) %></div>
                            <div class="hint">Пример: http://kr.battle.net/sc2/ko/profile/106432/1/SoundWeRRa/</div>
                        </td>
                    </tr>
                    <tr>
                        <th>Дополнительная информация:</th>
                        <td><%= Html.TextAreaFor( model => model.details, new { @class = "wide_textarea" } )%></td>
                    </tr>
                    <tr>
                        <th></th>
                        <td>
                            <input type="submit" value="Сохранить" class="button button-confirm" />
                            <%= Html.ActionLink("Просмотр профайла", "", new { controller = "Profile" }, new { @class="button" })%>
                        </td>
                    </tr>
                </table>

            <% } %>

        </div>
    </div>



  

</asp:Content>

