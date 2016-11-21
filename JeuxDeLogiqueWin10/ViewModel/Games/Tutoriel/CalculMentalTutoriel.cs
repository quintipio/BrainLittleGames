using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Media.Imaging;
using JeuxDeLogiqueWin10.Interface;

namespace JeuxDeLogiqueWin10.ViewModel.Games.Tutoriel
{
    /// <summary>
    /// Tutoriel pour le calculMental
    /// </summary>
    public class CalculMentalTutoriel : ITutoriel
    {
        public Dictionary<int, string> TexteTutoList { get; set; }
        public Dictionary<int, BitmapImage> ImageTutoList { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public CalculMentalTutoriel()
        {
            TexteTutoList = new Dictionary<int, string>
            {
                {1, ResourceLoader.GetForCurrentView("Tutoriel").GetString("CalculMental1")},
                {2, ResourceLoader.GetForCurrentView("Tutoriel").GetString("CalculMental2")},
                {3, ResourceLoader.GetForCurrentView("Tutoriel").GetString("CalculMental3")},
                {4, ResourceLoader.GetForCurrentView("Tutoriel").GetString("CalculMental4")},
                {5, ResourceLoader.GetForCurrentView("Tutoriel").GetString("SwitchKeyboard")}
            };

            ImageTutoList = new Dictionary<int, BitmapImage>
            {
                {1, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/CalculMental1.png"))},
                {2, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/CalculMental2.png"))},
                {3, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/CalculMental3.png"))},
                {4, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/CalculMental4.png"))},
                {5, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/Switch.png"))}
            };
        }
    }
}
