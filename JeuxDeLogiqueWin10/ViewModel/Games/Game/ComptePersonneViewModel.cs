using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using JeuxDeLogiqueWin10.Abstract;
using JeuxDeLogiqueWin10.Enum;
using JeuxDeLogiqueWin10.Model;

namespace JeuxDeLogiqueWin10.ViewModel.Games.Game
{
    /// <summary>
    /// Structure pour le retour des mouvements à la vue
    /// </summary>
    public struct Mouvement
    {
        public int InLeft;
        public int InUp;
        public int OutRight;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="inLeft">nombre de point entrant à gauche</param>
        /// <param name="inUp">nombre de point entrant en haut</param>
        /// <param name="outRight">nombre de point sortant</param>
        public Mouvement(int inLeft, int inUp, int outRight)
        {
            InLeft = inLeft;
            InUp = inUp;
            OutRight = outRight;
        }
    }

    /// <summary>
    /// ViewModel du compteur de va et vient
    /// </summary>
    public class ComptePersonneViewModel : AbstractGame
    {
        /// <summary>
        /// le nombre de tours à faire
        /// </summary>
        private int _nombreTours;

        /// <summary>
        /// le nombre de tours fait par le joueur
        /// </summary>
        private int _nombreToursFait;

        /// <summary>
        /// le résultat à faire
        /// </summary>
        public int Resultat {get; private set; }

        /// <summary>
        /// liste des points entrant à gauche
        /// </summary>
        private List<int> _listeInLeft;

        /// <summary>
        /// liste des points entrant en haut
        /// </summary>
        private List<int> _listeInUp;

        /// <summary>
        /// liste de spoints sortant
        /// </summary>
        private List<int> _listeOut;

        /// <summary>
        /// A combien de va et vient le jeu en est sur une animation
        /// </summary>
        private int _nbVaVient;

        /// <summary>
        /// Générateur aléatoire
        /// </summary>
        private readonly Random _random;

        /// <summary>
        /// Nombre d'objet max caché
        /// </summary>
        private int _nbPointMax;

        /// <summary>
        /// Le nombre de point au départ du jeu 
        /// </summary>
        private int _nbPointDepart;

        #region Property Mouvement

        private KeyTime _keyTimeAnim;
        private Duration _durationAnim;
        private double? _valueBoxUp;
        private double _valueCatLeft;
        private double _valueCatUp;
        private double _valueCatRight;
        
        public KeyTime KeyTimeAnim
        {
            get { return _keyTimeAnim; }
            set { _keyTimeAnim = value; OnPropertyChanged(); }
        }

        public Duration DurationAnim
        {
            get { return _durationAnim; }
            set { _durationAnim = value; OnPropertyChanged(); }
        }
        public double ?ValueBoxUp
        {
            get { return _valueBoxUp; }
            set { _valueBoxUp = value;OnPropertyChanged(); }
        }

        public double ValueCatLeft
        {
            get { return _valueCatLeft; }
            set { _valueCatLeft = value;OnPropertyChanged(); }
        }

        public double ValueCatUp
        {
            get { return _valueCatUp; }
            set { _valueCatUp = value;OnPropertyChanged(); }
        }

        public double ValueCatRight 
        {
            get { return _valueCatRight; }
            set { _valueCatRight = value;OnPropertyChanged(); }
        }


        #endregion

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="exercice">l'exercice</param>
        public ComptePersonneViewModel(Exercice exercice) : base(exercice)
        {
            _random = new Random();

        }

        public override void StartGame()
        {
            StartChrono();
            _nombreTours = 6;
            _nbPointMax = 10;
            _nombreToursFait = 0;
            CompteurErreurs = 0;
        }

        public async override Task<Resultats> CalculResult()
        {
            StopChrono();
            var score = ((_nombreTours - CompteurErreurs)*100)/_nombreTours;
            return await SaveResult(score);
        }

