using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using JeuxDeLogiqueWin10.Model;

namespace JeuxDeLogiqueWin10.Views.Games
{
    /// <summary>
    /// Page pour afficher un tutoriel
    /// </summary>
    public sealed partial class TutorielView 
    {

        private Exercice _exercice;
        private int _increment;


        public TutorielView()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Disabled;
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _exercice = e.Parameter as Exercice;
            if (_exercice != null)
            {
                HeaderTextBlock.Text = _exercice.Nom;
                _increment = 0;
                NextPage();
            }
        }
        

        private void RetourButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(_exercice.Page, _exercice);
        }

        /// <summary>
        /// permet de naviguer au prochain texte ou d'afficher le bouton pour retourner au jeu
        /// </summary>
        private void NextPage()
        {
            _increment++;
            ImageTuto.Source = _exercice.Tutoriel.ImageTutoList[_increment];
            InstructionTextBlock.Text = _exercice.Tutoriel.TexteTutoList[_increment];
            
            if (_increment >= _exercice.Tutoriel.TexteTutoList.Count)
            {
                SuivantButton.Visibility = Visibility.Visible;
                JeuButton.Visibility = Visibility.Visible;
            }
        }

        private void SuivantButton_Click(object sender, RoutedEventArgs e)
        {
            NextPage();
        }


    }
}
