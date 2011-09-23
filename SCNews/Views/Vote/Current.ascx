<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SCNews.Controllers.Voting>" %>

<% if (Model != null && Model.Vote.status != 0) { %>
    <div class="miniblock">
        <div class="title-middle">
            <div class="title-left">
                <div class="title-right">
                    <div class="title-text">
                    Голосование
                    </div>
                </div>
            </div>
        </div>
        <div class="content">
            <div class="">
            <form action="/Vote/Vote?returnUrl=<%= Request.RawUrl %>" method="post" id="form_vote">
                    <div class="message-error" id="vote_error" style="display: none;"></div>
			        <h3><%= Html.Encode( Model.Vote.question_name ) %></h3>
			        <input type="hidden" name="vote_id" value="<%= Model.Vote.id %>" />
			        <input type="hidden" name="vd" value="<%= ViewData["AlreadyVoted"].ToString() %>" />
		            <table class="poll">
		                <% if (Convert.ToBoolean( ViewData["AlreadyVoted"] ) || Model.Vote.status == 2) { %>
    		                <% foreach (var answer in Model.Answers ) {%>
		                    <tr><td class="poll_text" style="width: 10px;"><%= answer.order_n %>.</td><td class="poll_text"><%= Html.Encode( answer.answer_text ) %></td></tr>
		                    <tr><td></td><td class="poll_text_result">голосов: <%= ViewData["CountOfAnswer" + answer.order_n].ToString() %> (<%= ViewData["PercentOfAnswer" + answer.order_n].ToString() %>%)</td></tr>
		                    <tr><td></td><td class="poll_result"><img src="/Content/img/poll_ps.gif" style='width:<%= ViewData["PercentOfAnswer" + answer.order_n].ToString() %>%' height="16" border="0" alt="" /></td></tr>
		                    <% } %>
		                <% } else { %>
		                    <% foreach (var answer in Model.Answers ) {%>
		                    <tr>
		                        <td class="poll_text"><%= answer.order_n %>.</td>
		                        <td class="poll_text">
		                            <label>
		                                <input type="radio" name="answer_order_n" value="<%= answer.order_n %>" />
		                                <span><%= Html.Encode( answer.answer_text ) %></span>
		                            </label>
        		                    
		                        </td>
		                    </tr>
		                    <% } %>
		                <% } %>
        
		            </table>	
                
            </form>
            </div> 
            <div class="chat-textfield-div">
	            <% if (Page.User.Identity.IsAuthenticated) { %>
	                <% if (Convert.ToBoolean( ViewData["AlreadyVoted"] ) || Model.Vote.status == 2) {%>
	                
	                <% } else { %>
	                    <input type="button" class="button" value="Проголосовать" id="btn_vote" />
	                <% } %>
	            <% } else { %>
	                <div class="chat-input-text">Голосовать могут только авторизированные пользователи.</div>
	            <% } %>
	    </div>
        </div>
    </div>

<script type="text/javascript">
    $(function() {
        $("#btn_vote").click(function() {
            var checked = false;
            $("#vote_error").html("").hide();
            $("input[name=answer_order_n]:checked").each(function() {
                checked = true;
            });
            if (checked) {
                $("#form_vote").submit();
            }
            else {
                $("#vote_error").html("Необходимо выбрать один вариантов.").show();
            }
        });
    });
</script>

<% } %>