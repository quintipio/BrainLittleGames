using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Media.Imaging;
using JeuxDeLogiqueWin10.Interface;

namespace JeuxDeLogiqueWin10.ViewModel.Games.Tutoriel
{
    class MemoireChiffreTutoriel : ITutoriel
    {
        public Dictionary<int, string> TexteTutoList { get; set; }
        public Dictionary<int, BitmapImage> ImageTutoList { get; set; }

         /// <summary>
        /// Constructeur
        /// </summary>
        public MemoireChiffreTutoriel()
        {
            TexteTutoList = new Dictionary<int, string>
            {
                {1, ResourceLoader.GetForCurrentView("Tutoriel").GetString("MemoireChiffre1")},
                {2, ResourceLoader.GetForCurrentView("Tutoriel").GetString("MemoireChiffre2")},
                {3, ResourceLoader.GetForCurrentView("Tutoriel").GetString("MemoireChiffre3")},
                {4, ResourceLoader.GetForCurrentView("Tutoriel").GetString("MemoireChiffre4")},
            };

            ImageTutoList = new Dictionary<int, BitmapImage>
            {
                {1, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/MemChiffre1.png"))},
                {2, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/MemChiffre2.png"))},
                {3, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/MemChiffre3.png"))},
                {4, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/MemChiffre3.png"))}
            };
        }
    }
}
