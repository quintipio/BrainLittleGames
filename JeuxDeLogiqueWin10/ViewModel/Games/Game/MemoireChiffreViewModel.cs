using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JeuxDeLogiqueWin10.Abstract;
using JeuxDeLogiqueWin10.Context;
using JeuxDeLogiqueWin10.Enum;
using JeuxDeLogiqueWin10.Model;
using JeuxDeLogiqueWin10.Utils;

namespace JeuxDeLogiqueWin10.ViewModel.Games.Game
{
    /// <summary>
    /// Controleur pour le jeu de mémorisation des chiffres
    /// </summary>
    public class MemoireChiffreViewModel : AbstractGame
    {
        /// <summary>
        /// indique le niveau (nombre de chiffres) en cours
        /// </summary>
        private int _niveau;

        /// <summary>
        /// une map générer aléatoirement en clé, l'emplacement, en valeur le chiffre
        /// </summary>
        private readonly Dictionary<int,int> _listeEmplacements;

        /// <summary>
        /// Génrateur d'aléatoire
        /// </summary>
        private readonly Random _random;

        /// <summary>
        /// Le nombre de tours à jouer au total
        /// </summary>
        private int _nbTours;

        /// <summary>
        /// le nombre de tours jouer
        /// </summary>
        private int _tours;

        /// <summary>
        /// temps d'apparation de la suite de chiffres en ms
        /// </summary>
        public int TempsApparition;

        /// <summary>
        /// Permet de savoir sur quel chiffre doit appuyer normalement le joueur
        /// </summary>
        private int _resultatTheorique;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="exercice">l'exercice</param>
        public MemoireChiffreViewModel(Exercice exercice) : base(exercice)
        {
            _random = new Random();
            _listeEmplacements = new Dictionary<int, int>();
        }

        /// <summary>
        /// Démarrage du jeu
        /// </summary>
        public override void StartGame()
        {
            _niveau = 1;
            _nbTours = 8;
            _tours = 0;
            CompteurErreurs = 0;

            if (Difficulte.Equals(DifficulteEnum.FACILE))
            {
                TempsApparition = 2000;
            }

                if (Difficulte.Equals(DifficulteEnum.MOYEN))
                {
                    TempsApparition = 1600;
                }

                if (Difficulte.Equals(DifficulteEnum.DIFFICILE))
                {
                    TempsApparition = 1100;
                }

            StartChrono();
        }

        /// <summary>
        /// Genere la liste des emplacements en fonction du niveau
        /// </summary>
        public Dictionary<int,int> GenererSuite()
        {
            _listeEmplacements.Clear();
            _resultatTheorique = 1;
            var listeDejaMis = new List<int>();
            for (var i = 0; i < _niveau + 3; i++)
            {
                int tmp;
                do
                {
                    tmp = _random.Next(1, _niveau + 4);
                } while (listeDejaMis.Contains(tmp));
                listeDejaMis.Add(tmp);
                _listeEmplacements.Add(i,tmp);
            }
            _tours++;
            return _listeEmplacements;
        }

        /// <summary>
        /// Retourne le niveau en cours
        /// </summary>
        /// <returns>le niveau</returns>
        public int GetNiveau()
        {
            return _niveau;
        }

        /// <summary>
        /// Vérifie le résultat tapé par un utilisateur 
        /// </summary>
        /// <param name="nb">le nombre tapé (donc la position dans la liste-1)</param>
        /// <returns>1 = OK, 2 = Faux, 3 = Terminé et Ok, 4 = terminé et faux</returns>
        public int VerifResult(int nb)
        {
            var resultat = nb == _resultatTheorique;
            int retour;

            if (resultat)
            {
                retour = (nb == _listeEmplacements.Count) ? 3 : 1;
            }
            else
            {
                retour = (nb == _listeEmplacements.Count - 1) ? 4 : 2;
            }

            if (retour == 3)
            {
                if (_niveau < 6)
                {
                    _niveau++;
                }
            }

            if (retour == 2 || retour == 4)
            {
                CompteurErreurs++;
                if (_niveau > 1)
                {
                    _niveau--;
                }
            }
            _resultatTheorique++;
            return retour;
        }

        /// <summary>
        /// Indique si le joueur à terminer de jouer
        /// </summary>
        /// <returns>true si le jeu est terminé</returns>
        public bool IsJeuFini()
        {
            return _tours >= _nbTours;
        }

        /// <summary>
        /// Fin du jeu et calcul du résultat
        /// </summary>
        /// <returns>les résultats à afficher</returns>
        public  async override Task<Resultats> CalculResult()
        {
            StopChrono();

            //calcul de l'age pour connaitre la marge de temps à rajouter au temps min
            var age = DateUtils.IntervalleEntreDeuxDatesAnnee(ContextAppli.ContextUtilisateur.EnCoursUser.DateNaissance,DateUtils.GetMaintenant());
            var nbMilisecAge = (age < 30) ? 0 : (((age - 20)/10) * 300);
            var tempsMax = 0;
            var tempsMin = (TempsApparition + nbMilisecAge + 3000 + 2500) * _nbTours;

            if (Difficulte.Equals(DifficulteEnum.FACILE))
            {
                tempsMax = (TempsApparition + nbMilisecAge + 3000 + 4000)*_nbTours;
            }

            if (Difficulte.Equals(DifficulteEnum.MOYEN))
            {
                tempsMax = (TempsApparition + nbMilisecAge + 3000 + 3500)*_nbTours;
            }

            if (Difficulte.Equals(DifficulteEnum.DIFFICILE))
            {
                tempsMax = (TempsApparition + nbMilisecAge + 3000 + 3000)*_nbTours;
            }

            //calcul de la note de temps
            int noteTemps;
            if (TempsPasse <= tempsMin) noteTemps = 100;
            else if (TempsPasse >= tempsMax) noteTemps = 0;
            else
            {
                noteTemps = 100 - ((TempsPasse - tempsMin) / ((tempsMax - tempsMin) / 100));
            }

            //prise en compte des erreurs
            var noteAvecErreurs = ((_nbTours - CompteurErreurs) * noteTemps) / _nbTours;

            return await SaveResult((noteAvecErreurs <= 0) ? 0 : ((noteTemps * 3) + noteAvecErreurs) / 4);
        }
    }
}
