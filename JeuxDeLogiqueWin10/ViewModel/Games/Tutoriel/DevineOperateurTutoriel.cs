using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Media.Imaging;
using JeuxDeLogiqueWin10.Interface;

namespace JeuxDeLogiqueWin10.ViewModel.Games.Tutoriel
{
    /// <summary>
    /// tutoriel pour le jeu de trouver l'opérateur
    /// </summary>
    class DevineOperateurTutoriel : ITutoriel
    {
        public Dictionary<int, string> TexteTutoList { get; set; }
        public Dictionary<int, BitmapImage> ImageTutoList { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public DevineOperateurTutoriel()
        {
            TexteTutoList = new Dictionary<int, string>
            {
                {1, ResourceLoader.GetForCurrentView("Tutoriel").GetString("DevineOp1")},
                {2, ResourceLoader.GetForCurrentView("Tutoriel").GetString("DevineOp2")},
                {3, ResourceLoader.GetForCurrentView("Tutoriel").GetString("DevineOp3")},
                {4, ResourceLoader.GetForCurrentView("Tutoriel").GetString("DevineOp4")}
            };

            ImageTutoList = new Dictionary<int, BitmapImage>
            {
                {1, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/DevineOp1.png"))},
                {2, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/DevineOp1.png"))},
                {3, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/DevineOp2.png"))},
                {4, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/DevineOp2.png"))}
            };
        }
    }
}
