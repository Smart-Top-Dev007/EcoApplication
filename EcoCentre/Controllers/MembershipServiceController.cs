using System;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using MembershipService.Mvc;

namespace EcoCentre.Controllers
{
    /// <summary>
    /// HTTP API to membership/roles/profiles
    /// Returns relevant HTTP status codes to indicate success (200) or failure (401/404)
    /// </summary>
    [DoNotRedirectToLogin, OutputCache(Location = OutputCacheLocation.None)] 
    public class MembershipServiceController : Controller
    {
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }

        // GET /MembershipService/LoggedOnAs
        public string LoggedOnAs()
        {
            return User.Identity.Name;
        }

        // POST /MembershipService/Register?name=someuser&password=somepassword&email=email@example.com&passwordQuestion=some+question&passwordAnswer=some+Answer
        // Note that the parameters may be given either in the querystring or in the POST body (a.k.a. Request.Form)
        [HttpPost] public HttpStatusCodeResult Register(string name, string password, string email, string passwordQuestion, string passwordAnswer)
        {
            MembershipCreateStatus createStatus;
            Membership.CreateUser(name, password, email, passwordQuestion, passwordAnswer, isApproved: true, providerUserKey: null, status: out createStatus);

            if (createStatus == MembershipCreateStatus.Success)
                return new HttpStatusCodeResult(200, "OK");
            else
                return new HttpStatusCodeWithBodyResult(409, null, createStatus.ToString());
        }

        // POST /MembershipService/LogOn?name=myname&password=mypassword
        // Note that the parameters may be given either in the querystring or in the POST body (a.k.a. Request.Form)
        [HttpPost] public HttpStatusCodeResult LogOn(string name, string password)
        {
            if (Membership.ValidateUser(name, password)) {
                FormsAuthentication.SetAuthCookie(name, createPersistentCookie: false);
                return new HttpStatusCodeResult(200, "OK");
            }

            return new HttpStatusCodeResult(401);
        }

        // POST /MembershipService/LogOut
        // Note that the parameters may be given either in the querystring or in the POST body (a.k.a. Request.Form)
        [HttpPost] public HttpStatusCodeResult LogOut()
        {
            FormsAuthentication.SignOut();
            return new HttpStatusCodeResult(200);
        }

        // GET /MembershipService/Roles
        public string Roles()
        {
            // Returns a list of roles for the current visitor, one role per line
            return string.Join(Environment.NewLine, System.Web.Security.Roles.GetRolesForUser());
        }

        // GET /MembershipService/Profile/someKey
        [HttpGet] public new ActionResult Profile(string key)
        {
            object propertyValue = HttpContext.Profile[key];

            if (propertyValue == null)
                return HttpNotFound();

            return Content(propertyValue.ToString());
        }

        // POST /MembershipService/Profile/someKey?value=myValue
        // Note that the parameter may be given either in the querystring or in the POST body (a.k.a. Request.Form)
        [HttpPost] public new HttpStatusCodeResult Profile(string key, string value)
        {
            HttpContext.Profile[key] = value;
            return new HttpStatusCodeResult(200);
        }
    }
}