using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace KeyVaultManagement.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Configuration")]
        public string ConfigName { get; set; }

        public SelectList ConfigList { get; set; }
    }
}
