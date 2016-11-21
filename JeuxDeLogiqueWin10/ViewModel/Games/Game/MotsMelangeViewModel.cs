using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using JeuxDeLogiqueWin10.Abstract;
using JeuxDeLogiqueWin10.Context;
using JeuxDeLogiqueWin10.Enum;
using JeuxDeLogiqueWin10.Model;
using JeuxDeLogiqueWin10.Strings;
using JeuxDeLogiqueWin10.Utils;

namespace JeuxDeLogiqueWin10.ViewModel.Games.Game
{
    /// <summary>
    /// ViewModel pour le jeu des lettres mélangées
    /// </summary>
    public class MotsMelangeViewModel : AbstractGame
    {
        /// <summary>
        /// Indique le nombre de tours pour finir le jeu
        /// </summary>
        private int _nbTours;

        /// <summary>
        /// compte le nombre de tours joué
        /// </summary>
        private int _compteurTours;

        /// <summary>
        /// la liste des mots le temps du jeu
        /// </summary>
        private List<string> _listeMots;

        /// <summary>
        /// liste des mots pouvant être similaire
        /// </summary>
        private List<string> _listeMotsSimilaire;

        /// <summary>
        /// Générateur aléatoire
        /// </summary>
        private readonly Random _random;

        /// <summary>
        /// le mot choisi aléatoirement à retrouver
        /// </summary>
        private string _motChoisi;

        private string _motMelange;

        /// <summary>
        /// le mot avec les lettres mélangées à afficher
        /// </summary>
        public string MotMelange
        {
            get { return _motMelange; }

            private set
            {
                _motMelange = value;
                OnPropertyChanged();
            }
        }

        private string _motEntree;

