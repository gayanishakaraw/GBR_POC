﻿using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using GBR.PlmOn.Models;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using GBR.Entity;
using System.Net.Http.Formatting;
using System.Web.Providers.Entities;
using System.Net;
using System.IO;
using System.Xml.Serialization;

namespace GBR.PlmOn.Account
{
    public partial class Login : Page
    {
        static HttpClient client;
        private Tenant mTenant { get; set; }

        public Login()
        {
            client = new HttpClient();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register";
            // Enable this once you have account confirmation enabled for password reset functionality
            //ForgotPasswordHyperLink.NavigateUrl = "Forgot";
            OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }

            //Tenant Resolution Via URL

            //Extract the tenant key from te URL
            var tenantKeyArray = Page.Request.Url.Host.Split('.');
            string tenantKey = string.Empty;

            //I know this ugly, we shall steal the code from ASPNETBOILERPLATE tenancy framework
            if (tenantKeyArray.Length > 0)
                tenantKey = tenantKeyArray[0];

            mTenant = ResolveTenant(tenantKey);
            
        }

        //async Task<Tenant> GetTenantAsync(string tenantKey)
        //{
        //    Tenant tenant = null;
        //    HttpResponseMessage response = await client.GetAsync(tenantKey);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        tenant = await response.Content.ReadAsAsync<Tenant>();
        //    }

        //    return tenant;
        //}

        private Tenant ResolveTenant(string tenantKey)
        {
            Tenant currentTenant = null;

            WebClient webclient = new WebClient();
            webclient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            using (Stream data = webclient.OpenRead(new Uri(string.Format("http://tenantmanager.GBR.com/api/Tenant?tenantKey={0}", tenantKey))))
            {
                using (StreamReader reader = new StreamReader(data))
                {
                    string s = reader.ReadToEnd();
                    currentTenant = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Tenant>(s);
                }
            }

            if (currentTenant == null)
                return currentTenant;

            //Add to session
            Session["TenantId"] = currentTenant.Id;
            Session["TenantName"] = currentTenant.Name;
            Session["TenantStatus"] = currentTenant.Status;
            Session["ConnectionString"] = currentTenant.DbConnectionString;

            //Page.Response.Write(currentTenant.DbConnectionString);

            return currentTenant;
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Validate the user password
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

                // This doen't count login failures towards account lockout
                // To enable password failures to trigger lockout, change to shouldLockout: true
                var result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout: false);

                switch (result)
                {
                    case SignInStatus.Success:
                        IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                        Session["LoggedUser"] = Email.Text;
                        break;
                    case SignInStatus.LockedOut:
                        Response.Redirect("/Account/Lockout");
                        Session["LoggedUser"] = string.Empty;
                        Session["TenantId"] = string.Empty;
                        Session["TenantName"] = string.Empty;
                        Session["TenantStatus"] = string.Empty;
                        break;
                    case SignInStatus.RequiresVerification:
                        Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}", 
                                                        Request.QueryString["ReturnUrl"],
                                                        RememberMe.Checked),
                                          true);
                        break;
                    case SignInStatus.Failure:
                    default:
                        FailureText.Text = "Invalid login attempt";
                        Session["LoggedUser"] = string.Empty;
                        ErrorMessage.Visible = true;
                        break;
                }
            }
        }
    }
}