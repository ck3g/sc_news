using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using SCNews.Helpers;
using SCNews.Models;

namespace SCNews.Controllers
{

    [HandleError]
    public class AccountController : Controller
    {

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        protected override void Initialize( RequestContext requestContext )
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize( requestContext );
        }

        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public ActionResult LogOn()
        {
            return View();
        }
/**/
        [HttpPost]
        public ActionResult LogOn( String userName, String password, Boolean rememberMe, String returnUrl )
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser( userName, password ))
                {
                    
                    FormsService.SignIn( userName, rememberMe );

                    if (rememberMe)
                    {
                        var cookie = Request.Cookies[".ASPXAUTH"];
                        if (cookie != null)
                            cookie.Expires = DateTime.UtcNow.AddMonths( 1 );
                    }

                    if (!String.IsNullOrEmpty( returnUrl )) {
                        return Redirect( returnUrl );
                    }
                    else {
                        return RedirectToAction( "Index", "News" );
                    }
                }
                ModelState.AddModelError( "", "Имя пользователя или пароль неверны." );
            }

            // If we got this far, something failed, redisplay form
            return View();
        }
/**/

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            var db = new NewsDataContext();
            var user = db.Users.SingleOrDefault( u => u.UserName == User.Identity.Name );
            user.LastActivityDate = DateTime.UtcNow.AddMinutes( -10 );
            db.SubmitChanges();

            FormsService.SignOut();

            return RedirectToAction( "Index", "News" );
        }

        // **************************************
        // URL: /Account/Register
        // **************************************

        
        public ActionResult Register()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        [CaptchaValidator]
        [HttpPost]
        public ActionResult Register( RegisterModel model, bool captchaValid )
        {
            if (!captchaValid)
            {
                ModelState.AddModelError( "", "Неверный код защиты" );
                return View( model );
            }

            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipService.CreateUser( model.UserName, model.Password, model.Email );

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsService.SignIn( model.UserName, false /* createPersistentCookie */);
                    var profileRepository = new ProfileRepository();
                    profileRepository.CreateProfile( profileRepository.GetUserId( model.UserName ) );
                    return RedirectToAction( "Index", "News" );
                }
                else
                {
                    ModelState.AddModelError( "", AccountValidation.ErrorCodeToString( createStatus ) );
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View( model );
        }

        // **************************************
        // URL: /Account/ChangePassword
        // **************************************

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword( ChangePasswordModel model )
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword( User.Identity.Name, model.OldPassword, model.NewPassword ))
                {
                    return RedirectToAction( "ChangePasswordSuccess" );
                }
                else
                {
                    ModelState.AddModelError( "", "Текущий пароль неверный или неподходящий новый пароль." );
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View( model );
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult PasswordReset()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PasswordReset(String userName)
        {
            MembershipService.ResetPassword( userName );
            return RedirectToAction("PasswordResetFinal");
        }

        public ActionResult PasswordResetFinal()
        {
            return View();
        }

    }
}
