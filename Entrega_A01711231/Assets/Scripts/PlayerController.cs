using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float moveBounds = 8f; // Límites de movimiento en X
    public float moveBoundsY = 4f; // Límites de movimiento en Y
    
    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.3f; // Tiempo entre disparos automáticos
    
    [Header("Health")]
    public int maxHealth = 3;
    private int currentHealth;
    
    private float nextFireTime = 0f;
    private Vector3 movement;
    
    void Start()
    {
        // Posicionar al jugador en la parte inferior de la pantalla
        transform.position = new Vector3(0, -5f, 0);
        currentHealth = maxHealth;
    }
    
    void Update()
    {
        HandleMovement();
        HandleShooting();
    }
    
    void HandleMovement()
    {
        // Obtener input de las flechas
        float horizontal = 0f;
        float vertical = 0f;
        
        if (Input.GetKey(KeyCode.LeftArrow))
            horizontal = -1f;
        else if (Input.GetKey(KeyCode.RightArrow))
            horizontal = 1f;
            
        if (Input.GetKey(KeyCode.UpArrow))
            vertical = 1f;
        else if (Input.GetKey(KeyCode.DownArrow))
            vertical = -1f;
        
        // Aplicar movimiento
        movement = new Vector3(horizontal, vertical, 0).normalized;
        Vector3 newPosition = transform.position + movement * moveSpeed * Time.deltaTime;
        
        // Clampear posición dentro de los límites
        newPosition.x = Mathf.Clamp(newPosition.x, -moveBounds, moveBounds);
        newPosition.y = Mathf.Clamp(newPosition.y, -6f, moveBoundsY);
        
        transform.position = newPosition;
    }
    
    void HandleShooting()
    {
        // Disparo automático
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }
    
    void Shoot()
    {
        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position + Vector3.up * 0.5f;
        
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        
        // Disparar hacia arriba (hacia el jefe)
        bulletScript.Initialize(Vector3.up, 8f, Color.yellow, true);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Detectar colisión con balas del jefe
        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet != null && !bullet.isPlayerBullet)
        {
            Debug.Log("¡Jugador golpeado por bala del jefe!");
            // Destruir la bala del jefe
            Destroy(other.gameObject);
            
            // Reducir vida del jugador
            TakeDamage(1);
        }
    }
    
    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player HP: {currentHealth}/{maxHealth}");
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        Debug.Log("¡El jugador ha muerto!");
        // Destruir el jugador
        Destroy(gameObject);
    }
}