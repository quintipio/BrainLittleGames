using System;
using Windows.ApplicationModel;

namespace JeuxDeLogiqueWin10.Context
{
    public static class ContextStatic
    {
        /// <summary>
        /// le nom de l'application
        /// </summary>
        public const string NomAppli = "Brain Little Games";

        /// <summary>
        /// adresse de support
        /// </summary>
        public const string Support = "";

        /// <summary>
        /// version de l'application
        /// </summary>
        public const string Version = "2.1.1";

        /// <summary>
        /// nom du développeur
        /// </summary>
        public const string Developpeur = "";

        /// <summary>
        /// Nom du fichier de la base de donnée
        /// </summary>
        public const string Database = "database.db";

        /// <summary>
        /// Nom d'utilisateur à taper pour débloquer le mode développeur
        /// </summary>
        public const string CheatFullModeName = "";

        /// <summary>
        /// Date de naissance à saisir pour débloquer le mode développeur
        /// </summary>
        public static readonly DateTime CheatFullModeDateNaissance = new DateTime(1900, 01, 01);
    }
}
