using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using JeuxDeLogiqueWin10.Context;
using JeuxDeLogiqueWin10.Enum;
using JeuxDeLogiqueWin10.Model;

namespace JeuxDeLogiqueWin10.Views.Games
{
    /// <summary>
    /// Page pour le choix de la difficulté
    /// </summary>
    public sealed partial class DifficulteView
    {
        private Exercice _exercice;

        public DifficulteView()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Disabled;
        }
        
        
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //si c'est un exercice fournit en paramètre, il s'agit du mode jeu normal, donc on ouvre l'exercice
            if (e.Parameter is Exercice)
            {
                _exercice = e.Parameter as Exercice;
                HeaderTextBlock.Text = _exercice.Nom;
            }

            //si c'est un enum du mode d'ouverture, on ouvre le jeu en mode evaluation
            if (e.Parameter is ModeOuvertureJeuEnum)
            {
                var v = (ModeOuvertureJeuEnum) e.Parameter;
                if (v.Equals(ModeOuvertureJeuEnum.ModeEval))
                {
                    ContextAppli.ContextUtilisateur.LaunchEval();
                    DifficulteGrid.Visibility = Visibility.Visible;
                    DfficulteGridView.Margin = new Thickness(0,125,0,0);
                    _exercice = await ContextAppli.ContextUtilisateur.ModeEval.ChangeExerciceEval();
                }
            }
        }

        #region ChoixDifficulté
        private void FacileButton_OnClick(object sender, RoutedEventArgs e)
        {
            _exercice.Difficulte = DifficulteEnum.FACILE;
            ((Frame)Window.Current.Content).Navigate(_exercice.Page, _exercice);
        }

        private void MoyenButton_OnClick(object sender, RoutedEventArgs e)
        {
            _exercice.Difficulte = DifficulteEnum.MOYEN;
            ((Frame)Window.Current.Content).Navigate(_exercice.Page, _exercice);
        }

        private void DifficileButton_OnClick(object sender, RoutedEventArgs e)
        {
            _exercice.Difficulte = DifficulteEnum.DIFFICILE;
            ((Frame)Window.Current.Content).Navigate(_exercice.Page, _exercice);
        }
        #endregion
    }
}
