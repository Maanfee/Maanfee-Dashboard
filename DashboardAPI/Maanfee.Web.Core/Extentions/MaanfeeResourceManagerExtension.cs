using System.Resources;

namespace Maanfee.Web.Core
{
    public static class MaanfeeResourceManagerExtension
    {
        public static string MaanfeeGetResourceValue<T>(this string Key)
        {
            return new ResourceManager(typeof(T)).GetString(Key);
        }
    }
}
