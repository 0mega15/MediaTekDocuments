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
<img width="1145" height="388" alt="image" src="https://github.com/user-attachments/assets/86d29e91-c2d1-468b-88e1-898f128af9a1" />
## Les différentes fonctionnalitées
### Gestion des documents
<img width="1091" height="883" alt="image" src="https://github.com/user-attachments/assets/6e094134-8a35-49c6-b3ab-f4dd96e3945c" />
La gestion des livres, dvd et revues suivant le même principe, seul l'onglet des revues sera présenté<br>
Lors de l'appui d'un des 3 boutons de la gestion des documents il est possible lors de la modification de changer indépendamment des informations de la revue sélectionné. La suppression supprimera la revue sélectioné et l'ajout videra les champs pré remplis.<br>
#### Gestion des commandes
<img width="1100" height="886" alt="image" src="https://github.com/user-attachments/assets/eb8c9087-3278-497c-af93-547fecde3444" />
Lors de la selection d'un document il est possible de voir les commandes qui lui sont associé, et donc aussi de les modifier ou supprimer.<br>
## La base de données
La base de données 'mediatek86 ' est au format MySQL.<br>
Voici sa structure :<br>
![img4](https://github.com/CNED-SLAM/MediaTekDocuments/assets/100127886/4314f083-ec8b-4d27-9746-fecd1387d77b)
<br>On distingue les documents "génériques" (ce sont les entités Document, Revue, Livres-DVD, Livre et DVD) des documents "physiques" qui sont les exemplaires de livres ou de DVD, ou bien les numéros d’une revue ou d’un journal.<br>
Chaque exemplaire est numéroté à l’intérieur du document correspondant, et a donc un identifiant relatif. Cet identifiant est réel : ce n'est pas un numéro automatique. <br>
Un exemplaire est caractérisé par :<br>
. un état d’usure, les différents états étant mémorisés dans la table Etat ;<br>
. sa date d’achat ou de parution dans le cas d’une revue ;<br>
. un lien vers le fichier contenant sa photo de couverture de l'exemplaire, renseigné uniquement pour les exemplaires des revues, donc les parutions (chemin complet) ;
<br>
Un document a un titre (titre de livre, titre de DVD ou titre de la revue), concerne une catégorie de public, possède un genre et est entreposé dans un rayon défini. Les genres, les catégories de public et les rayons sont gérés dans la base de données. Un document possède aussi une image dont le chemin complet est mémorisé. Même les revues peuvent avoir une image générique, en plus des photos liées à chaque exemplaire (parution).<br>
Une revue est un document, d’où le lien de spécialisation entre les 2 entités. Une revue est donc identifiée par son numéro de document. Elle a une périodicité (quotidien, hebdomadaire, etc.) et un délai de mise à disposition (temps pendant lequel chaque exemplaire est laissé en consultation). Chaque parution (exemplaire) d'une revue n'est disponible qu'en un seul "exemplaire".<br>
Un livre a aussi pour identifiant son numéro de document, possède un code ISBN, un auteur et peut faire partie d’une collection. Les auteurs et les collections ne sont pas gérés dans des tables séparées (ce sont de simples champs textes dans la table Livre).<br>
De même, un DVD est aussi identifié par son numéro de document, et possède un synopsis, un réalisateur et une durée. Les réalisateurs ne sont pas gérés dans une table séparée (c’est un simple champ texte dans la table DVD).
Enfin, 3 tables permettent de mémoriser les données concernant les commandes de livres ou DVD et les abonnements. Une commande est effectuée à une date pour un certain montant. Un abonnement est une commande qui a pour propriété complémentaire la date de fin de l’abonnement : il concerne une revue.  Une commande de livre ou DVD a comme caractéristique le nombre d’exemplaires commandé et concerne donc un livre ou un DVD.<br>
<br>
La base de données est remplie de quelques exemples pour pouvoir tester son application. Dans les champs image (de Document) et photo (de Exemplaire) doit normalement se trouver le chemin complet vers l'image correspondante. Pour les tests, vous devrez créer un dossier, le remplir de quelques images et mettre directement les chemins dans certains tuples de la base de données qui, pour le moment, ne contient aucune image.<br>
Lorsque l'application sera opérationnelle, c'est le personnel de la médiathèque qui sera en charge de saisir les informations des documents.
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
