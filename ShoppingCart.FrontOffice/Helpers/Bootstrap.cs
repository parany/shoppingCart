using System.Collections.Generic;

namespace ShoppingCart.Helpers
{
    public class Bootstrap
    {
        public const string BundleBase = "~/Content/css/";

        public class Theme
        {
            public const string Cyborg = "cyborg";
            public const string Darkly = "darkly";
            public const string Default = "default";
            public const string Flatly = "flatly";
            public const string Lumen = "lumen";
            public const string Simplex = "simplex";
            public const string Spacelab = "spacelab";
            public const string Superhero = "superhero";
            public const string United = "united";
            public const string Yeti = "yeti";
        }

        public static HashSet<string> Themes = new HashSet<string>
        {
            Theme.Cyborg,
            Theme.Darkly,
            Theme.Default,
            Theme.Flatly,
            Theme.Lumen,
            Theme.Simplex,
            Theme.Spacelab,
            Theme.Superhero,
            Theme.United,
            Theme.Yeti
        };

        public static string Bundle(string themename)
        {
            return BundleBase + themename;
        }
    }
}