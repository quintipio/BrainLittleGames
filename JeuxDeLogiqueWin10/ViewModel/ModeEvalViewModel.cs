using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JeuxDeLogiqueWin10.Business;
using JeuxDeLogiqueWin10.Context;
using JeuxDeLogiqueWin10.Model;

namespace JeuxDeLogiqueWin10.ViewModel
{
    /// <summary>
    /// ViewModel du mode évaluation
    /// </summary>
    public class ModeEvalViewModel
    {

        /// <summary>
        /// pour le mode évaluation la liste des exercices à jouer
        /// </summary>
        private List<Exercice> _listeExerciceModeEval;


        /// <summary>
        /// Pour connaitre quel est l'emplacement de l'exercice en cours
        /// </summary>
        private int _emplacementListEval;

        /// <summary>
        /// pour la connexion à la base de donnée (user)
        /// </summary>
        private UtilisateurBusiness _utilisateurBusiness;

        /// <summary>
        /// pour la connexion à la base de donnée (scores)
        /// </summary>
        private ScoreBusiness _scoreBusiness;

        /// <summary>
        /// nombre de mini jeu à éxécuter pour le mode évaluation
        /// </summary>
        private const int NbMiniJeu = 3;

        /// <summary>
        /// la liste des résultats de chaque exercice
        /// </summary>
        private List<Resultats> _listeResultat; 

        /// <summary>
        /// Démarre le mode évaluation 
        /// </summary>
        public void StartModeEval()
        {
            _emplacementListEval = -1;
            _listeExerciceModeEval = new List<Exercice>();
            //sélection de 3 jeux aléatoire
            var listIdTotal = ContextAppli.ExercicesList.Select(exercice => exercice.Id).ToList();
            var listEmplacementSelect = new List<int>();
            var random = new Random();

            for (var i = 0; i < NbMiniJeu; i++)
            {
                int emp;
                do
                {
                    emp = random.Next(1, listIdTotal.Count);
                } while (listEmplacementSelect.Contains(emp));
                listEmplacementSelect.Add(emp);
                
            }

            foreach (var emp in listEmplacementSelect)
            {
                foreach (var exercice in ContextAppli.ExercicesList.Where(exercice => exercice.Id == listIdTotal[emp]))
            {
                    _listeExerciceModeEval.Add(exercice);
                }
            }
        }

        /// <summary>
        /// Change l'exercice d'évaluation en cours
        /// </summary>
        /// <returns>l'exercice d'évaluation maintenant en cours</returns>
        public async Task<Exercice> ChangeExerciceEval()
        {
            if (_utilisateurBusiness == null)
            {
                _utilisateurBusiness = new UtilisateurBusiness();
                await _utilisateurBusiness.Initialization;
            }
            _emplacementListEval++;
            if (_emplacementListEval < _listeExerciceModeEval.Count)
            {
                var exerciceEvalEnCours = _listeExerciceModeEval[_emplacementListEval];
                await _utilisateurBusiness.ReinitTuto(exerciceEvalEnCours.Id, ContextAppli.ContextUtilisateur.EnCoursUser.Id);
                return exerciceEvalEnCours;
            }
            return null;
        }

        /// <summary>
        /// Ajoute à la liste des résultats d'évaluation les résultats d'un exercice
        /// </summary>
        /// <param name="resultats">le résultat à ajouter</param>
        public void AjouterResultat(Resultats resultats)
        {
            if (_listeResultat == null)
            {
                _listeResultat = new List<Resultats>();
            }
            _listeResultat.Add(resultats);
        }

        /// <summary>
        /// Calcul une moyenne du score de l'évaluation et la sauvegarde en base
        /// </summary>
        /// <returns>le résultat moyen de l'évaluation</returns>
        public async Task<Resultats> GetResultats()
        {
            //préparation des objets et calcul du résultat final
            var resultatFinal = new Resultats
            {
                Erreurs = 0,
                Exercice = null,
                ScoreExercice = null
            };

            var scoreFinal = new Score
            {
                IdExercice = 0,
                IdUtilisateur = ContextAppli.ContextUtilisateur.EnCoursUser.Id,
                NbSecondes = 0,
                Resultat = 0
            };

            var pointTot = 0;
            foreach (var results in _listeResultat)
            {
                resultatFinal.Erreurs += results.Erreurs;
                scoreFinal.NbSecondes += results.ScoreExercice.NbSecondes;
                pointTot += results.ScoreExercice.Resultat;
            }
            scoreFinal.Resultat = pointTot/NbMiniJeu;
            resultatFinal.ScoreExercice = scoreFinal;

            //sauvegarde en base
            if (_scoreBusiness == null)
            {
                _scoreBusiness = new ScoreBusiness();
                await _scoreBusiness.Initialization;
            }
            await _scoreBusiness.SaveScore(scoreFinal.IdExercice, scoreFinal.Resultat, scoreFinal.NbSecondes);

            return resultatFinal;
        }
    }
}
