﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div class="miniblock">
    <div class="title-middle">
        <div class="title-left">
            <div class="title-right">
                <div class="title-text">
                Последние комментарии
                </div>
            </div>
        </div>
    </div>
    <div class="content">
        <% Html.RenderAction( "LatestComments", "News" ); %>        
    </div>
</div>