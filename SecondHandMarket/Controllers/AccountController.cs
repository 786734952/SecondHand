using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using SecondHandMarket.Models;
using SecondHandMarket.common;

namespace SecondHandMarket.Controllers
{
    public class AccountController : Controller
    {

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = Membership.GetUser(model.UserName);
                if (user != null)
                {
                    if (!user.IsApproved)
                    {
                        TempData["ErrorMsg"] =
                            string.Format("账号未激活，请查收激活邮件。没有收到邮件？<a href='{0}'>点击发送激活邮件</a>",
                                          Url.Action("SendConfirmationEmail") + "?u=" +
                                          SecurityHelper.TripleDESCrypto(model.UserName));
                        return RedirectToAction("LogOn");
                    }
                }

                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "提供的用户名或密码不正确。");
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // 尝试注册用户
                MembershipCreateStatus createStatus;
                var user = Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, false, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);

                    SendConfirmationEmail(user);

                    TempData["SuccessMsg"] = "激活邮件已经发送，请前往邮箱查收";
                    return RedirectToAction("Logon", "Account");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        public ActionResult SendConfirmationEmail(string u)
        {
            if (!string.IsNullOrWhiteSpace(u))
            {
                var userName = SecurityHelper.TripleDESCryptoDe(u);
                var user = Membership.GetUser(userName);
                SendConfirmationEmail(user);

                TempData["SuccessMsg"] = "激活邮件已经发送，请前往邮箱查收";
                return RedirectToAction("LogOn", "Account");
            }

            return Content("Error");
        }

        private void SendConfirmationEmail(MembershipUser user)
        {
            var email = user.Email;
            SendMail("二手交易平台账号注册确认", GetConfirmationBody(user, email), email);
        }

        private void SendMail(string title, string content, string email)
        {
            var smtp = new SmtpClient("smtp.163.com", 25);
            smtp.Credentials = new NetworkCredential("secondhandmarket@163.com", "second123456");
            var message = new MailMessage();
            message.From = new MailAddress("secondhandmarket@163.com", "二手平台admin(no-reply)");
            message.To.Add(email);
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            message.Body = content;
            message.Subject = title;
            message.SubjectEncoding = Encoding.UTF8;

            new Task(() =>
            {
                smtp.SendMailAsync(message);
            }).Start();
        }

        private string GetConfirmationBody(MembershipUser user, string email)
        {
            var emailTpl = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/EmailConfirmation.html"));
            var token = SecurityHelper.TripleDESCrypto(email + "||" + user.UserName + "||" + new Random().NextDouble());

            var url = Request.Url.Scheme + "://" + Request.Url.Authority +
                      Url.Action("Confirmation", "Account") +
                      string.Format("?token={0}", token);

            var body = string.Format(emailTpl, user.UserName, url);
            return body;
        }

        //
        // GET: /Account/ChangePassword
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // 在某些出错情况下，ChangePassword 将引发异常，
                // 而不是返回 false。
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "当前密码不正确或新密码无效。");
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }
        public ActionResult ForgetPwd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPwd(string userName, string verifyCode)
        {
            if (Session["ValidateCode"] == null)
            {
                ModelState.AddModelError("ValidateCode", "验证码已过期");
            }
            else if (verifyCode != Session["ValidateCode"].ToString())
            {
                ModelState.AddModelError("ValidateCode", "请输入正确的验证码");
            }
            var user = Membership.GetUser(userName);
            if (user == null)
            {
                ModelState.AddModelError("UserName", "用户名不存在");
            }
            if (ModelState.IsValid)
            {
                var email = user.Email;
                var title = "找回密码";
                var newPwd = user.ResetPassword();
                var content = GetForgetPwdBody(user, newPwd);
                SendMail(title, content, email);
                TempData["SuccessMsg"] = "系统发送了新密码到您的电子邮件，请查收";
            }

            return View();
        }

        private string GetForgetPwdBody(MembershipUser user, string newPwd)
        {
            var emailTpl = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/EmailForgetPwd.html"));
            return string.Format(emailTpl, user.UserName, newPwd);
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // 请参见 http://go.microsoft.com/fwlink/?LinkID=177550 以查看
            // 状态代码的完整列表。
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "用户名已存在。请输入不同的用户名。";

                case MembershipCreateStatus.DuplicateEmail:
                    return "该电子邮件地址的用户名已存在。请输入不同的电子邮件地址。";

                case MembershipCreateStatus.InvalidPassword:
                    return "提供的密码无效。请输入有效的密码值。";

                case MembershipCreateStatus.InvalidEmail:
                    return "提供的电子邮件地址无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidAnswer:
                    return "提供的密码取回答案无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidQuestion:
                    return "提供的密码取回问题无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidUserName:
                    return "提供的用户名无效。请检查该值并重试。";

                case MembershipCreateStatus.ProviderError:
                    return "身份验证提供程序返回了错误。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";

                case MembershipCreateStatus.UserRejected:
                    return "已取消用户创建请求。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";

                default:
                    return "发生未知错误。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";
            }
        }
        #endregion

        public ActionResult Confirmation(string token)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                token = SecurityHelper.TripleDESCryptoDe(token);
                var infos = token.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                var email = infos[0];
                var userName = infos[1];

                var user = Membership.GetUser(userName);
                if (!user.IsApproved)
                {
                    if (user.Email == email)
                    {
                        user.IsApproved = true;
                        Membership.UpdateUser(user);
                        TempData["SuccessMsg"] = "账号激活成功，请登录";
                        return RedirectToAction("LogOn");
                    }
                }
                else
                {
                    return Content("该账号已经激活!");
                }
            }

            return Content("");
        }

        public ActionResult ResetXXX(string userName)
        {
            var user = Membership.GetUser(userName);
            var pwd = user.ResetPassword();
            return Content(user.ChangePassword(pwd, "123456").ToString());
        }
    }
}
