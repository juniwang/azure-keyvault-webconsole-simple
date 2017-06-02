using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;
using Owin;
using KeyVaultManagement.Models;

namespace KeyVaultManagement.Middlewares
{
    public static class ConfigMiddleware
    {
        public static async Task GetConfig(IOwinContext context, Func<Task> next)
        {
            if (context.Request.Path.Value != "/")
            {
                var configs = ((ConfigSection)ConfigurationManager.GetSection("keyVault")).Configurations;
                var config = configs.Where(c => c.ConfigName == context.Request.Cookies["ConfigName"]).FirstOrDefault();

                if (config != null)
                {
                    var keyVaultConfig = new KeyVaultConfig
                    {
                        ConfigName = config.ConfigName,
                        KeyVaultName = config.KeyVaultName,
                        ClientID = config.ClientID,
                        ClientSecret = config.ClientSecret
                    };

                    context.Set("config", keyVaultConfig);
                    
                    await next();
                }
                else
                {
                    context.Response.Redirect("/?returnUrl=" + HttpUtility.UrlEncode(context.Request.Path.Value));
                }
            }
            else
            {
                await next();
            }
        }
    }
}