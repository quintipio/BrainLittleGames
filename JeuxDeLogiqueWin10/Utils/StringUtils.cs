using System;
using System.Collections.Generic;
using System.Linq;

namespace JeuxDeLogiqueWin10.Utils
{
    /// <summary>
    /// Classe utilitaire pour la gestion des chaines de caractères
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// Liste de caractères spéciaux
        /// </summary>
        private static char[] oldChar = { 'À', 'Á', 'Â', 'Ã', 'Ä', 'Å', 'à', 'á', 'â', 'ã', 'ä', 'å', 'Ò', 'Ó', 'Ô', 'Õ', 'Ö', 'Ø', 'ò', 'ó', 'ô', 'õ', 'ö', 'ø', 'È', 'É', 'Ê', 'Ë', 'è', 'é', 'ê', 'ë', 'Ì', 'Í', 'Î', 'Ï', 'ì', 'í', 'î', 'ï', 'Ù', 'Ú', 'Û', 'Ü', 'ù', 'ú', 'û', 'ü', 'ÿ', 'Ñ', 'ñ', 'Ç', 'ç', '°' };
        
        /// <summary>
        /// Liste des caractères normaux pour le remplacement
        /// </summary>
        private static char[] newChar = { 'A', 'A', 'A', 'A', 'A', 'A', 'a', 'a', 'a', 'a', 'a', 'a', 'O', 'O', 'O', 'O', 'O', 'O', 'o', 'o', 'o', 'o', 'o', 'o', 'E', 'E', 'E', 'E', 'e', 'e', 'e', 'e', 'I', 'I', 'I', 'I', 'i', 'i', 'i', 'i', 'U', 'U', 'U', 'U', 'u', 'u', 'u', 'u', 'y', 'N', 'n', 'C', 'c', ' ' };
		

        /// <summary>
        /// Converti en une liste de string en une seule string avec des \r\n en séparation
        /// </summary>
        /// <param name="liste"></param>
        /// <returns></returns>
        public static string ConvertListStringToString(IEnumerable<string> liste)
        {
            var retour = "";

            foreach (var chaine in liste)
            {
                retour += chaine + "\r\n";
            }
            return retour;
        }

        /// <summary>
        /// Supprime tout les accents espace et ponctuation d'une chaine de caractère
        /// </summary>
        /// <param name="mot">le mot à modifier</param>
        /// <returns>le mot</returns>
        public static string SupprimerCaracSpeciaux(string mot)
        {
            if (!string.IsNullOrWhiteSpace(mot))
            {
                mot = mot.Trim().ToLower();
                foreach (var carac in mot)
                {
                    var i = 0;
                    foreach (var monc in oldChar)
                    {
                        mot = mot.Replace(monc, newChar[i]);
                        i++;
                    }
                }
                return mot;
            }
            return "";
        }

        /// <summary>
        /// Compare deux mots et indiquent si ils contiennent les mêmes lettres
        /// </summary>
        /// <param name="motA">le premier mot a comparer</param>
        /// <param name="motB">le deuxième mot à comparer</param>
        /// <returns>true si il y a les mêmes lettres</returns>
        public static bool MotsContiennentMemeLettres(string motA, string motB)
        {
            var motTmp = motA;
            if (motTmp.Length == motB.Length)
            {
                foreach (var ch in motB)
                {
                    if (motTmp.Contains(ch.ToString()))
                    {
                        motTmp = motTmp.Remove(motTmp.IndexOf(ch), 1);
                    }
                }
                return string.IsNullOrWhiteSpace(motTmp);
            }
            return false;
        }
    }
}
