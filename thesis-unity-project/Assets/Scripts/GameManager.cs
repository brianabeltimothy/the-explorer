using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    [SerializeField] private VideoClip cutscene1;
    [SerializeField] private Transform savePoint1;

    public static GameManager Instance;
    public bool menuIsOpen;

    private PauseManager pauseManager;
    private InventoryManager inventoryManager;
    private InputManager inputManager;
    private Player player;
    private GameObject playerObject;

    private void Awake()
    {
        Instance = this;

        playerObject = GameObject.FindWithTag("Player");
        player = playerObject.GetComponent<Player>();

        inputManager = FindAnyObjectByType<InputManager>();
        inventoryManager = FindAnyObjectByType<InventoryManager>();
        pauseManager = FindAnyObjectByType<PauseManager>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CutsceneManager.Instance.PlayCutscene(cutscene1, OnCutsceneEnd);
        playerObject.transform.position = savePoint1.position;
    }

    private void Update()
    {
        HandleMenuInput();
        HandlePlayerState();
    }

    private void HandleMenuInput()
    {
        if (inputManager.Inventory)
        {
            if (inventoryManager.inventoryIsOpened)
            {
                inventoryManager.CloseInventory();
                menuIsOpen = false;
            }
            else if (!menuIsOpen)
            {
                inventoryManager.OpenInventory();
                menuIsOpen = true;
            }
        }
        else if (inputManager.Pause && !menuIsOpen)
        {
            pauseManager.PauseGame();
            menuIsOpen = true;
        }

        if (menuIsOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void HandlePlayerState()
    {
        if (player.currentHits == player.maxHits && !UIManager.Instance.youDieInProgress)
        {
            StartCoroutine(UIManager.Instance.ShowGameOverScreen());
        }

        if (player.isInteracting)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnCutsceneEnd()
    {
        StartCoroutine(DisplayInstructionText());
    }

    private IEnumerator DisplayInstructionText()
    {
        UIManager.Instance.ChangeInstructionText("");
        UIManager.Instance.ChangeInstructionText("Press [F] to turn on Flashlight");
        yield return new WaitForSeconds(3f);
        UIManager.Instance.ChangeInstructionText("");
    }
}