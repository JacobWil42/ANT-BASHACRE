using UnityEngine;

public class Player : MonoBehaviour
{
    private RoomManager roomManager;
    private Room currentRoom;
    
    public float moveSpeed = 5f;
    private bool collisionEnabled = true;
    private Rigidbody2D rb;
    private BoxCollider2D playerCollider;

    // Player statistics
    public float maxHealth = 100f;
    public float currentHealth;
    public float attackSpeed = 1f;
    public float damage = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = 0f; // Disable gravity for top-down movement
        rb.freezeRotation = true; // Prevent rotation
        rb.drag = 5f; // Add some drag to prevent sliding
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        playerCollider = GetComponent<BoxCollider2D>();
        if (playerCollider == null)
        {
            playerCollider = gameObject.AddComponent<BoxCollider2D>();
        }

        // Initialize current health
        currentHealth = maxHealth;
    }

    public void SetRoomManager(RoomManager manager)
    {
        roomManager = manager;
    }

    void Update()
    {
        // Toggle collision with 'C' key
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCollision();
        }
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        //update the players position using default Unity inputs
        transform.Translate(Vector3.right * horizontalInput * moveSpeed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput * moveSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        CheckCurrentRoom();
    }

    void CheckCurrentRoom()
    {
        if (roomManager != null)
        {
            Room newRoom = roomManager.GetRoomAtPosition(transform.position);
            if (newRoom != null && newRoom != currentRoom)
            {
                currentRoom = newRoom;
                OnRoomEnter(currentRoom);
            }
        }
    }

    void OnRoomEnter(Room room)
    {
        Debug.Log($"Entered room: {room.name}");
        // Add any other logic you want to execute when entering a new room
    }

    void ToggleCollision()
    {
        collisionEnabled = !collisionEnabled;
        rb.simulated = collisionEnabled;
        playerCollider.enabled = collisionEnabled;
        Debug.Log(collisionEnabled ? "Collision enabled" : "Collision disabled");
    }

    // Function to update player statistics
    public void UpdateStats(float healthChange = 0f, float speedChange = 0f, float damageChange = 0f, float attackSpeedChange = 0f)
    {
        currentHealth = Mathf.Clamp(currentHealth + healthChange, 0f, maxHealth);
        moveSpeed += speedChange;
        damage += damageChange;
        attackSpeed += attackSpeedChange;

        Debug.Log($"Updated Player Stats: Health: {currentHealth}/{maxHealth}, Speed: {moveSpeed}, " +
                  $"Damage: {damage}, Attack Speed: {attackSpeed}");
    }

    // Function to deal damage to the player
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log($"Player took {damageAmount} damage. Current Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Function to heal the player
    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log($"Player healed for {healAmount}. Current Health: {currentHealth}/{maxHealth}");
    }

    // Function called when the player dies
    private void Die()
    {
        Debug.Log("Player has died!");
        // Add any death logic here (e.g., respawn, game over screen, etc.)
    }
}