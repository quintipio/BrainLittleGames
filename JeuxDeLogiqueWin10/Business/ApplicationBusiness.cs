using System.Threading.Tasks;
using JeuxDeLogiqueWin10.Abstract;
using JeuxDeLogiqueWin10.Model;

namespace JeuxDeLogiqueWin10.Business
{
    /// <summary>
    /// Classe pour la communication avec la BDD pour l'application
    /// </summary>
    class ApplicationBusiness : AbstractBusiness
    {
        /// <summary>
        /// Active le mode complet du jeu 
        /// </summary>
        public async Task ActiverFullMode()
        {
            var res = await Bdd.Connection.Table<Appli>().Where(x => x.Id == 1).FirstOrDefaultAsync();
            res.IsFull = true;
            await Bdd.UpdateDonnee(res);
        }

        public async Task<bool> GetFullMode()
        {
            var res = await Bdd.Connection.Table<Appli>().Where(x => x.Id == 1).FirstOrDefaultAsync();
            return res != null && res.IsFull;
        }

    }
}
