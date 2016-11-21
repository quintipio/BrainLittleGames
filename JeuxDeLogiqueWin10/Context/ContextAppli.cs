using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using JeuxDeLogiqueWin10.Model;
using JeuxDeLogiqueWin10.ViewModel.Games.Tutoriel;
using JeuxDeLogiqueWin10.Views.Games.Game;

namespace JeuxDeLogiqueWin10.Context
{
    /// <summary>
    /// Context de toute l'application
    /// </summary>
    public static class ContextAppli
    {
        /// <summary>
        /// Liste des catégories d'exercices
        /// </summary>
        public static List<Categorie> CategoriesList { get; set; }

        /// <summary>
        /// Liste des exercices
        /// </summary>
        public static List<Exercice> ExercicesList { get; set; }

        /// <summary>
        /// Contexte d'un utilisateur chargé
        /// </summary>
        public static ContextUtilisateur ContextUtilisateur { get; set; }

        /// <summary>
        /// Initialisation de l'application
        /// </summary>
        public static void Init()
        {
            CategoriesList = new List<Categorie>
            {
                new Categorie(1,ResourceLoader.GetForCurrentView().GetString("Calcul")),
                new Categorie(2,ResourceLoader.GetForCurrentView().GetString("Compte")),
                new Categorie(3,ResourceLoader.GetForCurrentView().GetString("Memoire")),
                new Categorie(4,ResourceLoader.GetForCurrentView().GetString("Reflexe")),
                new Categorie(5,ResourceLoader.GetForCurrentView().GetString("Reflexion"))
            };

            //attention l'id exercice 0 est réservé à l'évaluation
            ExercicesList = new List<Exercice>
            {
                new Exercice(1,ResourceLoader.GetForCurrentView().GetString("CalculMental"),1,typeof(CalculMentalGameView),new CalculMentalTutoriel(),true),
                new Exercice(2,ResourceLoader.GetForCurrentView().GetString("PyramideChiffre"),1,typeof(PyramideChiffreView),new PyramideChiffresTutoriel(),false),
                new Exercice(3,ResourceLoader.GetForCurrentView().GetString("JeuHoraire"),1,typeof(JeuHoraireView),new JeuHoraireTutoriel(),false),
                new Exercice(4,ResourceLoader.GetForCurrentView().GetString("DevineOperateur"),1,typeof(DevineOperateurView),new DevineOperateurTutoriel(),false),
                new Exercice(5,ResourceLoader.GetForCurrentView().GetString("ComptePersonne"),2,typeof(ComptePersonneView),new ComptePersonneTutoriel(),false),
                new Exercice(6,ResourceLoader.GetForCurrentView().GetString("CompteChiffre"),2,typeof(CompteChiffreView),new CompteChiffreTutoriel(),false),
                new Exercice(7,ResourceLoader.GetForCurrentView().GetString("MemoireChiffre"),3,typeof(MemoireChiffreView),new MemoireChiffreTutoriel(),true),
                new Exercice(8,ResourceLoader.GetForCurrentView().GetString("MemoireMots"),3,typeof(MemoireMotsView),new MemoireMotsTutoriel(),false),
                new Exercice(9,ResourceLoader.GetForCurrentView().GetString("MemoireCarte"),3,typeof(MemoireCarteView),new MemoireCarteTutoriel(),false),
                new Exercice(10,ResourceLoader.GetForCurrentView().GetString("JeuCouleurs"),4,typeof(JeuCouleursView),new CouleursTutoriel(),false),
                new Exercice(11,ResourceLoader.GetForCurrentView().GetString("JeuObjetCouleur"),4,typeof(TrouveObjetCouleurView),new TrouveObjetCouleurTutoriel(),true),
                new Exercice(12,ResourceLoader.GetForCurrentView().GetString("MotsMelange"),5,typeof(MotsMelangeView),new MotsMelangeTutoriel(),true),
            };

            //choix aléatoire de deux jeux à rendre disponible
            /*var _random = new Random();
            var listeJeuTrial = new List<int>();

            for (var i = 0; i < 2; i++)
            {
                var alea = _random.Next(1, ExercicesList.Count + 1);
                if (!listeJeuTrial.Contains(alea))
                {
                    listeJeuTrial.Add(alea);
                }
            }

            foreach (var id in listeJeuTrial)
            {
                var exec = ExercicesList.First(x => x.Id == id);
                exec.IsDispoTrial = true;
            }*/
        }

        /// <summary>
        /// Charge le contexte Utilisateur d'un utilisateur
        /// </summary>
        /// <param name="user">l'utilisateur à charger</param>
        public static void ChargerContextUtilisateur(User user)
        {
            ContextUtilisateur = ContextUtilisateur.GetContexteUtilisateur(user);
        }
    }
}
