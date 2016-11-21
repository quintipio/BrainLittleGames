using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using JeuxDeLogiqueWin10.Abstract;
using JeuxDeLogiqueWin10.Context;
using JeuxDeLogiqueWin10.Enum;
using JeuxDeLogiqueWin10.Model;
using JeuxDeLogiqueWin10.Strings;
using JeuxDeLogiqueWin10.Utils;

namespace JeuxDeLogiqueWin10.ViewModel.Games.Game
{
    /// <summary>
    /// Classe de controleur pour le mini jeu de la mémoire des mots
    /// </summary>
    public class MemoireMotsViewModel : AbstractGame
    {
        /// <summary>
        /// Générateur d'aléatoire
        /// </summary>
        private readonly Random _random;

        /// <summary>
        /// Liste des mots trouvé alétoirement dans la liste
        /// </summary>
        private List<String> _motsAleatoire;

        /// <summary>
        /// Liste de smots préformatter
        /// </summary>
        private readonly List<String> _motsAleatoireFormater;

        /// <summary>
        /// liste des mots trouvés
        /// </summary>
        private readonly List<String> _motsTrouve;

        /// <summary>
        /// le temps restant pour le timer
        /// </summary>
        private int _tempsRestant;

        /// <summary>
        /// Timer pour le compte à rebours
        /// </summary>
        private readonly DispatcherTimer _timer;

        /// <summary>
        /// true pour la partie ou le joueur doit mémoriser les mots, false pour la partie ou le joueur doit les écrire
        /// </summary>
        public bool IsModeMemoireMot;

        /// <summary>
        /// Delegate pour la fin compte à rebours Lecture
        /// </summary>
        public delegate void FinCompteAReboursMemMotLectureHandler();

        /// <summary>
        /// Delegate pour la fin compte à rebours Ecriture
        /// </summary>
        public delegate void FinCompteAReboursMemMotEcritureHandler();

        /// <summary>
        /// Evènement
        /// </summary>
        public event FinCompteAReboursMemMotLectureHandler OnFinLecture;

        /// <summary>
        /// Evènement
        /// </summary>
        public event FinCompteAReboursMemMotEcritureHandler OnFinEcriture;

        /// <summary>
        /// Liste des mots trouvé alétoirement dans la liste
        /// </summary>
        public List<String> MotsAleatoire
        {
	        get
	        {
	            return _motsAleatoire;
	        }
	        set
	        {
	            _motsAleatoire = value;
	            OnPropertyChanged();
	        }
        }

        /// <summary>
        /// Pour afficher le temps restant selon le timer
        /// </summary>
        public int TempsRestant
        {
            get { return _tempsRestant; }
            set
            {
                _tempsRestant = value;
                OnPropertyChanged();
            }
        }



        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="exercice"></param>
        public MemoireMotsViewModel(Exercice exercice) : base(exercice)
        {
            _random = new Random();
            _motsTrouve = new List<string>();
            MotsAleatoire = new List<string>();
            _motsAleatoireFormater = new List<string>();

            IsModeMemoireMot = true;

            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1000) };
            _timer.Tick += timer_Tick;
            TempsRestant = 120;

            CompteurErreurs = 0;
        }

        private void timer_Tick(object sender, object e)
        {
            //Si il s'agit de la fin du compte à rebours en mode mémorisation
            if (TempsRestant <= 0 && IsModeMemoireMot)
            {
                IsModeMemoireMot = false;
                TempsRestant = 140;
                StartChrono();
                OnFinLecture();
            }

            //si il s'agit de la fin du compte a rebours en mode écriture
            if (TempsRestant <= 0 && !IsModeMemoireMot)
            {
                TempsRestant = 0;
                _timer.Stop();
                StopChrono();
                OnFinEcriture();
            }
            //retrait d'une seconde
            TempsRestant--;
        }

        /// <summary>
        /// trouve une liste de mots aléatoire à afficher
        /// </summary>
        public async Task GenererMots()
        {
            //ouverture du fichier et récupération des mots
            var file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(@"Rsc\mots\liste" + ListeLangues.GetLangueEnCours().Diminutif + ".txt");
            
            var listeMots = (await FileIO.ReadLinesAsync(file)).Where(x => x.Length <= 7 && x.Length >= 4).ToList();
            var nbMots = 0;
            if (Difficulte.Equals(DifficulteEnum.FACILE))
            {
                nbMots = 15;
            }
            else if(Difficulte.Equals(DifficulteEnum.MOYEN))
            {
                nbMots = 21;
            }
            else
            {
                nbMots = 30;
            }

            //sélection de 30 aléatoire
            var increment = 0;
            do
            {
                string motTmp;
                do
                {
                    var nb = _random.Next(listeMots.Count);
                    motTmp = listeMots[nb];
                } while (MotsAleatoire.Contains(motTmp));
                MotsAleatoire.Add(motTmp);
                increment++;

            } while (increment < nbMots);

            //pour comparer plus vite les mots formatter on les stocks simplifier
            foreach (var mot in MotsAleatoire)
            {
                _motsAleatoireFormater.Add(StringUtils.SupprimerCaracSpeciaux(mot.ToLower()));
            }
        }

        public DifficulteEnum GetDifficulte()
        {
            return Difficulte;
        }

        /// <summary>
        /// Démarrage du mini jeu
        /// </summary>
        public override void StartGame()
        {
            _timer.Start();
        }

        /// <summary>
        /// Vérifie si la chaine en paramètre se trouve dans la liste des
        /// </summary>
        /// <param name="mot"></param>
        /// <returns></returns>
        public bool IsMotInListe(string mot)
        {
            var trouve = _motsAleatoireFormater.Contains(StringUtils.SupprimerCaracSpeciaux(mot.ToLower()));
            if (trouve)
            {
                _motsTrouve.Add(mot);
            }

            return trouve;
        }

        /// <summary>
        /// Retourne la liste des mots qui n'ont pas été trouvé par le joueur
        /// </summary>
        /// <returns>la liste des mots</returns>
        public IEnumerable<string> GetMotsNonTrouve()
        {
            var listeMotsJoueur = _motsTrouve.Select(StringUtils.SupprimerCaracSpeciaux).ToList();
            return _motsAleatoire.Where(mot => !listeMotsJoueur.Contains(StringUtils.SupprimerCaracSpeciaux(mot))).ToList();
        }

        /// <summary>
        /// Calcul les résultats du mini jeu
        /// </summary>
        /// <returns>le résultat</returns>
        public async override Task<Resultats> CalculResult()
        {
            var age = DateUtils.IntervalleEntreDeuxDatesAnnee(ContextAppli.ContextUtilisateur.EnCoursUser.DateNaissance, DateUtils.GetMaintenant());

            CompteurErreurs = 0;
            var score = (100*_motsTrouve.Count)/MotsAleatoire.Count;
            var scoreAge = ((age - 20)/10); //on rajoute un mot par décénnie
            score += scoreAge;

            score = (score>=100)?100:score;

            CompteurErreurs = MotsAleatoire.Count - _motsTrouve.Count;

            return await SaveResult(score);
        }
    }
}
