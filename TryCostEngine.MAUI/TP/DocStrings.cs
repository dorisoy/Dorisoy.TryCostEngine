using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TryCostEngine.MAUI.Models
{
    // 这是api文档所需要的
    public static partial class DocStrings
    {
        /// <summary>
        /// 要加速该方法，请按以下方式运行：string saveTypename = DocStrings.GetSaveTypename（type）；
        /// 只计算一次文档字符串。GetMemberDescription（saveTypename, member）；
        /// </summary>
        /// <param name="saveTypename"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetMemberDescription(string saveTypename, MemberInfo member)
        {
            string name;

            //if (member.GetType().IsClass)
            //    name = saveTypename;

            if (member is PropertyInfo property)
                name = saveTypename + "_" + property.Name;
            else if (member is MethodInfo method)
                name = saveTypename + "_method_" + GetSaveMethodIdentifier(method);
            else
                name = saveTypename;

            //'BasicInfo_Agent1st'
            var field = typeof(DocStrings).GetField(name, BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField);
            if (field == null)
                return null;

            var desc = (string)field.GetValue(null);

            return desc;
        }

        public static string GetSaveTypename(Type t) => Regex.Replace(t.ConvertToCSharpSource(), @"[\.]", "_").Replace("<T>", "").TrimEnd('_');

        private static string GetSaveMethodIdentifier(MethodInfo method) => Regex.Replace(method.ToString().Replace("TryCostEngine.MAUI.Models.T", "T"), "[^A-Za-z0-9_]", "_");  // method signature
    }
}
