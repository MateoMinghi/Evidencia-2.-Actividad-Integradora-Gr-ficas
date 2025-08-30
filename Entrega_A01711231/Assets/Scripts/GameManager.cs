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
        Debug.Log("GameManager Start - Verificando referencias UI:");
        Debug.Log($"patternText assigned: {patternText != null}");
        Debug.Log($"timerText assigned: {timerText != null}");
        
        // Inicializar UI
        UpdatePatternUI();
    }
    
    void Update()
    {
        gameTimer += Time.deltaTime;
        
        // Determinar patrón actual basado en el tiempo
        // Usar la misma lógica que BossController: 0-based indexing pero mostrar 1-based
        int patternIndex = Mathf.FloorToInt(gameTimer / 10f) % 3; // 0, 1, 2
        int displayPattern = patternIndex + 1; // 1, 2, 3 para mostrar
        
        if (displayPattern != currentPattern)
        {
            currentPattern = displayPattern;
            Debug.Log($"GameManager - Cambiando a patrón {currentPattern} (índice {patternIndex})");
        }
        
        UpdatePatternUI();
    }
    
    void UpdatePatternUI()
    {
        Debug.Log($"UpdatePatternUI - currentPattern: {currentPattern}, gameTimer: {gameTimer:F1}");
        
        if (patternText != null)
        {
            string patternName = GetPatternName(currentPattern);
            string newText = "Patrón " + currentPattern + ": " + patternName;
            patternText.text = newText;
            
            // Forzar actualización del UI
            patternText.enabled = false;
            patternText.enabled = true;
            
            Debug.Log($"Pattern text updated to: {newText}");
        }
        else
        {
            Debug.LogError("patternText is NULL! Check if it's assigned in the Inspector.");
        }
        
        if (timerText != null)
        {
            float patternTime = gameTimer % 10f;
            float remainingTime = 10f - patternTime;
            string newTimerText = "Tiempo restante: " + remainingTime.ToString("F1") + "s";
            timerText.text = newTimerText;
            
            // Forzar actualización del UI
            timerText.enabled = false;
            timerText.enabled = true;
            
            Debug.Log($"Timer text updated to: {newTimerText}");
        }
        else
        {
            Debug.LogError("timerText is NULL! Check if it's assigned in the Inspector.");
        }
        
        // Forzar actualización completa del Canvas
        Canvas.ForceUpdateCanvases();
    }
    
    string GetPatternName(int pattern)
    {
        switch (pattern)
        {
            case 1: return "Espiral Rotativa";
            case 2: return "Ondas Sinusoidales";
            case 3: return "Rafagas Moviles"; // Ahora con movimiento
            default: return "Desconocido";
        }
    }
}