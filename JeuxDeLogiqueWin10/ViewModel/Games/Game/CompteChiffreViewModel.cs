using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using JeuxDeLogiqueWin10.Abstract;
using JeuxDeLogiqueWin10.Context;
using JeuxDeLogiqueWin10.Enum;
using JeuxDeLogiqueWin10.Model;
using JeuxDeLogiqueWin10.Utils;

namespace JeuxDeLogiqueWin10.ViewModel.Games.Game
{
    


    /// <summary>
    /// ViewModel du jeu de compteur de chiffres
    /// </summary>
    public class CompteChiffreViewModel : AbstractGame
    {
        /// <summary>
        /// la liste des chiffres affichées avec tout les paramètres
        /// </summary>
        private List<Chiffre> _listeChiffre;

        /// <summary>
        /// le résultat attendue
        /// </summary>
        private int _resultat;

        /// <summary>
        /// le nombre de torus total à jouer
        /// </summary>
        private readonly int _nombreTours;

        /// <summary>
        /// Le nombre de tours fait par le joueur
        /// </summary>
        private int _nombreToursFait;

        /// <summary>
        /// la question à afficher
        /// </summary>
        public string Question { get; private set; }


        private readonly Random _random;

        /// <summary>
        /// Contructeur
        /// </summary>
        /// <param name="exercice">l'exercice en cours</param>
        public CompteChiffreViewModel(Exercice exercice) : base(exercice)
        {
            StartChrono();
            CompteurErreurs = 0;
            _nombreTours = 15;
            _nombreToursFait = 0;
            _random = new Random();
            _listeChiffre = new List<Chiffre>();
        }

        public override void StartGame()
        {
            StartChrono();
        }

        /// <summary>
        /// Genere une liste d'objet 'chiffre' à afficher
        /// </summary>
        /// <param name="maxWidth">Taille en hauteur</param>
        /// <param name="maxHeight">taille en largeur</param>
        /// <returns>la liste des chiffres</returns>
        public IEnumerable<Chiffre> GenererListeChiffre(int maxWidth, int maxHeight)
        {
            _listeChiffre = new List<Chiffre>();
            int nbChiffreMaxGenerer;

            //pour définir le nombre de chiffre à générer
            if (Difficulte.Equals(DifficulteEnum.FACILE))
            {
                if (_nombreToursFait <= 4)
                {
                    nbChiffreMaxGenerer = 6;
                }
                else if (_nombreToursFait <= 10)
                {
                    nbChiffreMaxGenerer = 10;
                }
                else
                {
                    nbChiffreMaxGenerer = 13;
                }
            }
            else if (Difficulte.Equals(DifficulteEnum.MOYEN))
            {
                if (_nombreToursFait <= 4)
                {
                    nbChiffreMaxGenerer = 8;
                }
                else if (_nombreToursFait <= 10)
                {
                    nbChiffreMaxGenerer = 14;
                }
                else
                {
                    nbChiffreMaxGenerer = 18;
                }
            }
            else
            {
                if (_nombreToursFait <= 4)
                {
                    nbChiffreMaxGenerer = 12;
                }
                else if (_nombreToursFait <= 8)
                {
                    nbChiffreMaxGenerer = 18;
                }
                else
                {
                    nbChiffreMaxGenerer = 23;
                }
            }

            int maxChoix = 0;

            //génération des chiffres
            for (var i = 0; i < nbChiffreMaxGenerer; i++)
            {
                var chiffre = _random.Next(2, 7);

                var color = new Color();
                switch (_random.Next(0, 3))
                {
                    case 0://noir
                        color = Color.FromArgb(255, 0, 0, 0);
                        break;

                    case 1: //bleu
                        color = Color.FromArgb(255, 0, 0, 180);
                        break;

                    case 2: //rouge
                        color = Color.FromArgb(255, 255, 0, 0);
                        break;
                }

                //animation si on est à plus de la moitié
                var animation = MouvementEnum.Aucun;
                if (i >= _nombreTours / 2)
                {
                    var aleatoireAnim = _random.Next(0, 9);

                    switch (aleatoireAnim)
                    {
                        case 8:
                            animation = MouvementEnum.Agrandisssement;
                            break;

                        case 7:
                            animation = MouvementEnum.Agrandisssement;
                            break;

                        case 6:
                            animation = MouvementEnum.Rotation;
                            break;

                        case 5:
                            animation = MouvementEnum.TranslationVerticale;
                            break;

                        case 4:
                            animation = MouvementEnum.TranslationHorizontale;
                            break;

                        case 3:
                            animation = MouvementEnum.Rotation;
                            break;
                    }
                }

                var chiffreTmp = new Chiffre(i, chiffre, color, animation);
                var testEmplacement = false;
                var marginTopTmp = 0;
                var marginLeftTmp = 0;
                do
                {
                    marginTopTmp = _random.Next(10, (animation.Equals(MouvementEnum.TranslationHorizontale) || animation.Equals(MouvementEnum.TranslationVerticale)) ? maxHeight - 115 : maxHeight - 10);
                    marginLeftTmp = _random.Next(10, (animation.Equals(MouvementEnum.TranslationHorizontale) || animation.Equals(MouvementEnum.TranslationVerticale)) ? maxWidth - 115 : maxWidth - 10);

                    if (_listeChiffre.Any(c => (c.MarginLeft - 40 > marginLeftTmp && c.MarginLeft + 40 < marginLeftTmp) &&
                                                (c.MarginTop - 40 > marginTopTmp && c.MarginTop + 40 < marginTopTmp)))
                    {
                        testEmplacement = true;
                    }
                } while (testEmplacement);
                chiffreTmp.MarginTop = marginTopTmp;
                chiffreTmp.MarginLeft = marginLeftTmp;

                _listeChiffre.Add(chiffreTmp);
                maxChoix = (i <= (_nombreTours / 2)) ? 3 : 4;
                if (maxChoix == 4 && _listeChiffre.All(x => x.Mouvement == MouvementEnum.Aucun))
                {
                    maxChoix = 3;
                }
            }


            //génération de la question
            var typeQuestion = TypeQuestionEnum.TypeChiffre;
            switch (_random.Next(1, maxChoix))
            {
                case 1:
                    typeQuestion = TypeQuestionEnum.TypeChiffre;
                    break;

                case 2:
                    typeQuestion = TypeQuestionEnum.TypeCouleur;
                    break;

                case 3:
                    typeQuestion = TypeQuestionEnum.TypeMouvement;
                    break;
            }

            Question = ResourceLoader.GetForCurrentView().GetString("combien") + " ";
            if (typeQuestion.Equals(TypeQuestionEnum.TypeChiffre))
            {
                Question += ResourceLoader.GetForCurrentView().GetString("de") + " ";
                var liste = _listeChiffre.Select(x => x.Nombre).Distinct().ToList();
                var empAl = _random.Next(0, liste.Count());
                Question += liste[empAl].ToString();
                _resultat = _listeChiffre.Count(x => x.Nombre == liste[empAl]);
            }
            else if (typeQuestion.Equals(TypeQuestionEnum.TypeCouleur))
            {
                Question += ResourceLoader.GetForCurrentView().GetString("de") + " ";
                var liste = _listeChiffre.Select(x => x.Couleur).Distinct().ToList();
                var empAl = _random.Next(0, liste.Count());
                var col = liste[empAl];
                _resultat = _listeChiffre.Count(x => x.Couleur.Equals(col));

                if (col.Equals(Color.FromArgb(255, 0, 0, 0)))
                {
                    Question += ResourceLoader.GetForCurrentView().GetString("Noir");
                }
                else if (col.Equals(Color.FromArgb(255, 0, 0, 180)))
                {
                    Question += ResourceLoader.GetForCurrentView().GetString("Bleu");
                }
                else
                {
                    Question += ResourceLoader.GetForCurrentView().GetString("Rouge");
                }
            }
            else if (typeQuestion.Equals(TypeQuestionEnum.TypeMouvement))
            {
                var liste = _listeChiffre.Where(x => x.Mouvement != MouvementEnum.Aucun).Select(x => x.Mouvement).Distinct().ToList();
                var empAl = _random.Next(0, liste.Count());
                switch (liste[empAl])
                {
                    case MouvementEnum.Rotation:
                        Question += ResourceLoader.GetForCurrentView().GetString("Tournent");
                        _resultat = _listeChiffre.Count(x => x.Mouvement == MouvementEnum.Rotation);
                        break;

                    case MouvementEnum.Agrandisssement:
                        Question += ResourceLoader.GetForCurrentView().GetString("ChangeTaille");
                        _resultat = _listeChiffre.Count(x => x.Mouvement == MouvementEnum.Agrandisssement);
                        break;

                    case MouvementEnum.TranslationVerticale:
                        Question += ResourceLoader.GetForCurrentView().GetString("Bougent");
                        _resultat = _listeChiffre.Count(x => x.Mouvement == MouvementEnum.TranslationHorizontale || x.Mouvement == MouvementEnum.TranslationVerticale);
                        break;

                    case MouvementEnum.TranslationHorizontale:
                        Question += ResourceLoader.GetForCurrentView().GetString("Bougent");
                        _resultat = _listeChiffre.Count(x => x.Mouvement == MouvementEnum.TranslationHorizontale || x.Mouvement == MouvementEnum.TranslationVerticale);
                        break;
                }
            }
            Question += " ?";
            return _listeChiffre;

        }

