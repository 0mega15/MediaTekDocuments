# MediatekDocuments
Cette application permet de gérer les documents (livres, DVD, revues) d'une médiathèque. Elle a été codée en C# sous Visual Studio 2022. C'est une application de bureau, prévue d'être installée sur plusieurs postes accédant à la même base de données.
L'application exploite une API REST pour accéder à la BDD MySQL.<br>
Seules des fonctionnalités ajoutées sont présentées ici (gestion des commandes et authentification), pour retrouver les explications sur l'application de base, veuillez vous rendre sur ce dépôt : https://github.com/CNED-SLAM/MediaTekDocuments
## Présentation
Voici les fonctionnalitées ajoutées : <br>
Il est désormais possible : <br>
- D'ajouter, modifier ou supprimer un document (ce dernier cas n'étant possible uniquement s'il elle n'est raccroché à aucun exemplaire ou commande).
- De gérer les commandes en ajoutant des commandes de livres, dvd ou revue ainsi que de les modifier ou supprimer. De plus les commandes à moins de 30jours de leur date d'éxpiration seront affiché au lancement de l'application.
- Gérer le suivi de l'état des exemplaires en ayant la possibilité de modifier leur état.
- De se connecter via un couple login password sachant qu'en fonction de la personne des actions sont restreintes<br>
<img width="1145" height="388" alt="image" src="https://github.com/user-attachments/assets/86d29e91-c2d1-468b-88e1-898f128af9a1" /><br>
## Les différentes fonctionnalitées
### Gestion des documents
<img width="1091" height="883" alt="image" src="https://github.com/user-attachments/assets/6e094134-8a35-49c6-b3ab-f4dd96e3945c" /><br>
La gestion des livres, dvd et revues suivant le même principe, seul l'onglet des revues sera présenté<br>
Lors de l'appui d'un des 3 boutons de la gestion des documents il est possible lors de la modification de changer indépendamment des informations de la revue sélectionné. La suppression supprimera la revue sélectioné et l'ajout videra les champs pré remplis.<br>
#### Gestion des commandes
<img width="1100" height="886" alt="image" src="https://github.com/user-attachments/assets/eb8c9087-3278-497c-af93-547fecde3444" />
Lors de la selection d'un document il est possible de voir les commandes qui lui sont associé, et donc aussi de les modifier ou supprimer.<br>
## L'API REST
L'accès à la BDD se fait à travers une API REST protégée par une authentification basique.<br>
Le code de l'API se trouve ici :<br>
https://github.com/CNED-SLAM/rest_mediatekdocuments<br>
avec toutes les explications pour l'utiliser (dans le readme).
## Installation de l'application
Ce mode opératoire permet d'installer l'application pour pouvoir travailler dessus.<br>
- Installer Visual Studio 2019 entreprise et les extension Specflow et newtonsoft.json (pour ce dernier, voir l'article "Accéder à une API REST à partir d'une application C#" dans le wiki de ce dépôt : consulter juste le début pour la configuration, car la suite permet de comprendre le code existant).<br>
- Télécharger le code et le dézipper puis renommer le dossier en "mediatekdocuments".<br>
- Récupérer et installer l'API REST nécessaire (https://github.com/CNED-SLAM/rest_mediatekdocuments) ainsi que la base de données (les explications sont données dans le readme correspondant).
