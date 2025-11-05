using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SelectMenuUI : MonoBehaviour
{

    public CharacterData[] characters;

    public Image topPlayerImage;
    public TMP_Text topPlayerName;
    public TMP_Text topPlayerText;
    public Button topPrevButton;
    public Button topNextButton;

    public Image bottomPlayerImage;
    public TMP_Text bottomPlayerName;
    public TMP_Text bottomPlayerText;
    public Button bottomPrevButton;
    public Button bottomNextButton;

    public TMP_Text modeName;
    public TMP_Text modeText;
    public Button modePrevButton;
    public Button modeNextButton;

    public Button playButton;
    public Button lockInButton;

    public string gameSceneName = "Game";
    public string mainMenuSceneName = "MainMenu";

    private int topIndex = 0;
    private int bottomIndex = 0;

    private string[] modes = { "1v1 - Solo", "1v1 - Co-op", "3v3 - Solo", "3v3 - Co-op" };
    private int modeIndex = 0;

    private bool lockedIn = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (characters != null && characters.Length > 0)
        {
            UpdateTopUI();
            UpdateBottomUI();
        }

        if (modes.Length > 0)
        {
            UpdateModeUI();
        }

        if (playButton != null)
        {
            playButton.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Top Player

    public void OnTopNext()
    {
        if (lockedIn) return;
        if (!isCoopMode()) return;

        topIndex = (topIndex + 1) % characters.Length;
        UpdateTopUI();
    }

    public void OnTopPrev()
    {
        if (lockedIn) return;
        if (!isCoopMode()) return;

        topIndex = (topIndex - 1 + characters.Length) % characters.Length;
        UpdateTopUI();
    }

    void UpdateTopUI()
    {
        if (characters.Length == 0) return;
        var c = characters[topIndex];

        if (topPlayerImage != null)
        {
            topPlayerImage.sprite = c.sprite;
        }

        if (topPlayerName != null)
        {
            topPlayerName.text = c.characterName;
        }
    }

    // Bottom Player

    public void OnBottomNext()
    {
        if (lockedIn) return;
        bottomIndex = (bottomIndex + 1) % characters.Length;
        UpdateBottomUI();
    }

    public void OnBottomPrev()
    {
        if (lockedIn) return;
        bottomIndex = (bottomIndex - 1 + characters.Length) % characters.Length;
        UpdateBottomUI();
    }

    void UpdateBottomUI()
    {
        if (characters.Length == 0) return;
        var c = characters[bottomIndex];

        if (bottomPlayerImage != null)
        {
            bottomPlayerImage.sprite = c.sprite;
        }

        if (bottomPlayerName != null)
        {
            bottomPlayerName.text = c.characterName;
        }
    }

    // Mode Selection

    public void OnModeNext()
    {
        if (lockedIn) return;
        modeIndex = (modeIndex + 1) % modes.Length;
        UpdateModeUI();
    }

    public void OnModePrev()
    {
        if (lockedIn) return;
        modeIndex = (modeIndex - 1 + modes.Length) % modes.Length;
        UpdateModeUI();
    }

    void UpdateModeUI()
    {
        if (modeName != null)
        {
            modeName.text = modes[modeIndex];
        }

        UpdateTopInteractable();
    }

    // Lock In & Play

    public void OnLockIn()
    {
        lockedIn = true;

        if (playButton != null)
        {
            playButton.interactable = true;
        }

        if (lockInButton != null)
        {
            lockInButton.interactable = false;
        }

        UpdateTopInteractable();
        UpdateBottomInteractable();
        UpdateModeInteractable();
    }

    public void OnPlay()
    {
        GameData.topPlayerName = characters[topIndex].characterName;
        GameData.bottomPlayerName = characters[bottomIndex].characterName;
        GameData.selectedMode = modes[modeIndex];

        SceneManager.LoadScene(gameSceneName);
    }

    public void OnBackToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // Disable Top

    private bool isCoopMode()
    {
        return modes[modeIndex].Contains("Co-op");
    }

    private void UpdateTopInteractable()
    {
        bool canInteract = !lockedIn && isCoopMode();

        if (topPrevButton != null) topPrevButton.interactable = canInteract;
        if (topNextButton != null) topNextButton.interactable = canInteract;

        if (topPlayerName != null) topPlayerName.alpha = canInteract ? 1f : 0.4f;
        if (topPlayerText != null) topPlayerText.alpha = canInteract ? 1f : 0.4f;

        if (topPrevButton != null) topPrevButton.GetComponentInChildren<TMP_Text>().alpha = canInteract ? 1f : 0.4f;
        if (topNextButton != null) topNextButton.GetComponentInChildren<TMP_Text>().alpha = canInteract ? 1f : 0.4f;

        if (topPlayerImage != null)
        {
            Color c = topPlayerImage.color;
            c.a = canInteract ? 1f : 0.4f;
            topPlayerImage.color = c;
        }
    }

    private void UpdateBottomInteractable()
    {
        bool canInteract = !lockedIn;

        if (bottomPrevButton != null) bottomPrevButton.interactable = canInteract;
        if (bottomNextButton != null) bottomNextButton.interactable = canInteract;

        if (bottomPlayerName != null) bottomPlayerName.alpha = canInteract ? 1f : 0.4f;
        if (bottomPlayerText != null) bottomPlayerText.alpha = canInteract ? 1f : 0.4f;

        if (bottomPrevButton != null) bottomPrevButton.GetComponentInChildren<TMP_Text>().alpha = canInteract ? 1f : 0.4f;
        if (bottomNextButton != null) bottomNextButton.GetComponentInChildren<TMP_Text>().alpha = canInteract ? 1f : 0.4f;

        if (bottomPlayerImage != null)
        {
            Color c = bottomPlayerImage.color;
            c.a = canInteract ? 1f : 0.4f;
            bottomPlayerImage.color = c;
        }
    }

    private void UpdateModeInteractable()
    {
        bool canInteract = !lockedIn;

        if (modePrevButton != null) modePrevButton.interactable = canInteract;
        if (modeNextButton != null) modeNextButton.interactable = canInteract;

        if (modeName != null) modeName.alpha = canInteract ? 1f : 0.4f;
        if (modeText != null) modeText.alpha = canInteract ? 1f : 0.4f;

        if (modePrevButton != null) modePrevButton.GetComponentInChildren<TMP_Text>().alpha = canInteract ? 1f : 0.4f;
        if (modeNextButton != null) modeNextButton.GetComponentInChildren<TMP_Text>().alpha = canInteract ? 1f : 0.4f;
    }
}
