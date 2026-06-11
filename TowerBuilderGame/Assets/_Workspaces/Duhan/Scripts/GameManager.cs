using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Üreticiler")]
    public BlockSpawner spawnerP1;
    public BlockSpawner spawnerP2;

    [Header("Arayüz (UI) Elemanları")]
    public GameObject pnlGameOver;
    public TextMeshProUGUI txtWinner;
    public TextMeshProUGUI txtScores;

    [Header("Oyun Modu Kuralları")]
    public GameObject ceilingLimit;
    private string playerMode; // Singleplayer veya Multiplayer
    private string gameMode;   // HeightMode veya FillMode

    [Header("Mobil Kontroller")]
    public GameObject p2ControlsPanel;
    [HideInInspector] public BlockController activeBlockP1;
    [HideInInspector] public BlockController activeBlockP2;

    private bool p1Alive = true;
    private bool p2Alive = true;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // 1. MODLARI HAFIZADAN ÇEK
        playerMode = PlayerPrefs.GetString("PlayerMode", "Singleplayer");
        gameMode = PlayerPrefs.GetString("GameMode", "HeightMode");

        // 2. TAVAN KURALINI UYGULA
        if (ceilingLimit != null)
        {
            if (gameMode == "FillMode") ceilingLimit.SetActive(true);
            else if (gameMode == "HeightMode") ceilingLimit.SetActive(false);
        }

        // 3. OYUNCU SAYISINA GÖRE SAHNEYİ DÜZENLE
        if (playerMode == "Singleplayer")
        {
            p2Alive = false;
            if (spawnerP2 != null) spawnerP2.gameObject.SetActive(false);
            if (p2ControlsPanel != null) p2ControlsPanel.SetActive(false); // P2 butonlarını gizle

            GameObject platformP2 = GameObject.Find("Platform_P2");
            if (platformP2 != null) platformP2.SetActive(false);

            // P1'i Merkeze al
            if (spawnerP1 != null)
            {
                Vector3 spawnerPos = spawnerP1.transform.position;
                spawnerP1.transform.position = new Vector3(0, spawnerPos.y, spawnerPos.z);
            }

            GameObject platformP1 = GameObject.Find("Platform_P1");
            if (platformP1 != null)
            {
                Vector3 platformPos = platformP1.transform.position;
                platformP1.transform.position = new Vector3(0, platformPos.y, platformPos.z);
            }

            Vector3 camPos = Camera.main.transform.position;
            Camera.main.transform.position = new Vector3(0, camPos.y, camPos.z);
        }
        else if (playerMode == "Multiplayer")
        {
            p2Alive = true;
            if (spawnerP2 != null) spawnerP2.gameObject.SetActive(true);
            if (p2ControlsPanel != null) p2ControlsPanel.SetActive(true); // P2 butonlarını aç

            GameObject platformP2 = GameObject.Find("Platform_P2");
            if (platformP2 != null) platformP2.SetActive(true);

            // P1'i Sola al (Senin belirlediğin -6 konumuna)
            if (spawnerP1 != null)
            {
                Vector3 spawnerPos = spawnerP1.transform.position;
                spawnerP1.transform.position = new Vector3(-6f, spawnerPos.y, spawnerPos.z);
            }

            GameObject platformP1 = GameObject.Find("Platform_P1");
            if (platformP1 != null)
            {
                Vector3 platformPos = platformP1.transform.position;
                platformP1.transform.position = new Vector3(-6f, platformPos.y, platformPos.z);
            }

            Vector3 camPos = Camera.main.transform.position;
            Camera.main.transform.position = new Vector3(0, camPos.y, camPos.z);
        }

        pnlGameOver.SetActive(false);

        // 4. İLK BLOKLARI DOĞUR!
        if (playerMode == "Singleplayer")
        {
            if (spawnerP1 != null) spawnerP1.SpawnBlock();
        }
        else if (playerMode == "Multiplayer")
        {
            if (spawnerP1 != null) spawnerP1.SpawnBlock();
            if (spawnerP2 != null) spawnerP2.SpawnBlock();
        }
    }

    public void EliminatePlayer(int playerID)
    {
        if (playerID == 1 && p1Alive)
        {
            p1Alive = false;
            if (spawnerP1 != null) spawnerP1.canSpawn = false;
        }
        else if (playerID == 2 && p2Alive)
        {
            p2Alive = false;
            if (spawnerP2 != null) spawnerP2.canSpawn = false;
        }

        if (playerMode == "Singleplayer")
        {
            if (!p1Alive) ShowGameOver();
        }
        else
        {
            if (!p1Alive && !p2Alive) ShowGameOver();
        }
    }

    private void ShowGameOver()
    {
        int p1Score = 0;
        int p2Score = 0;

        BlockController[] allBlocks = FindObjectsOfType<BlockController>();
        foreach (BlockController block in allBlocks)
        {
            if (block.isLanded)
            {
                if (block.playerID == 1) p1Score++;
                else if (block.playerID == 2) p2Score++;
            }
        }

        if (playerMode == "Singleplayer")
        {
            int highScore = PlayerPrefs.GetInt("HighScore", 0);

            if (p1Score > highScore)
            {
                highScore = p1Score;
                PlayerPrefs.SetInt("HighScore", highScore);
                txtWinner.text = "YENİ REKOR!";
            }
            else
            {
                txtWinner.text = "OYUN BİTTİ!";
            }

            txtScores.text = "Skorun: " + p1Score + "\nEn Yüksek Skor: " + highScore;
        }
        else
        {
            if (p1Score > p2Score) txtWinner.text = "🏆 1. OYUNCU KAZANDI! 🏆";
            else if (p2Score > p1Score) txtWinner.text = "🏆 2. OYUNCU KAZANDI! 🏆";
            else txtWinner.text = "🤝 BERABERE! 🤝";

            txtScores.text = "1. Oyuncu: " + p1Score + "\n2. Oyuncu: " + p2Score;
        }

        pnlGameOver.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void MobileMoveLeft(int playerID)
    {
        if (playerID == 1 && activeBlockP1 != null) activeBlockP1.MoveLeft();
        else if (playerID == 2 && activeBlockP2 != null) activeBlockP2.MoveLeft();
    }

    public void MobileMoveRight(int playerID)
    {
        if (playerID == 1 && activeBlockP1 != null) activeBlockP1.MoveRight();
        else if (playerID == 2 && activeBlockP2 != null) activeBlockP2.MoveRight();
    }
}