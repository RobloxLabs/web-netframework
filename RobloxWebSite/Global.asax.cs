using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.UI.WebControls;
using System.Net;
using Roblox.Configuration;
using Roblox.EventLog;
using Roblox.Web.Maintenance;
using Roblox.Platform.Membership;
using Roblox.Platform.Roles;
using Roblox.Platform.Email;
using Roblox.Platform.Authentication;
using Roblox.Platform.Security;

namespace Roblox.Website {
    public class Global : System.Web.HttpApplication {

        public static ILogger Logger { get; private set; }

        public static MembershipDomainFactories MembershipDomainFactories { get; private set; }
        public static RolesDomainFactories RolesDomainFactories { get; private set; }
        public static EmailDomainFactories EmailDomainFactories { get; private set; }
        public static AuthenticationDomainFactories AuthenticationDomainFactories { get; private set; }
        public static SecurityDomainFactories SecurityDomainFactories { get; private set; }

        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            CreateLogger();
            ConfigureDomainFactories();
            DoOtherConfig();

            ConfigureCookieConstraint();
        }

        public void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            if (exc != null)
            {
                // If this is an unhandled exception, skip to the actual exception and handle it
                if (exc.GetType() == typeof(HttpUnhandledException) && exc.InnerException != null)
                    exc = exc.InnerException;

                if (exc.GetType() == typeof(HttpException))
                {
                    if (Response.IsClientConnected)
                        Response.Redirect("~/RobloxDefaultErrorPage.aspx?code=" + ((HttpException)exc).GetHttpCode(), true);
                }
                else
                {
                    LogException(exc);
                }
            }
            else
            {
                // Something went very wrong
                if (Response.IsClientConnected)
                    Response.Redirect("~/RobloxDefaultErrorPage.aspx?code=500", true);
            }
        }

        public void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var user = MembershipDomainFactories.UserFactory.GetCurrentUser();
            if (user != null && user.AccountStatus != Platform.Membership.AccountStatus.Ok)
            {
                var isPrivileged = MembershipDomainFactories.RoleSetValidator.IsPrivilegedUser(user);
                if (!isPrivileged)
                {
                    var notApprovedPage = "/Membership/NotApproved.aspx";

                    if (!Request.Url.AbsolutePath.ToLower().EndsWith(notApprovedPage.ToLower()))
                    {
                        Response.Redirect("~" + notApprovedPage, true);
                    }
                }
            }
        }

        public static string LogException(Exception e)
        {
            // TODO: Log using uber-advanced exception logging technology
            Logger.Error(e);
            return Guid.NewGuid().ToString();
        }

        private void ConfigureCookieConstraint()
        {
            CookieConstraintSettings.SetCookieConstraintSettings(
                () => Roblox.Web.Code.Properties.Settings.Default.IsCookieConstraintEnabled,
                () => Roblox.Web.Code.Properties.Settings.Default.CookieConstraintCookieName,
                () => Roblox.Web.Code.Properties.Settings.Default.CookieConstraintPassword,
                () => Roblox.WebsiteSettings.Properties.Settings.Default.CookieConstraint_RedirectDomain,
                () => Roblox.WebsiteSettings.Properties.Settings.Default.CookieConstraint_RedirectURL,
                () => Roblox.WebsiteSettings.Properties.Settings.Default.CookieConstraint_ProtectedPageExtension,
                () => Roblox.Web.Maintenance.Properties.Settings.Default.CookieConstraintIpBypassRangeCsv,
                () => Roblox.Web.Code.Properties.Settings.Default.CookieConstraint_AllowedButtonValuesCSV
            );
        }

        /// <summary>
        /// Sets up and configures the logger for the current application and its assemblies.
        /// </summary>
        private void CreateLogger()
        {
            // Construct application event logger
            Logger = new RobloxWebsiteLogger();

            if (WebsiteSettings.Properties.Settings.Default.IsGeneralLogExceptionThrottlingEnabled)
            {
                // Exception throttling support
                Logger = new ExceptionThrottlingLogger(
                    Logger,
                    () => WebsiteSettings.Properties.Settings.Default.ExceptionLogCountBeforeThrottling,
                    () => WebsiteSettings.Properties.Settings.Default.ExceptionLogThrottlingInterval
                );
            }

            // Roblox.Configuration logging
            ConfigurationLogging.OverrideDefaultConfigurationLogging(
                (msg) => Logger.Error(msg),
                (msg) => Logger.Warning(msg),
                (msg) => Logger.Info(msg)
            );

            StaticLoggerRegistry.SetLogger(Logger);
        }

        private void ConfigureDomainFactories()
        {
            RolesDomainFactories = new RolesDomainFactories(
                logger: Logger
            );
            EmailDomainFactories = new EmailDomainFactories(
                logger: Logger
            );
            MembershipDomainFactories = new MembershipDomainFactories(
                logger: Logger,
                rolesDomainFactories: RolesDomainFactories,
                emailDomainFactories: EmailDomainFactories
            );
            SecurityDomainFactories = new SecurityDomainFactories(
                logger: Logger
            );
            AuthenticationDomainFactories = new AuthenticationDomainFactories(
                logger: Logger,
                securityDomainFactories: SecurityDomainFactories,
                membershipDomainFactories: MembershipDomainFactories,
                emailDomainFactories: EmailDomainFactories
            );

            // Hack for RSCL
            StaticDomainFactoriesRegistry.SetDomainFactories(
                EmailDomainFactories
            );
        }

        private void DoOtherConfig()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            WebControl.DisabledCssClass = "disabled";
        }
    }
}
