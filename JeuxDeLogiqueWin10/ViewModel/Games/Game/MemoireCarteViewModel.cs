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
    /// Controleur du jeu de mémoire de cartes
    /// </summary>
    public class MemoireCarteViewModel : AbstractGame
    {
        private readonly Random _random;

        /// <summary>
        /// La liste des lettres à trouvées placer de façon aléatoire dans la liste
        /// </summary>
        public List<char> ListeLettre { get; set; }

        /// <summary>
        /// le nombre de réponse attendue
        /// </summary>
        private int NbReponsesAttendue { get; set; }

        /// <summary>
        /// le nombre de réponses entrées par l'utilisateur
        /// </summary>
        private int NbReponsesFaites { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="exercice">l'exerice</param>
        public MemoireCarteViewModel(Exercice exercice) : base(exercice)
        {
            _random = new Random();
        }

        public override void StartGame()
        {
            NbReponsesFaites = 0;
            GenererAlphabet();
            StartChrono();
        }

        public async override Task<Resultats> CalculResult()
        {
            StopChrono();

            //calcul de l'age pour connaitre la marge de temps à rajouter au temps min
            var age = DateUtils.IntervalleEntreDeuxDatesAnnee(ContextAppli.ContextUtilisateur.EnCoursUser.DateNaissance, DateUtils.GetMaintenant());
            var nbMilisecAge = (age < 30) ? 0 : (((age - 20) / 10) * 10000);

            int tempsMin;
            int tempsMax;

            if (Difficulte.Equals(DifficulteEnum.FACILE))
            {
                tempsMin = (45000 + nbMilisecAge);
                tempsMax = (240000 + nbMilisecAge);
            }
            else if (Difficulte.Equals(DifficulteEnum.MOYEN))
            {
                tempsMin = (130000 + nbMilisecAge);
                tempsMax = (400000 + nbMilisecAge);
            }
            else
            {
                tempsMin = (160000 + nbMilisecAge);
                tempsMax = (420000 + nbMilisecAge);
            }

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

        /// <summary>
        /// Indique si le mini jeu est fini ou non
        /// </summary>
        /// <returns>true si oui</returns>
        public bool IsJeuFini()
        {
            return NbReponsesFaites >= NbReponsesAttendue;
        }

        /// <summary>
        /// Indique si les deux choix sont correctes
        /// </summary>
        /// <returns>1 : attente de la deuxième réponse, 2 : choix correct, 3 : choix incorrect</returns>
        public int IsCorrect(string a, string b)
        {
            if (a != null && b != null)
            {
                if (a.Equals(b))
                {
                    NbReponsesFaites++;
                    return 2;
                }
                CompteurErreurs++;
                return 3;
            }
            return 1;
        }

        /// <summary>
        /// retourne la difficulté
        /// </summary>
        /// <returns>la difficulté</returns>
        public DifficulteEnum GetDifficulte()
        {
            return Difficulte;
        }

        /// <summary>
        /// Génère l'emplacement des lettres de l'aplahabet à retrouver
        /// </summary>
        public void GenererAlphabet()
        {
            int nbLettres;
            if (Difficulte.Equals(DifficulteEnum.FACILE))
            {
                nbLettres = 10;
            }
            else if (Difficulte.Equals(DifficulteEnum.MOYEN))
            {
                nbLettres = 20;
            }
            else
            {
                nbLettres = 25;
            }
            ListeLettre = new List<char>(nbLettres*2);
            NbReponsesAttendue = nbLettres;

            //liste des lettres
            var listeA = new List<char>(nbLettres);
            for (var i = 0; i < nbLettres; i++)
            {
                listeA.Add((char)(i + 65));
            }

            //liste des emplacements
            var listeB = new List<int>(nbLettres*2);
            for (var i = 0; i < nbLettres*2; i++)
            {
                listeB.Add(i); 
                ListeLettre.Add(' ');
            }

            do
            {
                //choix aléatoire d'une lettre à placer
                var lettreAleatoire = listeA[_random.Next(0, listeA.Count)];

                //choix aléatoire de deux position
                var positionA = listeB[_random.Next(0, listeB.Count)];
                int positionB;
                int pos;
                do
                {
                    pos = listeB[_random.Next(0, listeB.Count)];
                } while (positionA == pos);
                positionB = pos;
                //insertion de la lettre dans les 2 positions
                ListeLettre[positionA] = lettreAleatoire;
                ListeLettre[positionB] = lettreAleatoire;

                listeA.Remove(lettreAleatoire);
                listeB.Remove(positionA);
                listeB.Remove(positionB);
            } while (listeA.Count != 0);
            
        }

    }
}
