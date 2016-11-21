using System;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using JeuxDeLogiqueWin10.Context;
using JeuxDeLogiqueWin10.Enum;
using JeuxDeLogiqueWin10.Interface;
using JeuxDeLogiqueWin10.Model;
using JeuxDeLogiqueWin10.ViewModel.Games.Game;

namespace JeuxDeLogiqueWin10.Views.Games.Game
{
    /// <summary>
    /// Code behind de la vue du jeu de mémoire des cartes
    /// </summary>
    public sealed partial class MemoireCarteView : IViewGame<MemoireCarteViewModel>
    {
        /// <summary>
        /// ViewModel
        /// </summary>
        public MemoireCarteViewModel ViewModel { get; set; }

        private TextBlock _textBlockOuvert;
        private Rectangle _rectangleOuvert;
        private bool _verrouiller;

        /// <summary>
        /// Constructeur
        /// </summary>
        public MemoireCarteView()
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
            ViewModel = new MemoireCarteViewModel(e.Parameter as Exercice);
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


        //fin du compte à rebours et lancement du jeu
        private void OnCompteAReboursEnd()
        {
            CompteAReboursGrid.Visibility = Visibility.Collapsed;
            GridJeu.Visibility = Visibility.Visible;
            ViewModel.StartGame();
            GenererGrid();
        }

        private void LaunchTutoButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(TutorielView), ViewModel.ExerciceEnCours);
        }

        /// <summary>
        /// Genere une grille de l'aplhabet
        /// </summary>
        private void GenererGrid()
        {
            var difficulte = ViewModel.GetDifficulte();
            if (difficulte.Equals(DifficulteEnum.FACILE))
            {
                for (var i = 0; i < 4; i++)
                {
                    GridJeu.ColumnDefinitions.Add(new ColumnDefinition());
                }

                for (var i = 0; i < 5; i++)
                {
                    GridJeu.RowDefinitions.Add(new RowDefinition());
                }
            }
            else if (difficulte.Equals(DifficulteEnum.MOYEN))
            {
                for (var i = 0; i < 4; i++)
                {
                    GridJeu.ColumnDefinitions.Add(new ColumnDefinition());
                }

                for (var i = 0; i < 10; i++)
                {
                    GridJeu.RowDefinitions.Add(new RowDefinition());
                }
            }
            else
            {
                for (var i = 0; i < 5; i++)
                {
                    GridJeu.ColumnDefinitions.Add(new ColumnDefinition());
                }

                for (var i = 0; i < 10; i++)
                {
                    GridJeu.RowDefinitions.Add(new RowDefinition());
                }
            }

            var colonne = 0;
            var ligne = 0;

            foreach (var lettre in ViewModel.ListeLettre)
            {
                var textblock = new TextBlock()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0),
                    TextWrapping = TextWrapping.Wrap,
                    Text = lettre.ToString(),
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 35,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)),
                    Visibility = Visibility.Collapsed,
                    Name = "TextBlock-"+colonne+":"+ligne,
                    Tag = lettre
                };

                var rectangle = new Rectangle
                {
                    Width = 50,
                    Height = 30,
                    Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)),
                    Fill = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)),
                    Margin = new Thickness(0),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Visibility = Visibility.Visible,
                    Tag = textblock
                };
                rectangle.Tapped += Rectangle_Tapped;

                Grid.SetRow(textblock,ligne);
                Grid.SetRow(rectangle,ligne);
                Grid.SetColumn(textblock,colonne);
                Grid.SetColumn(rectangle,colonne);

                GridJeu.Children.Add(rectangle);
                GridJeu.Children.Add(textblock);
                colonne++;
                if (colonne % GridJeu.ColumnDefinitions.Count == 0)
                {
                    colonne = 0;
                    ligne++;
                }
            }
        }

        private async void Rectangle_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (!_verrouiller)
            {
                var text = ((Rectangle)sender).Tag as TextBlock;
                text.Visibility = Visibility.Visible;
                ((Rectangle)sender).Visibility = Visibility.Collapsed;

                //si aucune case ouverte, on mémorise la lettre
                if (_textBlockOuvert == null)
                {
                    _textBlockOuvert = text;
                    _rectangleOuvert = sender as Rectangle;
                }
                else
                {
                    _verrouiller = true;
                    await Task.Delay(TimeSpan.FromMilliseconds(500));
                    //si les deux cartes sont correctes
                    if (ViewModel.IsCorrect(_textBlockOuvert.Text, text.Text) == 2)
                    {
                        ((TextBlock)_rectangleOuvert.Tag).Tag = null;
                        _rectangleOuvert.Tag = null;
                        ((TextBlock)((Rectangle)sender).Tag).Tag = null;
                        ((Rectangle)sender).Tag = null;


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
                    else
                    {
                        ((Rectangle)sender).Visibility = Visibility.Visible;

                    }

                    _rectangleOuvert = null;
                    _textBlockOuvert = null;

                    foreach (var element in GridJeu.Children)
                    {
                        if (element is Rectangle && (((Rectangle)element).Tag != null))
                        {
                            element.Visibility = Visibility.Visible;
                            ((TextBlock)((Rectangle)element).Tag).Visibility = Visibility.Collapsed;
                        }
                    }
                    _verrouiller = false;
                } 
            }
            
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(MenuView));
        }
    }
}
