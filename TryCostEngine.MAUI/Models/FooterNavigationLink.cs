
namespace TryCostEngine.MAUI.Models
{
    public class NavigationFooterLink
    {
        public string Name { get; set; }

        public string Link { get; set; }

        public NavigationFooterLink() { }

        public NavigationFooterLink(string name, string link)
        {
            Name = name;
            Link = link;
        }

    }
}
