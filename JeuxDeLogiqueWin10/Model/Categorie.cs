namespace JeuxDeLogiqueWin10.Model
{
    /// <summary>
    /// Model des catégories d'exercice
    /// </summary>
    public class Categorie
    {
         /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// nom
        /// </summary>
        public string Nom { get; set; }
        
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="nom">nom</param>
        public Categorie(int id, string nom)
        {
            Id = id;
            Nom = nom;
        }

        #region override
        public override string ToString()
        {
            return Nom;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Categorie)obj);
        }

        private bool Equals(Categorie other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
        #endregion
    }
}
