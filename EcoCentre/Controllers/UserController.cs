using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using EcoCentre.Models.Commands;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Clients.Commands;
using EcoCentre.Models.Domain.Hubs;
using EcoCentre.Models.Domain.User;
using EcoCentre.Models.Domain.User.Commands;
using EcoCentre.Models.Domain.User.Queries;
using FluentValidation;

namespace EcoCentre.Controllers
{
    using System;

    public class UserController : Controller
    {
        private readonly Repository<User> _userRepository;
        private readonly SetupAdminCommand _setupAdminCommand;
        private readonly LoginCommand _loginCommand;
        private readonly UserDeleteCommand _userDeleteCommand;
        private readonly UserListQuery _userListQuery;
        private readonly CreateUserCommand _createUserCommand;
        private readonly UserDetailsQuery _userDetailsQuery;
        private readonly UpdateUserCommand _updateUserCommand;
	    private readonly CurrentUserDetailsQuery _currentUserDetailsQuery;
	    private readonly Repository<Hub> _hubRepository;

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

        public UserController(Repository<User> userRepository,SetupAdminCommand setupAdminCommand, LoginCommand loginCommand, UserDeleteCommand userDeleteCommand,
            UserListQuery userListQuery, CreateUserCommand createUserCommand, UserDetailsQuery userDetailsQuery, UpdateUserCommand updateUserCommand,
			CurrentUserDetailsQuery currentUserDetailsQuery, Repository<Hub> hubRepository)
        {
            _userRepository = userRepository;
            _setupAdminCommand = setupAdminCommand;
            _loginCommand = loginCommand;
            _userDeleteCommand = userDeleteCommand;
            _userListQuery = userListQuery;
            _createUserCommand = createUserCommand;
            _userDetailsQuery = userDetailsQuery;
            _updateUserCommand = updateUserCommand;
	        _currentUserDetailsQuery = currentUserDetailsQuery;
	        _hubRepository = hubRepository;
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            if (!_userRepository.Query.Any())
                return RedirectToAction("Setup");

	        var model = GetLoginPageViewModel();
            return View(model);
        }

	    private LoginPageViewModel GetLoginPageViewModel()
	    {
		    var model = new LoginPageViewModel()
		    {
			    Hubs = _hubRepository.Query.OrderBy(x => x.Name).ToList(),
				HubId = HttpContext.Request.Cookies[AuthenticationContext.HubidCookie]?.Value
		    };
		    return model;
	    }

	    [AllowAnonymous]
        public ActionResult Logout(LogoutCommand command)
        {
	        if (User.Identity.IsAuthenticated)
	        {
		        command.Execute();
	        }
			return RedirectToAction("Login");
        }


        [AllowAnonymous]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Login(LoginData data)
        {
            if (!_userRepository.Query.Any())
                return RedirectToAction("Setup");
            try
            {
                _loginCommand.Execute(data);
            }
            catch (ValidationException e)
            {
                foreach(var error in e.Errors)
                    ModelState.AddModelError("",error.ErrorMessage);
	            var model = GetLoginPageViewModel();
				return View(model);
            }
            return RedirectToAction("Index", "Default", new { });
        }


        [AllowAnonymous]
        public ActionResult Setup()
        {
            if (_userRepository.Query.Any())
                return RedirectToAction("Login");
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [AllowAnonymous]
        public ActionResult Setup(string login, string password, string email)
        {
            if (_userRepository.Query.Any())
                return RedirectToAction("Login");
            _setupAdminCommand.Execute(login, password,email);
            return RedirectToAction("Login");
        }

        public ActionResult Index(UserListQueryParams param)
        {
            if (string.IsNullOrEmpty(param.Id))
            {
                var data = _userListQuery.Execute();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = _userDetailsQuery.Execute(param.Id);
                return Json(new ExistingUserViewModel(data), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Current()
        {
	        var data = _currentUserDetailsQuery.Execute();
			return Json(data, JsonRequestBehavior.AllowGet);
        }

        [RequireWriteRightsForPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(UserViewModel model)
        {
            _createUserCommand.Execute(model);
            return Json("Ok");
        }

        [RequireWriteRightsForPost]
        [AcceptVerbs(HttpVerbs.Put)]
        public ActionResult Index(ExistingUserViewModel model)
        {
            _updateUserCommand.Execute(model);
            return Json("Ok");
        }

        [AcceptVerbs(HttpVerbs.Delete)]
        public ActionResult Index(string id)
        {
            _userDeleteCommand.Execute(id);
            return Json("Ok");
        }

        public ActionResult IndexTemplate()
        {
            return View();
        }
        public ActionResult NewTemplate()
        {
            return View();
        }
    }
}