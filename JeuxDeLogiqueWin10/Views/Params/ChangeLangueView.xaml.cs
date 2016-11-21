using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using JeuxDeLogiqueWin10.Strings;

namespace JeuxDeLogiqueWin10.Views.Params
{
    /// <summary>
    /// Control du changement de langue
    /// </summary>
    public sealed partial class ChangeLangueView
    {

        private ObservableCollection<ListeLangues.LanguesStruct> _listeLangues;

        public ChangeLangueView()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Disabled;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _listeLangues = new ObservableCollection<ListeLangues.LanguesStruct>(ListeLangues.GetListesLangues());
            ComboListeLangue.ItemsSource = _listeLangues;
            ComboListeLangue.SelectedItem = ListeLangues.GetLangueEnCours();
        }



        private void comboListeLangue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboListeLangue.SelectedItem is ListeLangues.LanguesStruct)
            {
                ListeLangues.ChangeLangueAppli((ListeLangues.LanguesStruct)ComboListeLangue.SelectedItem);
                AvertissementTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}
