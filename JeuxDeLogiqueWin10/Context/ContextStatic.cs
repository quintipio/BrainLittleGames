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
        public const string Support = "mail@mail.fr";

        /// <summary>
        /// version de l'application
        /// </summary>
        public const string Version = "2.1.0";

        /// <summary>
        /// nom du développeur
        /// </summary>
        public const string Developpeur = "Nom Dev";

        /// <summary>
        /// Nom du fichier de la base de donnée
        /// </summary>
        public const string Database = "database.db";

        /// <summary>
        /// Nom d'utilisateur à taper pour débloquer le mode développeur
        /// </summary>
        public const string CheatFullModeName = "Mot_de_passe_secret";

        /// <summary>
        /// Date de naissance à saisir pour débloquer le mode développeur
        /// </summary>
        public static readonly DateTime CheatFullModeDateNaissance = new DateTime(1980, 01, 01);
    }
}
