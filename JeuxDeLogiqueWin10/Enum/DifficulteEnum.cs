using System;
using Windows.ApplicationModel.Resources;

namespace JeuxDeLogiqueWin10.Enum
{
    public sealed class DifficulteEnum
    {

        private readonly string name;
        public readonly int value;

        public static readonly DifficulteEnum FACILE = new DifficulteEnum(1, ResourceLoader.GetForCurrentView().GetString("Facile"));
        public static readonly DifficulteEnum MOYEN = new DifficulteEnum(2, ResourceLoader.GetForCurrentView().GetString("Moyen"));
        public static readonly DifficulteEnum DIFFICILE = new DifficulteEnum(3, ResourceLoader.GetForCurrentView().GetString("Difficile"));

        private DifficulteEnum(int value, String name)
        {
            this.name = name;
            this.value = value;
        }

        public override String ToString()
        {
            return name;
        }


    }
}
