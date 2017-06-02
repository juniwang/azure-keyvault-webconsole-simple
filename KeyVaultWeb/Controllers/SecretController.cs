using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using KeyVaultManagement.Models;

namespace KeyVaultManagement.Controllers
{
    public class SecretController : Controller
    {
        private KeyVaultConfig keyVaultConfig;
        private MyKeyVaultClient keyVaultClient;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            this.keyVaultConfig = HttpContext.GetOwinContext().Get<KeyVaultConfig>("config");
            this.keyVaultClient = new MyKeyVaultClient(this.keyVaultConfig);
            ViewBag.configName = this.keyVaultConfig.ConfigName;

            base.OnActionExecuting(filterContext);
        }

        //
        // GET: /Secret/Index
        [HandleError]
        public async Task<ActionResult> Index()
        {
            SecretIndexViewModel model = new SecretIndexViewModel()
            {
                Secrets = await keyVaultClient.ListSecrets()
            };

            return View(model);
        }

        //
        // POST: /Secret/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public async Task<ActionResult> Delete(string name)
        {
            await keyVaultClient.DeleteSecret(name);
            return RedirectToAction("Index", "Secret");
        }

        //
        // GET: /Secret/Edit
        [HandleError]
        public async Task<ActionResult> Edit()
        {
            var id = Url.RequestContext.RouteData.Values["id"];
            var name = id != null ? id.ToString() : null;

            SecretEditViewModel model;
            if (!string.IsNullOrEmpty(name))
            {
                var secret = await keyVaultClient.GetSecret(name);
                model = new SecretEditViewModel
                {
                    IsEdit = true,
                    Name = secret.Name,
                    Value = secret.Value,
                    SetValue = secret.Enabled ?? false,
                    Enabled = secret.Enabled ?? false,
                    ActivationDateEnabled = secret.ActivationDate.HasValue,
                    ActivationDate = secret.ActivationDate ?? DateTime.Now,
                    ExpirationDateEnabled = secret.ExpirationDate.HasValue,
                    ExpirationDate = secret.ExpirationDate ?? DateTime.Now.AddYears(2)
                };
            }
            else
            {
                model = new SecretEditViewModel
                {
                    IsEdit = false,
                    Name = string.Empty,
                    Value = string.Empty,
                    SetValue = true,
                    Enabled = true,
                    ActivationDateEnabled = false,
                    ActivationDate = DateTime.Now,
                    ExpirationDateEnabled = false,
                    ExpirationDate = DateTime.Now.AddYears(2)
                };
            }

            return View(model);
        }

        //
        // POST: /Secret/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public async Task<ActionResult> Edit(SecretEditViewModel model)
        {
            var secret = new MySecretItem
            {
                Name = model.Name,
                Value = model.Value,
                Enabled = model.Enabled,
                ActivationDate = model.ActivationDateEnabled ? (DateTime?)model.ActivationDate : null,
                ExpirationDate = model.ExpirationDateEnabled ? (DateTime?)model.ExpirationDate : null,
            };

            if (model.SetValue)
            {
                await keyVaultClient.SetSecret(secret);
            }
            else
            {
                await keyVaultClient.UpdateSecret(secret);
            }

            return RedirectToAction("Index", "Secret");
        }
    }
}