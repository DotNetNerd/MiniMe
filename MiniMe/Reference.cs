using System.Web;
using MiniMe.CoffeeScript;
using MiniMe.JavaScript;
using MiniMe.Less;
using MiniMe.Scss;
using MiniMe.StyleSheet;
using MiniMe.Cdn;

namespace MiniMe
{
    public static class Reference
    {
        public static IMiniStyleSheetBuilder StyleSheet
        {
            get
            {
                return Get<MiniStyleSheetBuilder>("Reference_Stylesheet");
            }
        }        

        public static IMiniJavaScriptBuilder JavaScript
        {
            get
            {
                return Get<MiniJavaScriptBuilder>("Reference_JavaScript");
            }
        }

        public static IMiniCdnList Cdn
        {
            get
            {
                return Get<MiniCdnList>("Reference_Cdn");
            }
        }

        public static IMiniCoffeeScriptBuilder CoffeeScript
        {
            get
            {
                return Get<MiniCoffeeScriptBuilder>("Reference_CoffeeScript");
            }
        }

        public static IMiniScssBuilder Scss
        {
            get
            {
                return Get<MiniScssBuilder>("Reference_Scss");
            }
        }

		public static IMiniLessBuilder Less
		{
			get
			{
				return Get<MiniLessBuilder>("Reference_Less");
			}
		}

        private static T Get<T>(string key) where T:new()
        {
            if (HttpContext.Current.Items[key] == null) HttpContext.Current.Items[key] = new T();
            return (T)HttpContext.Current.Items[key];
        }
    }
}