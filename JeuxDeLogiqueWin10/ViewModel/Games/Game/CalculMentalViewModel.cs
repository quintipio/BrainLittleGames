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
    /// Controleur du mini jeu de calculMental
    /// </summary>
    public class CalculMentalViewModel : AbstractGame
    {
        /// <summary>
        /// compte le nombre de calculs effectués
        /// </summary>
        private int _compteurOperations;

        /// <summary>
        /// Définis le nombre d'opération à effectuer pour finir le mini jeu
        /// </summary>
        private readonly int _totalOperations;
        
        /// <summary>
        /// permet de connaitre le résultat attendu de l'opération en cours
        /// </summary>
        private int _resultatAttendu;

        /// <summary>
        /// Point d'arrivé du textblock du milieu en haut
        /// </summary>
        private double? _marginHaut;

        /// <summary>
        /// Point d'arrivé du textblock du bas
        /// </summary>
        private double? _marginCentre;

        /// <summary>
        /// Point d'arrivé du textblock du bas
        /// </summary>
        public double? MarginCentre
        {
            get { return _marginCentre; }
            set
            {
                _marginCentre = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Point d'arrivé du textblock du milieu en haut
        /// </summary>
        public double? MarginHaut
        {
            get { return _marginHaut; }
            set
            {
                _marginHaut = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Générateur aléatoire
        /// </summary>
        private readonly Random _random;

        /// <summary>
        /// Table de multiplication pour la génération des divisions
        /// </summary>
        private static readonly int[,] TableMultiplication = 
        {
            {4,6,8,10,12,14,16,18,20},
            {6,9,12,15,18,21,24,27,30},
            {8,12,16,20,24,28,32,36,40},
            {10,15,20,25,30,35,40,45,50},
            {12,18,24,30,36,42,48,54,60},
            {14,21,28,35,42,49,56,63,70},
            {16,24,32,40,48,56,64,72,80},
            {18,27,36,45,54,63,72,81,90},
            {20,30,40,50,60,70,80,90,100}
        };

        /// <summary>
        /// Constructeur
        /// </summary>
        public CalculMentalViewModel(Exercice exercice,int totalOperation) : base(exercice)
        {
            _random = new Random();
            _totalOperations = totalOperation;
        }

        /// <summary>
        /// Démarre le jeu
        /// </summary>
        public override void StartGame()
        {
            StartChrono();
            _compteurOperations = 0;
            CompteurErreurs = 0;
            _resultatAttendu = 0;
        }

        /// <summary>
        /// Vérifie si un résultat est correct ou non
        /// </summary>
        /// <param name="resultat">le résultat entré par le joueur</param>
        /// <returns>true si ok sinon false</returns>
        public bool VerifierOperation(int resultat)
        {
            if (_resultatAttendu != resultat)
            {
                CompteurErreurs++;
                return false;
            }
            return true;
        }

        /// <summary>
        /// controle si le jeu est fini et si oui, arrete tout
        /// </summary>
        /// <returns>true si terminer</returns>
        public bool IsJeuFini()
        {
            if (_compteurOperations > _totalOperations)
            {
                StopChrono();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Calculera le score du joueur
        /// </summary>
        public async override Task<Resultats> CalculResult()
        {
            //calcul de l'age pour connaitre la marge de temps à rajouter au temps min
            var age = DateUtils.IntervalleEntreDeuxDatesAnnee(ContextAppli.ContextUtilisateur.EnCoursUser.DateNaissance,DateUtils.GetMaintenant());
            var nbMilisecAge = (age < 30) ? 0 : (((age - 20)/10) * 400);

            //calcul de l'interval de temps de résolution du jeu en fonction de l'age et de la diffculté
            var tempsMin = (1800+nbMilisecAge) * _totalOperations;
            var tempsMax = 0;

            if (Difficulte.Equals(DifficulteEnum.FACILE))
            {
                tempsMax = (10000+(nbMilisecAge * 3)) * _totalOperations;
            }

            if (Difficulte.Equals(DifficulteEnum.MOYEN))
            {
                tempsMax = (12000 +( nbMilisecAge * 3)) * _totalOperations;
            }

            if (Difficulte.Equals(DifficulteEnum.DIFFICILE))
            {
                tempsMax = (15000 +( nbMilisecAge * 3)) * _totalOperations;
            }
            
            //calcul de la note de temps
            int noteTemps;
            if (TempsPasse <= tempsMin) noteTemps = 100;
            else if (TempsPasse >= tempsMax) noteTemps = 0;
            else
            {
                noteTemps = 100 - ((TempsPasse - tempsMin)/((tempsMax - tempsMin) / 100));
            }

            //prise en compte des erreurs
            var noteAvecErreurs = ((_totalOperations - CompteurErreurs)*noteTemps)/_totalOperations;

            //calcul de la note finale et sauvegarde
            return await SaveResult(noteAvecErreurs);
        }



        /// <summary>
        /// Genère une opération à calculer en fonction de la difficulté et enregistre le résultat attendu
        /// </summary>
        /// <returns>l'opération à effectuer</returns>
        public string GenererOperation()
        {
            //génération de l'opération
            var operation = GetSigneOperation();

            int premierNombre;
            int deuxiemeNombre;

            //génération du premier nombre
            var valMaxA = 0;
            if (operation.Equals(OperationEnum.Addition))
            {
                if (Difficulte.Equals(DifficulteEnum.FACILE))
                {
                    valMaxA = 13;
                }

                else if (Difficulte.Equals(DifficulteEnum.MOYEN))
                {
                    valMaxA = 31;
                }
                else
                {
                    valMaxA = 101;
                }
            }

            else if (operation.Equals(OperationEnum.Soustraction))
            {
                if (Difficulte.Equals(DifficulteEnum.FACILE))
                {
                    valMaxA = 10;
                }

                else if (Difficulte.Equals(DifficulteEnum.MOYEN))
                {
                    valMaxA = 31;
                }
                else
                {
                    valMaxA = 101;
                }
            }

            else if (operation.Equals(OperationEnum.Multiplication))
            {
                if (Difficulte.Equals(DifficulteEnum.FACILE))
                {
                    valMaxA = 11;
                }

                else if (Difficulte.Equals(DifficulteEnum.MOYEN))
                {
                    valMaxA = 12;
                }
                else
                {
                    valMaxA = 14;
                }
            }

            //cas particulier pour la division (on génère les deux chiffres directement)
            if (operation.Equals(OperationEnum.Division))
            {
                valMaxA = 9;
                deuxiemeNombre = _random.Next(valMaxA);
                premierNombre = TableMultiplication[deuxiemeNombre, _random.Next(valMaxA)];
                deuxiemeNombre += 2;
            }
            else
            {
                premierNombre = _random.Next(valMaxA);

                //si ce n'est pas une division on doit générer le deuxième nombre
                //pour une addition ou une multiplication, ca reste la même valeur que le premier
                if (operation.Equals(OperationEnum.Addition) || operation.Equals(OperationEnum.Multiplication))
                {
                    deuxiemeNombre = _random.Next(valMaxA);
                }
                //si c'est une soustraction, cas particulier
                else
                {
                    deuxiemeNombre = Difficulte.Equals(DifficulteEnum.FACILE) ? _random.Next(valMaxA, valMaxA + premierNombre) : _random.Next(valMaxA);
                }

            }

            //calcul et retour de l'affichage
            _compteurOperations++;
            switch (operation)
            {
                case OperationEnum.Addition:
                    _resultatAttendu = premierNombre + deuxiemeNombre;
                    return premierNombre + " + " + deuxiemeNombre+" = ";

                case OperationEnum.Soustraction:
                    _resultatAttendu = deuxiemeNombre - premierNombre;
                    return deuxiemeNombre + " - " + premierNombre + " = ";

                case OperationEnum.Multiplication:
                    _resultatAttendu = premierNombre * deuxiemeNombre;
                    return premierNombre + " x " + deuxiemeNombre + " = ";

                case OperationEnum.Division:
                    _resultatAttendu = (premierNombre == 0 || deuxiemeNombre == 0) ? 0 : premierNombre / deuxiemeNombre;
                    return premierNombre + " / " + deuxiemeNombre + " = ";
            }

            return null;
        }

        /// <summary>
        /// Génère un signe d'opération pour un calcul
        /// </summary>
        /// <returns>le type d'opération</returns>
        private OperationEnum GetSigneOperation()
        {
            int valMax;
            if (Difficulte.Equals(DifficulteEnum.FACILE))
            {
                valMax = 4;
            }
            else if (Difficulte.Equals(DifficulteEnum.MOYEN))
            {
                valMax = 4;
            }
            else
            {
                valMax = 5;
            }
            int value;
            do
            {
                value = _random.Next(valMax);
            } while (value == 0);

            OperationEnum retour;
            System.Enum.TryParse(value.ToString(), out retour);
            return retour;
        }

    }
}
