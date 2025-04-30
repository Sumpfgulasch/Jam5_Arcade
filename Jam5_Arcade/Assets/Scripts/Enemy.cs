using Audio;
using UnityEngine;
using DG.Tweening; // Requires DOTween package
using FMOD.Studio; // Assuming FMOD is used

public class Enemy : MonoBehaviour
{
    private float speed;
    private Transform playerTransform;
    private bool initialized = false;

    // Call this after instantiating the enemy
    public void Init(float enemySpeed, Transform targetPlayer)
    {
        this.speed = enemySpeed;
        this.playerTransform = targetPlayer;
        this.initialized = true;
    }

    void Start()
    {
        if (!initialized)
        {
            Debug.LogError("Enemy not initialized before Start! Call Init() after Instantiating.");
            // Provide default behaviour or destroy?
             // Destroy(gameObject);
             // return;
             // Fallback: Try to find player if not provided via Init
             GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
             if (playerObj != null) playerTransform = playerObj.transform;
             else Debug.LogError("Player not found by tag 'Player' for fallback initialization.");
        }

        // DOTween overshoot scale animation
        transform.localScale = Vector3.zero;
        transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack); // Simple overshoot scale up

        // Play spawn sound
        if (AudioManager.Instance != null)
        {
            // Assuming Play3DAudio attached to AudioManager is okay for 2D
            AudioManager.Instance.Play2DAudio(AudioEvent.SpawnEnemy); // Play at enemy position
        }
        else
        {
            Debug.LogWarning("AudioManager Instance not found. Cannot play SpawnEnemy sound.");
        }
    }

    void Update()
    {
        if (!initialized || playerTransform == null) return; // Don't move if not ready or player lost

        // Move towards the player
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        transform.position += directionToPlayer * speed * Time.deltaTime;

        // Optional: Make the enemy look at the player (if sprite orientation requires it)
        // float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 90f;
        // transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

     // Note: Collision logic is handled by the Player script in this design
     // If enemy needs to react to other things, add collision methods here.

     // Optional: Clean up DOTween tweens if the object is destroyed unexpectedly
     void OnDestroy()
     {
         transform.DOKill(); // Kill any active DOTween animations on this object
     }
     
     void OnCollisionEnter(Collision collision)
     {
         // Check if the collision is with the 'Body' part and an Enemy
         if (collision.collider.CompareTag("PlayerBody"))
         {
             // Trigger Game Over sequence (e.g., show UI, stop game)
             FindObjectOfType<GameManager>()?.GameOver(); // Example call
             Time.timeScale = 0; // Simple pause
         }
         // Check if the collision is with the 'Beam' part and an Enemy
         else if (collision.collider.CompareTag("PlayerBeam"))
         {
             Debug.Log("Player Beam collided with Enemy - Destroying Enemy!");
             // Destroy the enemy GameObject
             AudioManager.Instance.Play2DAudio(AudioEvent.DestroyEnemy); // Play at collision point
             Destroy(gameObject); // Destroy self (the enemy)
             // Optionally play a different sound for beam hit
         }
     }
}