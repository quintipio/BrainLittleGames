using Windows.UI.Xaml.Navigation;
using JeuxDeLogiqueWin10.Context;

namespace JeuxDeLogiqueWin10.Views.Params
{
    /// <summary>
    /// Control de la page a propos de
    /// </summary>
    public sealed partial class AppdView 
    {

        public AppdView()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Disabled;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            HeaderTextBlock.Text = ContextStatic.NomAppli;
            VersionTextBlock.Text = ContextStatic.Version;
            DeveloppeurTextBlock.Text = ContextStatic.Developpeur;
        }
    }
}
