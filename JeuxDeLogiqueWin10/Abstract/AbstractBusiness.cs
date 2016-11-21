using System.Threading.Tasks;
using JeuxDeLogiqueWin10.Com;
using JeuxDeLogiqueWin10.Interface;

namespace JeuxDeLogiqueWin10.Abstract
{
    /// <summary>
    /// Classe abstraite pour la gestion des données
    /// </summary>
    public abstract class AbstractBusiness : IAsyncInitialization
    {
        /// <summary>
        /// constructeur de la classe abstraite
        /// </summary>
        protected AbstractBusiness()
        {
            Initialization = InitializeAsync();
        }

        /// <summary>
        /// Connexion à la base de donnée
        /// </summary>
        protected ComSqlite Bdd { get; private set; }

        /// <summary>
        /// delegate pour l'init du business
        /// </summary>
        public Task Initialization { get; private set; }

        /// <summary>
        /// Méthode à éxécuter pour initialiser le controleur
        /// </summary>
        /// <returns></returns>
        private async Task InitializeAsync()
        {
            Bdd = await ComSqlite.GetComSqlite();
        }
    }
}
