using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("Boss Settings")]
    public float moveSpeed = 2f;
    public float moveRange = 3f;
    
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform[] firePoints; // Puntos de disparo múltiples
    
    [Header("Pattern Timing")]
    public float patternDuration = 10f;
    
    [Header("Health")]
    public int maxHealth = 20;
    private int currentHealth;
    
    private int currentPattern = 0;
    private Vector3 startPosition;
    private bool movingRight = true;
    
    // Variables para patrones
    private float spiralAngle = 0f;
    private float waveTime = 0f;
    
    void Start()
    {
        startPosition = transform.position;
        currentHealth = maxHealth;
        StartCoroutine(PatternSequence());
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Detectar colisión con balas del jugador
        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet != null && bullet.isPlayerBullet)
        {
            // Destruir la bala del jugador
            Destroy(other.gameObject);
            
            // Reducir vida del jefe
            TakeDamage(1);
        }
    }
    
    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        // Detener todas las corrutinas
        StopAllCoroutines();
        // Destruir el jefe
        Destroy(gameObject);
    }
    
    void Update()
    {
        // Movimiento básico del jefe solo durante el patrón 3
        if (currentPattern == 2) // Patrón 3 (índice 2)
        {
            MoveBoss();
        }
    }
    
    void MoveBoss()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            if (transform.position.x > startPosition.x + moveRange)
                movingRight = false;
        }
        else
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            if (transform.position.x < startPosition.x - moveRange)
                movingRight = true;
        }
    }
    
    IEnumerator PatternSequence()
    {
        for (int pattern = 0; pattern < 3; pattern++)
        {
            currentPattern = pattern;
            yield return StartCoroutine(ExecutePattern(pattern));
        }
        
        // Reiniciar la secuencia
        StartCoroutine(PatternSequence());
    }
    
    IEnumerator ExecutePattern(int patternIndex)
    {
        float timer = 0f;
        
        // Resetear posición del jefe al inicio de cada patrón
        // Solo los patrones 1 y 2 resetean a la posición inicial
        if (patternIndex != 2)
        {
            transform.position = startPosition;
        }
        
        while (timer < patternDuration)
        {
            switch (patternIndex)
            {
                case 0:
                    SpiralPattern();
                    yield return new WaitForSeconds(0.1f);
                    timer += 0.1f;
                    break;
                case 1:
                    WavePattern();
                    yield return new WaitForSeconds(0.15f);
                    timer += 0.15f;
                    break;
                case 2:
                    MultiPointPattern();
                    yield return new WaitForSeconds(0.3f);
                    timer += 0.3f;
                    break;
            }
        }
    }
    
    // Patrón 1: Espiral rotativa
    void SpiralPattern()
    {
        int bulletsPerRing = 6;
        
        for (int i = 0; i < bulletsPerRing; i++)
        {
            float angle = (360f / bulletsPerRing) * i + spiralAngle;
            Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.up;
            
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.Initialize(direction, 3f, Color.red);
        }
        
        spiralAngle += 15f; // Rotación de la espiral
    }
    
    // Patrón 2: Ondas sinusoidales
    void WavePattern()
    {
        int bulletCount = 3;
        
        for (int i = 0; i < bulletCount; i++)
        {
            float offsetX = (i - bulletCount/2) * 0.5f;
            Vector3 spawnPos = transform.position + new Vector3(offsetX, 0, 0);
            
            // Dirección base hacia abajo con componente sinusoidal
            float waveOffset = Mathf.Sin(waveTime + i) * 0.5f;
            Vector3 direction = new Vector3(waveOffset, -1, 0).normalized;
            
            GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.Initialize(direction, 4f, Color.blue);
            bulletScript.SetWaveMotion(true, waveTime + i);
        }
        
        waveTime += Time.deltaTime * 5f;
    }
    
    // Patrón 3: Ráfagas en cruz
    void MultiPointPattern()
    {
        // Patrón reducido: solo 4 direcciones principales (cruz)
        Vector3[] directions = {
            Vector3.up,        // Arriba
            Vector3.down,      // Abajo  
            Vector3.left,      // Izquierda
            Vector3.right      // Derecha
        };
        
        // Crear una bala en cada dirección
        for (int i = 0; i < directions.Length; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.Initialize(directions[i], 4f, Color.green);
        }
    }
}