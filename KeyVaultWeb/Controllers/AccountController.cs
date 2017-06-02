using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using KeyVaultManagement.Models;

namespace KeyVaultManagement.Controllers
{
    public class AccountController : Controller
    {
        public AccountController()
        {
        }

        //
        // GET: /Account/Login
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            var configs = ((ConfigSection)ConfigurationManager.GetSection("keyVault")).Configurations;
            var model = new LoginViewModel()
            {
                ConfigList = new SelectList(configs.Select(config => new SelectListItem { Selected = true, Text = config.ConfigName, Value = config.ConfigName }).ToList(), "Value", "Text")
            };
            
            return View(model);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Response.SetCookie(new HttpCookie("ConfigName", model.ConfigName));

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Secret");
            }
        }

        //
        // GET: /Account/LogOff
        public ActionResult LogOff()
        {
            HttpCookie myCookie = new HttpCookie("ConfigName");
            myCookie.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(myCookie);

            return RedirectToAction("Login", "Account");
        }
    }
}