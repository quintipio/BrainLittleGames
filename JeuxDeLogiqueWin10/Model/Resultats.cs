
namespace JeuxDeLogiqueWin10.Model
{
    /// <summary>
    /// Objet à passer au userControl d'affichage des résultats pour l'affichage
    /// </summary>
    public class Resultats
    {
        /// <summary>
        /// l'exercice concerné apr le résultat
        /// </summary>
        public Exercice Exercice { get; set; }

        /// <summary>
        /// le nombre de secondes passé
        /// </summary>
        public Score ScoreExercice { get; set; }

        /// <summary>
        /// le nombre d'erreurs
        /// </summary>
        public int Erreurs { get; set; }
    }
}
