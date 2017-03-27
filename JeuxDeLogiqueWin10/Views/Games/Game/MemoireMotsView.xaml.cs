using System;
using System.Collections.Generic;
using System.Linq;
using Windows.System;
using Windows.System.Display;
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
    /// Code Behind de la vue pour le mini jeu de la mémorisation des mots
    /// </summary>
    public sealed partial class MemoireMotsView : IViewGame<MemoireMotsViewModel>
    {
        /// <summary>
        /// ViewModel
        /// </summary>
        public MemoireMotsViewModel ViewModel { get; set; }

        private DisplayRequest _display;

        /// <summary>
        /// Constructeur
        /// </summary>
        public MemoireMotsView()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Disabled;
            Lanceur.OnCompteReboursEnd += OnCompteAReboursEnd;
        }
        

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                 AppViewBackButtonVisibility.Collapsed;
            ViewModel = new MemoireMotsViewModel(e.Parameter as Exercice);
            _display = new DisplayRequest();
            _display.RequestActive();


            ViewModel.OnFinLecture += OnCompteAReboursLectureEnd;
            ViewModel.OnFinEcriture += OnCompteAReboursEcritureEnd;

            LaunchTutoButton.Visibility =
                (ContextAppli.ContextUtilisateur.ModeJeu.Equals(ModeOuvertureJeuEnum.ModeEval))
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            CompteAReboursGrid.Visibility = Visibility.Visible;
            GridJeu.Visibility = Visibility.Collapsed;
            ScoreGrid.Visibility = Visibility.Collapsed;

            CreerTableau();
            MotTextBox.Visibility = Visibility.Collapsed;
            ValidButton.Visibility = Visibility.Collapsed;
            FinishButton.Visibility = Visibility.Collapsed;

            //si le tuto n'a pas déjà été vu, visionnage
            if (!await ViewModel.IsTutoPasse())
            {
                ((Frame)Window.Current.Content).Navigate(typeof(TutorielView), ViewModel.ExerciceEnCours);
            }

            //Lancement du compte à rebours du jeu
            Lanceur.StartCompteARebours();
        }
        

        #region Evenement Timer
        //fin du compte à rebours et lancement du jeu
        private async void OnCompteAReboursEnd()
        {
            CompteAReboursGrid.Visibility = Visibility.Collapsed;
            GridJeu.Visibility = Visibility.Visible;
            ViewModel.StartGame();
            await ViewModel.GenererMots();
            ChargerMots(ViewModel.MotsAleatoire);
        }

        //fin du compte à rebours pour la mémorisation des mots du joueur
        private void OnCompteAReboursLectureEnd()
        {
            ValidButton.Visibility = Visibility.Visible;
            MotTextBox.Visibility = Visibility.Visible;
            EraseMots();
            MotTextBox.Focus(FocusState.Keyboard);
        }

        //fin du compte à rebours pour la partie du temps d'écriture de smots du joueur
        private void OnCompteAReboursEcritureEnd()
        {
            var liste = ViewModel.GetMotsNonTrouve();
            foreach (var mot in liste)
            {
                AjouterMot(mot,true);
            }
            ValidButton.Visibility = Visibility.Collapsed;
            ReboursTextBlock.Visibility = Visibility.Collapsed;
            MotTextBox.Visibility = Visibility.Collapsed;
            FinishButton.Visibility = Visibility.Visible;
        }

        #endregion
        private void LaunchTutoButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(TutorielView), ViewModel.ExerciceEnCours);   
        }

        //vérification de la présence d'un mot et si ok, ajout
        private void ValidButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsMotInListe(MotTextBox.Text))
            {
                AjouterMot(MotTextBox.Text,false);
            }
            MotTextBox.Text = "";
            MotTextBox.Focus(FocusState.Keyboard);
        }

        /// <summary>
        /// méthode pour créer le tableau des mots (juste les cases et les textblock)
        /// </summary>
        private void CreerTableau()
        {
            ListeMotsMotGrid.ColumnDefinitions.Add(new ColumnDefinition());
            ListeMotsMotGrid.ColumnDefinitions.Add(new ColumnDefinition());
            ListeMotsMotGrid.ColumnDefinitions.Add(new ColumnDefinition());
            var nbMots = 0;
            if (ViewModel.GetDifficulte().Equals(DifficulteEnum.FACILE))
            {
                for (var i = 0; i < 5; i++)
                {
                    ListeMotsMotGrid.RowDefinitions.Add(new RowDefinition());
                }
                nbMots = 15;
            }
            else if (ViewModel.GetDifficulte().Equals(DifficulteEnum.MOYEN))
            {
                for (var i = 0; i < 7; i++)
                {
                    ListeMotsMotGrid.RowDefinitions.Add(new RowDefinition());
                }
                nbMots = 21;
            }
            else
            {
                for (var i = 0; i < 10; i++)
                {
                    ListeMotsMotGrid.RowDefinitions.Add(new RowDefinition());
                }
                nbMots = 30;
            }

            var column = 0;
            var line = 0;
            for (var i = 0; i < nbMots; i++)
            {
                var textBlock = new TextBlock()
                {
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)),
                    FontSize = 18,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Text = ""
                };

                Grid.SetColumn(textBlock, column);
                Grid.SetRow(textBlock,line);
                ListeMotsMotGrid.Children.Add(textBlock);
                column++;
                if (column%ListeMotsMotGrid.ColumnDefinitions.Count == 0)
                {
                    column = 0;
                    line++;
                }
            }
        }

        /// <summary>
        /// Charge une liste de 30 mots dans les textblock
        /// </summary>
        /// <param name="listeMots">la liste des mots à afficher</param>
        private void ChargerMots(IReadOnlyList<string> listeMots)
        {
            var i = 0;
            foreach (var text in ListeMotsMotGrid.Children.OfType<TextBlock>())
            {
                if (i < listeMots.Count)
                {
                    text.Text = listeMots[i];
                    i++;
                }
            }

        }

        /// <summary>
        /// Efface les mots affichés
        /// </summary>
        private void EraseMots()
        {
            foreach (var text in ListeMotsMotGrid.Children.OfType<TextBlock>())
            {
                text.Text = "";
            }
        }

        /// <summary>
        /// Ajoute un mot dans un des champs de text vide de la grid
        /// </summary>
        /// <param name="mot">le mot à ajouter</param>
        /// <param name="isRed">indique si le mot doit être surligner en rouge</param>
        private void AjouterMot(string mot,bool isRed)
        {
            foreach (var text in ListeMotsMotGrid.Children.OfType<TextBlock>().Where(text => String.IsNullOrWhiteSpace(text.Text)))
            {
                text.Text = mot;
                if (isRed)
                {
                    text.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                }
                break;
            }
        }

        private async void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            _display.RequestRelease();
            //vérification si le jeu est fini
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

        private void MotTextBox_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                if (ViewModel.IsMotInListe(MotTextBox.Text))
                {
                    AjouterMot(MotTextBox.Text, false);
                }
                MotTextBox.Text = "";
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(MenuView));
        }
    }
}
