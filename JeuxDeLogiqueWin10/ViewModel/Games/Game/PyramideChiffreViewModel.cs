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
    /// ViewModel du jeu de tricalcul
    /// </summary>
    public class PyramideChiffreViewModel : AbstractGame
    {
        /// <summary>
        /// Générateur aléatoire
        /// </summary>
        private readonly Random _random;

        /// <summary>
        /// le numéro du tours en cours
        /// </summary>
        private int _toursEnCours;

        /// <summary>
        /// le nombre de tours à faire
        /// </summary>
        private readonly int _nbTours;

        /// <summary>
        /// le resultat attendu du tricalcul en cours
        /// </summary>
        public int _resultatAttendu;

        #region propriété

        /// <summary>
        /// Propriété de la liste du haut des opérations
        /// </summary>
        private readonly List<OperationEnum> _premiereLigneOperation;

        /// <summary>
        /// Propriété de la liste du haut des opérations
        /// </summary>
        private readonly List<OperationEnum> _deuxiemeLigneOperation;

        /// <summary>
        /// Propriété de la liste du haut des opérations
        /// </summary>
        private readonly List<OperationEnum> _troisiemeLigneOperation;

        /// <summary>
        /// Proprité de la liste du haut des chiffres
        /// </summary>
        public readonly List<int> LigneChiffre;
        #endregion

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="exercice">l'exercice en cours</param>
        public PyramideChiffreViewModel(Exercice exercice) : base(exercice)
        {
            _random = new Random();
            LigneChiffre = new List<int>();
            _premiereLigneOperation = new List<OperationEnum>();
            _deuxiemeLigneOperation = new List<OperationEnum>();
            _troisiemeLigneOperation = new List<OperationEnum>();
            _nbTours = 10;
            _toursEnCours = 0;
            _resultatAttendu = 0;
        }

        public override void StartGame()
        {
            StartChrono();
        }

        /// <summary>
        /// Genere la première ligne de chiffre du tricalcul et les opérations de tours les lignes
        /// </summary>
        public void GenererOperation()
        {
            LigneChiffre.Clear();
            _premiereLigneOperation.Clear();
            _deuxiemeLigneOperation.Clear();
            _troisiemeLigneOperation.Clear();

            if (Difficulte.Equals(DifficulteEnum.FACILE))
            {
                for (var i = 0; i < 3; i++)
                {
                    LigneChiffre.Add(_random.Next(1, 11));
                } 
                _deuxiemeLigneOperation.Add(OperationEnum.Addition);
                _deuxiemeLigneOperation.Add(OperationEnum.Addition);
                _troisiemeLigneOperation.Add(OperationEnum.Addition);

                _resultatAttendu = (LigneChiffre[0] + LigneChiffre[1]) + (LigneChiffre[1] + LigneChiffre[2]);
            }
            else if (Difficulte.Equals(DifficulteEnum.MOYEN))
            {
                for (var i = 0; i < 4; i++)
                {
                    LigneChiffre.Add(_random.Next(1, 11));
                }
                _premiereLigneOperation.Add(OperationEnum.Addition);
                _premiereLigneOperation.Add(OperationEnum.Addition);
                _premiereLigneOperation.Add(OperationEnum.Addition);
                _deuxiemeLigneOperation.Add(OperationEnum.Addition);
                _deuxiemeLigneOperation.Add(OperationEnum.Addition);
                _troisiemeLigneOperation.Add(OperationEnum.Addition);

                var resA = LigneChiffre[0] + LigneChiffre[1];
                var resB = LigneChiffre[1] + LigneChiffre[2];
                var resC = LigneChiffre[2] + LigneChiffre[3];

                _resultatAttendu = (resA + resB) + (resB + resC);
            }
            else
            {
                for (var i = 0; i < 4; i++)
                {
                    LigneChiffre.Add(_random.Next(1, 11));
                }
                _premiereLigneOperation.Add(GetSigneOperation());
                _premiereLigneOperation.Add(GetSigneOperation());
                _premiereLigneOperation.Add(GetSigneOperation());
                _deuxiemeLigneOperation.Add(OperationEnum.Addition);
                _deuxiemeLigneOperation.Add(OperationEnum.Addition);
                _troisiemeLigneOperation.Add(OperationEnum.Addition);

                var resA = (_premiereLigneOperation[0].Equals(OperationEnum.Addition)) ? LigneChiffre[0] + LigneChiffre[1] : LigneChiffre[0] * LigneChiffre[1];
                var resB = (_premiereLigneOperation[1].Equals(OperationEnum.Addition)) ? LigneChiffre[1] + LigneChiffre[2] : LigneChiffre[1] * LigneChiffre[2];
                var resC = (_premiereLigneOperation[2].Equals(OperationEnum.Addition)) ? LigneChiffre[2] + LigneChiffre[3] : LigneChiffre[2] * LigneChiffre[3];

                _resultatAttendu = (resA + resB) + (resB + resC);
            }
        }

        /// <summary>
        /// Retourne un signe d'opération (addition ou multiplication) aléatoire
        /// </summary>
        /// <returns>le signe addition ou multiplication</returns>
        private OperationEnum GetSigneOperation()
        {
            var aleatoire = _random.Next(1, 11);
            return aleatoire <= 6 ? OperationEnum.Addition : OperationEnum.Multiplication;
        }

        /// <summary>
        /// Retourne le caractère à afficher en fonction de sa position et de sa ligne
        /// </summary>
        /// <param name="ligne">le numéro de ligne de l'opérateur rechercher</param>
        /// <param name="numero"> la position de l'opérateur recherché</param>
        /// <returns>le signe à afficher</returns>
        public string GetSigne(int ligne, int numero)
        {
            var liste = new List<OperationEnum>();
            switch (ligne)
            {
                case 1:
                    liste = _premiereLigneOperation;
                    break;

                case 2:
                    liste = _deuxiemeLigneOperation;
                    break;

                case 3:
                    liste = _troisiemeLigneOperation;
                    break;
            }

            if (liste.Count > numero)
            {
                var op = liste[numero];

                if (op.Equals(OperationEnum.Addition))
                {
                    return "+";
                }
                if (op.Equals(OperationEnum.Soustraction))
                {
                    return "-";
                }
                if (op.Equals(OperationEnum.Multiplication))
                {
                    return "x";
                }
                return op.Equals(OperationEnum.Division) ? "/" : "";
            }
            return "";
        }

        /// <summary>
        /// Vérifie su le résultat entrée est correct
        /// </summary>
        /// <param name="resultat">le résultat du joueur</param>
        /// <returns>true si correct</returns>
        public bool IsResultatCorrect(int resultat)
        {
            _toursEnCours++;
            return resultat == _resultatAttendu;
        }

        /// <summary>
        /// Permet de savoir si le jeu est terminé ou non
        /// </summary>
        /// <returns>true si le jeu est terminé</returns>
        public bool IsJeuFini()
        {
            return _toursEnCours >= _nbTours;
        }

        /// <summary>
        /// Affiche ou non la première ligne (en partant du haut) de la pyramide. (Seulement pour le niveau diffcile)
        /// </summary>
        /// <returns>true si difficile</returns>
        public bool IsAffichePremiereLigne()
        {
            return !Difficulte.Equals(DifficulteEnum.FACILE);
        }

        public async override Task<Resultats> CalculResult()
        {
            StopChrono();
            //calcul de l'age pour connaitre la marge de temps à rajouter au temps min
            var age = DateUtils.IntervalleEntreDeuxDatesAnnee(ContextAppli.ContextUtilisateur.EnCoursUser.DateNaissance, DateUtils.GetMaintenant());
            var nbMilisecAge = (age < 30) ? 0 : (((age - 20) / 10) * 800);

            //calcul de l'interval de temps de résolution du jeu en fonction de l'age et de la diffculté
            var tempsMin = 0;
            var tempsMax = 0;

            if (Difficulte.Equals(DifficulteEnum.FACILE))
            {
                tempsMax = (25000 + nbMilisecAge + 650) * _nbTours;
                tempsMin = (5000 + nbMilisecAge + 650) * _nbTours;
            }

            if (Difficulte.Equals(DifficulteEnum.MOYEN))
            {
                tempsMax = (23000 + nbMilisecAge + 650) * _nbTours;
                tempsMin = (6000 + nbMilisecAge + 650) * _nbTours;
            }

            if (Difficulte.Equals(DifficulteEnum.DIFFICILE))
            {
                tempsMax = (20000 + nbMilisecAge + 650) * _nbTours;
                tempsMin = (6000 + nbMilisecAge + 650) * _nbTours;
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

            //calcul de la note finale et sauvegarde
            return await SaveResult(noteAvecErreurs);
        }
    }
}
