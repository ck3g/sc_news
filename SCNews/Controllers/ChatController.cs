using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCNews.Models;

namespace SCNews.Controllers
{
    public class ChatController : Controller
    {
        ChatRepository chatRepository = new ChatRepository();

        //
        // GET: /Chat/

        public ActionResult Index()
        {
            return View( "Index" );
        }

        [OutputCache( Duration = 6, VaryByParam = "none" )]
        public ActionResult DrawContent() 
        {
            var messages = chatRepository.GetLastMessages().OrderBy( m => m.texted_at );
            return PartialView( "ChatWindow", messages );
        }

        public ActionResult AddMessage( string messageText ) 
        {
            if (messageText == "")
                return Json( new { result = "error" }, JsonRequestBehavior.AllowGet );

            var chatMessage = new ChatMessage();
            chatMessage.id = Guid.NewGuid();
            chatMessage.texted_at = DateTime.UtcNow;
            chatMessage.author_id = chatRepository.GetAuthorId( User.Identity.Name );
            chatMessage.is_deleted = false;
            chatMessage.text = messageText;

            chatRepository.Add( chatMessage );

            return Json( new { result = "ok", authorName = User.Identity.Name, text = messageText }, JsonRequestBehavior.AllowGet );    
        }

        [Authorize( Roles = "Administrators, Power Users" )]
        public ActionResult RemoveMessage( string messageId )
        {
            if (messageId != "")
                chatRepository.RemoveMessage( new Guid( messageId ) );

            var messages = chatRepository.GetLastMessages().OrderBy( m => m.texted_at );
            return View( "ChatWindow", messages );
        }

    }
}
