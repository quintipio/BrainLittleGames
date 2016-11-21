using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using JeuxDeLogiqueWin10.Context;
using JeuxDeLogiqueWin10.Interface;
using JeuxDeLogiqueWin10.Model;
using JeuxDeLogiqueWin10.Utils;
using JeuxDeLogiqueWin10.ViewModel;

namespace JeuxDeLogiqueWin10.Views
{
    /// <summary>
    /// Page d'acceuil pour la création ou la sélection des utilisateurs
    /// </summary>
    public sealed partial class AcceuilView : IView<AcceuilViewModel>
    {
        /// <summary>
        /// ViewModel
        /// </summary>
        public AcceuilViewModel ViewModel { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public AcceuilView()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Disabled;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ApparitionLogo.Begin();
            ViewModel = new AcceuilViewModel();
            await ViewModel.Initialization;
            HeaderTextBlock.Text = ContextStatic.NomAppli;

            DateNaissancePicker.Date = DateUtils.GetMaintenant();
            //DateNaissancePicker.MaxYear = DateUtils.SoustraitAnnee(DateUtils.GetMaintenant(), 2);
            //DateNaissancePicker.MinYear = DateUtils.SoustraitAnnee(DateUtils.GetMaintenant(), 140);
            HeaderTextBlock.Text = ContextStatic.NomAppli;
        }
        
        private void ShowAddUserButton_Click(object sender, RoutedEventArgs e)
        {
            ListUserGrid.Visibility = Visibility.Collapsed;
            AjoutUserGrid.Visibility = Visibility.Visible;
            DateNaissancePicker.Date = DateUtils.SoustraitAnnee(DateUtils.GetMaintenant(), 18);
        }

        private async void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (DateNaissancePicker.Date != null)
            {
                var erreurs = await ViewModel.AjouterUtilisateur(NomTextBox.Text, DateNaissancePicker.Date.Value.DateTime);

                if (erreurs.Count > 0)
                {
                    await MessageBox.ShowAsync(StringUtils.ConvertListStringToString(erreurs));
                }
                else
                {
                    DateNaissancePicker.Date = DateUtils.SoustraitAnnee(DateUtils.GetMaintenant(), 18);
                    NomTextBox.Text = "";
                    AjoutUserGrid.Visibility = Visibility.Collapsed;
                    ListUserGrid.Visibility = Visibility.Visible;
                }
            }
            
        }

        private void CancelAddUserButton_Click(object sender, RoutedEventArgs e)
        {
            AjoutUserGrid.Visibility = Visibility.Collapsed;
            ListUserGrid.Visibility = Visibility.Visible;
        }

        private void OuvrirUtilisateur_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ChargerUtilisateur(((Button) sender).Tag as User);
            ((Frame)Window.Current.Content).Navigate(typeof(MenuView));

        }
    }
}
