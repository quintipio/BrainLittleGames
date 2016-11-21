using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Media.Imaging;
using JeuxDeLogiqueWin10.Interface;

namespace JeuxDeLogiqueWin10.ViewModel.Games.Tutoriel
{
    /// <summary>
    /// Tutoriel pour le jeu de coptage des chiffres
    /// </summary>
    class CompteChiffreTutoriel : ITutoriel
    {
        public Dictionary<int, string> TexteTutoList { get; set; }
        public Dictionary<int, BitmapImage> ImageTutoList { get; set; }


        /// <summary>
        /// Constructeur
        /// </summary>
        public CompteChiffreTutoriel()
        {
            TexteTutoList = new Dictionary<int, string>
            {
                {1, ResourceLoader.GetForCurrentView("Tutoriel").GetString("CompteChiffre1")},
                {2, ResourceLoader.GetForCurrentView("Tutoriel").GetString("CompteChiffre2")},
                {3, ResourceLoader.GetForCurrentView("Tutoriel").GetString("CompteChiffre3")},
                {4, ResourceLoader.GetForCurrentView("Tutoriel").GetString("SwitchKeyboard")}
            };

            ImageTutoList = new Dictionary<int, BitmapImage>
            {
                {1, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/CompteChiffre1.png"))},
                {2, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/CompteChiffre2.png"))},
                {3, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/CompteChiffre3.png"))},
                {4, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/Switch.png"))}
            };
        }
    }
}
