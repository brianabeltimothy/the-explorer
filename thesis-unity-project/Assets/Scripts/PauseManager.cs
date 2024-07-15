using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour
{
    private InputManager inputManager;
    private bool IsPaused = false;

    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject canvasBackground;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button resumeButton;

    private void Awake() {
        inputManager = FindAnyObjectByType<InputManager>();
    }

    private void Start() {
        pause.SetActive(false);
        optionButton.onClick.AddListener(Options);
        resumeButton.onClick.AddListener(ContinueGame);
    }

    // private void Update()
    // {
    //     if(inputManager.Pause)
    //     {
    //         PauseGame();
    //     }
    // }

    public void PauseGame()
    {
        IsPaused = true;
        canvasBackground.SetActive(true);

        pause.SetActive(true);
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        Time.timeScale = 0; // Pause
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
        Cursor.visible = true;
    }

    public void ContinueGame()
    {
        IsPaused = false;
        canvasBackground.SetActive(false);

        pause.SetActive(false);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        Time.timeScale = 1; // Resume game
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor
        Cursor.visible = false;
        EventSystem.current.SetSelectedGameObject(null);

        MenuManager.Instance.menuIsOpen = false;
    }

    public void Options()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void MainMenu()
    {
        //go to main menu
    }
}
