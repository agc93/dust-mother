using DustMother.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace DustMother.App.Pages
{
    public class WidgetPage<TSave, TModel> : Page where TSave : WingmanSave where TModel : SaveViewModel<TSave>, new()
    {
        protected WidgetHandle _self;

        public WidgetPage()
        {

        }
        public WidgetPage(DependencyObject self)
        {
            ViewModel?.SetContext(self);
        }

        public TModel ViewModel { get; set; } = new TModel();
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null && e.Parameter is WidgetHandle)
            {
                _self = e.Parameter as WidgetHandle;
            }
            await this.ViewModel?.CheckFileAccessAsync();
            await this.ViewModel?.Refresh();
        }
    }
}
