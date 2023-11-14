using System.Collections.Generic;

namespace EcoCentre.Models.ViewModel.MainMenu
{
    public class MenuItem
    {
        private readonly Domain.User.User _user;

        public MenuItem()
        {
            
        }

        public MenuItem(Domain.User.User user, string text, string url, bool showToReadonly = false, bool showToCustomer = false, bool showToAdmin = true)
        {
            _user = user;
            Text = text;
            Url = url;
            ShowToReadonly = showToReadonly;
            ShowToCustomer = showToCustomer;
            ShowToAdmin = showToAdmin;
            SubItems = new List<MenuItem>();
        }
        public bool IsVisible
        {
            get
            {
                if (_user.IsGlobalAdmin) return true;
                if (_user.IsAdmin && ShowToAdmin) return true;
                if (_user.IsReadOnly && ShowToReadonly) return true;
                if (ShowToCustomer && !_user.IsReadOnly) return true;
                return false;
            }
        }
        public IList<MenuItem> SubItems { get; set; } 
        public string Text { get; set; }
        public string Url { get; set; }
        public bool ShowToReadonly { get; set; }
        public bool ShowToCustomer { get; set; }
        public bool ShowToAdmin { get; set; }
    }
}