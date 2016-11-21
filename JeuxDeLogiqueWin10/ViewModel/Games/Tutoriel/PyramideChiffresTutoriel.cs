using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Media.Imaging;
using JeuxDeLogiqueWin10.Interface;

namespace JeuxDeLogiqueWin10.ViewModel.Games.Tutoriel
{
    class PyramideChiffresTutoriel : ITutoriel
    {
        public Dictionary<int, string> TexteTutoList { get; set; }
        public Dictionary<int, BitmapImage> ImageTutoList { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public PyramideChiffresTutoriel()
        {
            TexteTutoList = new Dictionary<int, string>
            {
                {1, ResourceLoader.GetForCurrentView("Tutoriel").GetString("Tricalcul1")},
                {2, ResourceLoader.GetForCurrentView("Tutoriel").GetString("Tricalcul2")},
                {3, ResourceLoader.GetForCurrentView("Tutoriel").GetString("Tricalcul3")},
                {4, ResourceLoader.GetForCurrentView("Tutoriel").GetString("Tricalcul4")},
                {5, ResourceLoader.GetForCurrentView("Tutoriel").GetString("Tricalcul5")},
                {6, ResourceLoader.GetForCurrentView("Tutoriel").GetString("Tricalcul6")},
                {7, ResourceLoader.GetForCurrentView("Tutoriel").GetString("SwitchKeyboard")},

            };

            ImageTutoList = new Dictionary<int, BitmapImage>
            {
                {1, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/Tricalcul1.png"))},
                {2, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/Tricalcul1.png"))},
                {3, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/Tricalcul2.png"))},
                {4, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/Tricalcul3.png"))},
                {5, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/Tricalcul4.png"))},
                {6, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/Tricalcul4.png"))},
                {7, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/Switch.png"))}
            };
        }
    }
}
