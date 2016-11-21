using System;
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
    /// Code behind de la Vue pour le mini jeu des mots et des couleurs
    /// </summary>
    public sealed partial class JeuCouleursView : IViewGame<JeuCouleurViewModel>
    {
        /// <summary>
        /// ViewModel
        /// </summary>
        public JeuCouleurViewModel ViewModel { get; set; }


        private readonly Random _random;


        /// <summary>
        /// Constructeur
        /// </summary>
        public JeuCouleursView()
        {
            InitializeComponent();
            _random = new Random();

            Lanceur.OnCompteReboursEnd += OnCompteAReboursEnd;
            NavigationCacheMode = NavigationCacheMode.Disabled;
        }
        
        
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                 AppViewBackButtonVisibility.Collapsed;
            ViewModel = new JeuCouleurViewModel(e.Parameter as Exercice);


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
            //Mise en place des élements graphiques
            CompteAReboursGrid.Visibility = Visibility.Collapsed;
            GridJeu.Visibility = Visibility.Visible;

            //Démarrage du jeu
            ViewModel.StartGame();
            var listeCouleur = ViewModel.GetListeCouleurs();

            //Ajout des boutons de jeu
            CouleurButtonGrid.ColumnDefinitions.Add(new ColumnDefinition());
            CouleurButtonGrid.ColumnDefinitions.Add(new ColumnDefinition());
            var nbLigne = 0;
            for (var i = 0; i < listeCouleur.Count; i++)
            {
                if (i%2 == 0)
                {
                    CouleurButtonGrid.RowDefinitions.Add(new RowDefinition());
                    nbLigne++;
                }
                var button = new Button
                {
                    Content = listeCouleur[i].Nom,
                    BorderBrush = null,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Background = new SolidColorBrush(Color.FromArgb(255, 60, 59, 60)),
                    BorderThickness = new Thickness(0),
                    Margin = new Thickness(0, 0, 0, 0),
                    Tag = listeCouleur[i].Valeur
                };
                button.Click += SelectColor_Click;
                Grid.SetColumn(button,(i%2 == 0)?0:1);
                Grid.SetRow(button,nbLigne-1);
                 CouleurButtonGrid.Children.Add(button);
            }

            //Lancement
            GenererCouleur();
        }

        /// <summary>
        /// Genere une couleur et son mot et le place sur la zone de jeu
        /// </summary>
        private void GenererCouleur()
        {
            var couleur = ViewModel.GetCouleur();
            TextCouleur.FontSize = _random.Next(30, 60);
            TextCouleur.Text = couleur.Nom;
            TextCouleur.Foreground = couleur.Valeur;
            TextCouleur.Tag = couleur.Valeur;
            var marginHor = _random.Next(0, (int)(TextCouleurGrid.ActualWidth - TextCouleur.ActualWidth));
            if (marginHor > TextCouleurGrid.ActualWidth - 100)
            {
                marginHor =(int)(TextCouleurGrid.ActualWidth - marginHor);
            }
            var marginVer = _random.Next(0, (int)(TextCouleurGrid.ActualHeight - TextCouleur.ActualHeight));
            TextCouleur.Margin = new Thickness(marginHor, marginVer, 0, 0);

            
        }

        //lors de la sélection d'une couleur
        private async void SelectColor_Click(object sender, RoutedEventArgs e)
        {
            var color = ((Button) sender).Tag as SolidColorBrush;
            //si le résultat est bon
            if (ViewModel.VerifCouleur(color))
            {
                //et que le jeu n'est pas fini
                if (!ViewModel.IsJeuFini())
                {
                    //on change de couleur
                    GenererCouleur();
                }
                else
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

        private void LaunchTutoButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(TutorielView), ViewModel.ExerciceEnCours);
        }

        private void GridJeu_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GridJeu.RowDefinitions[1].Height = new GridLength(ActualHeight < 600 ? 1 : 250,
                ActualHeight < 600 ? GridUnitType.Auto : GridUnitType.Pixel);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(MenuView));
        }
    }
}
