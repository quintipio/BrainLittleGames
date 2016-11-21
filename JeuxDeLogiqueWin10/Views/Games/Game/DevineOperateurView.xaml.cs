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
    /// Cod behind de la vue pour le jeu du choix d'opérateur
    /// </summary>
    public sealed partial class DevineOperateurView : IViewGame<DevineOperateurViewModel>
    {
        /// <summary>
        /// ViewModel
        /// </summary>
        public DevineOperateurViewModel ViewModel { get; set; }

        private bool _verrouiller;

        /// <summary>
        /// Constructeur
        /// </summary>
        public DevineOperateurView()
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
            ViewModel = new DevineOperateurViewModel(e.Parameter as Exercice, 40);
            _verrouiller = false;


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



        private async void Signe_Click(object sender, RoutedEventArgs e)
        {
            
            var signe = ((Button) sender).Content;
            bool reponse;
            if (signe.Equals("+"))
            {
                reponse = ViewModel.IsOperateurCorrect(OperationEnum.Addition);
            }
            else if (signe.Equals("-"))
            {
                reponse = ViewModel.IsOperateurCorrect(OperationEnum.Soustraction);
            }
            else if (signe.Equals("x"))
            {
                reponse = ViewModel.IsOperateurCorrect(OperationEnum.Multiplication);
            }
            else
            {
                reponse = ViewModel.IsOperateurCorrect(OperationEnum.Division);
            }

            var img = reponse ? new BitmapImage(new Uri(@"ms-appx:///Rsc/img/right.png")) : new BitmapImage(new Uri(@"ms-appx:///Rsc/img/wrong.png"));

            var margin = (int)(GridAffichage.ActualHeight / 2);
            int marginInverse;
            int.TryParse("-" + margin, out marginInverse);
            ViewModel.MarginHaut = marginInverse +
                                                               (int)OperationOldTextBlock.ActualHeight;
            ViewModel.MarginCentre = (int)(GridJeu.ActualHeight / 2);

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
            if (ViewModel.IsJeuFini() && !_verrouiller)
            {
                _verrouiller = true;
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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(MenuView));
        }
    }
}
