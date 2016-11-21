using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
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
    /// Code behind de la vue du jeu des formes et des couleurs
    /// </summary>
    public sealed partial class TrouveObjetCouleurView : IViewGame<TrouveObjetCouleurViewModel>
    {
        /// <summary>
        /// ViewModel
        /// </summary>
        public TrouveObjetCouleurViewModel ViewModel { get; set; }

        private bool _isStarting;
        private bool _verrouiller;

        /// <summary>
        /// Constructeur
        /// </summary>
        public TrouveObjetCouleurView()
        {
            InitializeComponent();
            Lanceur.OnCompteReboursEnd += OnCompteAReboursEnd;
            NavigationCacheMode = NavigationCacheMode.Disabled;
        }
        
        
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _isStarting = true;
            _verrouiller = false;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                AppViewBackButtonVisibility.Collapsed;
            ViewModel = new TrouveObjetCouleurViewModel(e.Parameter as Exercice);


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
                ((Frame) Window.Current.Content).Navigate(typeof (TutorielView),
                    ViewModel.ExerciceEnCours);
            }
            //Lancement du compte à rebours du jeu
            Lanceur.StartCompteARebours();
        }
        

        private void LaunchTutoButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame) Window.Current.Content).Navigate(typeof (TutorielView),
                ViewModel.ExerciceEnCours);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame) Window.Current.Content).Navigate(typeof (MenuView));
        }

        //fin du compte à rebours et lancement du jeu
        private void OnCompteAReboursEnd()
        {
            //Mise en place des élements graphiques
            CompteAReboursGrid.Visibility = Visibility.Collapsed;
            GridJeu.Visibility = Visibility.Visible;

            //Démarrage du jeu
            ViewModel.StartGame();
        }

        /// <summary>
        /// Affiche les boutons
        /// </summary>
        private void GenerateButton()
        {
            var size = (GridOBjetButton.ActualHeight >= GridOBjetButton.ActualWidth/5)
                ? GridOBjetButton.ActualWidth/5
                : GridOBjetButton.ActualHeight;

            PolygonButtonA.Points = PolygonEnum.GeneratePolygon(ViewModel.ListeFormeDefaut[0].Id,size).GetPoints();
            PolygonButtonA.Stroke = new SolidColorBrush( ViewModel.ListeFormeDefaut[0].Couleur);
            PolygonButtonA.Fill = new SolidColorBrush(ViewModel.ListeFormeDefaut[0].Couleur);
            PolygonButtonA.Tag = ViewModel.ListeFormeDefaut[0];

            PolygonButtonB.Points = PolygonEnum.GeneratePolygon(ViewModel.ListeFormeDefaut[1].Id, size).GetPoints();
            PolygonButtonB.Stroke = new SolidColorBrush(ViewModel.ListeFormeDefaut[1].Couleur);
            PolygonButtonB.Fill = new SolidColorBrush(ViewModel.ListeFormeDefaut[1].Couleur);
            PolygonButtonB.Tag = ViewModel.ListeFormeDefaut[1];

            PolygonButtonC.Points = PolygonEnum.GeneratePolygon(ViewModel.ListeFormeDefaut[2].Id, size).GetPoints();
            PolygonButtonC.Stroke = new SolidColorBrush(ViewModel.ListeFormeDefaut[2].Couleur);
            PolygonButtonC.Fill = new SolidColorBrush(ViewModel.ListeFormeDefaut[2].Couleur);
            PolygonButtonC.Tag = ViewModel.ListeFormeDefaut[2];

            PolygonButtonD.Points = PolygonEnum.GeneratePolygon(ViewModel.ListeFormeDefaut[3].Id, size).GetPoints();
            PolygonButtonD.Stroke = new SolidColorBrush(ViewModel.ListeFormeDefaut[3].Couleur);
            PolygonButtonD.Fill = new SolidColorBrush(ViewModel.ListeFormeDefaut[3].Couleur);
            PolygonButtonD.Tag = ViewModel.ListeFormeDefaut[3];

            PolygonButtonE.Points = PolygonEnum.GeneratePolygon(ViewModel.ListeFormeDefaut[4].Id, size).GetPoints();
            PolygonButtonE.Stroke = new SolidColorBrush(ViewModel.ListeFormeDefaut[4].Couleur);
            PolygonButtonE.Fill = new SolidColorBrush(ViewModel.ListeFormeDefaut[4].Couleur);
            PolygonButtonE.Tag = ViewModel.ListeFormeDefaut[4];
        }

        /// <summary>
        /// Génère un puzzle à l'écran
        /// </summary>
        private void GeneratePuzzle()
        {
            var formes = ViewModel.GeneratePuzzle();
            PolygonA.Points.Clear();
            foreach (var point in formes[0].Dessin)
            {
                PolygonA.Points.Add(point);
            }
            PolygonA.Fill = new SolidColorBrush(formes[0].Couleur);
            PolygonA.Stroke = new SolidColorBrush(formes[0].Couleur);

            PolygonB.Points.Clear();
            foreach (var point in formes[1].Dessin)
            {
                PolygonB.Points.Add(point);
            }
            PolygonB.Fill = new SolidColorBrush(formes[1].Couleur);
            PolygonB.Stroke = new SolidColorBrush(formes[1].Couleur);
            RedimmensionnePolygon();
        }

        private async void PolygonButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (!_verrouiller)
            {
                _verrouiller = true;

                var forme = ((Forme)(((Polygon)sender).Tag));

                ImageValid.Visibility = Visibility.Visible;
                ImageValid.Source = ViewModel.IsReponseCorrect(forme) ? new BitmapImage(new Uri(@"ms-appx:///Rsc/img/right.png")) : new BitmapImage(new Uri(@"ms-appx:///Rsc/img/wrong.png"));
                await Task.Delay(TimeSpan.FromMilliseconds(400));
                ImageValid.Visibility = Visibility.Collapsed;


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
                    GeneratePuzzle();
                }

                _verrouiller = false;
            }
            
        }

        private void GridOBjetButton_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_isStarting)
            {
                GeneratePuzzle();
                GenerateButton();
                _isStarting = false;
            }
            else
            {
                GenerateButton();
            }
        }

        /// <summary>
        /// pour redimensionner les éléments de la page
        /// </summary>
        private void RedimmensionnePolygon()
        {
            if (GridObjet.ActualWidth <= 400 || GridObjet.ActualHeight <= 400)
            {
                var sizePoly = (GridObjet.ActualWidth <= GridObjet.ActualHeight)
                ? GridObjet.ActualWidth
                : GridObjet.ActualHeight;
                sizePoly = sizePoly/2;
                PolygonA.Points.Clear();
                var polyA = PolygonEnum.GeneratePolygon(ViewModel.ListeIdFormeEnCours[0], sizePoly);
                foreach (var point in polyA.GetPoints())
                {
                    PolygonA.Points.Add(point);
                }
                PolygonB.Points.Clear();
                var polyB = PolygonEnum.GeneratePolygon(ViewModel.ListeIdFormeEnCours[1], sizePoly);
                foreach (var point in polyB.GetPoints())
                {
                    PolygonB.Points.Add(point);
                }
            }
        }

        private void GridObjet_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RedimmensionnePolygon();
        }
    }
}
