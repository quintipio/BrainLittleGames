using SQLite;

namespace JeuxDeLogiqueWin10.Model
{
    /// <summary>
    /// permet de savoir si un utilisateur à vu le tuto à la première utilisation d'un mini
    /// </summary>
    [Table("tutoPasse")]
    public class TutorielPasse
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
        public bool isPasse { get; set; }

        /// <summary>
        /// Genere un id pour sqlite ( méthode à appeler après avoir remplis)
        /// </summary>
        public void GenerateId()
        {
           Id =  IdUtilisateur + "-" + IdExercice;
        }


        #region override

        public override string ToString()
        {
            return isPasse.ToString();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((TutorielPasse)obj);
        }

        private bool Equals(TutorielPasse other)
        {
            return IdUtilisateur == other.IdUtilisateur && IdExercice == other.IdExercice;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = IdUtilisateur;
                hashCode = (hashCode * 397) ^ IdExercice;
                return hashCode;
            }
        }

        #endregion

    }
}
