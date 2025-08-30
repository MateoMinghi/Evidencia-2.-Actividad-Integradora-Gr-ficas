using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private bool useWaveMotion = false;
    private float wavePhase = 0f;
    private Vector3 baseDirection;
    
    void Update()
    {
        MoveBullet();
        CheckBounds();
    }
    
    public void Initialize(Vector3 dir, float spd, Color color)
    {
        direction = dir.normalized;
        baseDirection = direction;
        speed = spd;
        
        // Cambiar color del sprite
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = color;
        
        // Registrar la bala
        BulletCounter.Instance.AddBullet();
    }
    
    public void SetWaveMotion(bool useWave, float phase)
    {
        useWaveMotion = useWave;
        wavePhase = phase;
    }
    
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
    
    void CheckBounds()
    {
        // Destruir si sale de pantalla
        if (transform.position.x < -10 || transform.position.x > 10 ||
            transform.position.y < -8 || transform.position.y > 8)
        {
            DestroyBullet();
        }
    }
    
    void DestroyBullet()
    {
        BulletCounter.Instance.RemoveBullet();
        Destroy(gameObject);
    }
    
    void OnDestroy()
    {
        // Asegurarse de que se reste del contador
        if (BulletCounter.Instance != null)
            BulletCounter.Instance.RemoveBullet();
    }
}