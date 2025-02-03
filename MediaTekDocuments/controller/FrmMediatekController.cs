using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.dal;
using System;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmMediatek
    /// </summary>
    public class FrmMediatekController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmMediatekController()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return access.GetAllGenres();
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return access.GetAllLivres();
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Liste d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return access.GetAllDvd();
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return access.GetAllRevues();
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return access.GetAllRayons();
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return access.GetAllPublics();
        }


        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocuement">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocuement)
        {
            return access.GetExemplairesRevue(idDocuement);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return access.CreerExemplaire(exemplaire);
        }

        /// <summary>
        /// récupère les commandes d'un livre
        /// </summary>
        /// <param name="idLivre">id du livre concerné</param>
        /// <returns>Liste d'objets CommandeDocument</returns>
        public List<CommandeDocument> GetAllCommandes(string id)
        {
            return access.GetAllCommandes(id);
        }

        /// <summary>
        /// Crée une commande d'un livre dans la bdd
        /// </summary>
        /// <param name="commande">L'objet Commande concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerCommande(Commande commande)
        {
            return access.CreerCommande(commande);
        }

        /// <summary>
        /// Crée une commande de livre dans la bdd
        /// </summary>
        /// <param name="commandedocument">L'objet commande concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerCommandeDocument(string id, int nbExemplaire, string idLivreDvd, string idSuivi)
        {
            return access.CreerCommandeDocument(id, nbExemplaire, idLivreDvd, idSuivi);
        }

        /// <summary>
        /// Modifie l'étape de suivi d'une commande
        /// </summary>
        /// <param name="id">Id de la commande de document à modifier</param>
        /// <param name="idSuivi">Id de l'étape de suivi</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool ModifierSuiviCommandeDocument(string id, string idSuivi)
        {
            return access.ModifierSuiviCommandeDocument(id, idSuivi);
        }

        /// <summary>
        /// Supprime une commande dans la bdd
        /// </summary>
        /// <param name="id">Id de la commande de document à supprimer</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool supprimerCommande(string id)
        {
            return access.supprimerCommande(id);
        }

        /// <summary>
        /// récupère les commandes d'une revue
        /// </summary>
        /// <param name="idRevue">id de la revue concernée</param>
        /// <returns>Liste d'objets Abonnement</returns>
        public List<Abonnement> GetAllCommandesRevue(string idRevue)
        {
            return access.GetAllCommandesRevue(idRevue);
        }

        /// <summary>
        /// crée un abonnement pour une revue
        /// </summary>
        /// <param name="idRevue">id de la revue concernée</param>
        /// <returns>Liste d'objets Abonnement</returns>
        public bool CreerAbonnement(string id, DateTime dateFinAbonnement, string idRevue)
        {
            return access.CreerAbonnement(id, dateFinAbonnement, idRevue);
        }

        /// <summary>
        /// getter sur les échéances
        /// </summary>
        /// <returns>Liste d'objets Abonnement</returns>
        public List<Abonnement> GetAllEcheanceAbonnement()
        {
            return access.GetAllEcheanceAbonnement();
        }

        /// <summary>
        /// getter sur les utilisateurs
        /// </summary>
        /// <returns>Liste d'objets Abonnement</returns>
        public string GetUtilisateur(string login, string password)
        {
            List<Utilisateur> LeUser = access.GetUtilisateur(login);
            foreach (var user in LeUser)
            {
                if(user.password == password)
                {
                    return user.idService;
                }
            }
            return "";
        }

    }
}
