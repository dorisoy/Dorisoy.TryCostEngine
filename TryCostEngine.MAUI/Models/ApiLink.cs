using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using MudBlazor.Charts;
using MudBlazor;
using CostEngine;
using System.ComponentModel;

namespace TryCostEngine.MAUI.Models
{
    public enum Grouping
    {
        /// <summary>
        /// 类别
        /// </summary>
        [Description("类别")]
        Categories,

        /// <summary>
        /// 继承
        /// </summary>
        [Description("继承")]
        Inheritance,

        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None
    }

    public static class ApiLink
    {
        public static string GetApiLinkFor(Type type)
        {
            return $"api/{GetComponentName(type)}";
        }

        public static string GetComponentLinkFor(Type type)
        {
            return $"components/{GetComponentName(type)}";
        }

        public static string GetModelLinkFor(Type type)
        {
            return $"tp/{GetComponentName(type)}";
        }

      
        public static Type GetTypeFromComponentLink(string component)
        {
            //component = "costengine.profit.baseprofit";
            //component = "baseprofit";

            if (component.Contains('#') == true)
            {
                component = component.Substring(0, component.IndexOf('#'));
            }

            if (string.IsNullOrEmpty(component))
                return null;

            if (s_inverseSpecialCase.TryGetValue(component, out var type))
                return type;

            var ass = Assembly.GetEntryAssembly().GetReferencedAssemblies();
            var assnot = ass.Where(x => !x.Name.StartsWith("Microsoft.") && !x.Name.StartsWith("System.")).ToList();

            //var assembly = typeof(MudComponentBase).Assembly;

            //获取CostEngine程序集
            var ceAassembly = ass.Where(x => x.Name.StartsWith("CostEngine")).Select(x => Assembly.Load(x)).First();
            var edAassembly = ass.Where(x => x.Name.StartsWith("CostEngine.Data")).Select(x => Assembly.Load(x)).First();


            Type rx = null;

            //查找CostEngine程序集
            foreach (var x in ceAassembly.GetTypes())
            {
                //Console.WriteLine($"{x.Name}");

                if (new string(x.Name.ToLowerInvariant().TakeWhile(c => c != '`').ToArray()) == $"{component}".ToLowerInvariant())
                {
                    if (x.Name.Contains('`'))
                    {
                        rx= x.MakeGenericType(typeof(T));
                        return rx;
                    }
                    else if (x.Name.ToLowerInvariant() == $"{component}".ToLowerInvariant())
                    {
                        rx = x;
                        return rx;
                    }
                }
            }


            //查找CostEngine.Data程序集
            if (rx == null)
            {
                foreach (var x in edAassembly.GetTypes())
                {
                    //Console.WriteLine($"{x.Name}");

                    if (new string(x.Name.ToLowerInvariant().TakeWhile(c => c != '`').ToArray()) == $"{component}".ToLowerInvariant())
                    {
                        if (x.Name.Contains('`'))
                        {
                            rx = x.MakeGenericType(typeof(T));
                            return rx;
                        }
                        else if (x.Name.ToLowerInvariant() == $"{component}".ToLowerInvariant())
                        {
                            rx = x;
                            return rx;
                        }
                    }
                }
            }

            return rx;
        }


        public static Assembly[] GetSolutionAssemblies()
        {
            var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                                .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x)));
            return assemblies.ToArray();
        }

        private static string GetComponentName(Type type)
        {
            if (!s_specialCaseComponents.TryGetValue(type, out var component))
            {
                //using CostEngine.Entity;
                //using CostEngine.Profit;
                //using CostEngine.Util;

                component = new string(type.ToString()
                    .Replace("CostEngine.Entity.", "")
                    .Replace("CostEngine.Profit.", "")
                    .Replace("CostEngine.Util.", "")
                    .Replace("CostEngine.", "")

                    .Replace("CostEngine.Data.", "")
                   
                    .TakeWhile(c => c != '`').ToArray())
                    .ToLowerInvariant();
            }

            return component;
        }

        private static Dictionary<Type, string> s_specialCaseComponents =
            new()
            {
                [typeof(MudFab)] = "buttonfab",
                [typeof(MudIcon)] = "icons",
                [typeof(MudProgressCircular)] = "progress",
                [typeof(MudText)] = "typography",
                [typeof(MudSnackbarProvider)] = "snackbar",
                [typeof(Bar)] = "barchart",
                [typeof(Donut)] = "donutchart",
                [typeof(Line)] = "linechart",
                [typeof(Pie)] = "piechart",
                [typeof(MudChip)] = "chips",
                [typeof(ChartOptions)] = "options"
            };

        // this is the inversion of above lookup
        private static Dictionary<string, Type> s_inverseSpecialCase =
            s_specialCaseComponents.ToDictionary(pair => pair.Value, pair => pair.Key);
    }
}
