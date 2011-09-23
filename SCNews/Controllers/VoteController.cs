using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCNews.Helpers;
using SCNews.Models;

namespace SCNews.Controllers
{
    public class VoteController : Controller
    {
        VoteRepository voteRepository = new VoteRepository();

        //
        // GET: /Vote/
        [Authorize( Roles = "Administrators, Vote" )]
        [OutputCache(CacheProfile = "Cache1Hour")]
        public ActionResult Index( int? page )
        {
            const int pageSize = 25;
            var votesCount = voteRepository.GetCount();
            var votes = voteRepository.FindVotesOnPage(page ?? 1, pageSize);
            var noPinned = votes.Where( v => v.is_pinned ).Count() == 0;
            ViewData["NoPinned"] = noPinned;
            var noActive = votes.Where( v => v.status == 1 ).Count() == 0 && votes.Where( v => v.status == 2 ).Count() == 0;
            ViewData["NoActive"] = noActive;
            var paginatedVotes = new PaginatedList<Vote>( votes, page ?? 1, pageSize, votesCount );
            return View( paginatedVotes );
        }

        //
        // GET: /Vote/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        [OutputCache( CacheProfile = "Cache1Hour" )]
        public ActionResult Current()
        {
            var current = voteRepository.FindCurrent();
            if ( current == null )
                return View();
            
            var answers = voteRepository.FindVoteAnswers( current.id );
            var votesByUsers = voteRepository.FindVotesByUser( current.id );
            var total = votesByUsers.Count();
            foreach (var answer in answers)
            {
                var orderN = answer.order_n;
                var answerCount = votesByUsers.Where( a => a.answer_n == orderN ).Count();
                ViewData["CountOfAnswer" + orderN] = answerCount;
                var percent = total != 0 ? answerCount * 100 / total : 0;
                ViewData["PercentOfAnswer" + orderN] = percent;
            }

            ViewData["AlreadyVoted"] = false;
            if (User.Identity.IsAuthenticated)
            {
                var currentUserId = voteRepository.GetUserId( User.Identity.Name );
                if (votesByUsers.Where( v => v.user_id == currentUserId ).Count() != 0)
                {
                    ViewData["AlreadyVoted"] = true;
                }
            }
            
            var voting = new Voting{ Vote = current, Answers = answers };
            return View( voting );
        }

        [HttpPost]
        [Authorize]
        public ActionResult Vote( String returnUrl )
        {
            var voteByUser = new VoteByUser
                             {
                                 user_id = voteRepository.GetUserId( User.Identity.Name ),
                                 vote_id = Int64.Parse( Request.Form["vote_id"] ),
                                 answer_n = Int32.Parse( Request.Form["answer_order_n"] )
                             };
            voteRepository.Vote( voteByUser );
            
            return Redirect( returnUrl );
        }

        //
        // GET: /Vote/Create
        [Authorize( Roles = "Administrators, Vote" )]
        public ActionResult Create()
        {
            var vote = new Vote();
            return View( vote );
        } 

        //
        // POST: /Vote/Create

        [HttpPost]
        [Authorize( Roles = "Administrators, Vote")]
        public ActionResult Create(FormCollection collection, string[] answers)
        {
            var vote = new Vote();
            try
            {
                // TODO: Add insert logic here
                UpdateModel( vote );
                vote.created_at = DateTime.UtcNow;
                vote.question_name = Request.Form["question_name"];
                vote.question_text = Request.Form["question_text"];
                vote.status = 0;
                vote.is_pinned = false;
                vote.created_by = voteRepository.GetUserId( User.Identity.Name );

                var voteId = voteRepository.AddVote( vote );
                var orderN = 1;
                foreach (var answer in answers)
                {
                    if (String.IsNullOrEmpty( answer ))
                        continue;
                    var voteAnswer = new VoteAnswer();
                    voteAnswer.answer_text = answer;
                    voteAnswer.vote_id = voteId;
                    voteAnswer.order_n = orderN;
                    voteRepository.AddVoteAnswer( voteAnswer );
                    orderN++;
                }
                
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Vote/Edit/5
        [Authorize( Roles = "Administrators, Vote" )]
        public ActionResult Edit(Int64 id)
        {
            var vote = voteRepository.GetVote( id );
            var voteAnswers = voteRepository.FindVoteAnswers( id );

            var voting = new Voting {Vote = vote, Answers = voteAnswers};

            return View( voting );
        }

        //
        // POST: /Vote/Edit/5

        [HttpPost]
        [Authorize( Roles = "Administrators, Vote" )]
        public ActionResult Edit(int id, FormCollection collection, string[] answers )
        {
            try
            {
                // TODO: Add update logic here
                //vote.question_name = Request.Form["question_name"];
                //vote.question_text = Request.Form["questiont_text"];
                var vote = voteRepository.GetVote( id );
                UpdateModel( vote );
                voteRepository.Save();
                voteRepository.RemoveAnswers( id );
                var orderN = 1;
                foreach (var answer in answers)
                {
                    if (String.IsNullOrEmpty( answer ))
                        continue;
                    var voteAnswer = new VoteAnswer();
                    voteAnswer.answer_text = answer;
                    voteAnswer.vote_id = id;
                    voteAnswer.order_n = orderN;
                    voteRepository.AddVoteAnswer( voteAnswer );
                    orderN++;
                }

                voteRepository.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction( "Index" );
            }
        }

        //
        // GET: /Vote/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Vote/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize( Roles = "Administrators, Vote" )]
        public ActionResult Pin( Int64 id )
        {
            voteRepository.ResetPin();
            var vote = voteRepository.GetVote( id );
            vote.is_pinned = true;
            voteRepository.Save();
            return RedirectToAction( "Index" );
        }
    }

    public class Voting
    {
        public Vote Vote { get; set; }
        public IQueryable<VoteAnswer> Answers { get; set; }
    }
}
