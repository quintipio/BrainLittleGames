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
    public class JeuHoraireTutoriel : ITutoriel
    {
        public Dictionary<int, string> TexteTutoList { get; set; }
        public Dictionary<int, BitmapImage> ImageTutoList { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public JeuHoraireTutoriel()
        {
            TexteTutoList = new Dictionary<int, string>
            {
                {1, ResourceLoader.GetForCurrentView("Tutoriel").GetString("JeuHoraire1")},
                {2, ResourceLoader.GetForCurrentView("Tutoriel").GetString("JeuHoraire2")},
                {3, ResourceLoader.GetForCurrentView("Tutoriel").GetString("JeuHoraire3")},
                {4, ResourceLoader.GetForCurrentView("Tutoriel").GetString("JeuHoraire4")},
                {5, ResourceLoader.GetForCurrentView("Tutoriel").GetString("JeuHoraire5")},
                {6, ResourceLoader.GetForCurrentView("Tutoriel").GetString("JeuHoraire6")},
                {7, ResourceLoader.GetForCurrentView("Tutoriel").GetString("SwitchKeyboard")}
            };

            ImageTutoList = new Dictionary<int, BitmapImage>
            {
                {1, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/JeuHoraire1.png"))},
                {2, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/JeuHoraire2.png"))},
                {3, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/JeuHoraire3.png"))},
                {4, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/JeuHoraire4.png"))},
                {5, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/JeuHoraire5.png"))},
                {6, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/JeuHoraire5.png"))},
                {7, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/Switch.png"))}
            };
        }
    }
}
