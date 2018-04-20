# Jeu Du Berger - Tower Defense Game

![A screenshot](screenshot.png)

Un berger doit défendre ses enclos contre les loups chaque nuit à l’aide de son arme et de pièges posés le jour.


## Features développé intégralement par moi même 
- Statistiques Joueurs : 
	- Gestion des dégats
	- Animations
	- Gestions mort
  - Sons
  - Interaction avec les autres entités comme les loups via son arme
- IA des Loups:
  - Fonctionnalités communes à tous les loups : 
    - Algorithme d'attribution de cible, chaque enclos est ocmposé de barrière afin de donner un bon rendu les loups au corps à corps 
    vont essayer de se répartir au mieux au tour de l'enclos tandis que lesl oups à distance vont s'approrpier la barrière la plus
    proche de leurs cercle de rangeIn.
    - Une fois que le joueur les a attaué les loups se mettent à le poruchasser jusqu'à leurs mort ou la leur. Attention la condition de 
    défaite n'est pas la mort du berger, qui peut respawn , mais de ses moutons
    - Chaque type de loups a une petite lanterne pour mieux le repérer dans la nuit selon son type et ainsi agir en conséquence.
    - Les loups communiquent entre eux de manière virtuel en s'abonant à la meêm cible , lorsque leurs cible est morte tous les loups
    sont prévneus et cherche une nouvelle cible en conséquences
  - Fonctionnalités communes aux loups range (Aquatique / Montagne) :
    - Il dispose de deux cercles , un plus petit pour detecter lorsque leurs cilbe ets assez proche pour la considérer en range
    , un autre pour leur dire quand leurs cible est trop loin et elle devient out of range. L'idée était d'éviter à ce que les loups
    changent continument d'état quitte à ce que leurs portée maximale ne soit pas la même portée qui considére si oui ou non leurs 
    cible est en range. Une fois ne mode attaque ils se tourneront vers leurs cible.
  - Loups des montagnes
    - Il peut geler le joueur si il l'attaque de manière continu pendantu n certian temps, plsu d'expliquations par la suite.
  - Loups bosse
    - Si le joueur est en vie, le loups bosse le prendra en chasse. Si le joueur est mort, le tmeps que le joueur respawn , le bosse attaquera
    alors les enlos mais sera prévenu du respawn du joueur.
- Freezing System : 
  - Lorsque les loups de montagnes atatque le joueur, si le joueur subit du gel pendant un certain temps il se gèle pendant quelques secondes.
  - En suite le joueur ne peut plus être gelé pendant u ncertain temps
  - Le joueur peut également éviter le gel et remettre ) 0 le compteur si il arrive à éviter les attaque des loups desm ontagnes
- Pièges
  - Lapins ou aussi apellé Bait : Attire les sur lui si les loups sont assez proche, cela permet d'attirer les loups dans les pièges étant donné
  que le monde est assez ouvert
- UI
  - Aide au déclenchement des pannels de tutoriel pour guider le joueur
  - Barre de vie des loups qui s'affichent uniquement un petit laps de tmeps lorsqu'ils subissent des dégats
- Spawn Loups
  - Correction de certains bugs et amélioration du script et des spawn position
## Comment jouer ?

Clôner le projet et lancer JeuBerger1.3.exe qui se trouve dans le dossier VersionFinale
 
## Controles
La plus part des mécaniques sont expliquéEs dans le jeu grâce à des tutoriels qui se déclenchent de manière intelligenteS
- Pour poser des pièges il faut appuyer sur 1,2,3,4 situer au dessus des lettres du clavier
- On peut améliorer un piège ou le revendre (clique gauche/clique droit)
- Pour ajouter des moutons dans un enclos  il faut se diriger proche d'un enclos et appuyer sur + pour ajouter - pour enlever
et * pour le super mouton qui rendra invulnérable votre enclos pendant une nuit.
## Conclusion
Aboutissement d'un vrai projet, jeu selon mon point de vue quasiment livrable, il manqueerai peut être d'équilibrage mais
c'est un jeu qui demande au joueurs de la réflexion et du skill donc assez dure tout de même. Bilan : Très statisifait de ce jeu.



## Authors

* **Sacha Vanleene - Developper** 
* **Edouard François - Developper**
* **Arnaud Monteils - Developper**
* **Esmé James - Developper**
* **Théo Debay - Developper**
* **Wilfried Pouchous - Developper**
* **Hugo Brunet - Developper**
