using System;
using System.Threading.Tasks;
using JeuxDeLogiqueWin10.Abstract;
using JeuxDeLogiqueWin10.Context;
using JeuxDeLogiqueWin10.Enum;
using JeuxDeLogiqueWin10.Model;
using JeuxDeLogiqueWin10.Utils;

namespace JeuxDeLogiqueWin10.ViewModel.Games.Game
{
    /// <summary>
    /// Controleur du jeu pour le jeu des horaires
    /// </summary>
    public class JeuHoraireViewModel : AbstractGame
    {
        /// <summary>
        /// Génrateur aléatoire
        /// </summary>
        private readonly Random _random;

        /// <summary>
        /// Nombre de tours à joeur avant de finir
        /// </summary>
        private int _nombreTours;

        /// <summary>
        /// Heure avant
        /// </summary>
        private string _heureA;

        /// <summary>
        /// Heure après
        /// </summary>
        private string _heureB;

        /// <summary>
        /// Difference des minutes
        /// </summary>
        private int _differenceMinute;

        /// <summary>
        /// Différence des heures
        /// </summary>
        private int _differenceHeure;

        /// <summary>
        /// Compte le nombre de tours joué
        /// </summary>
        private int _compteTours;

        /// <summary>
        /// Heure avant
        /// </summary>
        public string HeureA
        {
            get
            {
                return _heureA;
            }

            set
            {
                _heureA = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Heure après
        /// </summary>
        public string HeureB
        {
            get
            {
                return _heureB;
            }

            set
            {
                _heureB = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="exercice">l'exercice</param>
        public JeuHoraireViewModel(Exercice exercice) : base(exercice)
        {
            _random = new Random();
        }

        /// <summary>
        /// Démarrage du jeu
        /// </summary>
        public override void StartGame()
        {
            StartChrono();
            CompteurErreurs = 0;
            _compteTours = 0;
            _nombreTours = 15;
        }

        /// <summary>
        /// Genere les deux horloges et calcul la différence entre les deux
        /// </summary>
        public void GenererHeure()
        {
            var heureA =  _random.Next(0, 12);
            var minuteA = _random.Next(0, 60);

            //si c'est en faciel ou moyen, on arrondi à heure finissant par 5 ou zero
            if (!Difficulte.Equals(DifficulteEnum.DIFFICILE) && minuteA%5 != 0 )
            {
                do
                {
                    minuteA++;
                } while (minuteA % 5 != 0);

                if (minuteA >= 60)
                {
                    minuteA = 55;
                }
            }

            var heureB = _random.Next(0, 12);
            var minuteB = _random.Next(0, 60);

            if (!Difficulte.Equals(DifficulteEnum.DIFFICILE) && minuteB % 5 != 0)
            {
                do
                {
                    minuteB++;
                } while (minuteB % 5 != 0);

                if (minuteB >= 60)
                {
                    minuteB = 55;
                }
            }

            var heureFin = DateUtils.AdditionHeure(heureA, minuteA, heureB, minuteB);

            HeureA = ((heureA.ToString().Length < 2)?"0"+heureA:heureA.ToString()) + " : " + ((minuteA.ToString().Length < 2) ? "0" + minuteA : minuteA.ToString());
            HeureB = ((heureFin.Hours.ToString().Length < 2) ? "0" + heureFin.Hours : heureFin.Hours.ToString()) + " : " + ((heureFin.Minutes.ToString().Length < 2) ? "0" + heureFin.Minutes : heureFin.Minutes.ToString());
            _differenceHeure = heureB;
            _differenceMinute = minuteB;
        }

        /// <summary>
        /// Vérifie si l'heure entré par le joueur est correcte ou non
        /// </summary>
        /// <param name="heure"></param>
        /// <param name="minute"></param>
        /// <returns>true si il n'y aucune différence</returns>
        public bool ControlResult(string heure, string minute)
        {
            int heureConvert;
            int minuteConvert;

            int.TryParse(heure, out heureConvert);
            int.TryParse(minute, out minuteConvert);

            var res = heureConvert.Equals(_differenceHeure) && minuteConvert.Equals(_differenceMinute);
            if (res)
            {
                _compteTours++;
            }
            else
            {
                CompteurErreurs++;
            }
            return res;
        }

        /// <summary>
        /// Permet de savoir si le jeu est fini ou non
        /// </summary>
        /// <returns>true si le jeu est fini</returns>
        public bool IsJeuFini()
        {
            return _compteTours >= _nombreTours;
        }

        /// <summary>
        /// Calcul du résultat et fin du jeu
        /// </summary>
        /// <returns>les résultats à afficher pour les scores</returns>
        public async override Task<Resultats> CalculResult()
        {
            StopChrono();

            //calcul de l'age pour connaitre la marge de temps à rajouter au temps min
            var age = DateUtils.IntervalleEntreDeuxDatesAnnee(ContextAppli.ContextUtilisateur.EnCoursUser.DateNaissance, DateUtils.GetMaintenant());
            var nbMilisecAge = (age < 30) ? 0 : (((age - 20) / 10) * 1500);


            //calcul de l'interval de temps de résolution du jeu en fonction de l'age et de la diffculté
            var tempsMin = (5000 + nbMilisecAge) * _nombreTours;
            var tempsMax = 0;

            if (Difficulte.Equals(DifficulteEnum.FACILE))
            {
                tempsMin = (6000 + nbMilisecAge) * _nombreTours;
                tempsMax = (20000 + (nbMilisecAge * 3)) * _nombreTours;
            }

            if (Difficulte.Equals(DifficulteEnum.MOYEN))
            {
                tempsMin = (5000 + nbMilisecAge) * _nombreTours;
                tempsMax = (15000 + (nbMilisecAge * 3)) * _nombreTours;
            }

            if (Difficulte.Equals(DifficulteEnum.DIFFICILE))
            {
                tempsMin = (4000 + nbMilisecAge) * _nombreTours;
                tempsMax = (10000 + (nbMilisecAge * 3)) * _nombreTours;
            }

            //calcul de la note de temps
            int noteTemps;
            if (TempsPasse <= tempsMin) noteTemps = 100;
            else if (TempsPasse >= tempsMax) noteTemps = 0;
            else
            {
                noteTemps = 100 - ((TempsPasse - tempsMin) / ((tempsMax - tempsMin) / 100));
            }

            //prise en comtpe des erreurs
            var noteAvecErreurs = 100 - ((CompteurErreurs * 100) / _nombreTours);

            //calcul de la note finale et sauvegarde
            return await SaveResult((noteAvecErreurs <= 0) ? 0 : ((noteTemps * 3) + noteAvecErreurs) / 4);
        }
    }
}
