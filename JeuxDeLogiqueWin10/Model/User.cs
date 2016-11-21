using System;
using SQLite;

namespace JeuxDeLogiqueWin10.Model
{
    /// <summary>
    /// Model de l'utilisateur
    /// </summary>
    [Table("users")]
    public class User
    {
        /// <summary>
        /// Id de l'utilisateur
        /// </summary>
        [PrimaryKey, AutoIncrement, Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// nom de l'utilisateur
        /// </summary>
        [Column("nom"),MaxLength(30)]
        public string Nom { get; set; }


        /// <summary>
        /// Date de naissance de l'utilisateur
        /// </summary>
        [Column("age")]
        public DateTime DateNaissance { get; set; }
        
        #region override
        public override string ToString()
        {
            return Nom;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((User) obj);
        }

        private bool Equals(User other)
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
