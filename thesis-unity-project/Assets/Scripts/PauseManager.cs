using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private InputManager inputManager;

    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject canvasBackground;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button resumeButton;
    
    [Header("Loading Settings")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingSlider;

    private void Awake() {
        inputManager = FindAnyObjectByType<InputManager>();
    }

    private void Start() {
        pause.SetActive(false);
        optionButton.onClick.AddListener(Options);
        resumeButton.onClick.AddListener(ContinueGame);
    }

    public void PauseGame()
    {
        canvasBackground.SetActive(true);

        pause.SetActive(true);
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        Time.timeScale = 0; // Pause
    }

    public void ContinueGame()
    {
        canvasBackground.SetActive(false);

        pause.SetActive(false);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        Time.timeScale = 1; // Resume game
        EventSystem.current.SetSelectedGameObject(null);

        GameManager.Instance.menuIsOpen = false;
    }

    public void Options()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void LoadLevelButton(string levelToLoad)
    {
        loadingScreen.SetActive(true);

        StartCoroutine(LoadLevelAsync(levelToLoad));
    }

    private IEnumerator LoadLevelAsync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
        
        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;
            yield return null;
        }
    }
}
