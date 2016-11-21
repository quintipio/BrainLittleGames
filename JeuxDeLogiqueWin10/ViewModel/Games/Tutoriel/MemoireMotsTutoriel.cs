using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Media.Imaging;
using JeuxDeLogiqueWin10.Interface;

namespace JeuxDeLogiqueWin10.ViewModel.Games.Tutoriel
{
    /// <summary>
    /// Tutoriel pour le jeu de mémorisation des mots
    /// </summary>
    class MemoireMotsTutoriel : ITutoriel
    {
        public Dictionary<int, string> TexteTutoList { get; set; }
        public Dictionary<int, BitmapImage> ImageTutoList { get; set; }


        /// <summary>
        /// Constructeur
        /// </summary>
        public MemoireMotsTutoriel()
        {
            TexteTutoList = new Dictionary<int, string>
            {
                {1, ResourceLoader.GetForCurrentView("Tutoriel").GetString("MemoireMot1")},
                {2, ResourceLoader.GetForCurrentView("Tutoriel").GetString("MemoireMot2")},
                {3, ResourceLoader.GetForCurrentView("Tutoriel").GetString("MemoireMot3")},
                {4, ResourceLoader.GetForCurrentView("Tutoriel").GetString("MemoireMot4")},
                {5, ResourceLoader.GetForCurrentView("Tutoriel").GetString("MemoireMot5")},
            };

            ImageTutoList = new Dictionary<int, BitmapImage>
            {
                {1, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/MemMot1.png"))},
                {2, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/MemMot1.png"))},
                {3, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/MemMot2.png"))},
                {4, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/MemMot3.png"))},
                {5, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/MemMot4.png"))}
            };
        }

    }
}
