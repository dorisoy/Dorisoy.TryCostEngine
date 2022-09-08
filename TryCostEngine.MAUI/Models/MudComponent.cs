using System;
using System.Collections.Generic;

namespace TryCostEngine.MAUI.Models
{
    public class MudComponent
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public bool IsNavGroup { get; set; }
        public bool Hide { get; set; }
        public bool NavGroupExpanded { get; set; }

        public List<MudComponent> GroupComponents { get; set; }

        Type _type;
        public Type Type { 
            get => _type; 
            set { 
                _type = value; 
                ComponentName = Type.Name.Replace("`1", "<T>");
            } 
        }
        public Type[] ChildTypes { get; set; }

        public string ComponentName { get; private set; }
    }
}
