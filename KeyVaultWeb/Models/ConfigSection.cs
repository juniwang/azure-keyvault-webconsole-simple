using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace KeyVaultManagement.Models
{
    public class ConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("keyVaultConfigurations", IsDefaultCollection = true, IsRequired = true)]
        public ConfigCollection Configurations
        {
            get
            {
                return (ConfigCollection)this["keyVaultConfigurations"];
            }
            set
            {
                this["keyVaultConfigurations"] = value;
            }
        }
    }

    [ConfigurationCollection(typeof(ConfigElement), AddItemName = "keyVaultConfiguration")]
    public class ConfigCollection : ConfigurationElementCollection, IEnumerable<ConfigElement>
    {
        public ConfigElement this[int index]
        {
            get { return BaseGet(index) as ConfigElement; }
        }

        public new IEnumerator<ConfigElement> GetEnumerator()
        {
            return (from item in Enumerable.Range(0, Count)
                    select this[item]).GetEnumerator();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var lConfigElement = element as ConfigElement;

            return lConfigElement != null ? lConfigElement.ConfigName : null;
        }
    }

    public class ConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string ConfigName
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("keyVaultName", IsRequired = true)]
        public string KeyVaultName
        {
            get
            {
                return (string)this["keyVaultName"];
            }
            set
            {
                this["keyVaultName"] = value;
            }
        }

        [ConfigurationProperty("clientID", IsRequired = true)]
        public string ClientID
        {
            get
            {
                return (string)this["clientID"];
            }
            set
            {
                this["clientID"] = value;
            }
        }

        [ConfigurationProperty("clientSecret", IsRequired = true)]
        public string ClientSecret
        {
            get
            {
                return (string)this["clientSecret"];
            }
            set
            {
                this["clientSecret"] = value;
            }
        }
    }
}