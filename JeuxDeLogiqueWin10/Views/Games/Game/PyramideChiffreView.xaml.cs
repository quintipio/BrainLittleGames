using System;
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
    /// Code behind de la vue du jeu de tricalcul
    /// </summary>
    public sealed partial class PyramideChiffreView : IViewGame<PyramideChiffreViewModel>
    {
        /// <summary>
        /// ViewModel
        /// </summary>
        public PyramideChiffreViewModel ViewModel { get; set; }

        private readonly DispatcherTimer _timer;
        private bool _isScreenKey;
        private bool _isBlock;

        /// <summary>
        /// Constructeur
        /// </summary>
        public PyramideChiffreView()
        {
            InitializeComponent();
            Lanceur.OnCompteReboursEnd += OnCompteAReboursEnd;
            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(650) };
            _timer.Tick += FinCompteAReboursChangement;
            NavigationCacheMode = NavigationCacheMode.Disabled;
        }
        
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                 AppViewBackButtonVisibility.Collapsed;
            ViewModel = new PyramideChiffreViewModel(e.Parameter as Exercice);

            LaunchTutoButton.Visibility =
                (ContextAppli.ContextUtilisateur.ModeJeu.Equals(ModeOuvertureJeuEnum.ModeEval))
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            CompteAReboursGrid.Visibility = Visibility.Visible;
            GridJeu.Visibility = Visibility.Collapsed;
            ScoreGrid.Visibility = Visibility.Collapsed;
            _isScreenKey = false;
            _isBlock = false;

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
            if (ViewModel.IsAffichePremiereLigne())
            {
                SigneAPremiereLigneTextBlock.Visibility = Visibility.Visible;
                SigneBPremiereLigneTextBlock.Visibility = Visibility.Visible;
                SigneCPremiereLigneTextBlock.Visibility = Visibility.Visible;

                ChiffreAPremiereLigneTextBlock.Visibility = Visibility.Visible;
                ChiffreBPremiereLigneTextBlock.Visibility = Visibility.Visible;
                ChiffreCPremiereLigneTextBlock.Visibility = Visibility.Visible;
                ChiffreDPremiereLigneTextBlock.Visibility = Visibility.Visible;

                ChiffreAPremiereLigneTextBlock.Text = "";
                ChiffreAPremiereLigneTextBlock.Text = "";
                ChiffreAPremiereLigneTextBlock.Text = "";
                ChiffreAPremiereLigneTextBlock.Text = "";
            }
            else
            {
                SigneAPremiereLigneTextBlock.Visibility = Visibility.Collapsed;
                SigneBPremiereLigneTextBlock.Visibility = Visibility.Collapsed;
                SigneCPremiereLigneTextBlock.Visibility = Visibility.Collapsed;

                ChiffreAPremiereLigneTextBlock.Visibility = Visibility.Collapsed;
                ChiffreBPremiereLigneTextBlock.Visibility = Visibility.Collapsed;
                ChiffreCPremiereLigneTextBlock.Visibility = Visibility.Collapsed;
                ChiffreDPremiereLigneTextBlock.Visibility = Visibility.Collapsed;

                ChiffreADeuxiemeLigneTextBlock.Text = "";
                ChiffreBDeuxiemeLigneTextBlock.Text = "";
                ChiffreCDeuxiemeLigneTextBlock.Text = "";
            }
            CompteAReboursGrid.Visibility = Visibility.Collapsed;
            GridJeu.Visibility = Visibility.Visible;
            ViewModel.StartGame();
            GenererPyramide();
        }

        /// <summary>
        /// Genere une pyramide à calculer
        /// </summary>
        private void GenererPyramide()
        {
            ViewModel.GenererOperation();
            if (ViewModel.IsAffichePremiereLigne())
            {
                ChiffreAPremiereLigneTextBlock.Text =
                    ViewModel.LigneChiffre[0].ToString();
                ChiffreBPremiereLigneTextBlock.Text =
                    ViewModel.LigneChiffre[1].ToString();
                ChiffreCPremiereLigneTextBlock.Text =
                    ViewModel.LigneChiffre[2].ToString();
                ChiffreDPremiereLigneTextBlock.Text =
                    ViewModel.LigneChiffre[3].ToString();

                SigneAPremiereLigneTextBlock.Text = ViewModel.GetSigne(1, 0);
                SigneBPremiereLigneTextBlock.Text = ViewModel.GetSigne(1, 1);
                SigneCPremiereLigneTextBlock.Text = ViewModel.GetSigne(1, 2);
            }
            else
            {
                ChiffreADeuxiemeLigneTextBlock.Text =
                    ViewModel.LigneChiffre[0].ToString();
                ChiffreBDeuxiemeLigneTextBlock.Text =
                    ViewModel.LigneChiffre[1].ToString();
                ChiffreCDeuxiemeLigneTextBlock.Text =
                    ViewModel.LigneChiffre[2].ToString();
            }

            SigneADeuxiemeLigneTextBlock.Text = ViewModel.GetSigne(2, 0);
            SigneBDeuxiemeLigneTextBlock.Text = ViewModel.GetSigne(2, 1);
            SigneTroisiemeLigneTextBlock.Text = ViewModel.GetSigne(3, 0);
        }

        private void Chiffre_Click(object sender, RoutedEventArgs e)
        {
            int outChiffre;
            int.TryParse(((Button)sender).Content.ToString(), out outChiffre);
            ResultText.Text += outChiffre.ToString();

        }

        private void EffacerButton_Click(object sender, RoutedEventArgs e)
        {
            ResultText.Text = "";
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ResultText.Text) && !_isBlock)
            {
                _isBlock = true;
                int resultat;
                int.TryParse(ResultText.Text, out resultat);
                ResImage.Source = ViewModel.IsResultatCorrect(resultat) ? new BitmapImage(new Uri(@"ms-appx:///Rsc/img/right.png")) : new BitmapImage(new Uri(@"ms-appx:///Rsc/img/wrong.png"));
                _timer.Start();
            }
            
        }

        private async void FinCompteAReboursChangement(object sender, object e)
        {
            _timer.Stop();
            ResImage.Source = null;
           
            if (_isScreenKey)
            {
                _isScreenKey = false;
                ChangeSize();
            }

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
            else
            {
                ResultText.Text = "";
                GenererPyramide();
            }
            _isBlock = false;
        }

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
                GridJeu.RowDefinitions[2].Height = new GridLength(200, GridUnitType.Pixel);
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
