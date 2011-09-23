<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SCNews.Models.ChangePasswordModel>" %>

<asp:Content ID="changePasswordTitle" ContentPlaceHolderID="_headContent" runat="server">
    <title>Смена пароля</title>
</asp:Content>

<asp:Content ID="changePasswordContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                    Смена пароля
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
            <p>
                Заполните следущие поля для смены пароля. 
            </p>
            <p>
                Минимальная длина нового пароля <%= Html.Encode(ViewData["PasswordLength"]) %> символов.
            </p>

            <% using (Html.BeginForm()) { %>
                <%= Html.ValidationSummary(true, "Ошибка при смене пароля.") %>
                <div>
                    <fieldset>
                        <legend>Смена пароля</legend>
                        
                        <div class="editor-label">
                            <%= Html.LabelFor(m => m.OldPassword) %>
                        </div>
                        <div class="editor-field">
                            <%= Html.PasswordFor(m => m.OldPassword) %>
                            <%= Html.ValidationMessageFor(m => m.OldPassword) %>
                        </div>
                        
                        <div class="editor-label">
                            <%= Html.LabelFor(m => m.NewPassword) %>
                        </div>
                        <div class="editor-field">
                            <%= Html.PasswordFor(m => m.NewPassword) %>
                            <%= Html.ValidationMessageFor(m => m.NewPassword) %>
                        </div>
                        
                        <div class="editor-label">
                            <%= Html.LabelFor(m => m.ConfirmPassword) %>
                        </div>
                        <div class="editor-field">
                            <%= Html.PasswordFor(m => m.ConfirmPassword) %>
                            <%= Html.ValidationMessageFor(m => m.ConfirmPassword) %>
                        </div>
                        
                        <p>
                            <input type="submit" value="Изменить" class="button button-confirm" />
                        </p>
                    </fieldset>
                </div>
            <% } %>    
            </div>
    </div>
    
</asp:Content>
