using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using JeuxDeLogiqueWin10.Context;
using JeuxDeLogiqueWin10.Enum;
using JeuxDeLogiqueWin10.Interface;
using JeuxDeLogiqueWin10.Model;
using JeuxDeLogiqueWin10.ViewModel;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

namespace JeuxDeLogiqueWin10.Views.UserPage
{
    /// <summary>
    /// Controle pour afficher des stats
    /// </summary>
    public sealed partial class StatView : IView<ConsultStatViewModel>
    {
        /// <summary>
        /// ViewModel
        /// </summary>
        public ConsultStatViewModel ViewModel { get; set; }



        public StatView()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Disabled;
        }
        
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ViewModel = new ConsultStatViewModel();
            await ViewModel.Initialization;

            //récupération des infos de l'exercice
            if (e.Parameter is Exercice)
            {
                HeaderTextBlock.Text = ((Exercice) e.Parameter).Nom;
                LineChart.Series.Add(new LineSeries
                {
                    ItemsSource = await ViewModel.ChargerScoreExercice(e.Parameter as Exercice),
                    Title = ((Exercice) e.Parameter).Nom,
                    IndependentValuePath = "Date",
                    DependentValuePath = "Resultat",
                    DependentRangeAxis = new LinearAxis() { Minimum = 0, Maximum = 100, Orientation = AxisOrientation.Y },
                    IsSelectionEnabled = true
                });
            }


            //récupération des infos de la catégorie et des exercices liés
            if (e.Parameter is Categorie)
            {
                HeaderTextBlock.Text = ((Categorie)e.Parameter).Nom;
                   LineChart.Series.Add( new LineSeries
                    {
                        ItemsSource = await ViewModel.ChargerScoreCategorie(e.Parameter as Categorie),
                        Title = ((Categorie)e.Parameter).Nom,
                        IndependentValuePath = "Date",
                        DependentValuePath = "Resultat",
                        DependentRangeAxis = new LinearAxis() { Minimum = 0, Maximum = 100, Orientation = AxisOrientation.Y },
                        IsSelectionEnabled = true
                    });
             

                foreach (var exercice in ContextAppli.ExercicesList.Where(x => x.IdCategorie == ((Categorie)e.Parameter).Id))
                {
                    LineChart.Series.Add(new LineSeries
                    {
                        ItemsSource = await ViewModel.ChargerScoreExercice(exercice),
                        Title = exercice.Nom,
                        IndependentValuePath = "Date",
                        DependentValuePath = "Resultat",
                        IsSelectionEnabled = true
                    });
                }

            }

            if (e.Parameter is ModeOuvertureStatsEnum && ((ModeOuvertureStatsEnum)e.Parameter).Equals(ModeOuvertureStatsEnum.Evaluation))
            {
                HeaderTextBlock.Text = ResourceLoader.GetForCurrentView().GetString("Evaluation");
                var res = await ViewModel.ChargerScoreEval();
                LineChart.Series.Add(new LineSeries
                {
                    ItemsSource =res,
                    Title = ResourceLoader.GetForCurrentView().GetString("Evaluation"),
                    IndependentValuePath = "Date",
                    DependentValuePath = "Value",
                    DependentRangeAxis = new LinearAxis() { Minimum = 0, Maximum = 20, Orientation = AxisOrientation.Y },
                    IsSelectionEnabled = true
                });
            }

            if (e.Parameter is ModeOuvertureStatsEnum && ((ModeOuvertureStatsEnum)e.Parameter).Equals(ModeOuvertureStatsEnum.General))
            {
                HeaderTextBlock.Text = ResourceLoader.GetForCurrentView().GetString("StatsGen");

                foreach (var categorie in ContextAppli.CategoriesList)
                {
                    LineChart.Series.Add(new LineSeries
                    {
                        ItemsSource = await ViewModel.ChargerScoreCategorie(categorie),
                        Title = categorie.Nom,
                        IndependentValuePath = "Date",
                        DependentValuePath = "Resultat",
                        DependentRangeAxis = new LinearAxis() { Minimum = 0, Maximum = 100, Orientation = AxisOrientation.Y },
                        IsSelectionEnabled = true
                    });
                }
            }

        }

        private void EndButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(MenuView));
        }
    }
}
