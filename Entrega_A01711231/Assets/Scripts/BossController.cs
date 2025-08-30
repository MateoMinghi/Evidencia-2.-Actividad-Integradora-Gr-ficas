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
    
    private int currentPattern = 0;
    private Vector3 startPosition;
    private bool movingRight = true;
    
    // Referencias para patrones
    private float spiralAngle = 0f;
    private float waveTime = 0f;
    
    void Start()
    {
        startPosition = transform.position;
        StartCoroutine(PatternSequence());
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
        
        // Debug log para ver qué patrón se está ejecutando
        Debug.Log($"Ejecutando patrón {patternIndex} (Patrón {patternIndex + 1} en UI)");
        
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
                    Debug.Log("Ejecutando MultiPointPattern - Patrón 3");
                    MultiPointPattern();
                    yield return new WaitForSeconds(0.3f); // Más lento para ráfagas más intensas
                    timer += 0.3f;
                    break;
            }
        }
    }
    
    // Patrón 1: Espiral rotativa
    void SpiralPattern()
    {
        int bulletsPerRing = 8;
        
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
        int bulletCount = 5;
        
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
    
    // Patrón 3: Ráfagas en cruz - SIMPLIFICADO
    void MultiPointPattern()
    {
        Debug.Log("Creando balas del patrón 3...");
        
        // Patrón simple: 4 direcciones principales (cruz) + diagonales
        Vector3[] directions = {
            Vector3.up,        // Arriba
            Vector3.down,      // Abajo  
            Vector3.left,      // Izquierda
            Vector3.right,     // Derecha
            new Vector3(1, 1, 0).normalized,   // Diagonal arriba-derecha
            new Vector3(-1, 1, 0).normalized,  // Diagonal arriba-izquierda
            new Vector3(1, -1, 0).normalized,  // Diagonal abajo-derecha
            new Vector3(-1, -1, 0).normalized  // Diagonal abajo-izquierda
        };
        
        // Crear una bala en cada dirección
        for (int i = 0; i < directions.Length; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.Initialize(directions[i], 4f, Color.green);
            Debug.Log($"Bala {i + 1} creada en dirección: {directions[i]}");
        }
    }
}