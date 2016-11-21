using System;

namespace JeuxDeLogiqueWin10.Utils
{
    /// <summary>
    /// Utilitaire pour gérer des dates
    /// </summary>
    public static class DateUtils
    {
        /// <summary>
        /// retourne l'objet DateTime à une heure précise
        /// </summary>
        /// <returns>La date précise</returns>
        public static DateTime GetMaintenant()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// soustrait un nombre d'années à une date
        /// </summary>
        /// <param name="date">la date de départ</param>
        /// <param name="annee">le nombre d'années à soustraire</param>
        /// <returns>la date avec le nombre d'année soustraite</returns>
        public static DateTime SoustraitAnnee(DateTime date, int annee)
        {
            return date.Subtract(TimeSpan.FromDays(annee*365));
        }

        /// <summary>
        /// Converti uen chaine de caractère ex : "01/08/2008" en DateTime
        /// </summary>
        /// <param name="date">La date à convertir</param>
        /// <returns>La DateTime</returns>
        public static DateTime StringEnDate(String date)
        {
            return Convert.ToDateTime(date);
        }

        /// <summary>
        /// Permet d'obtenir l'intervalle de jours entre deux dates en millisecondes
        /// </summary>
        /// <param name="oldDate"> la plus veille date</param>
        /// <param name="newDate">date la plus récente</param>
        /// <returns>Le nombre de Milisecondes</returns>
        public static int IntervalleEntreDeuxDatesMs(DateTime oldDate, DateTime newDate)
        {
            var ts = newDate - oldDate;
            return (int)ts.TotalMilliseconds;
        }

        /// <summary>
        /// Converti un nombre d emilisecondes en une chaine de caractère
        /// </summary>
        /// <param name="nbmilisecondes">le nombre de miliseconde</param>
        /// <returns>la chaine en XXhXXmXXs</returns>
        public static string ConvertNbMilisecondesString(int nbmilisecondes)
        {
            var nbSecondes = nbmilisecondes/1000;
            if (nbmilisecondes < 60000)
            {
                return nbSecondes + " s";
            }
            else if (nbmilisecondes >= 60000 && nbmilisecondes < 3600000)
            {
                var t = TimeSpan.FromSeconds(nbSecondes);
                return t.Minutes + " m " + t.Seconds + " s";
            }
            else
            {
                var t = TimeSpan.FromSeconds(nbSecondes);
                return t.Hours+" h "+ t.Minutes + " m " + t.Seconds + " s";
            }
        }

        /// <summary>
        /// Permet d'obtenir l'intervalle de jours entre deux dates en année
        /// </summary>
        /// <param name="oldDate"> la plus veille date</param>
        /// <param name="newDate">date la plus récente</param>
        /// <returns>Le nombre d'année entre les deux dates</returns>
        public static int IntervalleEntreDeuxDatesAnnee(DateTime oldDate, DateTime newDate)
        {
            return (int)newDate.Subtract(oldDate).TotalDays / 365; ;
        }

        /// <summary>
        /// Prend une heure et une date de départ et additionne des heures et des minutes
        /// </summary>
        /// <param name="heureDepart">l'heure de départ</param>
        /// <param name="minuteDepart">les minutes de départ</param>
        /// <param name="heureAjout"> le nombres de d'heures à ajouter</param>
        /// <param name="minuteAjout">le nombre de minutes à ajouter</param>
        /// <returns>la nouvelle heure</returns>
        public static TimeSpan AdditionHeure(int heureDepart, int minuteDepart, int heureAjout, int minuteAjout)
        {
            var tDepart = new TimeSpan(heureDepart,minuteDepart,0);
            return tDepart.Add(new TimeSpan(heureAjout, minuteAjout, 0));
        }
    }
}
