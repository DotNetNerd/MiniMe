using System.Configuration;

namespace MiniMe.Configuration
{
    public class MiniMeConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("MiniJsRelativePath", IsRequired = false)]
        public string MiniJsRelativePath
        {
            get
            {
                return (string)this["MiniJsRelativePath"];
            }
            set
            {
                this["MiniJsRelativePath"] = value;
            }
        }

        [ConfigurationProperty("MiniCssRelativePath", IsRequired = false)]
        public string MiniCssRelativePath
        {
            get
            {
                return (string)this["MiniCssRelativePath"];
            }
            set
            {
                this["MiniCssRelativePath"] = value;
            }
        }
    }
}
