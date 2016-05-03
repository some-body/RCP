namespace AdminFrontend.Models
{
    public class Breadcrump
    {
        public string Title { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public bool Unclickable { get; set; } = false;
    }
}