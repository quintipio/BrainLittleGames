using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JeuxDeLogiqueWin10.Abstract;
using JeuxDeLogiqueWin10.Business;
using JeuxDeLogiqueWin10.Context;
using JeuxDeLogiqueWin10.Model;

namespace JeuxDeLogiqueWin10.ViewModel
{
    /// <summary>
    /// juste une petite classe pour l'affichage des stats avec des double
    /// </summary>
    public class StructGraphEval
    {
        public double Value { get; set; }
        public DateTime Date { get; set; }

        public StructGraphEval(double value, DateTime date)
        {
            Value = value;
            Date = date;
        }
    }


   /// <summary>
    /// Controleur de la page des statistiques
    /// </summary>
    public sealed class ConsultStatViewModel : AbstractViewModel
    {
        /// <summary>
        /// Objet de gestion des scores dans la base de donnée
        /// </summary>
        private ScoreBusiness _scoreBusiness;

        /// <summary>
        /// Constructeur
        /// </summary>
        public ConsultStatViewModel()
        {
            Initialization = InitializeAsync();
        }

        /// <summary>
        /// Méthode d'initialisation
        /// </summary>
        /// <returns>la task</returns>
        public override async Task InitializeAsync()
        {
           _scoreBusiness = new ScoreBusiness();
            await _scoreBusiness.Initialization;
        }

        /// <summary>
        /// Charge les scores du joueur de l'exercice en paramètres
        /// </summary>
        /// <param name="exercice">l'exercice</param>
        /// <returns>les résultats</returns>
        public async Task<List<Score>> ChargerScoreExercice(Exercice exercice)
        {
            return await _scoreBusiness.GetStatsExerciceUtilisateur(ContextAppli.ContextUtilisateur.EnCoursUser.Id, exercice.Id);
        }

        /// <summary>
        /// Charge les scores des évaluations du joueur
        /// </summary>
        /// <returns>les scores</returns>
        public async Task<List<StructGraphEval>> ChargerScoreEval()
        {
            var res = await _scoreBusiness.GetStatsExerciceUtilisateur(ContextAppli.ContextUtilisateur.EnCoursUser.Id, 0);
            return res.Select(score => new StructGraphEval(Math.Round((double)(score.Resultat * 20) / 100, 2), score.Date)).ToList();
        }

        /// <summary>
        /// Charge les scores de la catégorie sélectionnée pour un joueur
        /// </summary>
        /// <param name="categorie">la catégorie</param>
        /// <returns>les scores</returns>
        public async Task<List<Score>> ChargerScoreCategorie(Categorie categorie)
        {
            return await _scoreBusiness.GetStatsCategorieUtilisateur(ContextAppli.ContextUtilisateur.EnCoursUser.Id,
                        categorie.Id);
        }
    }
}
