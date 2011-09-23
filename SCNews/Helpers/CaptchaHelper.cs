using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace SCNews.Helpers
{
    public static class CaptchaHelper
    {
        public static string GenerateCaptcha( this HtmlHelper helper )
        {

            var captchaControl = new Recaptcha.RecaptchaControl
             {
                 ID = "recaptcha",
                 Theme = "clean",
                 PublicKey = "6LcjU7oSAAAAAAc4zWAykYP1DuNCFknEb3jHxxpt",
                 PrivateKey = "6LcjU7oSAAAAAJ1OFv9iogMBGPrH6q9VlZzamYM_"
             };

            var htmlWriter = new HtmlTextWriter( new StringWriter() );

            captchaControl.RenderControl( htmlWriter );

            return htmlWriter.InnerWriter.ToString();
        }
    }
}
