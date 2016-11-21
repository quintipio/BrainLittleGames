using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JeuxDeLogiqueWin10.Abstract;
using JeuxDeLogiqueWin10.Model;

namespace JeuxDeLogiqueWin10.Business
{
    /// <summary>
    /// Gestion des utilisateurs dans la base de donnée
    /// </summary>
    public class UtilisateurBusiness : AbstractBusiness
    {
        /// <summary>
        /// Retourne la liste des utilisateurs en base
        /// </summary>
        public async Task<List<User>> GetAllUtilisateur()
        {
            return await Bdd.Connection.QueryAsync<User>("SELECT * FROM users");
        }

        /// <summary>
        /// Vérifie si un utilisateur existe déjà ou non
        /// </summary>
        /// <param name="nom">le nom à vérifier</param>
        /// <returns>true si l'utilisateur existe</returns>
        public async Task<bool> CheckUtilisateurExist(string nom)
        {
            var resultat = await Bdd.Connection.Table<User>().Where(x => x.Nom.Equals(nom)).CountAsync();
            return resultat > 0;
        }

        /// <summary>
        /// Ajoute un utilisateur
        /// </summary>
        /// <param name="nom">le nom du nouvel utilisateur</param>
        /// <param name="dateNaissance">la date de naissance du nouvel utilisateur</param>
        /// <returns>l'utilisateur crée</returns>
        public async Task<User> InsertUser(string nom, DateTime dateNaissance)
        {
            var userId = await Bdd.Connection.Table<User>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            var id = 1;
            if (userId != null)
            {
                id = userId.Id+1;
            }
            var user = new User { Id = id, Nom = nom, DateNaissance = dateNaissance };
            await Bdd.AjouterDonnee(user);
            return user;
        }

        /// <summary>
        /// efface les scores puis l'utilisateur
        /// </summary>
        /// <param name="user">l'utilisateur a effacer</param>
        /// <returns>le nombre de ligne affectés</returns>
        public async Task<int> DeleteUser(User user)
        {
            var scoreToDel = await Bdd.Connection.Table<Score>().Where(x => x.IdUtilisateur == user.Id).ToListAsync();
            if (scoreToDel != null && scoreToDel.Count > 0)
            {
                await Bdd.DeleteListeDonnee(scoreToDel);
            }

            var tutoPasseToDel = await Bdd.Connection.Table<TutorielPasse>().Where(x => x.IdUtilisateur == user.Id).ToListAsync();
            if (tutoPasseToDel != null && tutoPasseToDel.Count > 0)
            {
                await Bdd.DeleteListeDonnee(tutoPasseToDel);
            }

            return await Bdd.DeleteDonnee(user);
        }

        /// <summary>
        /// permet de savoir si un utilisatuer à déjà passé un tutoriel sur l'exercice
        /// </summary>
        /// <param name="idExercice">l'id de l'exercice</param>
        /// <param name="idUtilisateur">l'id de l'utilisateur</param>
        /// <returns>true si déjà passé</returns>
        public async Task<bool> IsTutoNécéssaire(int idExercice, int idUtilisateur)
        {
            var resultat = await Bdd.Connection.Table<TutorielPasse>()
                .Where(x => x.IdExercice == idExercice && x.IdUtilisateur == idUtilisateur && x.isPasse).CountAsync();
            return resultat > 0;
        }

        /// <summary>
        /// Permet de remettre à false en base l'état indiquant si un tuto est passé ou non
        /// </summary>
        /// <param name="idExercice">l'id de l'exercice à réinit</param>
        /// <param name="idUtilisateur">l'id de l'utilisateur</param>
        /// <returns>la task</returns>
        public async Task ReinitTuto(int idExercice,int idUtilisateur)
        {
            var tutoPasse = await Bdd.Connection.Table<TutorielPasse>()
                .Where(x => x.IdExercice == idExercice && x.IdUtilisateur == idUtilisateur).ToListAsync();
            if (tutoPasse != null && tutoPasse.Count > 0)
            {
                await Bdd.DeleteListeDonnee(tutoPasse);
            }
        }

        /// <summary>
        /// Inscrit en base qu'un utilisateur a passé un tuto
        /// </summary>
        /// <param name="idExercice">l'id de l'exercice</param>
        /// <param name="idUtilisateur">l'id de l'utilisateur</param>
        /// <returns></returns>
        public async Task SaveTutoPasse(int idExercice, int idUtilisateur)
        {
            var tuto = new TutorielPasse
            {
                isPasse = true,
                IdExercice = idExercice,
                IdUtilisateur = idUtilisateur
            };
            tuto.GenerateId();
            await Bdd.AjouterDonnee(tuto);
        }

    }
}
