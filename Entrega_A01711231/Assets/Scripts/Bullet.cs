using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private bool useWaveMotion = false;
    private float wavePhase = 0f;
    private Vector3 baseDirection;
    public bool isPlayerBullet = false; // Indica si la bala pertenece al jugador
    
    void Update()
    {
        MoveBullet();
        CheckBounds();
    }
    
    // Inicializa la bala con dirección, velocidad, color y tipo
    public void Initialize(Vector3 dir, float spd, Color color, bool isPlayerBullet = false)
    {
        direction = dir.normalized;
        baseDirection = direction;
        speed = spd;
        this.isPlayerBullet = isPlayerBullet;
        
        // Cambiar color del sprite
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = color;
        
        // Registrar la bala en el contador
        BulletCounter.Instance.AddBullet(this.isPlayerBullet);
    }
    
    // Configura el movimiento ondulante
    public void SetWaveMotion(bool useWave, float phase)
    {
        useWaveMotion = useWave;
        wavePhase = phase;
    }
    
    // Mueve la bala según su configuración
    void MoveBullet()
    {
        if (useWaveMotion)
        {
            // Movimiento ondulante
            float wave = Mathf.Sin(Time.time * 3f + wavePhase) * 0.5f;
            Vector3 waveDirection = baseDirection + new Vector3(wave, 0, 0);
            transform.position += waveDirection.normalized * speed * Time.deltaTime;
        }
        else
        {
            // Movimiento recto
            transform.position += direction * speed * Time.deltaTime;
        }
    }
    
    // Verifica si la bala sale de los límites de la pantalla
    void CheckBounds()
    {
        // Destruir si sale de pantalla
        if (transform.position.x < -10 || transform.position.x > 10 ||
            transform.position.y < -8 || transform.position.y > 8)
        {
            DestroyBullet();
        }
    }
    
    // Destruye la bala y actualiza el contador
    void DestroyBullet()
    {
        BulletCounter.Instance.RemoveBullet(isPlayerBullet);
        Destroy(gameObject);
    }
    
    void OnDestroy()
    {
        // Asegurarse de que se reste del contador al destruirse
        if (BulletCounter.Instance != null)
            BulletCounter.Instance.RemoveBullet(isPlayerBullet);
    }
}