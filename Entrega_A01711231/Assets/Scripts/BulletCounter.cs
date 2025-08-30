using UnityEngine;
using UnityEngine.UI;

public class BulletCounter : MonoBehaviour
{
    public static BulletCounter Instance;
    
    [Header("UI")]
    public Text counterText;
    
    private int activeBullets = 0;
    
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
    
    public void AddBullet()
    {
        activeBullets++;
        UpdateUI();
    }
    
    public void RemoveBullet()
    {
        activeBullets--;
        if (activeBullets < 0) activeBullets = 0;
        UpdateUI();
    }
    
    void UpdateUI()
    {
        if (counterText != null)
        {
            counterText.text = "Balas Activas: " + activeBullets.ToString();
        }
    }
}