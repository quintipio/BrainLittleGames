using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using JeuxDeLogiqueWin10.Abstract;
using JeuxDeLogiqueWin10.Business;
using JeuxDeLogiqueWin10.Context;
using JeuxDeLogiqueWin10.Model;

namespace JeuxDeLogiqueWin10.ViewModel
{

    /// <summary>
    /// Object pour regrouper les listes de son
    /// </summary>
    /// <typeparam name="T">le type des objets listé</typeparam>
    public class GroupInfoList<T> : List<object>
    {
        /// <summary>
        /// le nom du groupe
        /// </summary>
        public object Key { get; set; }

        /// <summary>
        /// la liste des objets
        /// </summary>
        /// <returns>la liste</returns>
        public new IEnumerator<object> GetEnumerator()
        {
            return base.GetEnumerator();
        }
    }


    /// <summary>
    /// Controleur du menu principal
    /// </summary>
    public class MenuViewModel : AbstractViewModel
    {

        /// <summary>
        /// pour la connexionà la base
        /// </summary>
        private UtilisateurBusiness _utilisateurBusiness;

        /// <summary>
        /// la liste des sons à afficher
        /// </summary>
        private CollectionViewSource _listeCollection;

        /// <summary>
        /// Propriété de la liste des sons à afficher
        /// </summary>
        public CollectionViewSource ListeCollection
        {
            get { return _listeCollection; }
            set
            {
                _listeCollection = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        public MenuViewModel()
        {
            Initialization = InitializeAsync();

            ListeCollection = new CollectionViewSource { IsSourceGrouped = true };

            var res = new List<GroupInfoList<Exercice>>();

            foreach (var categorie in ContextAppli.CategoriesList)
            {
                var groupe = new GroupInfoList<Exercice> { Key = categorie.Nom };
                var categorie1 = categorie;
                groupe.AddRange(ContextAppli.ExercicesList.Where(cat => cat.IdCategorie == categorie1.Id));
                if (groupe.Count > 0)
                {
                    res.Add(groupe);
                }
            }
            ListeCollection.Source = res;
        }

        /// <summary>
        /// Initialisation du controleur
        /// </summary>
        /// <returns></returns>
        public sealed async override Task InitializeAsync()
        {
            _utilisateurBusiness = new UtilisateurBusiness();
            await _utilisateurBusiness.Initialization;

           
        }

        /// <summary>
        /// Suppression du compte ouvert
        /// </summary>
        public async Task SupprimerCompte()
        {
            await _utilisateurBusiness.DeleteUser(ContextAppli.ContextUtilisateur.EnCoursUser);
            ContextAppli.ContextUtilisateur = null;
        }
    }
}
