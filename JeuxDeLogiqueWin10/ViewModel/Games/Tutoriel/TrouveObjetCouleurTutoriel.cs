using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Media.Imaging;
using JeuxDeLogiqueWin10.Interface;

namespace JeuxDeLogiqueWin10.ViewModel.Games.Tutoriel
{
    public class TrouveObjetCouleurTutoriel : ITutoriel
    {
        public Dictionary<int, string> TexteTutoList { get; set; }
        public Dictionary<int, BitmapImage> ImageTutoList { get; set; }

        public TrouveObjetCouleurTutoriel()
        {
            TexteTutoList = new Dictionary<int, string>
            {
                {1, ResourceLoader.GetForCurrentView("Tutoriel").GetString("TrouveObjetCouleur1")},
                {2, ResourceLoader.GetForCurrentView("Tutoriel").GetString("TrouveObjetCouleur2")},
                {3, ResourceLoader.GetForCurrentView("Tutoriel").GetString("TrouveObjetCouleur3")},
                {4, ResourceLoader.GetForCurrentView("Tutoriel").GetString("TrouveObjetCouleur4")},
                {5, ResourceLoader.GetForCurrentView("Tutoriel").GetString("TrouveObjetCouleur5")},
                {6, ResourceLoader.GetForCurrentView("Tutoriel").GetString("TrouveObjetCouleur6")}
            };

            ImageTutoList = new Dictionary<int, BitmapImage>
            {
                {1, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/TrouveObjetCouleur1.png"))},
                {2, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/TrouveObjetCouleur2.png"))},
                {3, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/TrouveObjetCouleur3.png"))},
                {4, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/TrouveObjetCouleur4.png"))},
                {5, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/TrouveObjetCouleur5.png"))},
                {6, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/TrouveObjetCouleur5.png"))}
            };
        }
    }
}
