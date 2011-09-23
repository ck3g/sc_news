<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div class="miniblock">
    <div class="title-middle">
        <div class="title-left">
            <div class="title-right">
                <div class="title-text">
                Чат
                </div>
            </div>
        </div>
    </div>
    <div class="content">
        <div class="chat-window">
        </div>
	    <div class="chat-textfield-div">
	        <% if (Page.User.Identity.IsAuthenticated) { %>
	            <input type="text" id="message_text" name="message_text" class="chat-textfield" />
	        <% } else { %>
	            <div class="chat-input-text">Сообщения могут оставлять только авторизированные пользователи.</div>
	        <% } %>
	    </div>
    </div>
</div>


<script type="text/javascript">
    var t = 0;

    var chatScroll = function() {
        var $item = $("div.chat-window");
        $item.animate({ scrollTop: 5000 }, "slow");
    }

    var chatSynchonize = function() {
        clearTimeout( t );
        $("div.chat-window").load("/Chat/DrawContent", function() {

            t = setTimeout("chatSynchonize()", 5000);
        });
    }

    $(function() {
        $("div.chat-window").load("/Chat/DrawContent", function() {
            chatScroll();
            chatSynchonize();
        });
        
        $("#message_text").keypress(function(event) {
            if (event.keyCode == '13') {
                var message = $("#message_text").val();
                $.getJSON(
                    "/Chat/AddMessage",
                    { messageText: message },
                    function(data) {
                        if (data.result == 'ok') {
                            //$("div.chat-window").append('<span class="chat-username">' + data.authorName + ': </span><span class="chat-message">' + data.text + '</span><br />');
                            $("#message_text").val("");
                            chatScroll();
                            chatSynchonize();
                        }
                    }
                );
            }
        });

        $("#sync").click(function() {
            chatSynchonize();
            chatScroll();
            return false;
        });
    });
</script>


