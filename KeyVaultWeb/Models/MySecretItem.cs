using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyVaultManagement.Models
{
    public class MySecretItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool? Enabled { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}