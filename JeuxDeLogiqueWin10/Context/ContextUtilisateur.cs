using JeuxDeLogiqueWin10.Enum;
using JeuxDeLogiqueWin10.Model;
using JeuxDeLogiqueWin10.ViewModel;

namespace JeuxDeLogiqueWin10.Context
{
    /// <summary>
    /// Contexte pour l'utilisateur
    /// </summary>
    public class ContextUtilisateur
    {
        /// <summary>
        /// Les infomations de l'utilisateur ouvert
        /// </summary>
        public User EnCoursUser { get; private set; }

        /// <summary>
        /// Mode de jeu en cours
        /// </summary>
        public ModeOuvertureJeuEnum ModeJeu { get; set; }

        /// <summary>
        /// Controleur du mode évaluation
        /// </summary>
        public ModeEvalViewModel ModeEval { get; private set; }


        /// <summary>
        /// Créer un contexte Utilisateur
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public  static ContextUtilisateur GetContexteUtilisateur(User user)
        {
            return new ContextUtilisateur {EnCoursUser = user};
        }

        /// <summary>
        /// Lance le mode évaluation
        /// </summary>
        public void LaunchEval()
        {
            ModeEval = new ModeEvalViewModel();
            ModeEval.StartModeEval();
        }

    }
}
