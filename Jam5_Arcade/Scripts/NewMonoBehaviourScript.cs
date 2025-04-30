/* 
Write me the scripts and create the necessary Unity objects for my game idea. The 2D game is about controlling a beam that can rotate in order to destroy incoming enemies

General
- the things described under "Objects" are all Unity objects. Create them using MCP.
- for player input, use the new input system from Unity

GameObjects
- Player
    - "body": filled circle with material
        - child object "beam": a long beam with a fixed length reaching out of the circle and consisting of a 2D plane; has a material with a blue color
    - player script
    - colliders
- Enemy
    - tiny filled cirlce, smaller than the player circle
    - Enemy script
    - colliders
- EnemyManager
    - EnemyManager script
- AudioManager
    - has AudioManager script (exists already)
- game over screen
    - has a restart button


Scripts
- Player
    - onStart: fire a placeholder AudioEvent, using AudioSystem.Play2DAudio (will be AudioEvent.PlayerMovement, doesnt exist yet)
    - mouse input rotates the player to the mouse position; has max rotation speed (public); deals with delta time
    - movementSpeed tracks the speed of the rotation; is sent to FMOD using AudioSystem.SetLocalParameter
    - if the "body" part collides with an enemy (probably use tags or layers), then placeholder AudioSystem.Play2DAudio call & game over screen
    - if the "beam" part collides with an enemy (probalby use tags or layers), the enemy is destroyed

- Enemy
    - init function: gets "speed" from outside
    - Update: moves the transform into the direction of the player with a fixed speed ("speed")
- EnemyManager
    - public spawnCircleRadius field; please visualize it in the Unity Editor with a gizmo circle
    - spawns a small group of enemies (enemyCount) every few seconds (spawnInterval)
        - assigns "enemySpeed" to enemy on instantiation
        - the position of the first enemy of a group is always random on the spawn circle (defined by spawnCircleRadius, center is the screen center)
        - the spawn positions of the following enimies are slightly offset into one direction and slightly offset in time (spawnIntervalWithinGroup)
        - the enemies of a group are spawned as children of a newly created transform (enemyGroupTransform); keep track of that transform
    - onUpdate: every enemyGroupTransform is rotated very slightly (enemyOffsetMultiplier) randomly over time using a perlin noise
    - has progression system over time: over time the group sizes of enemies (enemyCount) get bigger, enemyOffsetMultiplier increases, and enemySpeed increases

- GameManager
    - restart function: restarts the game
 */


/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
} */
