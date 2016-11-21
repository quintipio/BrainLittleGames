
using System;
using JeuxDeLogiqueWin10.Enum;
using JeuxDeLogiqueWin10.Interface;

namespace JeuxDeLogiqueWin10.Model
{
    /// <summary>
    /// model d'un exercice
    /// </summary>
    public class Exercice
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
        /// id de la catégorie de l'exercice
        /// </summary>
        public int IdCategorie { get; set; }

        /// <summary>
        /// le type de la page à ouvrir pour démarrer l'exercice
        /// </summary>
        public Type Page { get; set; }

        /// <summary>
        /// Le tutoriel de l'exerice
        /// </summary>
        public ITutoriel Tutoriel { get; set; }

        /// <summary>
        /// Difficulté choisie pour démarrer l'exercice
        /// </summary>
        public DifficulteEnum Difficulte { get; set; }

        /// <summary>
        /// Indique si ce mini jeu est disponible en mode trial
        /// </summary>
        public bool IsDispoTrial { get; set; }


        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="nom">nom</param>
        /// <param name="idCategorie">id de la catégorie de l'exercice</param>
        /// <param name="page">la page à ouvrir pour démarrer l'exercice</param>
        /// <param name="tutoriel">le type du tutoriel de l'exerice</param>
        /// <param name="isDispoTrial">Indique si le jeu est aussi disponible en mode trial</param>
        public Exercice(int id, string nom, int idCategorie,Type page, ITutoriel tutoriel,bool isDispoTrial)
        {
            Id = id;
            Nom = nom;
            IdCategorie = idCategorie;
            Page = page;
            Tutoriel = tutoriel;
            IsDispoTrial = isDispoTrial;
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
