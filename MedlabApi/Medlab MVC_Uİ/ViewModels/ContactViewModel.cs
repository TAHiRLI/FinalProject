namespace Medlab_MVC_Uİ.ViewModels
{
    public class ContactViewModel
    {
        public Dictionary<string, string> Setting { get; set;  } = new Dictionary<string, string>();
        public ContactMesssageViewModel ContactMesssageVm   { get; set; } = new ContactMesssageViewModel();
    }
}
