using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;

namespace JeuxDeLogiqueWin10.Strings
{
    /// <summary>
    /// classe fournissant la liste des langues disponibles
    /// </summary>
    public static class ListeLangues
    {
        /// <summary>
        /// structure pour faire une liste des langues
        /// </summary>
        public struct LanguesStruct
        {
            public int Id { get; set; }
            public string Nom { get; set; }
            public string Diminutif { get; set; }

            /// <summary>
            /// Structure pour créer une liste des langues
            /// </summary>
            /// <param name="id">l'id de la langue</param>
            /// <param name="nom">le nom complet de la langue</param>
            /// <param name="diminutif">le nom BCP-47</param>
            public LanguesStruct(int id, String nom, String diminutif)
                : this()
            {
                Id = id;
                Nom = nom;
                Diminutif = diminutif;
            }

            /// <summary>
            /// retourne le nom de la langue de la structure
            /// </summary>
            /// <returns>le nom de la la langue</returns>
            public override string ToString()
            {
                return Nom;
            }
            
            /// <summary>
            /// vérifie l'égalité de la structure
            /// </summary>
            /// <param name="obj">la version à comparer</param>
            /// <returns>true si égal</returns>
            public override bool Equals(object obj)
            {
                if (obj is LanguesStruct)
                {
                    return ((LanguesStruct)obj).Id.Equals(Id);
                }
                return false;
            }

            /// <summary>
            /// retourne le hascaode de la structure
            /// </summary>
            /// <returns>le hashcode</returns>
            public override int GetHashCode()
            {
                return Id.GetHashCode() * Nom.GetHashCode() * Diminutif.GetHashCode();
            }
        }

        /// <summary>
        /// Liste des langues disponibles dans l'application
        /// </summary>
       private static readonly List<LanguesStruct> ListeLanguesDispo = new List<LanguesStruct> {
            new LanguesStruct(1, ResourceLoader.GetForCurrentView().GetString("textEN"), "en"),
            new LanguesStruct(2, ResourceLoader.GetForCurrentView().GetString("textFR"), "fr"),
            new LanguesStruct(3, ResourceLoader.GetForCurrentView().GetString("textES"), "es"),
            new LanguesStruct(4, ResourceLoader.GetForCurrentView().GetString("textPT"), "pt"),
        };

        /// <summary>
        /// retourne la liste des langues disponibles dans l'application
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<LanguesStruct> GetListesLangues()
        {
           return ListeLanguesDispo;
        }

        /// <summary>
        /// change la langue de l'application
        /// </summary>
        /// <param name="langue">la nouvelle langue à appliquer</param>
        public static void ChangeLangueAppli(LanguesStruct langue)
        {
            var langueTelephone = Windows.System.UserProfile.GlobalizationPreferences.Languages[0];
            var langueAAppliquer = langue.Diminutif;
            if(langueTelephone.Contains("-"))
            {
                if(langue.Equals(langueTelephone.Substring(0,langueTelephone.IndexOf('-'))))
                {
                    langueAAppliquer = langueTelephone;
                }
            }
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = langueAAppliquer;
        }

        /// <summary>
        /// change la langue en cours d'utilisation à partir de l'id de la langue
        /// </summary>
        /// <param name="idLangue">l'id de la langue à utilisé</param>
        public static void ChangeLangueAppli(int idLangue)
        {
            foreach (var languesStruct in ListeLanguesDispo)
            {
                if (languesStruct.Id == idLangue)
                {
                   ChangeLangueAppli(languesStruct); 
                }
                break;
            }
        }

        /// <summary>
        /// retourne la langue en cours d'utilisation 
        /// </summary>
        /// <returns>retourne la langue en cours d'utilisation</returns>
        public static LanguesStruct GetLangueEnCours()
        {
           var retour = new LanguesStruct();

            //récupération de la langue si elle a déjà été modifié manuellement
           var langueEnCours = Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride;

            //si aucune déjà en place, recherche dans la liste des langues utilisé du téléphone pour la première qui correspond à celle dispo
            if(String.IsNullOrWhiteSpace(langueEnCours))
            {
                //ont parcours les langues du téléphone
                var fini = false;
                foreach (var langue in Windows.System.UserProfile.GlobalizationPreferences.Languages)
                {
                    if(!fini)
                    {
                        var langueTmp = langue;

                        //ont parcours les langues de l'appli (avec régionalisation)
                        foreach (var l in ListeLanguesDispo)
                        {
                            //si ont trouve, ont enregistre et ont arrete
                            if (l.Diminutif.Equals(langueTmp))
                            {
                                langueEnCours = l.Diminutif;
                                fini = true;
                            }
                        }

                        //si aucun résultat, ont recherche sans régionalisation
                        if (langueTmp.Contains("-") && !fini)
                        {
                            langueTmp = langueTmp.Substring(0, langueTmp.IndexOf('-'));
                            fini = false;
                            //ont parcours les langues de l'appli (sasn régionalisation)
                            foreach (var l in ListeLanguesDispo)
                            {
                                //si ont trouve, ont enregistre et ont arrete
                                if (l.Diminutif.Equals(langueTmp))
                                {
                                    langueEnCours = l.Diminutif;
                                    fini = true;
                                }
                            }
                        }
                    }
                    
                    if(fini)
                    {
                        break;
                    }
                }
            }

            //recherche de la langue en cours dans la liste des structures (avec regionalisation)
            var dejatrouve = false;
            foreach (var l in ListeLanguesDispo)
            {
                if (l.Diminutif.ToUpper().Equals(langueEnCours.ToUpper()))
                {
                    retour = l;
                    dejatrouve = true;
                    break;
                }
            }

            //(si aucun résultat ont supprime la régionalisation)petit traitement pour pouvoir le retrouver dans la structure
            if (langueEnCours.Contains("-") && !dejatrouve)
            {
                langueEnCours = langueEnCours.Substring(0, langueEnCours.IndexOf('-'));
                //recherche de la langue en cours dans la liste des structures (sans regionalisation)
                foreach (var l in ListeLanguesDispo)
                {
                    if (l.Diminutif.Equals(langueEnCours))
                    {
                        retour = l;
                        break;
                    }
                }
            }

            //si rien ne correspond c'est que par défaut c'est l'anglais
            if(retour.Id == 0)
            {
                retour = ListeLanguesDispo[0];
            }
            return retour;
        }


    }
}
