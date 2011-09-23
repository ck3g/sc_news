using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using SCNews.Helpers;

namespace SCNews.Models
{

    #region Models
    [PropertiesMustMatch( "NewPassword", "ConfirmPassword", ErrorMessage = "Новый пароль и его подтверждение должны совпадать." )]
    public class ChangePasswordModel
    {
        [Required]
        [DataType( DataType.Password )]
        [DisplayName( "Текущий пароль" )]
        public string OldPassword { get; set; }

        [Required]
        [ValidatePasswordLength]
        [DataType( DataType.Password )]
        [DisplayName( "Новый пароль" )]
        public string NewPassword { get; set; }

        [Required]
        [DataType( DataType.Password )]
        [DisplayName( "Подтверждение пароля" )]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required(ErrorMessage = "Необходимо указать имя пользователя")]
        [DisplayName( "Пользователь" )]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Необходимо указать пароль")]
        [DataType( DataType.Password )]
        [DisplayName( "Пароль" )]
        public string Password { get; set; }

        [DisplayName( "Запомнить меня" )]
        public bool RememberMe { get; set; }
    }

    
    [PropertiesMustMatch( "Password", "ConfirmPassword", ErrorMessage = "Пароль и его подтверждение должны совпадать." )]
    public class RegisterModel
    {
        [Required( ErrorMessage = "Необходимо указать имя пользователя" )]
        [DisplayName( "Пользователь" )]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Необходимо указать Email адрес")]
        [DataType( DataType.EmailAddress )]
        [DisplayName( "Электронная почта" )]
        public string Email { get; set; }

        [Required(ErrorMessage = "Необходимо указать пароль")]
        [ValidatePasswordLength]
        [DataType( DataType.Password )]
        [DisplayName( "Пароль" )]
        public string Password { get; set; }

        [Required(ErrorMessage = "Необходимо подтвердить пароль")]
        [DataType( DataType.Password )]
        [DisplayName( "Подтверждение пароля" )]
        public string ConfirmPassword { get; set; }

        
    }
    #endregion

    #region Services
    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.

    public interface IMembershipService
    {
        int MinPasswordLength { get; }

        bool ValidateUser( string userName, string password );
        MembershipCreateStatus CreateUser( string userName, string password, string email );
        bool ChangePassword( string userName, string oldPassword, string newPassword );

        void ResetPassword( string userName );
    }

    public class AccountMembershipService : IMembershipService
    {
        private readonly MembershipProvider _provider;

        public AccountMembershipService()
            : this( null )
        {
        }

        public AccountMembershipService( MembershipProvider provider )
        {
            _provider = provider ?? Membership.Provider;
        }

        public int MinPasswordLength
        {
            get
            {
                return _provider.MinRequiredPasswordLength;
            }
        }

        public bool ValidateUser( string userName, string password )
        {
            if (String.IsNullOrEmpty( userName )) return false;
            if (String.IsNullOrEmpty( password )) return false;

            if (String.IsNullOrEmpty( userName )) throw new ArgumentException( "Value cannot be null or empty.", "userName" );
            if (String.IsNullOrEmpty( password )) throw new ArgumentException( "Value cannot be null or empty.", "password" );

            return _provider.ValidateUser( userName, password );
        }

        public MembershipCreateStatus CreateUser( string userName, string password, string email )
        {
            if (String.IsNullOrEmpty( userName )) throw new ArgumentException( "Value cannot be null or empty.", "userName" );
            if (String.IsNullOrEmpty( password )) throw new ArgumentException( "Value cannot be null or empty.", "password" );
            if (String.IsNullOrEmpty( email )) throw new ArgumentException( "Value cannot be null or empty.", "email" );

            MembershipCreateStatus status;
            _provider.CreateUser( userName, password, email, null, null, true, null, out status );
            return status;
        }

        public bool ChangePassword( string userName, string oldPassword, string newPassword )
        {
            if (String.IsNullOrEmpty( userName )) throw new ArgumentException( "Value cannot be null or empty.", "userName" );
            if (String.IsNullOrEmpty( oldPassword )) throw new ArgumentException( "Value cannot be null or empty.", "oldPassword" );
            if (String.IsNullOrEmpty( newPassword )) throw new ArgumentException( "Value cannot be null or empty.", "newPassword" );

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                MembershipUser currentUser = _provider.GetUser( userName, true /* userIsOnline */);
                return currentUser.ChangePassword( oldPassword, newPassword );
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }

        public void ResetPassword(String userName)
        {
            var user = _provider.GetUser(userName, false);
            if (user == null)
                throw new Exception( "User name not found" );

            var newPassword = _provider.ResetPassword(userName, "");
            MailNewPassword( newPassword, user.Email );
        }

        public void MailNewPassword( String newPassword, String toEmail )
        {
            var message = new MailMessage("no-reply@starcraft.md", toEmail);
            message.Subject = "Восстановление пароля (StarCraft.md)";
            message.Body = String.Format( "Пароль от вашей учетной записи был восстановлен. Новый пароль: {0}", newPassword);
            
            var client = new SmtpClient("mail.starcraft.md");
            var smtpUserInfo = new System.Net.NetworkCredential( "dev@starcraft.md", "31415926" );
            client.UseDefaultCredentials = false;
            client.Credentials = smtpUserInfo;
            client.Send( message );
        }
    }

    public interface IFormsAuthenticationService
    {
        void SignIn( string userName, bool createPersistentCookie );
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn( string userName, bool createPersistentCookie )
        {
            if (String.IsNullOrEmpty( userName )) throw new ArgumentException( "Value cannot be null or empty.", "userName" );

            FormsAuthentication.SetAuthCookie( userName, createPersistentCookie );

        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
    #endregion

    #region Validation
    public static class AccountValidation
    {
        public static string ErrorCodeToString( MembershipCreateStatus createStatus )
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    //return "Username already exists. Please enter a different user name.";
                    return "Указанное имя пользователя уже существует. Пожалуйста, выберите другое.";

                case MembershipCreateStatus.DuplicateEmail:
                    //return "A username for that e-mail address already exists. Please enter a different e-mail address.";
                    return "Пользователь с указаным e-mail адресом уже существует. Пожалуйста, выберите другой e-mail адрес.";

                case MembershipCreateStatus.InvalidPassword:
                    //return "The password provided is invalid. Please enter a valid password value.";
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    //return "The e-mail address provided is invalid. Please check the value and try again.";
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    //return "The password retrieval answer provided is invalid. Please check the value and try again.";
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    //return "The password retrieval question provided is invalid. Please check the value and try again.";
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";
                    //return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
                    //return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
                    //return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
                    //return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
    }

    [AttributeUsage( AttributeTargets.Class, AllowMultiple = true, Inherited = true )]
    public sealed class PropertiesMustMatchAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "'{0}' и '{1}' не совпадают.";
        private readonly object _typeId = new object();

        public PropertiesMustMatchAttribute( string originalProperty, string confirmProperty )
            : base( _defaultErrorMessage )
        {
            OriginalProperty = originalProperty;
            ConfirmProperty = confirmProperty;
        }

        public string ConfirmProperty { get; private set; }
        public string OriginalProperty { get; private set; }

        public override object TypeId
        {
            get
            {
                return _typeId;
            }
        }

        public override string FormatErrorMessage( string name )
        {
            return String.Format( CultureInfo.CurrentUICulture, ErrorMessageString,
                OriginalProperty, ConfirmProperty );
        }

        public override bool IsValid( object value )
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties( value );
            object originalValue = properties.Find( OriginalProperty, true /* ignoreCase */).GetValue( value );
            object confirmValue = properties.Find( ConfirmProperty, true /* ignoreCase */).GetValue( value );
            return Object.Equals( originalValue, confirmValue );
        }
    }

    [AttributeUsage( AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "'{0}' минимальная длина {1} символов.";
        private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;

        public ValidatePasswordLengthAttribute()
            : base( _defaultErrorMessage )
        {
        }

        public override string FormatErrorMessage( string name )
        {
            return String.Format( CultureInfo.CurrentUICulture, ErrorMessageString,
                name, _minCharacters );
        }

        public override bool IsValid( object value )
        {
            string valueAsString = value as string;
            return ( valueAsString != null && valueAsString.Length >= _minCharacters );
        }
    }
    #endregion

}
