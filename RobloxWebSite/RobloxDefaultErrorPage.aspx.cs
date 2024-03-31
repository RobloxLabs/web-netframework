using System;
using System.Net;

namespace Roblox.Website
{
    public partial class RobloxDefaultErrorPage : System.Web.UI.Page
    {
        private int ResponseCode { get; set; }

        public RobloxResourceSet Resources { get; } = new RobloxResourceSet("CommonUI.Messages");

        protected void Page_Load(object sender, EventArgs e)
        {
            //CS email link
            CustomerServiceMessage.Text = String.Format(Resources["Description.ErrorPageLetUsKnow"], "<span class=\"SL_swap\" id=\"CsEmailLink\"><a href=\"mailto:info@roblox.com\">info@roblox.com</a></span>");

            var codeStr = Request["Code"] ?? string.Empty;
            HttpStatusCode? statusCode = null;
            if (Enum.TryParse<HttpStatusCode>(codeStr, out var codeOut))
                statusCode = codeOut;

            SetErrorInfo(statusCode);

            // Set the response status code to match the error
            Response.StatusCode = ResponseCode;
            Response.TrySkipIisCustomErrors = true;
        }

        private void SetErrorInfo(HttpStatusCode? statusCode, Exception exception = null)
        {
            switch (statusCode)
            {
                case HttpStatusCode.BadRequest:
                    ResponseCode = 400;
                    ErrorTitle.Text = Resources["Response.BadRequest"];
                    ErrorMessage.Text = Resources["Response.BadRequestDescription"];
                    break;
                case HttpStatusCode.Forbidden:
                    ResponseCode = 403;
                    ErrorTitle.Text = Resources["Response.AccessDenied"];
                    ErrorMessage.Text = Resources["Response.AccessDeniedDescription"];
                    break;
                case HttpStatusCode.NotFound:
                    ResponseCode = 404;
                    ErrorTitle.Text = Resources["Response.PageNotFound"];
                    ErrorMessage.Text = Resources["Response.PageNotFoundDescription"];
                    break;
                case HttpStatusCode.InternalServerError:
                    ResponseCode = 500;
                    ErrorTitle.Text = Resources["Response.InternalServerError"];
                    ErrorMessage.Text = Resources["Response.InternalServerErrorDescription"];
                    break;
                default: // Covers any other error codes the user might throw at us
                    ResponseCode = 500;
                    ErrorTitle.Text = Resources["Response.SomethingWentWrong"];
                    ErrorMessage.Text = Resources["Response.UnexpectedError"];
                    break;
            }

            if (exception != null)
                errorMsgLbl.Text = exception.ToString();
        }
    }
}