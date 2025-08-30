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
        // Inicializar posición del jugador en la parte inferior de la pantalla
        transform.position = new Vector3(0, -5f, 0);
        currentHealth = maxHealth;
    }
    
    void Update()
    {
        HandleMovement();
        HandleShooting();
    }
    
    // Maneja el movimiento del jugador basado en las teclas de flecha
    void HandleMovement()
    {
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
        
        // Calcular nueva posición
        movement = new Vector3(horizontal, vertical, 0).normalized;
        Vector3 newPosition = transform.position + movement * moveSpeed * Time.deltaTime;
        
        // Restringir movimiento dentro de los límites
        newPosition.x = Mathf.Clamp(newPosition.x, -moveBounds, moveBounds);
        newPosition.y = Mathf.Clamp(newPosition.y, -6f, moveBoundsY);
        
        transform.position = newPosition;
    }
    
    // Maneja el disparo automático del jugador
    void HandleShooting()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }
    
    // Instancia y configura una bala disparada por el jugador
    void Shoot()
    {
        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position + Vector3.up * 0.5f;
        
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        
        // Configurar bala para disparar hacia arriba
        bulletScript.Initialize(Vector3.up, 8f, Color.yellow, true);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si colisiona con una bala enemiga
        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet != null && !bullet.isPlayerBullet)
        {
            // Destruir la bala enemiga
            Destroy(other.gameObject);
            
            // Aplicar daño al jugador
            TakeDamage(1);
        }
    }
    
    // Reduce la salud del jugador y verifica si muere
    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    // Maneja la muerte del jugador
    void Die()
    {
        // Destruir el objeto del jugador
        Destroy(gameObject);
    }
}