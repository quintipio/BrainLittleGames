using System;
using System.Threading.Tasks;
using Windows.System.Display;
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
    /// Code behind de la vue pour le jeu de comptage des entrées sorties
    /// </summary>
    public sealed partial class ComptePersonneView : IViewGame<ComptePersonneViewModel>
    {
        /// <summary>
        /// ViewModel
        /// </summary>
        public ComptePersonneViewModel ViewModel { get; set; }

        private DisplayRequest _display;
        private bool _isStarting;
        private bool _isAnimation;
        private bool _isBlock;

        /// <summary>
        /// Constructeur
        /// </summary>
        public ComptePersonneView()
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
            _display = new DisplayRequest();
            ViewModel = new ComptePersonneViewModel(e.Parameter as Exercice);
            _isStarting = true;
            _isAnimation = false;
            _isBlock = false;


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

        //fin du compte à rebours et lancement du jeu
        private void OnCompteAReboursEnd()
        {
            CompteAReboursGrid.Visibility = Visibility.Collapsed;
            GridJeu.Visibility = Visibility.Visible;
            ViewModel.StartGame();
            
        }

        private void LaunchTutoButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(TutorielView), ViewModel.ExerciceEnCours);
        }

        /// <summary>
        /// Démarre l'animation pour faire les va et vient
        /// </summary>
        private async Task DemarreComptage()
        {
            _isAnimation = true;
            CheckPlaceEcran();
            _display.RequestActive();
            BoxImage.Visibility = Visibility.Collapsed;
            OkButton.IsEnabled = false;
            switch (ViewModel.GenererPointDepart())
            {
                case 1:
                    CatRes.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/catRes1.png"));
                    break;

                case 2:
                    CatRes.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/catRes2.png"));
                    break;

                case 3:
                    CatRes.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/catRes3.png"));
                    break;

                case 4:
                    CatRes.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/catRes4.png"));
                    break;

                case 5:
                    CatRes.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/catRes5.png"));
                    break;

                case 6:
                    CatRes.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/catRes6.png"));
                    break;

                case 7:
                    CatRes.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/catRes7.png"));
                    break;

                case 8:
                    CatRes.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/catRes8.png"));
                    break;

                case 9:
                    CatRes.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/catRes9.png"));
                    break;

                default:
                    CatRes.Source = null;
                    break;
            }
            await Task.Delay(TimeSpan.FromMilliseconds(500));

            ViewModel.GenererJeu();
            BoxImage.Visibility = Visibility.Visible;
            DescBoite.Begin();
            await Task.Delay(1000);
            CatRes.Source = null;

            do
            {
                var mouv = ViewModel.GetMouvement();
                switch (mouv.InLeft)
                {
                    case 1:
                        CatLeftImage.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/cat1.png"));
                         break;

                    case 2:
                        CatLeftImage.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/cat2.png"));
                        break;

                    case 3:
                        CatLeftImage.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/cat3.png"));
                        break;

                    case 4:
                        CatLeftImage.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/cat4.png"));
                        break;

                    case 5:
                        CatLeftImage.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/cat5.png"));
                        break;

                    case 6:
                        CatLeftImage.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/cat6.png"));
                        break;

                    case 7:
                        CatLeftImage.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/cat7.png"));
                        break;

                    default:
                        CatLeftImage.Source = null;
                        break;
                }

                switch (mouv.InUp)
                {
                    case 1:
                        CatUpImage.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/cat1.png"));
                        break;

                    case 2:
                        CatUpImage.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/cat2.png"));
                        break;

                    case 3:
                        CatUpImage.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/cat3.png"));
                        break;

                    case 4:
                        CatUpImage.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/cat4.png"));
                        break;

                    case 5:
                        CatUpImage.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/cat5.png"));
                        break;

                    case 6:
                        CatUpImage.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/cat6.png"));
                        break;

                    case 7:
                        CatUpImage.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/cat7.png"));
                        break;

                    default:
                        CatUpImage.Source = null;
                        break;
                }

                if (mouv.InLeft != 0 || mouv.InUp != 0)
                {
                    AddCatLeft.Begin();
                    AddCatUp.Begin();

                    RectangleRight.Visibility = Visibility.Visible;
                    RectangleBottom.Visibility = Visibility.Visible;

                    await Task.Delay(ViewModel.DurationAnim.TimeSpan);

                    RectangleRight.Visibility = Visibility.Collapsed;
                    RectangleBottom.Visibility = Visibility.Collapsed;
                }
                
                
                if (ViewModel.GetDifficulteEnum().Equals(DifficulteEnum.FACILE))
                {
                    await Task.Delay(300);
                }
                else if (ViewModel.GetDifficulteEnum().Equals(DifficulteEnum.MOYEN))
                {
                    await Task.Delay(100);
                }

                switch (mouv.OutRight)
                {
                    case 1:
                        CatRightImage.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/cat1.png"));
                        break;

                    case 2:
                        CatRightImage.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/cat2.png"));
                        break;

                    case 3:
                        CatRightImage.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/cat3.png"));
                        break;

                    case 4:
                        CatRightImage.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/cat4.png"));
                        break;

                    case 5:
                        CatRightImage.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/cat5.png"));
                        break;

                    case 6:
                        CatRightImage.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/cat6.png"));
                        break;

                    case 7:
                        CatRightImage.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/cat7.png"));
                        break;

                    default:
                        CatRightImage.Source = null;
                        break;
                }

                if (mouv.OutRight != 0)
                {
                    RemoveCat.Begin();
                    RectangleLeft.Visibility = Visibility.Visible;
                    await Task.Delay(ViewModel.DurationAnim.TimeSpan);
                    RectangleLeft.Visibility = Visibility.Collapsed;
                }
                await Task.Delay(400);
                

            } while (!ViewModel.IsVaEtVientFini());
            OkButton.IsEnabled = true;
            _display.RequestRelease();
            _isAnimation = false;
            CheckPlaceEcran();
        }

        /// <summary>
        /// Souleve le rectangle et affiche le comptage 
        /// </summary>
        private async Task FinComptage()
        {
            _isAnimation = true;
            CheckPlaceEcran();
            switch (ViewModel.Resultat)
            {
                case 1:
                    CatRes.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/catRes1.png"));
                    break;

                case 2:
                    CatRes.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/catRes2.png"));
                    break;

                case 3:
                    CatRes.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/catRes3.png"));
                    break;

                case 4:
                    CatRes.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/catRes4.png"));
                    break;

                case 5:
                    CatRes.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/catRes5.png"));
                    break;

                case 6:
                    CatRes.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/catRes6.png"));
                    break;

                case 7:
                    CatRes.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/catRes7.png"));
                    break;

                case 8:
                    CatRes.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/catRes8.png"));
                    break;

                case 9:
                    CatRes.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/catRes9.png"));
                    break;

                default:
                    CatRes.Source = null;
                    break;
            }
            MonteBoite.Begin();
            await Task.Delay(1000);
            ResTextBlock.Text = ViewModel.Resultat.ToString();
            int result;

            if (int.TryParse(ResultTextBox.Text, out result))
            {
                ImageValid.Source = ViewModel.IsReponseCorrecte(result) ? new BitmapImage(new Uri(@"ms-appx:///Rsc/img/right.png")) : new BitmapImage(new Uri(@"ms-appx:///Rsc/img/wrong.png"));
            }
            await Task.Delay(1000);

            _isAnimation = false;
            CheckPlaceEcran();

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
                ImageValid.Source = null;
                ResTextBlock.Text = "";
                ResultTextBox.Text = "";
                CatRes.Source = null;
                await DemarreComptage();
            }
            _isBlock = false;
        }

        private void Chiffre_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBox.Text += ((Button)sender).Content;
        }

        private void Effacer_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBox.Text = "";
        }

        private async void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(ResultTextBox.Text) && !_isBlock)
            {
                _isBlock = true;
                await FinComptage();
            }
        }

        private async void GridJeu_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //le premier jeu démarre ici pour obtenir la taille des éléments nécéssaire à la translation des chats et de la boite
            RectangleRight.Width = (int)(GridJeu.ActualWidth/2) + 50;
            RectangleLeft.Width = (int)(GridJeu.ActualWidth/2) + 50;
            ViewModel.ValueBoxUp = (int)GridJeu.ActualHeight * -1;
            ViewModel.ValueCatLeft = ((int) ((GridJeu.ActualWidth/2) + 50) + 700)*-1;
            ViewModel.ValueCatRight = ((int)((GridJeu.ActualWidth / 2) + 50) + 700);
            ViewModel.ValueCatUp = ((int)((GridJeu.ActualHeight / 2) + 50) + 700) * -1;
            if (_isStarting)
            {
                _isStarting = false;
                await DemarreComptage();
            }
            CheckPlaceEcran();
        }

        /// <summary>
        /// Place les élements sur l'écran en fonction de la taille
        /// </summary>
        private void CheckPlaceEcran()
        {
            if (ActualHeight < 500)
            {
                if (_isAnimation)
                {
                    GridJeu.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
                    GridJeu.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Pixel);
                }
                else
                {
                    GridJeu.RowDefinitions[0].Height = new GridLength(0, GridUnitType.Pixel);
                    GridJeu.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
                }
            }
            else
            {
                GridJeu.RowDefinitions[1].Height = new GridLength(275, GridUnitType.Pixel);
                GridJeu.RowDefinitions[0].Height = new GridLength(1,GridUnitType.Star);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(MenuView));
        }
    }
}
