using System.Configuration;

namespace Utils.Data.Access
{
    public static class Cipher
    {
        public static void SecureConnectionStrings()
        {
            Configuration config
                = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetEntryAssembly().Location);
            ConfigurationSection section = config.GetSection("connectionStrings");
            if (!section.SectionInformation.IsProtected)
            {
                //RsaProtectedConfigurationProvider
                //DataProtectionConfigurationProvider                
                section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                config.Save();
            }
        }
    }
}
