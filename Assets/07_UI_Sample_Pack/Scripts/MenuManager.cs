using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [Header("Screens")]
    [Tooltip("Alle Screens die der Manager verwalten soll.")]
    [SerializeField] private List<GameObject> screens;

    [Header("Startup")]
    [Tooltip("Screen der beim Start der Scene angezeigt wird. Leer lassen = alle aus.")]
    [SerializeField] private GameObject startScreen;

    [Header("Pause")]
    [Tooltip("Screen der bei Pause angezeigt wird.")]
    [SerializeField] private GameObject pauseScreen;
    [Tooltip("Pausiert das Spiel und zeigt den Pause-Screen. Pause-Key muss gesetzt sein.")]
    [SerializeField] private bool usePauseKey = false;
    [Tooltip("Taste zum Pausieren. Nur aktiv wenn usePauseKey aktiviert ist.")]
    [SerializeField] private Key pauseKey = Key.Escape;

    [Header("Overlay (optional)")]
    [Tooltip("Screen der bei Overlay angezeigt wird. Z.B. Inventar oder In-Game-Menu.")]
    [SerializeField] private GameObject overlayScreen;
    [Tooltip("Zeigt Overlay-Screen und pausiert das Spiel. Overlay-Key muss gesetzt sein.")]
    [SerializeField] private bool useOverlayKey = false;
    [Tooltip("Taste zum Öffnen des Overlays. Nur aktiv wenn useOverlayKey aktiviert ist.")]
    [SerializeField] private Key overlayKey = Key.Tab;

    private bool isPaused = false;
    private bool isOverlayOpen = false;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        if (startScreen != null)
            ShowScreen(startScreen);
        else
            HideAll();
    }

    void Update()
    {
        if (usePauseKey && pauseScreen != null)
            if (Keyboard.current[pauseKey].wasPressedThisFrame)
            {
                if (isPaused) Resume();
                else Pause();
            }

        if (useOverlayKey && overlayScreen != null)
            if (Keyboard.current[overlayKey].wasPressedThisFrame)
            {
                if (isOverlayOpen) CloseOverlay();
                else OpenOverlay();
            }
    }

    public void ShowScreen(GameObject target)
    {
        foreach (var screen in screens)
            if (screen != null)
                screen.SetActive(screen == target);
    }

    public void HideAll()
    {
        foreach (var screen in screens)
            if (screen != null)
                screen.SetActive(false);
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
        ShowScreen(pauseScreen);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1;
        HideAll();
    }

    public void OpenOverlay()
    {
        if (isPaused) return; // Kein Overlay während Pause
        isOverlayOpen = true;
        Time.timeScale = 0;
        ShowScreen(overlayScreen);
    }

    public void CloseOverlay()
    {
        isOverlayOpen = false;
        Time.timeScale = 1;
        HideAll();
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
}