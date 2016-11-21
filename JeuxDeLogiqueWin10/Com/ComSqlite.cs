using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using JeuxDeLogiqueWin10.Context;
using JeuxDeLogiqueWin10.Model;
using SQLite;

namespace JeuxDeLogiqueWin10.Com
{
    /// <summary>
    /// Singleton pour la connexion à la base de donnée SQLite (nécéssite le paquet qlite-net)
    /// </summary>
    public class ComSqlite
    {
        /// <summary>
        /// objet du singleton
        /// </summary>
        private static ComSqlite _comSqlite;

        /// <summary>
        /// connexion à la base
        /// </summary>
        public SQLiteAsyncConnection Connection { get; private set; }

        /// <summary>
        /// retourne une instance de l'objet de connexion à la base de donnée
        /// </summary>
        /// <returns>l'instance de connexion</returns>
        public async static Task<ComSqlite> GetComSqlite()
        {
            if (_comSqlite == null)
            {
                _comSqlite = new ComSqlite(Path.Combine(ApplicationData.Current.LocalFolder.Path, ContextStatic.Database));
                await _comSqlite.CreateDb();
            }
            return _comSqlite;
        }

        /// <summary>
        /// Constructeur créant la base si elle n'existe et vérifie si il faut effectuer des mises à jour
        /// </summary>
        /// <param name="nomFichier">le nom du fichier de la base de donnée</param>
        private ComSqlite(string nomFichier)
        {
            Connection = new SQLiteAsyncConnection(nomFichier);
        }

        /// <summary>
        /// vérifi l'existance d'une base de donnée
        /// </summary>
        /// <param name="nomFichier">le chemin du fichier de la base</param>
        /// <returns>true si existant</returns>
        private async static Task<bool> CheckDbExist(string nomFichier)
        {
            try
            {
                await ApplicationData.Current.LocalFolder.GetFileAsync(nomFichier);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Créer la base de donnée
        /// </summary>
        private async Task CreateDb()
        {
            await Connection.CreateTableAsync<Appli>();
            await Connection.CreateTableAsync<User>();
            await Connection.CreateTableAsync<Score>();
            await Connection.CreateTableAsync<TutorielPasse>();

            var resultatAppli = await Connection.Table<Appli>().Where(x => x.Id == 1).CountAsync();
            if (resultatAppli == 0)
            {
                var app = new Appli
                {
                    Id = 1,
                    IsFull = false,
                    Version = ContextStatic.Version
                };
                await Connection.InsertAsync(app);
            }
        }


        /// <summary>
        /// ajoute une donnée en base
        /// </summary>
        /// <typeparam name="T">le type de donnée à ajouter</typeparam>
        /// <param name="data">la donnée</param>
        public async Task AjouterDonnee<T>(T data)
        {
            await Connection.InsertAsync(data);
        }

        /// <summary>
        /// Ajoute une liste de donnée à la base
        /// </summary>
        /// <typeparam name="T">le type de donnée à ajouter</typeparam>
        /// <param name="data">la liste des données</param>
        public async Task AjouterListeDonnee<T>(IEnumerable<T> data)
        {
            await Connection.InsertAllAsync(data);
        }

        /// <summary>
        /// met à jour une donnée
        /// </summary>
        /// <typeparam name="T">le type de donnée</typeparam>
        /// <param name="data">la donnée</param>
        public async Task<int> UpdateDonnee<T>(T data)
        {
            return await Connection.UpdateAsync(data);
        }

        /// <summary>
        /// Met à jour plusieurs données
        /// </summary>
        /// <typeparam name="T">le type de donnée</typeparam>
        /// <param name="data">la liste des données</param>
        public async Task<int> UpdateListeDonnee<T>(IEnumerable<T> data)
        {
            return await Connection.UpdateAllAsync(data);
        }

        /// <summary>
        /// efface une donnée
        /// </summary>
        /// <typeparam name="T">le type de donnée</typeparam>
        /// <param name="data">la donnée</param>
        public async Task<int> DeleteDonnee<T>(T data)
        {
            return await Connection.DeleteAsync(data);
        }

        /// <summary>
        /// Efface une liste de données
        /// </summary>
        /// <typeparam name="T">le type de données à effacer</typeparam>
        /// <param name="data">la liste de données à effacer</param>
        /// <returns>le nombre de ligne effacé</returns>
        public async Task<int> DeleteListeDonnee<T>(IEnumerable<T> data)
        {
            var i = 0;
            foreach (var dataToDelete in data)
            {
                await Connection.DeleteAsync(dataToDelete);
                i++;
            }
            return i;
        }


    }
}
