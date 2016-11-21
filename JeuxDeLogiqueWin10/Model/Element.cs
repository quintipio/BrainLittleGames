namespace JeuxDeLogiqueWin10.Model
{
    /// <summary>
    /// Objet générique pour associer un nom à une valeur
    /// </summary>
    public class Element<T,TF>
    {
        public T Nom { get; set; }

        public TF Valeur { get; set; }



        public Element(T nom, TF valeur)
        {
            Nom = nom;
            Valeur = valeur;
        }

        public override string ToString()
        {
            return Nom.ToString();
        }
    }
}
