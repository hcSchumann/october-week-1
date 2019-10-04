# october-week-1

On the first week of octeber I intend to build an enemy AI that detects and chases the player in Unity.

The AI will use "sound" and a line of sight aproach to detect the player.


# DAY 1
On the first day I started by trying to implement the sound the detection using audio sources and listeners. Before I went too far and actualy built anything I decided it wasn't a good idea since I would probably have some issues handling multile audio listeners and such. I went to bed.

# DAY 2
On the next day, while discussing my idea with a friend from work (https://github.com/LeGustaVinho), he told me about this article on light source implemention: https://www.redblobgames.com/articles/visibility/.
I decided to use a similar implementation but applied to sound and with a degree of reflaction, to amplify the reach of the sounds's area. I was tired and went to bed without doing any actual work.

# DAY 3
On day 3 I was able to start working on my new plan. I implemented the first step, wich is detecting the edges of the objects around the player. Tomorrow I intend to calculate the area created by this points and make the enemy "hear" the player (enemy is inside this calculated area), as well as provide some feedback for it.
