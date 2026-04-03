using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [Header("Screens")]
    [SerializeField] private List<GameObject> registeredScreens;

    [Header("Startup")]
    [Tooltip("Name des Screens der beim Start angezeigt wird. Leer lassen = alle aus.")]
    [SerializeField] private string startScreenID;

    [Header("Pause")]
    [Tooltip("Screen der bei Pause angezeigt wird.")]
    [SerializeField] private string pauseScreenID;
    [Tooltip("Leer lassen wenn kein Keyboard-Shortcut gewünscht.")]
    [SerializeField] private Key pauseKey = Key.Escape;
    [SerializeField] private bool usePauseKey = false;

    private bool isPaused = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        if (!string.IsNullOrEmpty(startScreenID))
            ShowScreen(startScreenID);
        else
            HideAll();
    }

    void Update()
    {
        if (!usePauseKey) return;
        if (string.IsNullOrEmpty(pauseScreenID)) return;

        if (Keyboard.current[pauseKey].wasPressedThisFrame)
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void ShowScreen(string id)
    {
        bool found = false;

        foreach (var screen in registeredScreens)
        {
            if (screen == null) continue;
            bool isTarget = screen.name == id;
            screen.SetActive(isTarget);
            if (isTarget) found = true;
        }

        if (!found)
            Debug.LogWarning($"[MenuManager] Screen '{id}' nicht gefunden.");
    }

    public void HideAll()
    {
        foreach (var screen in registeredScreens)
        {
            if (screen != null)
                screen.SetActive(false);
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
        ShowScreen(pauseScreenID);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1;
        HideAll();
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
}