using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using JeuxDeLogiqueWin10.Context;
using JeuxDeLogiqueWin10.Enum;
using JeuxDeLogiqueWin10.Interface;
using JeuxDeLogiqueWin10.Model;
using JeuxDeLogiqueWin10.ViewModel.Games.Game;

namespace JeuxDeLogiqueWin10.Views.Games.Game
{
    /// <summary>
    /// Code behind de la vue pour le mini jeu de calcul mental
    /// </summary>
    public sealed partial class CalculMentalGameView : IViewGame<CalculMentalViewModel>
    {
        /// <summary>
        /// ViewModel
        /// </summary>
        public CalculMentalViewModel ViewModel { get; set; }


        private bool _isScreenKey ;
        
        /// <summary>
        /// Constructeur
        /// </summary>
        public CalculMentalGameView()
        {
            InitializeComponent();
            Lanceur.OnCompteReboursEnd += OnCompteAReboursEnd;
            NavigationCacheMode = NavigationCacheMode.Disabled;
            
        }
        
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                 AppViewBackButtonVisibility.Collapsed;
            ViewModel = new CalculMentalViewModel(e.Parameter as Exercice,30);
            _isScreenKey = false;

            LaunchTutoButton.Visibility =
                (ContextAppli.ContextUtilisateur.ModeJeu.Equals(ModeOuvertureJeuEnum.ModeEval))
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            CompteAReboursGrid.Visibility = Visibility.Visible;
            GridJeu.Visibility = Visibility.Collapsed;
            ScoreGrid.Visibility = Visibility.Collapsed;

            //si le tuto n'a pas déjà été vu, visionnage
            if  (!await ViewModel.IsTutoPasse())
            {
                ((Frame)Window.Current.Content).Navigate(typeof(TutorielView), ViewModel.ExerciceEnCours);
            }

            //Lancement du compte à rebours du jeu
            Lanceur.StartCompteARebours();
        }
        


        private void LaunchTutoButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(TutorielView), ViewModel.ExerciceEnCours);   
        }

        //fin du compte à rebours et lancement du jeu
        private void OnCompteAReboursEnd()
        {
            CompteAReboursGrid.Visibility = Visibility.Collapsed;
            GridJeu.Visibility = Visibility.Visible;
            ViewModel.StartGame();
            OperationEnCoursTextBlock.Text = ViewModel.GenererOperation();
        }




        #region Clavier
        private void Chiffre_Click(object sender, RoutedEventArgs e)
        {
            ResultatTextBox.Text += ((Button)sender).Content;
        }

        private void CaracMoins_Click(object sender, RoutedEventArgs e)
        {
            if (!ResultatTextBox.Text.Contains("-"))
            {
                ResultatTextBox.Text = "-" + ResultatTextBox.Text;
            }
            else
            {
                ResultatTextBox.Text = ResultatTextBox.Text.Remove(0, 1);
            }
        }

        private void Effacer_Click(object sender, RoutedEventArgs e)
        {
            ResultatTextBox.Text = "";
        }

        private async void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(ResultatTextBox.Text))
            {
                ImageOld.Source = null;
                int monResultat;
                int.TryParse(ResultatTextBox.Text, out monResultat);

                //vérification du résultat
                var img = ViewModel.VerifierOperation(monResultat) ? new BitmapImage(new Uri(@"ms-appx:///Rsc/img/right.png")) : new BitmapImage(new Uri(@"ms-appx:///Rsc/img/wrong.png"));
                OperationEnCoursTextBlock.Text += ResultatTextBox.Text;
                ResultatTextBox.Text = "";

                if (_isScreenKey)
                {
                    _isScreenKey = false;
                    ChangeSize();
                }


                var margin = (int) (GridAffichage.ActualHeight/2);
                int marginInverse;
                int.TryParse("-" + margin, out marginInverse);
                ViewModel.MarginHaut = marginInverse +
                                                                   (int) OperationOldTextBlock.ActualHeight;
                ViewModel.MarginCentre = (int) (GridJeu.ActualHeight/2);

                //animation pour le changement d'opération
                var operation = ViewModel.GenererOperation();
                OperationNewTextBlock.Text = operation;
                ImageResult.Source = img;
                StoryboardChangeOperation.Begin();
                await Task.Delay(TimeSpan.FromMilliseconds(200));
                StoryboardChangeOperation.Stop();

                ImageOld.Source = img;
                ImageResult.Source = null;
                OperationOldTextBlock.Text = OperationEnCoursTextBlock.Text;
                OperationEnCoursTextBlock.Text = operation;
                OperationNewTextBlock.Text = "";

                //vérification si le jeu est fini
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
            }
        }
        #endregion

        private void GotoOtherScreen_Click(object sender, RoutedEventArgs e)
        {
            _isScreenKey = !_isScreenKey;
            ChangeSize();
        }

        private void GridJeu_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChangeSize();
        }

        private void ChangeSize()
        {
            if (ActualHeight < 500)
            {
                if (_isScreenKey)
                {
                    GridJeu.RowDefinitions[0].Height = new GridLength(0, GridUnitType.Pixel);
                    GridJeu.RowDefinitions[1].Height = new GridLength(50, GridUnitType.Pixel);
                    GridJeu.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
                }
                else
                {
                    GridJeu.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
                    GridJeu.RowDefinitions[1].Height = new GridLength(50, GridUnitType.Pixel);
                    GridJeu.RowDefinitions[2].Height = new GridLength(0, GridUnitType.Pixel);
                }
            }
            else
            {
                GridJeu.RowDefinitions[2].Height = new GridLength(250, GridUnitType.Pixel);
                GridJeu.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
                GridJeu.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Pixel);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(MenuView));
        }
    }
}
