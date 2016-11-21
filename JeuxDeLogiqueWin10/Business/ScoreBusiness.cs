using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JeuxDeLogiqueWin10.Abstract;
using JeuxDeLogiqueWin10.Context;
using JeuxDeLogiqueWin10.Model;
using JeuxDeLogiqueWin10.Utils;

namespace JeuxDeLogiqueWin10.Business
{
    /// <summary>
    /// Classe gérant les résultats dans la base de donnée
    /// </summary>
    public class ScoreBusiness : AbstractBusiness
    {
        /// <summary>
        /// Sauvegarde un score à la fin d'un jeu
        /// </summary>
        /// <param name="idExercice">l'id de l'exercice ayant fait le score</param>
        /// <param name="score">le score à sauvegardé</param>
        /// <param name="nbSecondes">temps de résolution de l'exerice</param>
        public async Task<Score> SaveScore(int idExercice,int score, int nbSecondes)
        {
            var scoreToSave = new Score
            {
                Date = DateUtils.GetMaintenant(),
                IdExercice = idExercice,
                Resultat = score,
                IdUtilisateur = ContextAppli.ContextUtilisateur.EnCoursUser.Id,
                NbSecondes = nbSecondes
            };
            scoreToSave.GenerateId();
            await Bdd.AjouterDonnee(scoreToSave);
            return scoreToSave;
        }

        /// <summary>
        /// Retourne les résultats d'un exercice
        /// </summary>
        /// <param name="idUtilisateur">l'id de l'utilisateur pour les stats</param>
        /// <param name="idexercice">l'id de l'exercice pour les stats</param>
        /// <returns>la liste des scores de l'exercice</returns>
        public async Task<List<Score>> GetStatsExerciceUtilisateur(int idUtilisateur, int idexercice)
        {
            return 
                await Bdd.Connection.Table<Score>()
                    .Where(x => x.IdUtilisateur == idUtilisateur && x.IdExercice == idexercice)
                    .ToListAsync();
        }

        /// <summary>
        /// Retourne la liste des scores d'une categorie
        /// </summary>
        /// <param name="idUtilisateur">l'id de l'utilisateur</param>
        /// <param name="idCategorie">l'id de la catégorie</param>
        /// <returns>la liste des scores</returns>
        public async Task<List<Score>> GetStatsCategorieUtilisateur(int idUtilisateur, int idCategorie)
        {
            var listExercice = ContextAppli.ExercicesList.Where(x => x.IdCategorie == idCategorie);
            var chaineCaracListeExercice = "";
            var isPasse = false;
            foreach (var exercice in listExercice)
            {
                if (isPasse)
                {
                    chaineCaracListeExercice += " , ";
                }
                isPasse = true;
                chaineCaracListeExercice += exercice.Id.ToString();
            }
            return await Bdd.Connection.QueryAsync<Score>("SELECT * FROM score WHERE idExercice IN (" + chaineCaracListeExercice + ") AND idUtilisateur = "+idUtilisateur);
        }

        /// <summary>
        /// retourne les 3 premiers résultats du joueur sur un exercice
        /// </summary>
        /// <param name="idExercice">id de l'exercice</param>
        /// <param name="idUtilisateur">id de l'utilisateur</param>
        /// <returns>la liste des scores</returns>
        public async Task<List<Score>> GetListeTopScorePerso(int idExercice, int idUtilisateur)
        {
            return
                await
                    Bdd.Connection.Table<Score>()
                        .Where(x => x.IdExercice == idExercice && x.IdUtilisateur == idUtilisateur)
                        .OrderByDescending(i =>i.Resultat).Take(3).ToListAsync();
        }

        /// <summary>
        /// retourne les 3 premiers résultats du joueur sur un exercice
        /// </summary>
        /// <param name="idExercice">id de l'exercice</param>
        /// <returns>la liste des scores</returns>
        public async Task<List<Score>> GetListeTopScoreGlobal(int idExercice)
        {
            return
                await
                    Bdd.Connection.Table<Score>()
                        .Where(x => x.IdExercice == idExercice)
                        .OrderByDescending(i => i.Resultat).Take(3).ToListAsync();
        }

        /// <summary>
        /// A partir de l'id retourne le nom de l'utilisateur (oui je voulais pas le mettre dans l'utilisateur business vu que j'ai pas envie d'itinitialiser tout un objet juste pour une méthode)
        /// </summary>
        /// <param name="idUtilisateur">l'id de l'utilisateur</param>
        /// <returns>le nom</returns>
        public async Task<String> GetNomUtilisateur(int idUtilisateur)
        {
            return (await Bdd.Connection.Table<User>().Where(x => x.Id == idUtilisateur).FirstOrDefaultAsync()).Nom;

        }

    }
}
