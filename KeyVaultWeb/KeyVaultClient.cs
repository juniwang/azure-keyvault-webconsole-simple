using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using KeyVaultManagement.Models;

namespace KeyVaultManagement
{
    public class MyKeyVaultClient
    {
        private string keyVaultName;
        private string url;
        private KeyVaultClient keyVaultClient;

        public MyKeyVaultClient(KeyVaultConfig config)
        {
            this.keyVaultName = config.KeyVaultName;
            this.url = string.Format("https://{0}.vault.azure.cn/", keyVaultName.ToLower());
            var clientCredential = new ClientCredential(config.ClientID, config.ClientSecret);
            this.keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(
                   (authority, resource, scope) => GetAccessToken(authority, resource, scope, clientCredential)));
        }

        private static async Task<string> GetAccessToken(string authority, string resource, string scope, ClientCredential assertionCert)
        {
            var context = new AuthenticationContext(authority, TokenCache.DefaultShared);
            var result = await context.AcquireTokenAsync(resource, assertionCert).ConfigureAwait(false);

            return result.AccessToken;
        }

        public async Task<List<MySecretItem>> ListSecrets()
        {
            var results = await keyVaultClient.GetSecretsAsync(this.url);
            return results.Value.Select(result => new MySecretItem {
                Name = result.Identifier.Name,
                Enabled = result.Attributes.Enabled,
                ActivationDate = result.Attributes.NotBefore,
                ExpirationDate = result.Attributes.Expires
            }).ToList();
        }

        public async Task DeleteSecret(string name)
        {
            await keyVaultClient.DeleteSecretAsync(this.url, name);
        }

        public async Task<MySecretItem> GetSecret(string name)
        {
            try
            {
                var result1 = await keyVaultClient.GetSecretAsync(this.url, name);
                return new MySecretItem
                {
                    Name = result1.SecretIdentifier.Name,
                    Value = result1.Value,
                    Enabled = result1.Attributes.Enabled,
                    ActivationDate = result1.Attributes.NotBefore,
                    ExpirationDate = result1.Attributes.Expires
                };
            }
            catch (Exception ex)
            {
                try
                {
                    var results = await keyVaultClient.GetSecretsAsync(this.url);
                    var result2 = results.Value.Where(r => r.Identifier.Name == name).First();
                    return new MySecretItem
                    {
                        Name = result2.Identifier.Name,
                        Value = string.Empty,
                        Enabled = result2.Attributes.Enabled,
                        ActivationDate = result2.Attributes.NotBefore,
                        ExpirationDate = result2.Attributes.Expires
                    };
                }
                catch (Exception innerEx)
                {
                    throw ex;
                }
            }
        }

        public async Task<string> GetSecretValue(string name)
        {
            try
            {
                var result = await keyVaultClient.GetSecretAsync(this.url, name);
                return result.Value;
            }
            catch
            {
                return string.Empty;
            }
        }

        public async Task SetSecret(MySecretItem secret)
        {
            await keyVaultClient.SetSecretAsync(this.url, secret.Name, secret.Value, null, null, new SecretAttributes
            {
                Enabled = secret.Enabled,
                NotBefore = secret.ActivationDate,
                Expires = secret.ExpirationDate
            });
        }

        public async Task UpdateSecret(MySecretItem secret)
        {
            await keyVaultClient.UpdateSecretAsync(this.url, secret.Name, null, new SecretAttributes
            {
                Enabled = secret.Enabled,
                NotBefore = secret.ActivationDate,
                Expires = secret.ExpirationDate
            });
        }
    }
}