        /// <summary>
        /// Genere le nombre de point au départ dans la boite
        /// </summary>
        /// <returns>le nombre de point</returns>
        public int GenererPointDepart()
        {
            _nbPointDepart = _random.Next(0,6);
            return _nbPointDepart;
        }
        /// <summary>
        /// Retourne la difficulté
        /// </summary>
        /// <returns>la difficulté</returns>
        public DifficulteEnum GetDifficulteEnum()
        {
            return Difficulte;
        }


        /// <summary>
        /// Genere une suite de va et vient
        /// </summary>
        public void GenererJeu()
        {
            _listeInLeft = new List<int>();
            _listeInUp = new List<int>();
            _listeOut = new List<int>();
            _nbVaVient = 0;
            Resultat = 0;

            
            int nbVaVientToDo;

            if (Difficulte.Equals(DifficulteEnum.FACILE))
            {
                nbVaVientToDo = 5;
                SetSpeed(4300);
            }
            else if (Difficulte.Equals(DifficulteEnum.MOYEN))
            {
                nbVaVientToDo = 7;
                SetSpeed(_nombreToursFait<=5?4300:3000);
            }
            else
            {
                nbVaVientToDo = 10;
                SetSpeed(3000);
            }

            var nbPoint = _nbPointDepart;
            
            for (var i = 0; i < nbVaVientToDo; i++)
            {
                int nbInL;
                //personne entrant à gauche
                do
                {
                    nbInL = _random.Next(0, (_nbPointMax - nbPoint) > 8 ? 8 : _nbPointMax - nbPoint);
                } while ((nbInL + nbPoint) > _nbPointMax);

                nbPoint += nbInL;
                _listeInLeft.Add(nbInL);

                //si le niveau est difficile on ajoute une deuxième entrée
                if (Difficulte.Equals(DifficulteEnum.DIFFICILE))
                {
                    int nbInU;
                    do
                    {
                        nbInU = _random.Next(0, (_nbPointMax - nbPoint) > 8 ? 8 : _nbPointMax - nbPoint);
                    } while ((nbInU + nbPoint) > _nbPointMax);
                    nbPoint += nbInU;
                    _listeInUp.Add(nbInU);
                }
                else
                {
                    _listeInUp.Add(0);
                }

                //personne sortante
                var nbOut = _random.Next(0, nbPoint >= 8 ?8:nbPoint+1);

                nbPoint -= nbOut;
                _listeOut.Add(nbOut);
            }
            Resultat = nbPoint;
        }

        /// <summary>
        /// Spécifie la vitesse de défilement des animations
        /// </summary>
        /// <param name="nbMilisecondes">le nombre de milisecondes de la durée de l'animation</param>
        private void SetSpeed(int nbMilisecondes)
        {
            KeyTimeAnim = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(nbMilisecondes));
            DurationAnim = new Duration(TimeSpan.FromMilliseconds(nbMilisecondes));
        }

        /// <summary>
        /// Donne le mouvement du va et vient à faire
        /// </summary>
        /// <returns>les mouvements a effectuer</returns>
        public Mouvement GetMouvement()
        {
            var m = new Mouvement(_listeInLeft[_nbVaVient],_listeInUp[_nbVaVient],_listeOut[_nbVaVient]);
            _nbVaVient++;
            return m;
        }

        /// <summary>
        /// indique si les vas et vient sont finis
        /// </summary>
        /// <returns>true si les va et vient sont terminées</returns>
        public bool IsVaEtVientFini()
        {
            return _nbVaVient >= _listeOut.Count;
        }



        /// <summary>
        /// Indique si le jeu est terminé ou non
        /// </summary>
        /// <returns>true si fini</returns>
        public bool IsJeuFini()
        {
            return _nombreTours <= _nombreToursFait;
        }

        /// <summary>
        /// Indique si la repone entrée par le joueur est correcte ou non
        /// </summary>
        /// <param name="resultat">le résultat</param>
        /// <returns>true si le résultat est correct</returns>
        public bool IsReponseCorrecte(int resultat)
        {
            var res = Resultat == resultat;
            _nombreToursFait++;

            if (!res)
            {
                CompteurErreurs++;
            }
            return res;
        }
    }
}
