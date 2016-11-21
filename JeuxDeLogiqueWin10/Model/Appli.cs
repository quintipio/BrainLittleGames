using SQLite;

namespace JeuxDeLogiqueWin10.Model
{
    /// <summary>
    /// Model contenant des variables de l'application (version, paramètres....)
    /// </summary>
    [Table("appli")]
    public class Appli
    {
        [PrimaryKey, Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Version de l'application
        /// </summary>
        [Column("version")]
        public string Version { get; set; }

        /// <summary>
        /// indique si le mode développeur peut être activé
        /// </summary>
        [Column("isFull")]
        public bool IsFull { get; set; }
    }
}
