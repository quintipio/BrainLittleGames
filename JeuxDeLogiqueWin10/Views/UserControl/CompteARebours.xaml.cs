using System;
using Windows.UI.Xaml;

namespace JeuxDeLogiqueWin10.Views.UserControl
{
    /// <summary>
    /// User control pour lancer un compte à rebours avant le début d'un jeu
    /// </summary>
    public sealed partial class CompteARebours
    {
        /// <summary>
        /// Timer pour le compte à rebours
        /// </summary>
        private readonly DispatcherTimer _timer;

        /// <summary>
        /// nombre de secondes affiché
        /// </summary>
        private int _secondes;

        /// <summary>
        /// Delegate pour la fin compte à rebours
        /// </summary>
        public delegate void FinCompteAReboursHandler();

        /// <summary>
        /// Evènement
        /// </summary>
        public event FinCompteAReboursHandler OnCompteReboursEnd;

        /// <summary>
        /// Constructeur
        /// </summary>
        public CompteARebours()
        {
            InitializeComponent();

            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1000) };
            _timer.Tick += timer_Tick;
        }


        // Tick du Timer pour mettre à jour l'affichage
        private void timer_Tick(object sender, object e)
        {
            _secondes--;
            CaRText.Text = _secondes.ToString();
            if (_secondes == 0)
            {
                _timer.Stop();
                OnCompteReboursEnd();
            }
        }

        /// <summary>
        /// Démarre le compte à rebours
        /// </summary>
        public void StartCompteARebours()
        {
            _secondes = 3;
            CaRText.Text = _secondes.ToString();
            _timer.Start();
        }
    }
}
