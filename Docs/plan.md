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
    - whatever name Darin is using manager. place new background/floor objects into the scene at some interval 
    - Midi parser. begin parsing the midi file for the specified song
    - ObstacleManager. using a lock with audio manager, begin reading Events from the Queue in Midi Parser script.
        Generate an Obstacle if event is of correct type, sleep the delta time of the event, then read another event.
        If Queue is empty, call function on Darin's script to
            1. stop generating new background objects
            2. call function on each existing background object to stop moving 
    - audio manager. using a lock with ObstacleManager,  begin playing song for level


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