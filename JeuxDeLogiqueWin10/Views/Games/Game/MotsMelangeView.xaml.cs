using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using JeuxDeLogiqueWin10.Context;
using JeuxDeLogiqueWin10.Enum;
using JeuxDeLogiqueWin10.Interface;
using JeuxDeLogiqueWin10.Model;
using JeuxDeLogiqueWin10.ViewModel.Games.Game;

namespace JeuxDeLogiqueWin10.Views.Games.Game
{
    /// <summary>
    /// Code behind de la vue por le jeu des lettres mélangées 
    /// </summary>
    public sealed partial class MotsMelangeView : IViewGame<MotsMelangeViewModel>
    {

        /// <summary>
        /// ViewModel
        /// </summary>
        public MotsMelangeViewModel ViewModel { get; set; }

        public MotsMelangeView()
        {
            InitializeComponent();
            Lanceur.OnCompteReboursEnd += OnCompteAReboursEnd;
            NavigationCacheMode = NavigationCacheMode.Disabled;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                AppViewBackButtonVisibility.Collapsed;
            ViewModel = new MotsMelangeViewModel(e.Parameter as Exercice);


            LaunchTutoButton.Visibility =
                (ContextAppli.ContextUtilisateur.ModeJeu.Equals(ModeOuvertureJeuEnum.ModeEval))
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            CompteAReboursGrid.Visibility = Visibility.Visible;
            GridJeu.Visibility = Visibility.Collapsed;
            ScoreGrid.Visibility = Visibility.Collapsed;

            //si le tuto n'a pas déjà été vu, visionnage
            if (!await ViewModel.IsTutoPasse())
            {
                ((Frame)Window.Current.Content).Navigate(typeof(TutorielView),
                    ViewModel.ExerciceEnCours);
            }
            //Lancement du compte à rebours du jeu
            Lanceur.StartCompteARebours();
        }

        private void LaunchTutoButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(TutorielView),
                ViewModel.ExerciceEnCours);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(MenuView));
        }

        private async void OnCompteAReboursEnd()
        {
            //Mise en place des élements graphiques
            CompteAReboursGrid.Visibility = Visibility.Collapsed;
            GridJeu.Visibility = Visibility.Visible;

            //Démarrage du jeu
            ViewModel.StartGame();
            await ViewModel.ChargeListeMots();
            ViewModel.GenereMot();
            FocusOnText();
        }

        private void Efface_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.MotEntree = "";
            FocusOnText();
        }

        private async void Valid_Click(object sender, RoutedEventArgs e)
        {
            await ControleMot();
        }

        private async Task ControleMot()
        {
            if (ViewModel.ControleMot())
            {
                if (ViewModel.IsJeuFini())
                {
                    var res = await ViewModel.CalculResult();
                    if (ContextAppli.ContextUtilisateur.ModeJeu.Equals(ModeOuvertureJeuEnum.ModeJeu))
                    {
                        CompteAReboursGrid.Visibility = Visibility.Collapsed;
                        GridJeu.Visibility = Visibility.Collapsed;
                        ScoreGrid.Visibility = Visibility.Visible;
                        await Score.ChargerResultats(res);
                    }

                    if (ContextAppli.ContextUtilisateur.ModeJeu.Equals(ModeOuvertureJeuEnum.ModeEval))
                    {
                        ContextAppli.ContextUtilisateur.ModeEval.AjouterResultat(res);
                        var exercice = await ContextAppli.ContextUtilisateur.ModeEval.ChangeExerciceEval();
                        //si prochain exercice on l'ouvre
                        if (exercice != null)
                        {
                            exercice.Difficulte = ViewModel.ExerciceEnCours.Difficulte;
                            ((Frame)Window.Current.Content).Navigate(exercice.Page, exercice);
                        }
                        //sinon affichage des stats
                        else
                        {
                            var resEvalFnal = await ContextAppli.ContextUtilisateur.ModeEval.GetResultats();
                            CompteAReboursGrid.Visibility = Visibility.Collapsed;
                            GridJeu.Visibility = Visibility.Collapsed;
                            ScoreGrid.Visibility = Visibility.Visible;
                            await Score.ChargerEval(resEvalFnal);
                        }
                    }
                }
                else
                {
                    ViewModel.GenereMot();
                    FocusOnText();
                }
            }
            else
            {
                ViewModel.MotEntree = "";
            }
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GetHelp();
            FocusOnText();
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GenereMot();
            FocusOnText();
        }

        private async void TextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                ViewModel.MotEntree = MotEntreeTextBox.Text;
                await ControleMot();
            }
        }

        private void AfficherClavier_Click(object sender, RoutedEventArgs e)
        {
            FocusOnText();
        }

        private void FocusOnText()
        {
            if ( MotEntreeTextBox.FocusState != FocusState.Keyboard)
            {
                MotEntreeTextBox.Focus(FocusState.Keyboard);
            }
        }
    }
}
