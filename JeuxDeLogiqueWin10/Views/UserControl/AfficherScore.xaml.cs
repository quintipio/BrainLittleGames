using System;
using System.Globalization;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using JeuxDeLogiqueWin10.Business;
using JeuxDeLogiqueWin10.Context;
using JeuxDeLogiqueWin10.Enum;
using JeuxDeLogiqueWin10.Model;
using JeuxDeLogiqueWin10.Utils;
using JeuxDeLogiqueWin10.Views.UserPage;

namespace JeuxDeLogiqueWin10.Views.UserControl
{
    

    /// <summary>
    /// Outil pour afficher les scores à la fin d'un mini jeu
    /// </summary>
    public sealed partial class AfficherScore
    {
        /// <summary>
        /// Connexion à la base de donéne pour la récupération des scores
        /// </summary>
        private ScoreBusiness _scoreBusiness;

        /// <summary>
        /// L'exercice pour consulter les stats
        /// </summary>
        private Exercice _exercice;

        /// <summary>
        /// indique si il s'agit d'un mode eval
        /// </summary>
        private bool _isEval;

        
        /// <summary>
        /// Constructeur
        /// </summary>
        public AfficherScore()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Charge les stats à afficher
        /// </summary>
        public async Task ChargerResultats(Resultats resultats)
        {
            _isEval = false;
            if (_scoreBusiness == null)
            {
                _scoreBusiness = new ScoreBusiness();
                await _scoreBusiness.Initialization;
            }
            _exercice = resultats.Exercice;

            //pense à retrouvé le résultat tout juste ajouter, et à vérifier le nouveau record

            //partie résultats
            var listePerso = await _scoreBusiness.GetListeTopScorePerso(resultats.Exercice.Id, ContextAppli.ContextUtilisateur.EnCoursUser.Id);

            if (listePerso.Count >= 1)
            {
                RecordPerso1.Text = listePerso[0].Resultat.ToString();
            }
            if (listePerso.Count >= 2)
            {
                RecordPerso2.Text = listePerso[1].Resultat.ToString();
            }

            if (listePerso.Count >= 3)
            {
                RecordPerso3.Text = listePerso[2].Resultat.ToString();
            }
            var listeGlobal = await _scoreBusiness.GetListeTopScoreGlobal(resultats.Exercice.Id);
            if (listeGlobal.Count >= 1)
            {
                RecordGlob1.Text = listeGlobal[0].Resultat + " - " + await _scoreBusiness.GetNomUtilisateur(listeGlobal[0].IdUtilisateur);  
            }

            if (listeGlobal.Count >= 2)
            {
                RecordGlob2.Text = listeGlobal[1].Resultat + " - " + await _scoreBusiness.GetNomUtilisateur(listeGlobal[1].IdUtilisateur);
            }

            if (listeGlobal.Count >= 3)
            {
                RecordGlob3.Text = listeGlobal[2].Resultat + " - " + await _scoreBusiness.GetNomUtilisateur(listeGlobal[2].IdUtilisateur);
            }

            ScoreTextBlock.Text = resultats.ScoreExercice.Resultat + " / 100";
            TempsTextBlock.Text = DateUtils.ConvertNbMilisecondesString(resultats.ScoreExercice.NbSecondes);
            ErreurTextBlock.Text = resultats.Erreurs.ToString();
            if (listePerso[0].Equals(resultats.ScoreExercice) || listeGlobal[0].Equals(resultats.ScoreExercice)) NewRecordText.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Met en mémoire les résultat d'un exercice d'évaluation ou sinon affiche le résultat final de l'évaluation
        /// </summary>
        /// <param name="resultats">le résultat de l'exercice en cours de l'évaluation</param>
        public async Task ChargerEval(Resultats resultats)
        {
            _isEval = true;
            if (_scoreBusiness == null)
            {
                _scoreBusiness = new ScoreBusiness();
                await _scoreBusiness.Initialization;
            }

            //pense à retrouvé le résultat tout juste ajouter, et à vérifier le nouveau record

            //partie résultats
            var listePerso = await _scoreBusiness.GetListeTopScorePerso(0, ContextAppli.ContextUtilisateur.EnCoursUser.Id);

            if (listePerso.Count >= 1)
            {
                RecordPerso1.Text = Math.Round((double)(listePerso[0].Resultat * 20) / 100, 2).ToString(CultureInfo.InvariantCulture);
            }
            if (listePerso.Count >= 2)
            {
                RecordPerso2.Text = Math.Round((double)(listePerso[1].Resultat * 20) / 100, 2).ToString(CultureInfo.InvariantCulture);
            }

            if (listePerso.Count >= 3)
            {
                RecordPerso3.Text = Math.Round((double)(listePerso[2].Resultat * 20) / 100, 2).ToString(CultureInfo.InvariantCulture);
            }
            var listeGlobal = await _scoreBusiness.GetListeTopScoreGlobal(0);
            if (listeGlobal.Count >= 1)
            {
                RecordGlob1.Text = Math.Round((double)(listeGlobal[0].Resultat * 20) / 100, 2) + " - " + await _scoreBusiness.GetNomUtilisateur(listeGlobal[0].IdUtilisateur);
            }

            if (listeGlobal.Count >= 2)
            {
                RecordGlob2.Text = Math.Round((double)(listeGlobal[1].Resultat * 20) / 100, 2) + " - " + await _scoreBusiness.GetNomUtilisateur(listeGlobal[1].IdUtilisateur);
            }

            if (listeGlobal.Count >= 3)
            {
                RecordGlob3.Text = Math.Round((double)(listeGlobal[2].Resultat * 20) / 100, 2) + " - " + await _scoreBusiness.GetNomUtilisateur(listeGlobal[2].IdUtilisateur);
            }

            ScoreTextBlock.Text = Math.Round((double)(resultats.ScoreExercice.Resultat * 20) / 100, 2) + " / 20";
            TempsTextBlock.Text = DateUtils.ConvertNbMilisecondesString(resultats.ScoreExercice.NbSecondes);
            ErreurTextBlock.Text = resultats.Erreurs.ToString();
            if (listePerso[0].Equals(resultats.ScoreExercice) || listeGlobal[0].Equals(resultats.ScoreExercice)) NewRecordText.Visibility = Visibility.Visible;

        }

        private void EndButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(MenuView));
        }

        private void StatButton_OnClickButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isEval)
            {
                ((Frame)Window.Current.Content).Navigate(typeof(StatView), ModeOuvertureStatsEnum.Evaluation);
            }
            else
            {
            ((Frame)Window.Current.Content).Navigate(typeof(StatView), _exercice);
            }
        }
    }
}
