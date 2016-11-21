using JeuxDeLogiqueWin10.Abstract;

namespace JeuxDeLogiqueWin10.Interface
{
    /// <summary>
    /// Interface des vues normales
    /// </summary>
    /// <typeparam name="T">le type du viewModel associé</typeparam>
    public interface IView<T> where T : AbstractViewModel
    {
        T ViewModel { get; set; }
    }
}
