using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;
using JeuxDeLogiqueWin10.Abstract;
using JeuxDeLogiqueWin10.Context;
using JeuxDeLogiqueWin10.Enum;
using JeuxDeLogiqueWin10.Model;
using JeuxDeLogiqueWin10.Utils;

namespace JeuxDeLogiqueWin10.ViewModel.Games.Game
{
    /// <summary>
    /// Un enum des couleurs possibles sur le jeu du bon objet dans la bonne couleur
    /// </summary>
    public sealed class CouleursEnum
    {

        private readonly Color _color;
        private readonly int _value;

        public static readonly CouleursEnum ROUGE = new CouleursEnum(1, Color.FromArgb(255, 195, 45, 45));
        public static readonly CouleursEnum VERT = new CouleursEnum(2, Color.FromArgb(255, 45, 195, 45));
        public static readonly CouleursEnum BLEU = new CouleursEnum(3, Color.FromArgb(255, 45, 45, 195));
        public static readonly CouleursEnum GRIS = new CouleursEnum(4, Color.FromArgb(255, 137, 137, 137));
        public static readonly CouleursEnum NOIR = new CouleursEnum(5, Color.FromArgb(255, 0, 0, 0));

        private CouleursEnum(int value, Color color)
        {
            _color = color;
            _value = value;
        }

        public Color GetColor()
        {
            return _color;
        }

        public static Color GetColor(int value)
        {
            switch (value)
            {
                case 1:
                    return ROUGE.GetColor();

                case 2:
                    return VERT.GetColor();

               case 3:
                    return BLEU.GetColor();

                case 4:
                    return GRIS.GetColor();

                case 5:
                    return NOIR.GetColor();

                default:
                    return Color.FromArgb(0,255,255,255);

            }
        }
    }

    public sealed class PolygonEnum
    {
        private int _value;
        private readonly PointCollection _pointCollection;
        
        public static readonly PolygonEnum CARRE = GeneratePolygon(1,200);
        public static readonly PolygonEnum TRIANGLE = GeneratePolygon(2,200);
        public static readonly PolygonEnum MAISON = GeneratePolygon(3,200);
        public static readonly PolygonEnum PENTAGONE = GeneratePolygon(4,200);
        public static readonly PolygonEnum LOSANGE = GeneratePolygon(5,200);

        private PolygonEnum(int value, PointCollection pointCollection)
        {
            _pointCollection = pointCollection;
            _value = value;
        }

        public PointCollection GetPoints()
        {
            return _pointCollection;
        }

        public int GetValue()
        {
            return _value;
        }

        public static int GetValue(PointCollection points)
        {
            if (CARRE.GetPoints().Equals(points))
            {
                return CARRE.GetValue();
            }

            if (TRIANGLE.GetPoints().Equals(points))
            {
                return TRIANGLE.GetValue();
            }

            if (MAISON.GetPoints().Equals(points))
            {
                return MAISON.GetValue();
            }

            if (LOSANGE.GetPoints().Equals(points))
            {
                return LOSANGE.GetValue();
            }

            if (PENTAGONE.GetPoints().Equals(points))
            {
                return PENTAGONE.GetValue();
            }
            return 0;
        }

        public static PolygonEnum GeneratePolygon(int choix, double maxSize)
        {
            switch (choix)
            {//Size = 'p.Size' threw an exception of type 'System.InvalidOperationException'
                //Native View = To inspect the native object, enable native code debugging.
                case 1:
                    Point a = new Point(0, 0);
                    Point b = new Point(0, maxSize);
                    Point c = new Point(maxSize,maxSize);
                    Point d = new Point(maxSize, 0);
                    PointCollection p = new PointCollection();
                    p.Add(a);
                    p.Add(b);
                    p.Add(c);
                    p.Add(d);
                    PolygonEnum l = new PolygonEnum(1,p); 
                    return new PolygonEnum(1,
                        new PointCollection
                        {
                            new Point(0, 0),
                            new Point(0, maxSize),
                            new Point(maxSize, maxSize),
                            new Point(maxSize, 0)
                        });

                case 2:
                    return new PolygonEnum(2,
                        new PointCollection
                        {
                            new Point(maxSize/2, 0),
                            new Point(maxSize, maxSize),
                            new Point(0, maxSize)
                        });

                case 3:
                    return new PolygonEnum(3,
                        new PointCollection
                        {
                            new Point(maxSize/2, 0),
                            new Point(maxSize, maxSize/2),
                            new Point(maxSize, maxSize),
                            new Point(0, maxSize),
                            new Point(0, maxSize/2)
                        });

                case 4:
                    return new PolygonEnum(4,
                        new PointCollection
                        {
                            new Point(maxSize/2, 0),
                            new Point(maxSize, maxSize/2.66667),
                            new Point(maxSize/1.25, maxSize),
                            new Point(maxSize/5, maxSize),
                            new Point(0, maxSize/2.66667)
                        });

                case 5:
                    return new PolygonEnum(5,
                        new PointCollection
                        {
                            new Point(maxSize/2, 0),
                            new Point( maxSize-(maxSize/4),maxSize/2),
                            new Point(maxSize/2, maxSize),
                            new Point(maxSize/4, maxSize/2)
                        });
                default:
                    return null;
            }
        }