        /// <summary>
        /// le mot entré par l'utilisateur
        /// </summary>
        public string MotEntree
        {
            get { return _motEntree; }

            set
            {
                _motEntree = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// compte le nombre d'indice utlisé sur un mot
        /// </summary>
        private int _compteurIndice;

        /// <summary>
        /// compte le nombre d'indice total utilisé pendant la partie
        /// </summary>
        private int _compteurIndiceTotal;

        /// <summary>
        /// L'indice à afficher
        /// </summary>
        private string _indice;

        public string Indice
        {
            get { return _indice; }

            set
            {
                _indice = value;
                OnPropertyChanged();
            }
        }

        private bool _isAideEnable;

        /// <summary>
        /// Pour activer ou non le bouton d'aide
        /// </summary>
        public bool IsAideEnable
        {
            get { return _isAideEnable; }

            set
            {
                _isAideEnable = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="exercice">l'exercice en cours</param>
        public MotsMelangeViewModel(Exercice exercice) : base(exercice)
        {
            _random = new Random();
        }
        

        /// <summary>
        /// Démarrage du jeu
        /// </summary>
        public override void StartGame()
        {
            _compteurTours = 0;
            _compteurIndiceTotal = 0;
            _nbTours = 5;
            CompteurErreurs = 0;
            StartChrono();
        }

        /// <summary>
        /// charge à partir du fichier la liste des mots à utilsier
        /// </summary>
        /// <returns></returns>
        public async Task ChargeListeMots()
        {
            var file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(@"Rsc\mots\listeb" + ListeLangues.GetLangueEnCours().Diminutif + ".txt");
            
            if (Difficulte.Equals(DifficulteEnum.FACILE))
            {
                _listeMots = (await FileIO.ReadLinesAsync(file)).Where(x => x.Length <= 8 && x.Length >= 5).ToList();
            }

            if (Difficulte.Equals(DifficulteEnum.MOYEN))
            {
                _listeMots = (await FileIO.ReadLinesAsync(file)).Where(x => x.Length <= 13 && x.Length >= 5).ToList();
            }

            if (Difficulte.Equals(DifficulteEnum.DIFFICILE))
            {
                _listeMots = (await FileIO.ReadLinesAsync(file)).Where(x => x.Length <= 20 && x.Length >= 10).ToList();
            }
            StartChrono();
        }

        /// <summary>
        /// Choisi un mot à retrouver parmis la liste et mélange les lettres
        /// </summary>
        public void GenereMot()
        {
            MotMelange = "";
            Indice = "";
            _compteurIndice = 0;
            MotEntree = "";
            IsAideEnable = true;
            _motChoisi = _listeMots[_random.Next(_listeMots.Count)];
           var alea = new int[_motChoisi.Length];
            for (var i = 0; i < alea.Length; i++)
            {
                alea[i] = -1;
            }

            //mélange des mots
                for (var i = 0; i < alea.Length; i++)
            {
                int number;
                do
                {
                    number =  _random.Next(0, alea.Length);;
                } while (alea.Contains(number));
                alea[i] = number;
            }
            
            foreach (var t in alea)
            {
                MotMelange += _motChoisi.ElementAt(t);
                Indice += "_";
            }
            MotMelange = MotMelange.ToLower();

            //préparation d'une liste de mots pouvant être similaire
            _listeMotsSimilaire =
                _listeMots.Where(x => x.Length == _motChoisi.Length && StringUtils.MotsContiennentMemeLettres(_motChoisi,x))
                    .Select(x => StringUtils.SupprimerCaracSpeciaux(x).ToLower())
                    .ToList();
        }

        //génère un indice dans le champ indice
        public void GetHelp()
        {
            if (_compteurIndice < 3)
            {
                var nbLettreADemasque = Math.Floor((double)_motChoisi.Length / 4);
                var listePositionLettreMasque = new List<int>();
                var listePositionLettreADemasque = new List<int>();
                var i = 0;
                foreach (var lettre in Indice)
                {
                    if ('_'.Equals(lettre))
                    {
                        listePositionLettreMasque.Add(i);
                    }
                    i++;
                }

                do
                {
                    var pos = listePositionLettreMasque[_random.Next(0, listePositionLettreMasque.Count)];
                    if (!listePositionLettreADemasque.Contains(pos))
                    {
                        listePositionLettreADemasque.Add(pos);
                    }
                } while (listePositionLettreADemasque.Count < nbLettreADemasque);

                foreach (var pos in listePositionLettreADemasque)
                {
                    var ch = _motChoisi.ElementAt(pos);
                    var sb = new StringBuilder(Indice) {[pos] = ch};
                    Indice = sb.ToString();
                }
                _compteurIndice++;
                _compteurIndiceTotal++;
            }

            if (_compteurIndice >= 3)
            {
                IsAideEnable = false;
            }
        }

        /// <summary>
        /// Vérifie si le mot trouvé est bon
        /// </summary>
        /// <returns></returns>
        public bool ControleMot()
        {
            if (_listeMotsSimilaire.Contains(StringUtils.SupprimerCaracSpeciaux(MotEntree.ToLower())))
            {
                return true;
            }
            CompteurErreurs++;
            return false;
        }

        /// <summary>
        /// Vérifie si le jeu est fini
        /// </summary>
        /// <returns>true si le jeu est fini</returns>
        public bool IsJeuFini()
        {
            _compteurTours++;
            if (_compteurTours >= _nbTours)
            {
                StopChrono();
                return true;
            }
            return false;
        }

        /// <summary>
        /// calcul du score
        /// </summary>
        /// <returns></returns>
        public override async Task<Resultats> CalculResult()
        {
            //calcul de l'age pour connaitre la marge de temps à rajouter au temps min
            var age = DateUtils.IntervalleEntreDeuxDatesAnnee(ContextAppli.ContextUtilisateur.EnCoursUser.DateNaissance, DateUtils.GetMaintenant());
            var nbMilisecAge = (age < 30) ? 0 : (((age - 20) / 10) * 5000);

            //calcul de l'interval de temps de résolution du jeu en fonction de l'age et de la diffculté
            var tempsMin = (50000 + nbMilisecAge) *_nbTours;
            var tempsMax = 0;

            if (Difficulte.Equals(DifficulteEnum.FACILE))
            {
                tempsMax = (90000 + nbMilisecAge) * _nbTours;
            }

            if (Difficulte.Equals(DifficulteEnum.MOYEN))
            {
                tempsMax = (100000 + nbMilisecAge) * _nbTours;
            }

            if (Difficulte.Equals(DifficulteEnum.DIFFICILE))
            {
                tempsMax = (120000 + nbMilisecAge) * _nbTours;
            }

            //calcul de la note de temps
            int noteTemps;
            if (TempsPasse <= tempsMin) noteTemps = 100;
            else if (TempsPasse >= tempsMax) noteTemps = 0;
            else
            {
                noteTemps = 100 - ((TempsPasse - tempsMin) / ((tempsMax - tempsMin) / 100));
            }

            //pénalité des indices
            noteTemps = noteTemps - (2*_compteurIndiceTotal);
            noteTemps = noteTemps <= 0 ? 15 : noteTemps;

            //calcul de la note finale et sauvegarde
            return await SaveResult(noteTemps);
        }
    }
}
