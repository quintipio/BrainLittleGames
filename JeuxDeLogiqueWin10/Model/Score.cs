using System;
using SQLite;

namespace JeuxDeLogiqueWin10.Model
{
    /// <summary>
    /// Model des scores des utilisateurs pour chaque exercice
    /// </summary>
    [Table("score")]
    public class Score
    {
        /// <summary>
        /// id composé des champs devant être unique pour satisfaire Sqlite
        /// </summary>
        [PrimaryKey, Column("id")]
        public string Id { get; set; }

        /// <summary>
        /// id de l'utilisateur
        /// </summary>
        [Column("idUtilisateur")]
        public int IdUtilisateur { get; set; }

        /// <summary>
        /// id de l'exercice
        /// </summary>
        [Column("idExercice")]
        public int IdExercice { get; set; }

        /// <summary>
        /// score de l'exercice
        /// </summary>
        [Column("resultat")]
        public int Resultat { get; set; }

        /// <summary>
        /// date du score
        /// </summary>
        [Column("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Temps écoulé pour résoudre l'exercice
        /// </summary>
        [Column("nbsecondes")]
        public int NbSecondes { get; set; }

        /// <summary>
        /// a appeler une fois que tout les champs sont remplis pour génerer l'id de l'objet 
        /// et avant de le sauvegarder en base
        /// </summary>
        public void GenerateId()
        {
            Id = Date + "-" + IdExercice + "-" + IdUtilisateur;
        }

        #region override

        public override string ToString()
        {
            return Resultat.ToString();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Score) obj);
        }

        private bool Equals(Score other)
        {
            return IdUtilisateur == other.IdUtilisateur &&  IdExercice == other.IdExercice && 
                Date.Day == other.Date.Day && Date.Month == other.Date.Month && Date.Year == other.Date.Year
                && Date.Hour == other.Date.Hour && Date.Minute == other.Date.Minute && Date.Second == other.Date.Second;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = IdUtilisateur;
                hashCode = (hashCode * 397) ^ IdExercice;
                hashCode = (hashCode * 397) ^ Date.GetHashCode();
                return hashCode;
            }
        }

        #endregion
    }
}