        /// <summary>
        /// indique si le résultat entré par l'utilisateur est correct ou non
        /// </summary>
        /// <param name="resultat">le résultat de l'utilisateur</param>
        /// <returns>true si le résultat de l'utilisateur correspond à la valeur attendue</returns>
        public bool IsReponseCorrecte(int resultat)
        {
            var ret = resultat == _resultat;
            if (!ret) CompteurErreurs++;
            _nombreToursFait++;
            return ret;
        }

        /// <summary>
        /// indique si le jeu se termine ou non
        /// </summary>
        /// <returns>true si fini</returns>
        public bool IsJeuFini()
        {
            return _nombreTours <= _nombreToursFait;
        }

        public async override Task<Resultats> CalculResult()
        {
            StopChrono();
            //calcul de l'age pour connaitre la marge de temps à rajouter au temps min
            var age = DateUtils.IntervalleEntreDeuxDatesAnnee(ContextAppli.ContextUtilisateur.EnCoursUser.DateNaissance, DateUtils.GetMaintenant());
            var nbMilisecAge = (age < 30) ? 0 : (((age - 20) / 10) * 400);

            //calcul de l'interval de temps de résolution du jeu en fonction de l'age et de la diffculté
            var tempsMin = (1700 + nbMilisecAge) * _nombreTours;
            var tempsMax = 0;

            if (Difficulte.Equals(DifficulteEnum.FACILE))
            {
                tempsMax = (10000 + nbMilisecAge) * _nombreTours;
            }

            if (Difficulte.Equals(DifficulteEnum.MOYEN))
            {
                tempsMax = (12000 + nbMilisecAge) * _nombreTours;
            }

            if (Difficulte.Equals(DifficulteEnum.DIFFICILE))
            {
                tempsMax = (15000 + nbMilisecAge) * _nombreTours;
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
            var noteAvecErreurs = ((_nombreTours - CompteurErreurs) * noteTemps) / _nombreTours;

            //calcul de la note finale et sauvegarde
            return await SaveResult(noteAvecErreurs);
        }
    }
}
