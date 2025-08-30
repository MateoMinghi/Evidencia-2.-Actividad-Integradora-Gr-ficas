using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public Text patternText;
    public Text timerText;
    
    [Header("Boss Reference")]
    public BossController bossController;
    
    private float gameTimer = 0f;
    private int currentPattern = 1;
    
    void Start()
    {
        // Inicializar UI
        UpdatePatternUI();
    }
    
    void Update()
    {
        gameTimer += Time.deltaTime;
        
        // Determinar patrón actual basado en el tiempo
        int patternIndex = Mathf.FloorToInt(gameTimer / 10f) % 3; // 0, 1, 2
        int displayPattern = patternIndex + 1; // 1, 2, 3 para mostrar
        
        if (displayPattern != currentPattern)
        {
            currentPattern = displayPattern;
        }
        
        UpdatePatternUI();
    }
    
    // Actualiza la interfaz de usuario con el patrón y tiempo restante
    void UpdatePatternUI()
    {
        if (patternText != null)
        {
            string patternName = GetPatternName(currentPattern);
            string newText = "Patrón " + currentPattern + ": " + patternName;
            patternText.text = newText;
        }
        
        if (timerText != null)
        {
            float patternTime = gameTimer % 10f;
            float remainingTime = 10f - patternTime;
            string newTimerText = "Tiempo restante: " + remainingTime.ToString("F1") + "s";
            timerText.text = newTimerText;
        }
        
        // Forzar actualización completa del Canvas
        Canvas.ForceUpdateCanvases();
    }
    
    // Obtiene el nombre del patrón basado en el número
    string GetPatternName(int pattern)
    {
        switch (pattern)
        {
            case 1: return "Espiral Rotativa";
            case 2: return "Ondas Sinusoidales";
            case 3: return "Rafagas Moviles";
            default: return "Desconocido";
        }
    }
}