using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml.Media;
using JeuxDeLogiqueWin10.Abstract;
using JeuxDeLogiqueWin10.Context;
using JeuxDeLogiqueWin10.Enum;
using JeuxDeLogiqueWin10.Model;
using JeuxDeLogiqueWin10.Utils;

namespace JeuxDeLogiqueWin10.ViewModel.Games.Game
{
    /// <summary>
    /// Controleur pour le jeu des couleurs
    /// </summary>
    public class JeuCouleurViewModel : AbstractGame
    {
        /// <summary>
        /// Contiendras la liste des couleurs jouables
        /// </summary>
        private readonly List<Element<string,SolidColorBrush>> _listeCouleur;

        /// <summary>
        /// Générateur aléatoire
        /// </summary>
        private readonly Random _random;

        /// <summary>
        /// le nombre de tours à jouer
        /// </summary>
        private int _nombreToursTotal;

        /// <summary>
        /// Compte le nombre de tours passé
        /// </summary>
        private int _nombreTours;

        private Element<string, SolidColorBrush> _selectedColor;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="exercice">l'exercice associé</param>
        public JeuCouleurViewModel(Exercice exercice) : base(exercice)
        {
            _listeCouleur = new List<Element<string, SolidColorBrush>>();
            _random = new Random();
        }
        
        /// <summary>
        /// Méthode démarrant les variables du jeu
        /// </summary>
        public override void StartGame()
        {
            _nombreToursTotal = 29;
            _nombreTours = 0;
            CompteurErreurs = 0;

            _listeCouleur.Add(new Element<string, SolidColorBrush>(ResourceLoader.GetForCurrentView().GetString("Rouge"), new SolidColorBrush(Color.FromArgb(255,255,0,0))));
            _listeCouleur.Add(new Element<string, SolidColorBrush>(ResourceLoader.GetForCurrentView().GetString("Jaune"), new SolidColorBrush(Color.FromArgb(255,255,255,0))));
            _listeCouleur.Add(new Element<string, SolidColorBrush>(ResourceLoader.GetForCurrentView().GetString("Noir"), new SolidColorBrush(Color.FromArgb(255,0,0,0))));
            _listeCouleur.Add(new Element<string, SolidColorBrush>(ResourceLoader.GetForCurrentView().GetString("Bleu"), new SolidColorBrush(Color.FromArgb(255, 0, 0, 255))));

             if (Difficulte.Equals(DifficulteEnum.MOYEN) || Difficulte.Equals(DifficulteEnum.DIFFICILE))
             {
                 _listeCouleur.Add(new Element<string, SolidColorBrush>(ResourceLoader.GetForCurrentView().GetString("Orange"), new SolidColorBrush(Color.FromArgb(255, 255, 104, 0))));
                 _listeCouleur.Add(new Element<string, SolidColorBrush>(ResourceLoader.GetForCurrentView().GetString("Violet"), new SolidColorBrush(Color.FromArgb(255, 170, 0, 255))));
            }

            if (Difficulte.Equals(DifficulteEnum.DIFFICILE))
            {
                _listeCouleur.Add(new Element<string, SolidColorBrush>(ResourceLoader.GetForCurrentView().GetString("Rose"), new SolidColorBrush(Color.FromArgb(255, 244,114, 208))));
                _listeCouleur.Add(new Element<string, SolidColorBrush>(ResourceLoader.GetForCurrentView().GetString("Vert"), new SolidColorBrush(Color.FromArgb(255, 96, 169, 23))));
            }

            StartChrono();
        }

        /// <summary>
        /// Retourne la liste des couleurs disponible
        /// </summary>
        /// <returns>la liste des couleurs. clé le nom, valeur, le code</returns>
        public List<Element<string,SolidColorBrush>> GetListeCouleurs()
        {
            return _listeCouleur;
        }

        /// <summary>
        /// Vérifie si la couleur sélectionnée est correcte ou non
        /// </summary>
        /// <param name="color">la couleur</param>
        /// <returns>true si c'est la même</returns>
        public bool VerifCouleur(SolidColorBrush color)
        {
            var retour = _selectedColor.Valeur.Equals(color);
            if (!retour)
            {
                CompteurErreurs++;
            }
            return retour;
        }

        /// <summary>
        /// Permet 'obtenir une couleur aléatoire différente entre le mot et sa couleur
        /// </summary>
        /// <returns>la couleur en nom, la couleur du mot en valeur</returns>
        public Element<string,SolidColorBrush> GetCouleur()
        {
            _nombreTours++;
            _selectedColor = new Element<string, SolidColorBrush>(_listeCouleur[_random.Next(0, _listeCouleur.Count)].Nom,
                _listeCouleur[_random.Next(0, _listeCouleur.Count)].Valeur);
            return _selectedColor;
        }

        /// <summary>
        /// Indique si le mini jeu est mini terminé
        /// </summary>
        /// <returns>true si le mini jeu est terminé</returns>
        public bool IsJeuFini()
        {
            return _nombreTours > _nombreToursTotal;
        }

        /// <summary>
        /// Méthode d'arret du jeu et calculant les scores
        /// </summary>
        /// <returns>les résultats de l'exerice</returns>
        public async override Task<Resultats> CalculResult()
        {
            StopChrono();

            //calcul de l'age pour connaitre la marge de temps à rajouter au temps min
            var age = DateUtils.IntervalleEntreDeuxDatesAnnee(ContextAppli.ContextUtilisateur.EnCoursUser.DateNaissance, DateUtils.GetMaintenant());
            var nbMilisecAge = (age < 30) ? 0 : (((age - 20) / 10) * 250);

            var tempsMin = (800 + nbMilisecAge) * _nombreToursTotal;
            var tempsMax = (5000 + nbMilisecAge) * _nombreToursTotal;

            //calcul de la note de temps
            int noteTemps;
            if (TempsPasse <= tempsMin) noteTemps = 100;
            else if (TempsPasse >= tempsMax) noteTemps = 0;
            else
            {
                noteTemps = 100 - ((TempsPasse - tempsMin) / ((tempsMax - tempsMin) / 100));
            }
            
            //calcul de la note finale et sauvegarde
            return await SaveResult(noteTemps);
        }
    }
}
