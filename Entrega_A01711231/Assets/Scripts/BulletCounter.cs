using UnityEngine;
using UnityEngine.UI;

public class BulletCounter : MonoBehaviour
{
    public static BulletCounter Instance;
    
    [Header("UI")]
    public Text counterText;
    
    private int activeBullets = 0;
    private int playerBullets = 0;
    private int bossBullets = 0;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        UpdateUI();
    }
    
    public void AddBullet(bool isPlayerBullet = false)
    {
        activeBullets++;
        if (isPlayerBullet)
            playerBullets++;
        else
            bossBullets++;
        UpdateUI();
    }
    
    public void RemoveBullet(bool isPlayerBullet = false)
    {
        activeBullets--;
        if (activeBullets < 0) activeBullets = 0;
        
        if (isPlayerBullet)
        {
            playerBullets--;
            if (playerBullets < 0) playerBullets = 0;
        }
        else
        {
            bossBullets--;
            if (bossBullets < 0) bossBullets = 0;
        }
        UpdateUI();
    }
    
    void UpdateUI()
    {
        if (counterText != null)
        {
            counterText.text = $"Balas Activas: {activeBullets} (Jefe: {bossBullets}, Jugador: {playerBullets})";
        }
    }
}