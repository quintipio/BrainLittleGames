
using Windows.UI;
using JeuxDeLogiqueWin10.Enum;

namespace JeuxDeLogiqueWin10.Model
{
    /// <summary>
    /// Classe représentant un chiffre dans le jeu de compteur de chiffres
    /// </summary>
    public class Chiffre
    {
        /// <summary>
        /// l'id 
        /// </summary>
        public int Id { get; set; }
       
        /// <summary>
        /// son chiffre
        /// </summary>
        public int Nombre { get; set; }
        
        /// <summary>
        /// sa couleur d'affichage
        /// </summary>
        public Color Couleur { get; set; }

        /// <summary>
        /// son mouvement à l'écran
        /// </summary>
        public MouvementEnum Mouvement { get; set; }

        /// <summary>
        /// Marge de gauche
        /// </summary>
        public int MarginLeft { get; set; }

        /// <summary>
        /// marge du haut
        /// </summary>
        public int MarginTop { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="id">l'id de l'objet</param>
        /// <param name="nombre">le chiffre</param>
        /// <param name="couleur">sa couleur d'affichage</param>
        /// <param name="mouvement">son mouvement sur l'écran</param>
        public Chiffre(int id, int nombre, Color couleur, MouvementEnum mouvement)
        {
            Id = id;
            Nombre = nombre;
            Couleur = couleur;
            Mouvement = mouvement;
        }

        public override string ToString()
        {
            return Nombre + "-" + Couleur + "-" + Mouvement;
        }
    }
}
