using System.Globalization;

namespace blqw.Kanai.Core
{
    public static class ResourceStringManager
    {
        public readonly static ResourceStrings ZH_CN = new ResourceStrings();

        public static ResourceStrings GetResource(CultureInfo cultureInfo) => ZH_CN;
    }
}