        public static PointCollection GetPoints(int value)
        {
            switch (value)
            {
                case 1:
                    return CARRE.GetPoints();

                case 2:
                    return TRIANGLE.GetPoints();

                case 3:
                    return MAISON.GetPoints();

                case 4:
                    return PENTAGONE.GetPoints();

                case 5:
                    return LOSANGE.GetPoints();

                default:
                    return null;
            }
        }
    }

    /// <summary>
    /// contient les données d'une forme
    /// </summary>
    public struct Forme
    {
        public int Id;
        public PointCollection Dessin;
        public Color Couleur;

        /// <summary>
        /// Contructeur
        /// </summary>
        /// <param name="id">l'id</param>
        /// <param name="dessin">les points de coordonnées pour dessiner la forme</param>
        /// <param name="couleur">la couleur</param>
        public Forme(int id, PointCollection dessin, Color couleur)
        {
            Id = id;
            Dessin = dessin;
            Couleur = couleur;
        }
    }


    /// <summary>
    /// ViewModel du jeu de trouver l'objet et sa couleur
    /// </summary>
    public class TrouveObjetCouleurViewModel : AbstractGame
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
        /// permet de connaitre quels sont les figures en cours
        /// </summary>
        private readonly List<Forme> _puzzleEnCours;

        /// <summary>
        /// la liste des formes par défaut
        /// </summary>
        public List<Forme> ListeFormeDefaut { get; }

        /// <summary>
        /// Les deux id des formes princpales actuellement affichées
        /// </summary>
        public List<int> ListeIdFormeEnCours { get; }

        /// <summary>
        /// Contient la forme étant la bonne réponse au puzzle
        /// </summary>
        private Forme _solution;

        /// <summary>
        /// Contructeur
        /// </summary>
        /// <param name="exercice">l'exercice en cours</param>
        public TrouveObjetCouleurViewModel(Exercice exercice) : base(exercice)
        {
            ListeFormeDefaut = new List<Forme>
            {
                new Forme(1, PolygonEnum.CARRE.GetPoints(), CouleursEnum.BLEU.GetColor()),
                new Forme(2, PolygonEnum.TRIANGLE.GetPoints(), CouleursEnum.GRIS.GetColor()),
                new Forme(3, PolygonEnum.MAISON.GetPoints(), CouleursEnum.ROUGE.GetColor()),
                new Forme(4, PolygonEnum.PENTAGONE.GetPoints(), CouleursEnum.VERT.GetColor()),
                new Forme(5, PolygonEnum.LOSANGE.GetPoints(), CouleursEnum.NOIR.GetColor()),
            };
            ListeIdFormeEnCours = new List<int>();
            _puzzleEnCours = new List<Forme>();
        }

        //Démarrage du jeu
        public override void StartGame()
        {
            _compteurTours = 0;
            _nbTours = 20;
            StartChrono();
        }

