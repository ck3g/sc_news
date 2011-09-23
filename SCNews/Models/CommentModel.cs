using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCNews.Models
{
    public class CommentModel
    {
        public Int64 Id { get; set; } 
        public Int64 ParentId { get; set; } 
        public Guid AuthorId { get; set; } 
        public String Body { get; set; } 
        public DateTime CreatedAt { get; set; } 
        public String IpAddress { get; set; } 
        public Int32 VotedFor { get; set; } 
        public Int32 VotedAgainst { get; set; } 

    }
}
