using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Maanfee.Web.Core
{
    public static class AttributeManagerExtension
    {
        public static DisplayAttribute MaanfeeDisplayAttribute(this MemberInfo MemberInfo)
        {
            return MemberInfo.GetCustomAttribute<DisplayAttribute>(true) ?? null;
        }

        public static DescriptionAttribute MaanfeeDescriptionAttribute(this MemberInfo MemberInfo)
        {
            return MemberInfo.GetCustomAttribute<DescriptionAttribute>(true) ?? null;
        }

        public static MemberInfo[] MaanfeeGetMembers(this Type Type)
        {
            //Type t = typeof(T);
            return Type.GetMembers(BindingFlags.Static | BindingFlags.Public);
        }

    }
}
