using System.Collections.Generic;
using Windows.UI.Xaml.Media.Imaging;

namespace JeuxDeLogiqueWin10.Interface
{
    public interface ITutoriel
    {
        Dictionary<int,string> TexteTutoList { get; set; }

        Dictionary<int, BitmapImage> ImageTutoList { get; set; } 
    }
}
