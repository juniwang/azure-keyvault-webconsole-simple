using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace KeyVaultManagement.Models
{
    public class SecretIndexViewModel
    {
        public List<MySecretItem> Secrets { get; set; }
    }

    public class SecretEditViewModel
    {
        public bool IsEdit { get; set; }
        public bool SetValue { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Value")]
        public string Value { get; set; }

        [Display(Name = "Enabled")]
        public bool Enabled { get; set; }

        [Display(Name = "Set activation date")]
        public bool ActivationDateEnabled { get; set; }

        [Display(Name = "Activation date")]
        public DateTime ActivationDate { get; set; }

        [Display(Name = "Set expiration date")]
        public bool ExpirationDateEnabled { get; set; }

        [Display(Name = "Expiration date")]
        public DateTime ExpirationDate { get; set; }
    }
}