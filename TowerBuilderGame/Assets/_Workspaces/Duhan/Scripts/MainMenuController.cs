using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Paneller")]
    public GameObject pnlMainMenu;
    public GameObject pnlModeSelection;
    public GameObject pnlHowToPlay;
    public GameObject pnlSettings;

    // YENÝ: Hangi butona basýldýðýný (Single/Multi) aklýmýzda tutacaðýmýz deðiþken
    private string selectedPlayerMode = "Singleplayer";

    public void OpenPanel(GameObject panelToOpen)
    {
        pnlMainMenu.SetActive(false);
        pnlModeSelection.SetActive(false);
        pnlHowToPlay.SetActive(false);
        pnlSettings.SetActive(false);

        panelToOpen.SetActive(true);
    }

    public void BackToMainMenu()
    {
        OpenPanel(pnlMainMenu);
    }

    public void OnClickSingleplayer()
    {
        selectedPlayerMode = "Singleplayer"; // Kiþi sayýsýný hafýzaya al
        OpenPanel(pnlModeSelection); // Mod seçim ekranýný aç
    }

    public void OnClickMultiplayer()
    {
        selectedPlayerMode = "Multiplayer"; // Kiþi sayýsýný hafýzaya al
        OpenPanel(pnlModeSelection); // Mod seçim ekranýný aç
    }

    // Bu fonksiyonu Mod Seçim butonlarýna vereceðiz
    public void StartGame(string gameMode)
    {
        // 1. Oyuncu sayýsýný kaydet (GameManager bunu okuyacak)
        PlayerPrefs.SetString("PlayerMode", selectedPlayerMode);

        // 2. Oyun türünü kaydet (En Yükseðe Çýk veya Alaný Doldur)
        PlayerPrefs.SetString("GameMode", gameMode);

        // Oyunu baþlat!
        SceneManager.LoadScene("GameScene");
    }
}