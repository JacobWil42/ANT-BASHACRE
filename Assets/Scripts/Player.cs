using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _bashInitialSpeed = 20f;
    [SerializeField] private float _bashDuration = 0.2f;
[SerializeField] private float _bashDecayRate = 15f;
    [SerializeField] private float _bashCooldown = 1f;

    [Header("Input Keys")]
    [SerializeField] private KeyCode _moveUpKey = KeyCode.W;
    [SerializeField] private KeyCode _moveDownKey = KeyCode.S;
    [SerializeField] private KeyCode _moveLeftKey = KeyCode.A;
    [SerializeField] private KeyCode _moveRightKey = KeyCode.D;

    [Header("Health")]
    [SerializeField] private int _maxHealth = 100;
    private int _currentHealth;

    [Header("References")]
    [SerializeField] private HeldItems _heldItems;
    private RoomManager _roomManager;
    private Room _currentRoom;
    private Rigidbody2D _rb;
    private Collider2D _collider;

    private bool _isDashing;
    private float _dashTimeLeft;
    private float _bashCooldownTimeLeft;
    private Vector2 _dashDirection;
    private Vector2 _movement;
    private Vector2 _preBashVelocity;
    public float _currentSpeed;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _roomManager = FindObjectOfType<RoomManager>();
        _heldItems = GetComponent<HeldItems>();
        _currentHealth = _maxHealth;
    }

    private void Update()
    {
        // Get input for movement using Unity's Input system
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Create a movement vector
        _movement = new Vector2(horizontalInput, verticalInput);

        // Normalize the movement vector to ensure consistent speed in all directions
        _movement = _movement.normalized;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key pressed");
            if (CanBash())
            {
                Debug.Log("Bashing.......");
                StartBash();
            }
            else
            {
                Debug.Log("Cannot bash right now");
            }
        }

        UpdateCooldowns();
        // Debug log to check input and movement
        Debug.Log($"Input: ({horizontalInput}, {verticalInput}), Movement: {_movement}, Position: {transform.position}");

        if (_isDashing && _rb.velocity.magnitude <= _speed)
        {
            StopBash();
        }
    }

    private void FixedUpdate()
    {
        if (_isDashing)
        {
            float elapsedTime = _bashDuration - _dashTimeLeft;
            _currentSpeed = CalculateDecayingSpeed(elapsedTime);
            _rb.velocity = _dashDirection * _currentSpeed;

            _dashTimeLeft -= Time.fixedDeltaTime;
            if (_dashTimeLeft <= 0 || _currentSpeed <= _speed)
            {
                StopBash();
            }
        }
        else
        {
            MovePlayer();
        }
    }
private float CalculateDecayingSpeed(float elapsedTime)
{
    float t = elapsedTime / _bashDuration;
    float decayedSpeed = Mathf.Lerp(_bashInitialSpeed, _speed, t);
    return Mathf.Max(decayedSpeed, _speed);
}
private float CalculateExponentialDecay(float initialValue, float decayRate, float time)
{
    return initialValue * Mathf.Exp(-decayRate * time);
}


    private void MovePlayer()
    {
        _currentSpeed = _speed;
        _rb.velocity = _movement * _currentSpeed;
    }

    public Vector2 GetCurrentVelocity()
    {
        return _rb.velocity;
    }

    private bool CanBash()
    {
        return !_isDashing && _bashCooldownTimeLeft <= 0;
    }

    private void StartBash()
    {
        Debug.Log("BASH STARTED!");
        _isDashing = true;
        _dashTimeLeft = _bashDuration;
        _dashDirection = _movement != Vector2.zero ? _movement : transform.right;
        _preBashVelocity = _rb.velocity;
        _rb.velocity = _dashDirection * _bashInitialSpeed;
        _bashCooldownTimeLeft = _bashCooldown;
    }

    private void StopBash()
    {
        _isDashing = false;
    }

    private void UpdateCooldowns()
    {
        if (_bashCooldownTimeLeft > 0)
        {
            _bashCooldownTimeLeft -= Time.deltaTime;
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _heldItems.TriggerItemEffects(ItemEffectTrigger.OnDamageTaken);
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Handle player death
        Debug.Log("Player has died!");
        // You might want to reload the level, show a game over screen, etc.
    }

    public void Heal(int amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
    }

    public void PickupItem(ItemData item)
    {
        _heldItems.AddItem(item);
        _heldItems.TriggerItemEffects(ItemEffectTrigger.OnItemPickup);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Handle collisions with items, enemies, etc.
    }

    public void SetRoomManager(RoomManager roomManager)
    {
        _roomManager = roomManager;
    }

    public void UseActiveItem()
    {
        // Implement the logic for using the active item
        // This method should be called when the player wants to use their active item
    }
}