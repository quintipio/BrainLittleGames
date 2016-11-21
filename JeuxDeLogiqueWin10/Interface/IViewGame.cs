using JeuxDeLogiqueWin10.Abstract;

namespace JeuxDeLogiqueWin10.Interface
{
    /// <summary>
    /// Interface des vues pour les jeux
    /// </summary>
    /// <typeparam name="T">le type du viewmodel associé</typeparam>
    public interface IViewGame<T> where T : AbstractGame
    {
        T ViewModel { get; set; }
    }
}
