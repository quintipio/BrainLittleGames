using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
    /// Code Behind de la vue pour le jeu de la mémoire des chiffres
    /// </summary>
    public sealed partial class MemoireChiffreView : IViewGame<MemoireChiffreViewModel>
    {
        /// <summary>
        /// ViewModel
        /// </summary>
        public MemoireChiffreViewModel ViewModel { get; set; }

        private readonly DispatcherTimer _timer;
        private DispatcherTimer _timerAffichage;
        private int _compteRebours;
        private int _niveauEnCours;

        /// <summary>
        /// Constructeur
        /// </summary>
        public MemoireChiffreView()
        {
            InitializeComponent();
            
            _timer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(1000)};
            _timer.Tick +=_timer_Tick;
            Lanceur.OnCompteReboursEnd += OnCompteAReboursEnd;
            NavigationCacheMode = NavigationCacheMode.Disabled;
        }
        
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                 AppViewBackButtonVisibility.Collapsed;
            ViewModel = new MemoireChiffreViewModel(e.Parameter as Exercice);

            LaunchTutoButton.Visibility =
                (ContextAppli.ContextUtilisateur.ModeJeu.Equals(ModeOuvertureJeuEnum.ModeEval))
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            CompteAReboursGrid.Visibility = Visibility.Visible;
            GridJeu.Visibility = Visibility.Collapsed;
            ScoreGrid.Visibility = Visibility.Collapsed;
            _compteRebours = 3;

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
            LancementCaR();
        }


        #region Affichage d'une série
        /// <summary>
        /// Lance le compte à rebours avant de lancer un nouveau mémo
        /// </summary>
        private void LancementCaR()
        {
            ReinitVisible();
            _compteRebours = 3;
            _timer.Start();
        }

        /// <summary>
        /// Tick du compte à rebours avant de lancer une série
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _timer_Tick(object sender, object e)
        {
            _compteRebours--;
            TextCaR.Text = _compteRebours.ToString();

            if (_compteRebours <= 0)
            {
                _timer.Stop();
                TextCaR.Text = "";
                GenererSerie();
            }
        }

        /// <summary>
        /// Génère une série à mémoriser et l'affiche pendant un temps déterminer par la diffculté
        /// </summary>
        private void GenererSerie()
        {
            _niveauEnCours = ViewModel.GetNiveau();
            var suite = ViewModel.GenererSuite();

            switch (suite.Count)
            {
                case 4 :
                    Text00.Text = suite[0].ToString();
                    Text02.Text = suite[1].ToString();
                    Text20.Text = suite[2].ToString();
                    Text22.Text = suite[3].ToString();

                    Rect00.Tag = suite[0];
                    Rect02.Tag = suite[1];
                    Rect20.Tag = suite[2];
                    Rect22.Tag = suite[3];
                    break;
                
                case 5:
                    Text00.Text = suite[0].ToString();
                    Text02.Text = suite[1].ToString();
                    Text20.Text = suite[2].ToString();
                    Text22.Text = suite[3].ToString();
                    Text11.Text = suite[4].ToString();

                    Rect00.Tag = suite[0];
                    Rect02.Tag = suite[1];
                    Rect20.Tag = suite[2];
                    Rect22.Tag = suite[3];
                    Rect11.Tag = suite[4];
                    break;

                case 6:
                    Text20.Text = suite[0].ToString();
                    Text21.Text = suite[1].ToString();
                    Text11.Text = suite[2].ToString();
                    Text22.Text = suite[3].ToString();
                    Text12.Text = suite[4].ToString();
                    Text02.Text = suite[5].ToString();

                    Rect20.Tag = suite[0];
                    Rect21.Tag = suite[1];
                    Rect11.Tag = suite[2];
                    Rect22.Tag = suite[3];
                    Rect12.Tag = suite[4];
                    Rect02.Tag = suite[5];
                    break;

                case 7:
                    Text20.Text = suite[0].ToString();
                    Text21.Text = suite[1].ToString();
                    Text11.Text = suite[2].ToString();
                    Text01.Text = suite[3].ToString();
                    Text12.Text = suite[4].ToString();
                    Text02.Text = suite[5].ToString();
                    Text10.Text = suite[6].ToString();

                    Rect20.Tag = suite[0];
                    Rect21.Tag = suite[1];
                    Rect11.Tag = suite[2];
                    Rect01.Tag = suite[3];
                    Rect12.Tag = suite[4];
                    Rect02.Tag = suite[5];
                    Rect10.Tag = suite[6];
                    break;

                case 8:
                    Text00.Text = suite[0].ToString();
                    Text10.Text = suite[1].ToString();
                    Text20.Text = suite[2].ToString();
                    Text30.Text = suite[3].ToString();
                    Text01.Text = suite[4].ToString();
                    Text11.Text = suite[5].ToString();
                    Text21.Text = suite[6].ToString();
                    Text31.Text = suite[7].ToString();

                    Rect00.Tag = suite[0];
                    Rect10.Tag = suite[1];
                    Rect20.Tag = suite[2];
                    Rect30.Tag = suite[3];
                    Rect01.Tag = suite[4];
                    Rect11.Tag = suite[5];
                    Rect21.Tag = suite[6];
                    Rect31.Tag = suite[7];
                    break;

                case 9:
                    Text00.Text = suite[0].ToString();
                    Text10.Text = suite[1].ToString();
                    Text20.Text = suite[2].ToString();
                    Text01.Text = suite[3].ToString();
                    Text11.Text = suite[4].ToString();
                    Text21.Text = suite[5].ToString();
                    Text02.Text = suite[6].ToString();
                    Text12.Text = suite[7].ToString();
                    Text22.Text = suite[8].ToString();

                    Rect00.Tag = suite[0];
                    Rect10.Tag = suite[1];
                    Rect20.Tag = suite[2];
                    Rect01.Tag = suite[3];
                    Rect11.Tag = suite[4];
                    Rect21.Tag = suite[5];
                    Rect02.Tag = suite[6];
                    Rect12.Tag = suite[7];
                    Rect22.Tag = suite[8];
                    break;
            }
            AfficherNiveau(Visibility.Visible, _niveauEnCours);

            _timerAffichage = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(ViewModel.TempsApparition) };
            _timerAffichage.Tick += _timerAffichage_Tick; 
            _timerAffichage.Start();
        }

        private void _timerAffichage_Tick(object sender, object e)
        {
            AfficherNiveau(Visibility.Collapsed, _niveauEnCours);
            _timerAffichage.Stop();
        }

        /// <summary>
        /// Réinitialise l'affichage
        /// </summary>
        private void ReinitVisible()
        {
            Text00.Visibility = Visibility.Collapsed;
            Text10.Visibility = Visibility.Collapsed;
            Text20.Visibility = Visibility.Collapsed;
            Text30.Visibility = Visibility.Collapsed;
            Text01.Visibility = Visibility.Collapsed;
            Text11.Visibility = Visibility.Collapsed;
            Text21.Visibility = Visibility.Collapsed;
            Text31.Visibility = Visibility.Collapsed;
            Text02.Visibility = Visibility.Collapsed;
            Text12.Visibility = Visibility.Collapsed;
            Text22.Visibility = Visibility.Collapsed;

            Rect00.Visibility = Visibility.Collapsed;
            Rect10.Visibility = Visibility.Collapsed;
            Rect20.Visibility = Visibility.Collapsed;
            Rect30.Visibility = Visibility.Collapsed;
            Rect01.Visibility = Visibility.Collapsed;
            Rect11.Visibility = Visibility.Collapsed;
            Rect21.Visibility = Visibility.Collapsed;
            Rect31.Visibility = Visibility.Collapsed;
            Rect02.Visibility = Visibility.Collapsed;
            Rect12.Visibility = Visibility.Collapsed;
            Rect22.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Affichage d'un niveau
        /// </summary>
        /// <param name="visible">l'état de visibilité des caractères (et l'inverse pour les rectangle)</param>
        /// <param name="niveau">le niveau à afficher</param>
        private void AfficherNiveau(Visibility visible, int niveau)
        {
            var visibleReverse = (visible.Equals(Visibility.Visible)) ? Visibility.Collapsed : Visibility.Visible;
            ReinitVisible();
            switch (niveau)
            {
                case 1:
                    Text00.Visibility = visible;
                    Text02.Visibility = visible;
                    Text20.Visibility = visible;
                    Text22.Visibility = visible;

                    Rect00.Visibility = visibleReverse;
                    Rect02.Visibility = visibleReverse;
                    Rect20.Visibility = visibleReverse;
                    Rect22.Visibility = visibleReverse;
                    break;

                case 2:
                    Text00.Visibility = visible;
                    Text02.Visibility = visible;
                    Text20.Visibility = visible;
                    Text22.Visibility = visible;
                    Text11.Visibility = visible;

                    Rect00.Visibility = visibleReverse;
                    Rect02.Visibility = visibleReverse;
                    Rect20.Visibility = visibleReverse;
                    Rect22.Visibility = visibleReverse;
                    Rect11.Visibility = visibleReverse;
                    break;

                case 3:
                    Text20.Visibility = visible;
                    Text21.Visibility = visible;
                    Text11.Visibility = visible;
                    Text22.Visibility = visible;
                    Text12.Visibility = visible;
                    Text02.Visibility = visible;

                    Rect20.Visibility = visibleReverse;
                    Rect21.Visibility = visibleReverse;
                    Rect11.Visibility = visibleReverse;
                    Rect22.Visibility = visibleReverse;
                    Rect12.Visibility = visibleReverse;
                    Rect02.Visibility = visibleReverse;
                    break;

                case 4:
                    Text20.Visibility = visible;
                    Text21.Visibility = visible;
                    Text11.Visibility = visible;
                    Text01.Visibility = visible;
                    Text12.Visibility = visible;
                    Text02.Visibility = visible;
                    Text10.Visibility = visible;

                    Rect20.Visibility = visibleReverse;
                    Rect21.Visibility = visibleReverse;
                    Rect11.Visibility = visibleReverse;
                    Rect01.Visibility = visibleReverse;
                    Rect12.Visibility = visibleReverse;
                    Rect02.Visibility = visibleReverse;
                    Rect10.Visibility = visibleReverse;
                    break;

                case 5:
                    Text00.Visibility = visible;
                    Text10.Visibility = visible;
                    Text20.Visibility = visible;
                    Text30.Visibility = visible;
                    Text01.Visibility = visible;
                    Text11.Visibility = visible;
                    Text21.Visibility = visible;
                    Text31.Visibility = visible;

                    Rect00.Visibility = visibleReverse;
                    Rect10.Visibility = visibleReverse;
                    Rect20.Visibility = visibleReverse;
                    Rect30.Visibility = visibleReverse;
                    Rect01.Visibility = visibleReverse;
                    Rect11.Visibility = visibleReverse;
                    Rect21.Visibility = visibleReverse;
                    Rect31.Visibility = visibleReverse;
                    break;

                case 6:
                    Text00.Visibility = visible;
                    Text10.Visibility = visible;
                    Text20.Visibility = visible;
                    Text01.Visibility = visible;
                    Text11.Visibility = visible;
                    Text21.Visibility = visible;
                    Text02.Visibility = visible;
                    Text12.Visibility = visible;
                    Text22.Visibility = visible;

                    Rect00.Visibility = visibleReverse;
                    Rect10.Visibility = visibleReverse;
                    Rect20.Visibility = visibleReverse;
                    Rect01.Visibility = visibleReverse;
                    Rect11.Visibility = visibleReverse;
                    Rect21.Visibility = visibleReverse;
                    Rect02.Visibility = visibleReverse;
                    Rect12.Visibility = visibleReverse;
                    Rect22.Visibility = visibleReverse;
                    break;
            }
        }
        #endregion


        #region Résultat
        private async void Rect_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
                var resultat = ViewModel.VerifResult(((int)((Rectangle) sender).Tag));

                if (resultat == 1)
                {
                    AfficherChiffre((((Rectangle) sender).Name));
                }
                else
                {
                    AfficherChiffre((((Rectangle)sender).Name));
                    if (!ViewModel.IsJeuFini())
                    {
                        LancementCaR();
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

        /// <summary>
        /// Affiche le chiffre caché dérrière un rectangle
        /// </summary>
        /// <param name="name">le nom du rectangle</param>
        private void AfficherChiffre(string name)
        {
            if (name.Equals("Rect00"))
            {
                Rect00.Visibility = Visibility.Collapsed;
                Text00.Visibility = Visibility.Visible;
            }
            else if (name.Equals("Rect01"))
            {
                Rect01.Visibility = Visibility.Collapsed;
                Text01.Visibility = Visibility.Visible;
            }
            else if (name.Equals("Rect02"))
            {
                Rect02.Visibility = Visibility.Collapsed;
                Text02.Visibility = Visibility.Visible;
            }
            else if (name.Equals("Rect10"))
            {
                Rect10.Visibility = Visibility.Collapsed;
                Text10.Visibility = Visibility.Visible;
            }
            else if (name.Equals("Rect11"))
            {
                Rect11.Visibility = Visibility.Collapsed;
                Text11.Visibility = Visibility.Visible;
            }
            else if (name.Equals("Rect12"))
            {
                Rect12.Visibility = Visibility.Collapsed;
                Text12.Visibility = Visibility.Visible;
            }
            else if (name.Equals("Rect20"))
            {
                Rect20.Visibility = Visibility.Collapsed;
                Text20.Visibility = Visibility.Visible;
            }
            else if (name.Equals("Rect21"))
            {
                Rect21.Visibility = Visibility.Collapsed;
                Text21.Visibility = Visibility.Visible;
            }
            else if (name.Equals("Rect22"))
            {
                Rect22.Visibility = Visibility.Collapsed;
                Text22.Visibility = Visibility.Visible;
            }
            else if (name.Equals("Rect30"))
            {
                Rect30.Visibility = Visibility.Collapsed;
                Text30.Visibility = Visibility.Visible;
            }
            else if (name.Equals("Rect31"))
            {
                Rect31.Visibility = Visibility.Collapsed;
                Text31.Visibility = Visibility.Visible;
            }
        }

        #endregion

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(MenuView));
        }
    }
}
