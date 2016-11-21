using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Media.Imaging;
using JeuxDeLogiqueWin10.Interface;

namespace JeuxDeLogiqueWin10.ViewModel.Games.Tutoriel
{
    /// <summary>
    /// Tutoriel pour le jeu des couleurs
    /// </summary>
    class CouleursTutoriel : ITutoriel
    {
        public Dictionary<int, string> TexteTutoList { get; set; }
        public Dictionary<int, BitmapImage> ImageTutoList { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public CouleursTutoriel()
        {
            TexteTutoList = new Dictionary<int, string>
            {
                {1, ResourceLoader.GetForCurrentView("Tutoriel").GetString("JeuCouleur1")},
                {2, ResourceLoader.GetForCurrentView("Tutoriel").GetString("JeuCouleur2")},
                {3, ResourceLoader.GetForCurrentView("Tutoriel").GetString("JeuCouleur3")}
            };

            ImageTutoList = new Dictionary<int, BitmapImage>
            {
                {1, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/Couleur1.png"))},
                {2, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/Couleur2.png"))},
                {3, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/Couleur2.png"))}
            };
        }
    }
}
