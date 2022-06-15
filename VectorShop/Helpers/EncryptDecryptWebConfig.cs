using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Configuration;

namespace VectorShop.Helpers
{
    public class EncryptDecryptWebConfig
    {
        public static void EncryptConnString()
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            ConfigurationSection section = config.GetSection("connectionStrings");

            if (!section.SectionInformation.IsProtected)
            {
                section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                config.Save();
            }
        }
        public static void DecryptConnString()
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            ConfigurationSection section = config.GetSection("connectionStrings");
            if (section.SectionInformation.IsProtected)
            {
                section.SectionInformation.UnprotectSection();
                config.Save();
            }
        }
        public static void EncryptMailSettings()
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            ConfigurationSection section = config.GetSection("system.net/mailSettings/smtp");

            if (!section.SectionInformation.IsProtected)
            {
                section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                config.Save();
            }
        }
        public static void DecryptMailSettings()
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            ConfigurationSection section = config.GetSection("system.net/mailSettings/smtp");
            if (section.SectionInformation.IsProtected)
            {
                section.SectionInformation.UnprotectSection();
                config.Save();
            }
        }
    }
}