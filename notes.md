- collision with obstacles
    - new collider component on player
    - new collider component on obstacle
    - check for collisions in obstacle script
- make obstacle a generic prefab that can be initialized based on obstacle type. i.e. will change sprite, and collider on init
- in obstacle script: when you reach a certain x position, delete yourself, remove yourself from queue and map in obstacle generator

aesthetic
- get better placeholder character sprite with walking/running animation
- parallaxing on background