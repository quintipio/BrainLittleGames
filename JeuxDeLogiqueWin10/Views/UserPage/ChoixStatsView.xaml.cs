using System.Collections.ObjectModel;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using JeuxDeLogiqueWin10.Context;
using JeuxDeLogiqueWin10.Enum;
using JeuxDeLogiqueWin10.Model;

namespace JeuxDeLogiqueWin10.Views.UserPage
{
    /// <summary>
    /// Page pour choisir quels stats consulter
    /// </summary>
    public sealed partial class ChoixStatsView
    {

        public ChoixStatsView()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Disabled;
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is ModeOuvertureStatsEnum)
            {
                var mode = (ModeOuvertureStatsEnum)e.Parameter;


                if (mode.Equals(ModeOuvertureStatsEnum.Categories))
                {
                    HeaderTextBlock.Text = ResourceLoader.GetForCurrentView().GetString("StatsCateg");
                    ListeElementsStatsGridView.ItemsSource =
                        new ObservableCollection<Categorie>(ContextAppli.CategoriesList);
                }

                if (mode.Equals(ModeOuvertureStatsEnum.Exercices))
                {
                    HeaderTextBlock.Text = ResourceLoader.GetForCurrentView().GetString("StatsExer");
                    ListeElementsStatsGridView.ItemsSource =
                        new ObservableCollection<Exercice>(ContextAppli.ExercicesList);
                }
            }
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (((Button) sender).Tag is Categorie)
            {
                ((Frame)Window.Current.Content).Navigate(typeof(StatView),((Button)sender).Tag as Categorie);
            }

            if (((Button)sender).Tag is Exercice)
            {
                ((Frame)Window.Current.Content).Navigate(typeof(StatView),((Button)sender).Tag as Exercice);
            }
        }
    }
}
