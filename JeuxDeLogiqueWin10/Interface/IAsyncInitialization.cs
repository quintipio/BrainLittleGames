using System.Threading.Tasks;

namespace JeuxDeLogiqueWin10.Interface
{
    /// <summary>
    /// Interface pour initialiser une méthode asynchrone avec un constructeur
    /// </summary>
    public interface IAsyncInitialization
    {
        /// <summary>
        /// initialization
        /// </summary>
        Task Initialization { get; }
    }
}
