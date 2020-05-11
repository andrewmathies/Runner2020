# Plan
The data our game needs for a level is a set of one midi file and one audio file. Both files are for the same song.

## Launch scene
- list of levels to select from
- simple UI to select one of the levels
- choosing a level immediately launches the gameplay scene with specified song data
- options button changes UI
    - bar controls volume
    - ...

## Gameplay scene
- player
    - whenever player hits obstacle, call a function and log something
    - animator with running and jumping animations
    - jump script with Mario physics
- gamemanager
    - Moving object manager. (Darin) place new background/floor objects into the scene at some interval 
    - Midi parser. begin parsing the midi file
    - ObstacleManager. using a lock with audio manager, begin reading Midi data and placing obstacles
    - audio manager. using a lock with ObstacleManager,  begin playing audio for level


all songs must have at least one good Midi track.

a good track
    - can be beaten
    - contains the melody for most of the song

what if we need to stitch different sections of different tracks together to get the "melody track"
or rather, we will generate obstacles from different tracks of the same song. at any given time, a track that is playing the melody will be used to generate obstacles.
Can we figure that out just from the frequency of note on events in a given span of time, or must the track being used to generate obstacles be hand selected for the entire song? 

make a level like https://www.youtube.com/watch?v=CKA1zCwcYBE
- pretty harmony
- slow/simple rhythms

===========================
L = some number of frames before the player hits an obstacle where the player can jump

L must be constant in a song. Should it be constant across all songs? I just worry about some future song that is fast.
if the time between any two note On events in a song is greater than L, then we can't use that level

so we want something to check if a midi file is a valid level

when player hits jump button, change animation and state to jump for L frames
while in jump state, user cannot collide with an obstacle