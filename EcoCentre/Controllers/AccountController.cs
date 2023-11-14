using System.Web.Mvc;
using EcoCentre.Models.Domain.User.Commands;
using EcoCentre.Resources;

namespace EcoCentre.Controllers
{
    using System;

    public class AccountController : Controller
    {
        private readonly RequestPasswordResetCommand _requestPasswordResetCommand;
        private readonly ResetPasswordCommandViewModelValidator _resetValidator;
        private readonly ResetPasswordCommand _resetPasswordCommand;
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

        public AccountController(RequestPasswordResetCommand requestPasswordResetCommand,ResetPasswordCommandViewModelValidator resetValidator,ResetPasswordCommand resetPasswordCommand )
        {
            _requestPasswordResetCommand = requestPasswordResetCommand;
            _resetValidator = resetValidator;
            _resetPasswordCommand = resetPasswordCommand;
        }

        [AllowAnonymous]
        public ActionResult ForgottenPassword()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Reset(string key)
        {
            var model = new ResetPasswordCommandViewModel {Key = key};
            var valid = _resetValidator.Validate(model);
            foreach (var error in valid.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return View(model);
        }

        [AllowAnonymous]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Reset(ResetPasswordCommandViewModel model)
        {
            model.Reseting = true;
            var valid = _resetValidator.Validate(model);
            foreach (var error in valid.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            ViewBag.Success = valid.IsValid;
            if (valid.IsValid)
                _resetPasswordCommand.Execute(model);

            return View(model);
        }


        [AllowAnonymous]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ForgottenPassword(string loginOrEmail)
        {
            _requestPasswordResetCommand.LoginOrEmail = loginOrEmail;
            if (string.IsNullOrEmpty(loginOrEmail))
                ModelState.AddModelError("user", Forms.PasswordResetUsernameIsRequired);
            else
            {
                var result = _requestPasswordResetCommand.Execute();
                switch (result)
                {
                    case PasswordResetCommandResult.NoEmail:
                        ModelState.AddModelError("user", Forms.PasswordResetNoEmailConfigured);
                        break;
                    case PasswordResetCommandResult.UserNotFound:
                        ModelState.AddModelError("user", Forms.PasswordResetNoUserFound);
                        break;
                    case PasswordResetCommandResult.Success:
                        ViewBag.Success = true;
                        break;
                }
            }
            return View();
        }

    }
}
