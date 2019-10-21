# Welcome to Asteroids!

Asteroids! is a 2D space shooter made with Unity.

# Game

The game consists of kill as much as you can while surviving until the end of the level, killing/destroying will give you points (counter on the top-right). You have a total of 6  lives (counter top-left), if you get hit 6 times you will die and you will have to play from the beginning.

At the end of the level, you will find a final boss. Be careful!

# Controls

* A W S D/Arrows/Left joystick - Movement
* Space/A button - Use primary inventory slot
* Alt-Left/B button - Use secondary inventory slot
* Escape/Start button - Pause/Resume game

# Power-ups
## Blue power-up
![Image](https://i.imgur.com/L0QSQSB.png)

This power changes player basic shoot (Primary inventory slot) to a double shoot that is deactivated after a time frame.

## Red power-up
![Image](https://i.imgur.com/2oPBozC.png)

This power gives the player a missile that is stored in the secondary inventory slot. Once it is used a missile is launched that explodes after a time frame or when colliding with an enemy. High damage.

## Yellow power-up
![Image](https://i.imgur.com/LI7ya2M.png)

This power gives the player ship a shield that is stored in the secondary inventory slot. Once it is used a shield is deployed around the player and gives invulnerability during a time frame.

# Obstacles
## Asteroid
![Image](https://i.imgur.com/ouwrP8L.png)

Big rock with high health. When destroying it spawns smaller chunks with lower health. It also gives points.

# Enemies
## Kamikaze Enemy
### Blue
![Image](https://i.imgur.com/txBBSFf.png)

Moves up-down with horizontal speed. Medium health.

### Grey
![Image](https://i.imgur.com/hJ9rUEz.png)

Static. Medium health.

### Yellow
![Image](https://i.imgur.com/QsaEfPX.png)

Chaser. Follow the main ship trying to crash with him. Medium health.

## Shooter Enemy
![Image](https://i.imgur.com/rvf3837.png)

Moves up-down without horizontal speed. Shoots in straight line constantly. Low-medium health.

## Helix Enemy
![Image](https://i.imgur.com/UwEPFjV.png)

Static. Shoots in a circle pattern. High health.

## Boss
![Image](https://i.imgur.com/adTJiN7.png)

Highest health of the game. Moves up-down with horizontal speed. 4 different attacks:
-Invulnerability: Gets invincible during a low time frame.
-Missile: Launch a missile that explodes after a time frame, this explosion spawn bullets in a circle pattern. These bullets can travel in straight lines or sinusoidal.
-Basic Shooting: Shoots a bullet that follows a sinusoidal movement.
-Circle shooting: Shoots in a circle pattern.

These attacks are executed randomly within a constant time frame, when the boss health is below 1/3 it starts a rage mode reducing the time between these random attacks.

# Challenges during development

The most challenging thing it has been to have the most complete experience (gameplay, UI, music…) within the given time (12 days), also I hadn’t implemented any UI until now on Unity and this has difficulted a bit the development.


# Download!

[Windows](https://github.com/sliz3r/Asteroids/releases/tag/v1.0)
[MacOS](https://drive.google.com/file/d/1eO0b4fRc8oZzQdmlFl1XGT4Th5Uwh53-/view?usp=sharing)
