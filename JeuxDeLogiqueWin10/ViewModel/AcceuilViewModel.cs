using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using JeuxDeLogiqueWin10.Abstract;
using JeuxDeLogiqueWin10.Business;
using JeuxDeLogiqueWin10.Context;
using JeuxDeLogiqueWin10.Model;

namespace JeuxDeLogiqueWin10.ViewModel
{


    /// <summary>
    /// Controleur de la page d'acceuil
    /// </summary>
    public sealed class AcceuilViewModel : AbstractViewModel
    {
        /// <summary>
        /// pour la connexionà la base
        /// </summary>
        private UtilisateurBusiness _utilisateurBusiness;

        /// <summary>
        /// pour débloquer l'appli
        /// </summary>
        private ApplicationBusiness _applicationBusiness;

        /// <summary>
        /// liste des Utilisateurs
        /// </summary>
        private ObservableCollection<User> _listeUsers;

        /// <summary>
        /// Liste des utilisateurs
        /// </summary>
        public ObservableCollection<User> ListeUsers
        {
            get { return _listeUsers; }
            set
            {
                if (_listeUsers != value)
                {
                    _listeUsers = value;
                    OnPropertyChanged();
                }
            }
        }
        


        /// <summary>
        /// Constructeur
        /// </summary>
        public AcceuilViewModel()
        {
            Initialization = InitializeAsync();
        }

        /// <summary>
        /// Initialise appliBusiness utilisateurBusiness et charge les utilisateurs
        /// </summary>
        /// <returns></returns>
        public override async Task InitializeAsync()
        {
            _applicationBusiness = new ApplicationBusiness();
            await _applicationBusiness.Initialization;

            _utilisateurBusiness = new UtilisateurBusiness();
            await _utilisateurBusiness.Initialization;
            
            await ChargerListeUtilisateur();
        }

        /// <summary>
        /// Débloque le mode payant en vérifiant si le nom entré correspond au cheat
        /// </summary>
        /// <param name="nomUser">le nom d'utilisateur entré</param>
        /// <param name="dateNaissance">la date de naissance saisie</param>
        /// <returns>true si activer</returns>
        private async Task<bool> DebloqueAppli(string nomUser,DateTime dateNaissance)
        {
            if (ContextStatic.CheatFullModeName.Equals(nomUser) && DateTime.Equals(new DateTime(dateNaissance.Year, dateNaissance.Month, dateNaissance.Day),ContextStatic.CheatFullModeDateNaissance))
            {
                await _applicationBusiness.ActiverFullMode();
                return true;
            }
            return false;
        }


        /// <summary>
        /// Charge la liste des utilisateurs
        /// </summary>
        private async Task ChargerListeUtilisateur()
        {
            ListeUsers = new ObservableCollection<User>(await _utilisateurBusiness.GetAllUtilisateur());
        }

        /// <summary>
        /// Controle les données et ajoute un utilisateur
        /// </summary>
        /// <param name="nom">le nom du nouvel utilisateur</param>
        /// <param name="dateNaissance">la date de naissance de l'utilisateur</param>
        /// <returns>en cas de problème, la liste des erreurs</returns>
        public async Task<List<string>> AjouterUtilisateur(string nom, DateTime dateNaissance)
        {
            var retour = new List<string>();

            if (await DebloqueAppli(nom, dateNaissance))
            {
                retour.Add(ResourceLoader.GetForCurrentView("Errors").GetString("fullMode"));
            }
            else
            {
                //validate
                if (String.IsNullOrWhiteSpace(nom))
                {
                    retour.Add(ResourceLoader.GetForCurrentView("Errors").GetString("nomVide"));
                }
                else
                {
                    if (nom.Length > 30)
                    {
                        retour.Add(ResourceLoader.GetForCurrentView("Errors").GetString("nomTropLong"));
                    }

                    if (await _utilisateurBusiness.CheckUtilisateurExist(nom))
                    {
                        retour.Add(ResourceLoader.GetForCurrentView("Errors").GetString("nomExiste"));
                    }
                }

                //si aucune erreur, on fait un insert
                if (retour.Count == 0)
                {
                    ListeUsers.Add(await _utilisateurBusiness.InsertUser(nom, dateNaissance));
                }
            }
            return retour;
        }

        /// <summary>
        /// charge l'utilisateur en mémoire
        /// </summary>
        /// <param name="user">l'utilisateur à charger</param>
        /// <returns>tache async</returns>
        public void ChargerUtilisateur(User user)
        {
            ContextAppli.ChargerContextUtilisateur(user);
        }
    }
}
