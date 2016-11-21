using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
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
    /// Code behind de la vue du jeu du compteur de chiffre
    /// </summary>
    public sealed partial class CompteChiffreView : IViewGame<CompteChiffreViewModel>
    {
        /// <summary>
        /// ViewModel
        /// </summary>
        public CompteChiffreViewModel ViewModel { get; set; }

        private bool _isStarting;
        private bool _isScreenKey;

        /// <summary>
        /// Constructeur
        /// </summary>
        public CompteChiffreView()
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
            ViewModel = new CompteChiffreViewModel(e.Parameter as Exercice);
            _isStarting = true;

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
            ChangeSize();
            GridJeu.Visibility = Visibility.Visible;
            ViewModel.StartGame();
        }

        private void LaunchTutoButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(TutorielView), ViewModel.ExerciceEnCours);
        }
        

        /// <summary>
        /// Genere une liste de chiffre et l'affiche dans la grid du jeu
        /// </summary>
        private void GenererGridChiffre()
        {
            Resources.Clear();
            EcranGrid.Children.Clear();
            IEnumerable<Chiffre> liste = new List<Chiffre>();
            var increment = 1;
            var erreur = false;
            do
            {
                //try catch au cas ou mais en principe il ne sert plus
                try
                {
                    liste = ViewModel.GenererListeChiffre((int)EcranGrid.ActualWidth, (int)EcranGrid.ActualHeight - 50);
                    erreur = false;
                }
                catch (Exception)
                {
                    increment++;
                    erreur = true;
                }
            } while (erreur && increment != 3);
            

            var i = 0;
            foreach (var chiffre in liste)
            {
                i++;
                var text = new TextBlock
                {
                    Text = chiffre.Nombre.ToString(),
                    Foreground = new SolidColorBrush(chiffre.Couleur),
                    FontSize = 30,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(chiffre.MarginLeft, chiffre.MarginTop, 0, 0),
                    TextWrapping = TextWrapping.Wrap,
                    RenderTransform = new CompositeTransform()
                };

                if (!chiffre.Mouvement.Equals(MouvementEnum.Aucun))
                {
                    if (chiffre.Mouvement.Equals(MouvementEnum.Rotation))
                    {
                        var storyboard = new Storyboard
                        {
                            Duration = new Duration(TimeSpan.FromSeconds(1)),
                            RepeatBehavior = RepeatBehavior.Forever
                        };

                        var rotateAnimation = new DoubleAnimation()
                        {
                            From = 0,
                            To = 360,
                            Duration = storyboard.Duration
                        };

                        Storyboard.SetTarget(rotateAnimation, text);
                        Storyboard.SetTargetProperty(rotateAnimation, "(UIElement.RenderTransform).(CompositeTransform.Rotation)");
                        storyboard.Children.Add(rotateAnimation);
                        Resources.Add("Rotate"+i, storyboard);
                        storyboard.Begin();
                    }

                    if (chiffre.Mouvement.Equals(MouvementEnum.TranslationHorizontale))
                    {
                        var storyboard = new Storyboard
                        {
                            RepeatBehavior = RepeatBehavior.Forever
                        };

                        var translateAnimation = new DoubleAnimation()
                        {
                            From = 0,
                            To = 115,
                            Duration = storyboard.Duration
                        };

                        Storyboard.SetTarget(translateAnimation, text);
                        Storyboard.SetTargetProperty(translateAnimation, "(UIElement.RenderTransform).(CompositeTransform.TranslateX)");
                        storyboard.Children.Add(translateAnimation);
                        translateAnimation.AutoReverse = true;
                        Resources.Add("TranslateX" + i, storyboard);
                        storyboard.Begin();
                    }

                    if (chiffre.Mouvement.Equals(MouvementEnum.TranslationVerticale))
                    {
                        var storyboard = new Storyboard
                        {
                            RepeatBehavior = RepeatBehavior.Forever
                        };

                        var translateAnimation = new DoubleAnimation()
                        {
                            From = 0,
                            To = 115,
                            Duration = storyboard.Duration
                        };

                        Storyboard.SetTarget(translateAnimation, text);
                        Storyboard.SetTargetProperty(translateAnimation, "(UIElement.RenderTransform).(CompositeTransform.TranslateY)");
                        storyboard.Children.Add(translateAnimation);
                        translateAnimation.AutoReverse = true;
                        Resources.Add("TranslateX" + i, storyboard);
                        storyboard.Begin();
                    }
                    if (chiffre.Mouvement.Equals(MouvementEnum.Agrandisssement))
                    {
                        var storyboard = new Storyboard
                        {
                            Duration = new Duration(TimeSpan.FromSeconds(1)),
                            RepeatBehavior = RepeatBehavior.Forever
                        };

                        var scaleX = new DoubleAnimation()
                        {
                            From = 1,
                            To = 2,
                            Duration = storyboard.Duration,
                            AutoReverse = true
                        };
                        Storyboard.SetTarget(scaleX, text);
                        Storyboard.SetTargetProperty(scaleX, "(UIElement.RenderTransform).(CompositeTransform.ScaleX)");
                        storyboard.Children.Add(scaleX);

                        var scaleY = new DoubleAnimation()
                        {
                            From = 1,
                            To = 2,
                            Duration = storyboard.Duration,
                            AutoReverse = true
                        };
                        Storyboard.SetTarget(scaleY, text);
                        Storyboard.SetTargetProperty(scaleY, "(UIElement.RenderTransform).(CompositeTransform.ScaleY)");
                        storyboard.Children.Add(scaleY);

                        Resources.Add("Scale" + i, storyboard);
                        storyboard.Begin();
                    }
                }

                EcranGrid.Children.Add(text);
                QuestionTextBlock.Text = ViewModel.Question;
            }
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
            int result;

            if (!String.IsNullOrWhiteSpace(ResultTextBox.Text))
            {
                if (int.TryParse(ResultTextBox.Text, out result))
                {
                    if (ViewModel.IsReponseCorrecte(result))
                    {
                        ImageValid.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/right.png"));

                    }
                    else
                    {
                        ImageValid.Source = new BitmapImage(new Uri(@"ms-appx:///Rsc/img/wrong.png"));
                    }
                    await Task.Delay(TimeSpan.FromMilliseconds(200));
                }

                if (_isScreenKey)
                {
                    _isScreenKey = false;
                    ChangeSize();
                }


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
                            ((Frame) Window.Current.Content).Navigate(exercice.Page, exercice);
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
                    ResultTextBox.Text = "";
                    ImageValid.Source = null;
                    GenererGridChiffre();
                }
            }
        }

        private void GotoOtherScreen_Click(object sender, RoutedEventArgs e)
        {
            _isScreenKey = !_isScreenKey;
            ChangeSize();
        }

        private void GridJeu_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChangeSize();
            if (_isStarting)
            {
                GenererGridChiffre();
                _isStarting = false;
            }
        }

        private void ChangeSize()
        {
            if (ActualHeight < 500)
            {
                GoToScreen.Visibility = Visibility.Visible;
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
                GoToScreen.Visibility = Visibility.Collapsed;
                GridJeu.RowDefinitions[2].Height = new GridLength(200, GridUnitType.Pixel);
                GridJeu.RowDefinitions[1].Height = new GridLength(50, GridUnitType.Pixel);
                GridJeu.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(MenuView));
        }
    }
}
