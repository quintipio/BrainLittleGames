using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Store;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using JeuxDeLogiqueWin10.Context;
using JeuxDeLogiqueWin10.Enum;
using JeuxDeLogiqueWin10.Interface;
using JeuxDeLogiqueWin10.Model;
using JeuxDeLogiqueWin10.Utils;
using JeuxDeLogiqueWin10.ViewModel;
using JeuxDeLogiqueWin10.Views.Games;
using JeuxDeLogiqueWin10.Views.Params;
using JeuxDeLogiqueWin10.Views.UserPage;

namespace JeuxDeLogiqueWin10.Views
{
    /// <summary>
    /// Page du menu principal pour un utilisateur
    /// </summary>
    public sealed partial class MenuView : IView<MenuViewModel>
    {
        /// <summary>
        /// ViewModel
        /// </summary>
        public MenuViewModel ViewModel { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public MenuView()
        {
            InitializeComponent();
            ViewModel = new MenuViewModel();
        }
        
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await (Application.Current as App).CheckLicense();
            await ViewModel.Initialization;

            if (App.IsFull || !CurrentApp.LicenseInformation.IsTrial)
            {
                EvaluationPivot.Visibility = Visibility.Visible;
                FullGameSection.Visibility = Visibility.Visible;
                TrialGameSection.Visibility = Visibility.Collapsed;
            }
            else
            {
                TrialGameSection.Visibility = Visibility.Visible;
                EvaluationPivot.Visibility = Visibility.Collapsed;
                FullGameSection.Visibility = Visibility.Collapsed;
            }

            ApparitionLogo.Begin();
            HeaderTextBlock.Text = ContextStatic.NomAppli;
        }
        #region Exercice

        private void OuvrirExercice_Click(object sender, RoutedEventArgs e)
        {
            ContextAppli.ContextUtilisateur.ModeJeu = ModeOuvertureJeuEnum.ModeJeu;
            ((Frame)Window.Current.Content).Navigate(typeof(DifficulteView), ((Button)sender).Tag);
        }


        private async void OuvrirExerciceTrial_Click(object sender, RoutedEventArgs e)
        {
            var exercice = ((Button)sender).Tag as Exercice;

            if (exercice != null)
            {
                if (exercice.IsDispoTrial)
                {
                    ContextAppli.ContextUtilisateur.ModeJeu = ModeOuvertureJeuEnum.ModeJeu;
                    ((Frame)Window.Current.Content).Navigate(typeof(DifficulteView), exercice);
                }
                else
                {
                    await Launcher.LaunchUriAsync(new Uri("ms-windows-store://pdp/?PFN=" + Package.Current.Id.FamilyName));
                }
            }
            
        }


        private void DemarrerEval_Click(object sender, RoutedEventArgs e)
        {
            ContextAppli.ContextUtilisateur.ModeJeu = ModeOuvertureJeuEnum.ModeEval;
            ((Frame)Window.Current.Content).Navigate(typeof(DifficulteView), ModeOuvertureJeuEnum.ModeEval);
        }

        #endregion

        #region Compte
        private void StatsEvaluation_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(StatView),ModeOuvertureStatsEnum.Evaluation);
        }

        private void StatsCategorie_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(ChoixStatsView), ModeOuvertureStatsEnum.Categories);
        }

        private void StatExercice_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(ChoixStatsView), ModeOuvertureStatsEnum.Exercices);
        }

        private void ChangeUtilisateur_OnClick_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(AcceuilView));
        }


        private void StatGen_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(StatView), ModeOuvertureStatsEnum.General);
        }

        private void QuitterApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private async void SupprimerCompteButton_OnClick_Click(object sender, RoutedEventArgs e)
        {
            var result = await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView().GetString("AvertissementSupressionCompte"), ResourceLoader.GetForCurrentView().GetString("SupressionCompteTitre"), MessageBoxButton.OkCancel);
            if (result == MessageBoxResult.Ok)
            {
                await ViewModel.SupprimerCompte();
                ((Frame)Window.Current.Content).Navigate(typeof(AcceuilView));
            }
        }

        #endregion

        #region Paramètres
        private void ChangerLangue_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(ChangeLangueView));
        }

        private void Appd_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(AppdView));
        }

        private async void Rate_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store://review/?PFN="+ Package.Current.Id.FamilyName));
        }

        private async void Bugs_Click(object sender, RoutedEventArgs e)
        {
            var mailto = new Uri("mailto:?to=" + ContextStatic.Support + "&subject=Bugs ou suggestions pour " + ContextStatic.NomAppli);
            await Launcher.LaunchUriAsync(mailto);
        }
        #endregion

    }
}
