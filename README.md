# Solar Harvest

**Solar Harvest** est un jeu de jardinage rapide de style *Solarpunk* développé sous Unity.
Prenez le contrôle d'un drone agricole et gérez votre potager futuriste pour obtenir le meilleur score possible avant la fin du temps imparti !

---

## Le Jeu

Vous pilotez un drone dans un jardin suspendu. Votre mission est simple : **Planter, Arroser, Récolter**.
Mais attention, le temps presse ! Vous avez **5 minutes** pour maximiser vos profits.

### Mécaniques Clés
* **Cycle de vie** : Les plantes naissent, grandissent, deviennent mûres... et pourrissent si vous attendez trop !
* **Arrosage Actif** : L'arrosage n'est pas instantané, il accélère la croissance. Gérez votre temps !
* **Progression** : Commencez avec des graines basiques. Accumulez des points pour débloquer des graines plus rares et plus rentables.
* **Physique** : Le drone utilise un système de physique (Rigidbody) pour se déplacer et interagir avec les collisions du décor.

---

## Commandes (Contrôles)

| Action | Touche (Clavier/Souris) |
| :--- | :--- |
| **Se Déplacer** | `Z`, `Q`, `S`, `D` (ou WASD) |
| **Planter** | `Espace` |
| **Arroser** | `Maj` (Shift) |
| **Changer de Graine** | `Tab` (Débloque selon le score) |
| **Pause / Menu** | `Echap` (Escape) |

---

## Fonctionnalités Techniques

Ce projet a été réalisé avec **Unity** (C#) et met en œuvre plusieurs concepts :

* **Unity Input System** : Gestion moderne des contrôles.
* **Système de Particules** : Effets d'eau pour l'arrosage.
* **Raycasting** : Détection des plantes sous le drone pour l'arrosage.
* **Gestion de l'UI** : Menus (Main, Pause, Game Over), Timer et Score en temps réel.
* **Persistance des données** : Sauvegarde du *Meilleur Score* (High Score) via PlayerPrefs.
* **Audio Manager** : Gestion dynamique de la musique (changement en fin de partie) et des bruitages (Pitch aléatoire pour éviter la répétition).
* **Game Loop** : Gestion d'états (Jeu, Pause, Fin) et chronomètre.

---

## Installation et Lancement

1.  Clonez ce dépôt ou téléchargez le fichier ZIP.
2.  Ouvrez le dossier du projet via **Unity Hub** (Version recommandée : Unity 6 ou 2022+).
3.  Ouvrez la scène `MenuPrincipal` située dans le dossier `Scenes`.
4.  Appuyez sur **Play** pour lancer le jeu !

---

## Crédits & Assets

* **Moteur** : Unity
* **Développement** : Dumas Mathieu
* **Assets 3D** : Modèles de Drones et Végétations (Kenney / Assets Gratuits)
* **Audio** : Kenney Assets / Freesound.org

---

*Projet réalisé dans le cadre d'un exercice de développement de jeu vidéo.*
