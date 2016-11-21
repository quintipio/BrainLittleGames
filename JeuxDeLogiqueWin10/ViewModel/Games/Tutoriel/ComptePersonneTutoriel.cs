using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Media.Imaging;
using JeuxDeLogiqueWin10.Interface;

namespace JeuxDeLogiqueWin10.ViewModel.Games.Tutoriel
{
    /// <summary>
    /// Tutoriel pour e jeu de comtpage des entrées/sorties
    /// </summary>
    class ComptePersonneTutoriel : ITutoriel
    {
        public Dictionary<int, string> TexteTutoList { get; set; }
        public Dictionary<int, BitmapImage> ImageTutoList { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public ComptePersonneTutoriel()
        {
            TexteTutoList = new Dictionary<int, string>
            {
                {1, ResourceLoader.GetForCurrentView("Tutoriel").GetString("ComptePersonne1")},
                {2, ResourceLoader.GetForCurrentView("Tutoriel").GetString("ComptePersonne2")},
                {3, ResourceLoader.GetForCurrentView("Tutoriel").GetString("ComptePersonne3")},
                {4, ResourceLoader.GetForCurrentView("Tutoriel").GetString("ComptePersonne4")},
                {5, ResourceLoader.GetForCurrentView("Tutoriel").GetString("ComptePersonne5")}
            };

            ImageTutoList = new Dictionary<int, BitmapImage>
            {
                {1, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/ComptePersonne1.png"))},
                {2, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/ComptePersonne2.png"))},
                {3, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/ComptePersonne3.png"))},
                {4, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/ComptePersonne4.png"))},
                {5, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/ComptePersonne4.png"))}
            };
        }
    }
}