        /// <summary>
        /// Génère un puzzle à afficher
        /// </summary>
        /// <returns>les 2 formes à afficher</returns>
        public List<Forme> GeneratePuzzle()
        {
            var rand = new Random();
            var choixFacile = false;
            var premPos = false;
            _puzzleEnCours.Clear();
            //choix de la difficulté (est ce directement un objet à trouver ou faut il aussi réfléchir à sa couleur)
            var choixDifficulte = rand.Next(0, 100);

            if (Difficulte == DifficulteEnum.DIFFICILE)
            {
                choixFacile = choixDifficulte < 10;
                premPos = choixDifficulte <= 5;
            }

            if (Difficulte == DifficulteEnum.MOYEN)
            {
                choixFacile = choixDifficulte < 30;
                premPos = choixDifficulte <= 15;
            }

            if (Difficulte == DifficulteEnum.FACILE)
            {
                choixFacile = choixDifficulte < 50;
                premPos = choixDifficulte <= 25;
            }

            //si on doit deviner directement un des objets présent
            if (choixFacile)
            {
               var choixForme = rand.Next(1, ListeFormeDefaut.Count);

                //choix du bon objet
                _solution = ListeFormeDefaut.FirstOrDefault(x => x.Id == choixForme);
                
                //Création d'un objet faux
                Forme formeB;
                do
                {
                    var couleur = rand.Next(1, 6);
                    var polygon = rand.Next(1, 6);
                    formeB = new Forme(0, PolygonEnum.GetPoints(polygon), CouleursEnum.GetColor(couleur));
                } while (ListeFormeDefaut.Count(x => x.Couleur.Equals(formeB.Couleur) && x.Dessin.Equals(formeB.Dessin)) > 0 || (formeB.Dessin.Equals(_solution.Dessin) || formeB.Couleur.Equals(_solution.Couleur)));

                if (premPos)
                {
                    _puzzleEnCours.Add(_solution);
                    _puzzleEnCours.Add(formeB);
                }
                else
                {
                    _puzzleEnCours.Add(formeB);
                    _puzzleEnCours.Add(_solution);
                }
            }
            else //pour le choix difficile il faut générer deux objets différents de couleurs différentes et entre eux et entre les objets de base
            {
                //forme 1
                Forme formeA;
                do
                {
                    var couleur = rand.Next(1, 6);
                    var polygon = rand.Next(1, 6);
                    formeA = new Forme(0, PolygonEnum.GetPoints(polygon), CouleursEnum.GetColor(couleur));
                } while (ListeFormeDefaut.Count(x => x.Couleur.Equals(formeA.Couleur) && x.Dessin.Equals(formeA.Dessin)) > 0);

                //forme 2
                Forme formeB;

                var listeTmp =  ListeFormeDefaut.Where(x => !x.Dessin.Equals(formeA.Dessin) && !x.Couleur.Equals(formeA.Couleur));
                var idCouleurRestante = new List<Color>();
                var idFormeRestante = new List<PointCollection>();
                foreach (var forme in listeTmp)
                {
                    idCouleurRestante.Add(forme.Couleur);
                    idFormeRestante.Add(forme.Dessin);
                }

                do
                {
                    var couleur = idCouleurRestante[rand.Next(0, idCouleurRestante.Count)];
                    var polygon = idFormeRestante[rand.Next(0, idFormeRestante.Count)];
                    formeB = new Forme(0, polygon, couleur);
                } while ((ListeFormeDefaut.Count(x => x.Couleur.Equals(formeB.Couleur) && x.Dessin.Equals(formeB.Dessin)) > 0) || formeB.Dessin.Equals(formeA.Dessin) || formeB.Couleur.Equals(formeA.Couleur));

                _puzzleEnCours.Add(formeA);
                _puzzleEnCours.Add(formeB);
                _solution = ListeFormeDefaut.FirstOrDefault(x => !x.Dessin.Equals(formeB.Dessin) && !x.Dessin.Equals(formeA.Dessin) && !x.Couleur.Equals(formeB.Couleur) && !x.Couleur.Equals(formeA.Couleur));
            }

            ListeIdFormeEnCours.Clear();
            ListeIdFormeEnCours.Add(PolygonEnum.GetValue(_puzzleEnCours[0].Dessin));
            ListeIdFormeEnCours.Add(PolygonEnum.GetValue(_puzzleEnCours[1].Dessin));

            return _puzzleEnCours;
        }

        /// <summary>
        /// vérifie si la réponse est correcte
        /// </summary>
        /// <param name="forme">la forme répondu</param>
        /// <returns>true si la réponse est bonne</returns>
        public bool IsReponseCorrect(Forme forme)
        {
            var retour = forme.Dessin.Equals(_solution.Dessin) && forme.Couleur.Equals(_solution.Couleur);
            if (!retour)
            {
                CompteurErreurs++;
            }
            return retour;
        }

        //vérifie si le jeu est fini
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

        //Calcul le score final
        public async override Task<Resultats> CalculResult()
        {
            //calcul de l'age pour connaitre la marge de temps à rajouter au temps min
            var age = DateUtils.IntervalleEntreDeuxDatesAnnee(ContextAppli.ContextUtilisateur.EnCoursUser.DateNaissance, DateUtils.GetMaintenant());
            var nbMilisecAge = (age < 30) ? 0 : (((age - 20) / 10) * 600);

            //calcul de l'interval de temps de résolution du jeu en fonction de l'age et de la diffculté
            var tempsMin = (400 + nbMilisecAge) * _nbTours;
            var tempsMax = 0;

            if (Difficulte.Equals(DifficulteEnum.FACILE))
            {
                tempsMax = (10000 + (nbMilisecAge * 3)) * _nbTours;
            }

            if (Difficulte.Equals(DifficulteEnum.MOYEN))
            {
                tempsMax = (6000 + (nbMilisecAge * 3)) * _nbTours;
            }

            if (Difficulte.Equals(DifficulteEnum.DIFFICILE))
            {
                tempsMax = (3000 + (nbMilisecAge * 3)) * _nbTours;
            }

            //calcul de la note de temps
            int noteTemps;
            if (TempsPasse <= tempsMin) noteTemps = 100;
            else if (TempsPasse >= tempsMax) noteTemps = 0;
            else
            {
                noteTemps = 100 - ((TempsPasse - tempsMin) / ((tempsMax - tempsMin) / 100));
            }

            //prise en compte des erreurs
            var noteAvecErreurs = ((_nbTours - CompteurErreurs) * noteTemps) / _nbTours;

            //calcul de la note finale et sauvegarde
            return await SaveResult(noteAvecErreurs);
        }
    }
}
