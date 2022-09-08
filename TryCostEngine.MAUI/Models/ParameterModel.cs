using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using MudBlazor.Charts;
using MudBlazor;
using CostEngine;
using System.ComponentModel;
using CostEngine.Data.Models;

namespace TryCostEngine.MAUI
{
    public class ParameterModel
    {
        public ProtocolBaseModel Model { get; set; }
        public string JsonData { get; set; }
        public string TpmLangData { get; set; }
        public List<SelectListModel> Products { get; set; } = new();
    }
}
