using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
    /// Code behind de la vue du jeu des différences entres les heures
    /// </summary>
    public sealed partial class JeuHoraireView : IViewGame<JeuHoraireViewModel>
    {
        /// <summary>
        /// ViewModel
        /// </summary>
        public JeuHoraireViewModel ViewModel { get; set; }

        private int _position;
        private bool _verrouiller;
        private bool _isScreenKey;

        /// <summary>
        /// Constructeur
        /// </summary>
        public JeuHoraireView()
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
            _verrouiller = false;
            ViewModel = new JeuHoraireViewModel(e.Parameter as Exercice);


            LaunchTutoButton.Visibility =
                (ContextAppli.ContextUtilisateur.ModeJeu.Equals(ModeOuvertureJeuEnum.ModeEval))
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            CompteAReboursGrid.Visibility = Visibility.Visible;
            GridJeu.Visibility = Visibility.Collapsed;
            ScoreGrid.Visibility = Visibility.Collapsed;
            _isScreenKey = false;

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
            ChargerHoraire();
        }

        private void ChargerHoraire()
        {
            ReinitText();
            ViewModel.GenererHeure();

        }

        private async void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isScreenKey)
            {
                _isScreenKey = false;
                ChangeSize();
            }

            if (ViewModel.ControlResult(DifHeureTextBox.Text,DifMinuteTextBox.Text))
            {
                //vérification si le jeu est fini
                if (ViewModel.IsJeuFini() && !_verrouiller)
                {
                    var res = await ViewModel.CalculResult();
                    if (ContextAppli.ContextUtilisateur.ModeJeu.Equals(ModeOuvertureJeuEnum.ModeJeu))
                    {
                        _verrouiller = true;
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
                    ChargerHoraire();
                }
            }
            else
            {
                DifHeureTextBox.Foreground = new SolidColorBrush(Color.FromArgb(255,255,0,0));
                DifMinuteTextBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                _position = 0;
            }
            
        }

        private void Chiffre_Click(object sender, RoutedEventArgs e)
        {
            int outChiffre;
            int.TryParse(((Button) sender).Content.ToString(), out outChiffre);

            switch (_position)
            {
                case 0:
                    DifHeureTextBox.Text = outChiffre.ToString();
                    break;
                case 1:
                    DifHeureTextBox.Text = DifHeureTextBox.Text[0].ToString() + outChiffre;
                    break;
                case 2:
                    DifMinuteTextBox.Text = outChiffre.ToString();
                    break;
                case 3:
                    DifMinuteTextBox.Text = DifMinuteTextBox.Text[0].ToString() + outChiffre;
                    break;
            }

            _position++;
        }

        private void EffacerButton_Click(object sender, RoutedEventArgs e)
        {
            ReinitText();
        }

        private void ReinitText()
        {
            _position = 0;
            DifHeureTextBox.Text = "";
            DifMinuteTextBox.Text = "";

            DifHeureTextBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            DifMinuteTextBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
        }

        private void DifHeureTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _position = 0;
        }

        private void DifMinuteTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _position = 2;
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
