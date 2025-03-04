# MediatekDocuments
Cette application permet de gérer les documents (livres, DVD, revues) d'une médiathèque. Elle a été codée en C# sous Visual Studio 2019. C'est une application de bureau, prévue d'être installée sur plusieurs postes accédant à la même base de données.<br>
L'application exploite une API REST pour accéder à la BDD MySQL. Des explications sont données plus loin, ainsi que le lien de récupération.
## Présentation
Toutes les informations relatives à cette application ainsi que les fonctionnalités de base sont décrites dans le readme du dépôt d'origine accessible à cette adresse : https://github.com/CNED-SLAM/MediaTekDocuments <br>
Plusieurs nouvelles fenêtres ont été ajoutées :
## La fenêtre d'authentification
Lors du lancement de l'application une fenêtre d'authentification s'ouvre où il est demandé de renseigner un nom d'utilisateur et un mot de passe, afin de se connecter.<br><br>
![Capture d'écran 2025-03-03 224854](https://github.com/user-attachments/assets/5cda020c-9b4a-4829-95e2-8bff05b343fc)<br>
Plusieurs utilisateurs avec des droits différents ont été configurés :<br>
![Capture d'écran 2025-03-03 225956](https://github.com/user-attachments/assets/c8487abc-63a7-42cd-8ad3-2928c2a22959)<br>
- Charles et Jean du service administrateur et administratif ont accès à toutes les fonctionnalités de l'application.<br>
- Marie du service prêts n'a accès qu'aux onglets de consultation des documents.<br>
- Arnaud du service culture n'a aucun droit d'accès à l'application.<br>

## La fenêtre des abonnements qui expirent dans moins de 30 jours
A l'ouverture de l'application pour le service administrateur et administratif, une fenêtre de rappel s'affiche avec la liste des abonnements qui expirent dans moins de 30 jours. La liste comprend la date de fin d'abonnement, ainsi que le titre des revues concernées.<br><br>
![Capture d'écran 2025-03-03 231605](https://github.com/user-attachments/assets/498c66cf-6761-466d-97cd-ac3035338181)

## La fenêtre principale avec les nouveaux onglets
### Onglet commandes de livres
![Capture d'écran 2025-03-04 082233](https://github.com/user-attachments/assets/5874437d-abf9-4c91-903d-15629dbcd7d0)<br>
Cet onglet propose de rechercher un livre par son numéro et d'affiher les informations suivantes : <br>
- Dans un premier groupbox, toutes les informations détaillées du livre.<br>
- La liste des commandes de ce livre, triée par défaut sur la date de commande la plus récente. Cette liste comporte le nombre d'exemplaires, l'étape de suivi de commande, la date de commande et le montant. Il est possible de trier cette liste en clickant sur les entêtes des différentes colonnes des informations.<br>
- Dans un second groupbox les informations détaillées de la commande sélectionnée, avec la possibilité de la supprimer après confirmation, ou bien d'ajouter une nouvelle commande : le bouton "Ajouter une nouvelle commande" vide les champs, et affiche un bouton pour enregistrer cette nouvelle commande.<br>
- Dans un troisième groupbox la possibilité de modifier l'étape de suivi de commande.<br>
### Onglet commandes de Dvd
![Capture d'écran 2025-03-04 084112](https://github.com/user-attachments/assets/19c28222-f574-47c9-a121-fff0fe020d16)<br>
Totalement similaire à l'onglet des commandes de livres, cet onglet permet de rechercher un dvd par son numéro et d'afficher toutes les informations détaillées plus haut dans l'onglet des commandes de livres.
### Onglet commandes de revues
![Capture d'écran 2025-03-04 084822](https://github.com/user-attachments/assets/76fd90e0-0699-4b2c-83d1-882069fa43a5)<br>
Cet onglet permet de rechercher un numéro de revue et d'afficher les informations suivantes :<br>
- Dans un premier groupbox les informations détaillées de la revue recherchée.<br>
- Une liste de tous les abonnements qui concernent cette revue (un abonnement équivaut à une commande de revue), avec les possibilités de tris sur les colonnes de date de fin d'abonnement, de date de commande et de montant.<br>
- Un second groupbox qui affiche les informations détaillées de l'abonnement sélectionné, et qui permet d'ajouter un nouvel abonnement à cette revue ou de supprimer celui sélectionné après confirmation.

## La base de données
La base de données 'mediatek86 ' au format MySQL a été modifiée.<br>
Voici sa nouvelle structure :<br>
![2025-03-04 09 03 05](https://github.com/user-attachments/assets/159b3cc9-6c6b-4296-9510-71d2817285e2)<br>
Une table suivi contenant un id et une étape de suivi a été ajoutée. Elle est reliée à la table commandedocument sous forme de dépendance fonctionnelle. Cela signifie qu'une commande de document aura forcément une étape de suivi.
Deux nouvelles tables ont également été ajoutées dans la base de données pour gérer l'authentification : <br><br>
![Capture d'écran 2025-03-04 091239](https://github.com/user-attachments/assets/594a46f4-10d8-4b48-a21b-a578eacdd3fd)<br>
La table utilisateur permet d'enregistrer un id, un login, un password et un id de service par utilisateur. Cette table est reliée par une dépendance fonctionnelle à la table suivi qui comprend un id et un libelle de service (administateur, culture...). Ainsi est attribué à chaque utilisateur un service qui lui donne accès à différentes fonctionnalités de l'application.

## L'API REST
L'accès à la BDD se fait à travers une API REST protégée par une authentification basique.<br>
Le code de l'API se trouve ici :<br>
https://github.com/Lamerguez21/API-rest<br>
avec toutes les explications pour l'utiliser (dans le readme).

## Installation de l'application
Pour installer l'application pour pouvoir l'utiliser il suffit de télécharger uniquement le fichier installeur Mediatek.msi disponible dans la liste des fichiers du dépôt.<br>
Après avoir suivi les étapes d'installation l'application est prête à être utilisée.

