using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Media.Imaging;
using JeuxDeLogiqueWin10.Interface;

namespace JeuxDeLogiqueWin10.ViewModel.Games.Tutoriel
{
    /// <summary>
    /// Tutoriel pour le jeu de mémorisation des cartes
    /// </summary>
    public class MemoireCarteTutoriel : ITutoriel
    {
        public Dictionary<int, string> TexteTutoList { get; set; }
        public Dictionary<int, BitmapImage> ImageTutoList { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public MemoireCarteTutoriel()
        {
            TexteTutoList = new Dictionary<int, string>
            {
                {1, ResourceLoader.GetForCurrentView("Tutoriel").GetString("MemoireCarte1")},
                {2, ResourceLoader.GetForCurrentView("Tutoriel").GetString("MemoireCarte2")},
                {3, ResourceLoader.GetForCurrentView("Tutoriel").GetString("MemoireCarte3")},
                {4, ResourceLoader.GetForCurrentView("Tutoriel").GetString("MemoireCarte4")},
            };

            ImageTutoList = new Dictionary<int, BitmapImage>
            {
                {1, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/MemLettre1.png"))},
                {2, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/MemLettre2.png"))},
                {3, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/MemLettre3.png"))},
                {4, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/MemLettre4.png"))}
            };
        }
    }
}
