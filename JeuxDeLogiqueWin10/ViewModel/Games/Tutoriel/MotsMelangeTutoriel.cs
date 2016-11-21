using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Media.Imaging;
using JeuxDeLogiqueWin10.Interface;

namespace JeuxDeLogiqueWin10.ViewModel.Games.Tutoriel
{
    /// <summary>
    /// Tutoriel pour le jeu des lettres mélangées
    /// </summary>
    public class MotsMelangeTutoriel : ITutoriel
    {
        public Dictionary<int, string> TexteTutoList { get; set; }
        public Dictionary<int, BitmapImage> ImageTutoList { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public MotsMelangeTutoriel()
        {
            TexteTutoList = new Dictionary<int, string>
            {
                {1, ResourceLoader.GetForCurrentView("Tutoriel").GetString("MotMelange1")},
                {2, ResourceLoader.GetForCurrentView("Tutoriel").GetString("MotMelange2")},
                {3, ResourceLoader.GetForCurrentView("Tutoriel").GetString("MotMelange3")},
                {4, ResourceLoader.GetForCurrentView("Tutoriel").GetString("MotMelange4")},
                {5, ResourceLoader.GetForCurrentView("Tutoriel").GetString("MotMelange5")},
            };

            ImageTutoList = new Dictionary<int, BitmapImage>
            {
                {1, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/MotMel1.png"))},
                {2, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/MotMel2.png"))},
                {3, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/MotMel3.png"))},
                {4, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/MotMel4.png"))},
                {5, new BitmapImage(new Uri(@"ms-appx:///Rsc/tuto/MotMel4.png"))}
            };
        }
    }
}
