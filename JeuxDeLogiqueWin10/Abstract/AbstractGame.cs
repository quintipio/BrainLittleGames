using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JeuxDeLogiqueWin10.Business;
using JeuxDeLogiqueWin10.Context;
using JeuxDeLogiqueWin10.Enum;
using JeuxDeLogiqueWin10.Model;
using JeuxDeLogiqueWin10.Utils;

namespace JeuxDeLogiqueWin10.Abstract
{
    /// <summary>
    /// Classe abstraite pour gérer les jeux
    /// </summary>
    public abstract class AbstractGame : INotifyPropertyChanged 
    {
        /// <summary>
        /// delegate pour l'init
        /// </summary>
        public Task Initialization { get; private set; }

        /// <summary>
        /// Classe pour enregistrer les scores du jeux
        /// </summary>
        private ScoreBusiness _scoreBusiness;

        /// <summary>
        /// Classe pour enregistrer savoir si un joueur a passé le tuto
        /// </summary>
        private UtilisateurBusiness _utilisateurBusiness;

        /// <summary>
        /// Moment de lancement du mini jeu
        /// </summary>
        private DateTime _dateLancement;

        /// <summary>
        /// l'objet de l'exercice
        /// </summary>
        public Exercice ExerciceEnCours { get; set; }

        /// <summary>
        /// chronomètre
        /// </summary>
        protected int TempsPasse;

        /// <summary>
        /// Compte le nombre d'erreurs du joueur
        /// </summary>
        protected int CompteurErreurs;

        /// <summary>
        /// pour le InotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Difficulté de l'exerice
        /// </summary>
        protected DifficulteEnum Difficulte { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        protected AbstractGame(Exercice exercice)
        {
            ExerciceEnCours = exercice;
            Initialization = InitializeAsync();
            Difficulte = exercice.Difficulte;
        }


        /// <summary>
        /// Méthode à éxécuter pour initialiser le controleur
        /// </summary>
        /// <returns></returns>
        private async Task InitializeAsync()
        {
            _scoreBusiness = new ScoreBusiness();
            await _scoreBusiness.Initialization;

            _utilisateurBusiness = new UtilisateurBusiness();
            await _utilisateurBusiness.Initialization;

            TempsPasse = 0;

        }

        /// <summary>
        /// Démarre le chronomètre
        /// </summary>
        protected void StartChrono()
        {
            _dateLancement = DateUtils.GetMaintenant();
            TempsPasse = 0;
        }

        /// <summary>
        /// Arrete le chronomètre
        /// </summary>
        protected void StopChrono()
        {
            TempsPasse = DateUtils.IntervalleEntreDeuxDatesMs(_dateLancement, DateUtils.GetMaintenant());
            _dateLancement = new DateTime();

        }

        /// <summary>
        /// permet de savoir si un tuto est passé et si ce n'est pas le cas prévient la couche du dessus et passe en état sauvegardé
        /// </summary>
        /// <returns>true si déjà passé</returns>
        public async Task<bool> IsTutoPasse()
        {
            var retour = await _utilisateurBusiness.IsTutoNécéssaire(ExerciceEnCours.Id, ContextAppli.ContextUtilisateur.EnCoursUser.Id);
            if (!retour)
            {
                await _utilisateurBusiness.SaveTutoPasse(ExerciceEnCours.Id, ContextAppli.ContextUtilisateur.EnCoursUser.Id);
            }

            return retour;
        }


        /// <summary>
        /// Sauvegarde le score final
        /// </summary>
        protected async Task<Resultats> SaveResult(int result)
        {
           var score = await _scoreBusiness.SaveScore(ExerciceEnCours.Id,result,TempsPasse);
            return new Resultats
            {
                Exercice = ExerciceEnCours,
                Erreurs = CompteurErreurs,
                ScoreExercice = score,
            };
        }

        /// <summary>
        /// Méthode pour démarrer le jeu
        /// </summary>
        public abstract void StartGame();

        /// <summary>
        /// Méthode pour calculer le score à la fin du mini jeu
        /// </summary>
        public abstract Task<Resultats> CalculResult();
    }
}
