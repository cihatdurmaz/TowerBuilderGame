using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Her yerden kolayca ulaþabilmek için Singleton mantýðý kuruyoruz.
    public static GameManager instance;

    public BlockSpawner spawnerP1;
    public BlockSpawner spawnerP2;

    private bool p1Alive = true;
    private bool p2Alive = true;

    void Awake()
    {
        instance = this;
    }

    // Bir blok ölüm alanýna veya tavana deðdiðinde bu fonksiyon çalýþýr
    public void EliminatePlayer(int playerID)
    {
        if (playerID == 1 && p1Alive)
        {
            p1Alive = false;
            spawnerP1.canSpawn = false; // 1. Oyuncunun üretimini durdur
            Debug.Log("--- 1. OYUNCU ELENDÝ! Bekleniyor... ---");
        }
        else if (playerID == 2 && p2Alive)
        {
            p2Alive = false;
            spawnerP2.canSpawn = false; // 2. Oyuncunun üretimini durdur
            Debug.Log("--- 2. OYUNCU ELENDÝ! Bekleniyor... ---");
        }

        // Eðer iki oyuncu da elendiyse kazananý hesapla
        if (!p1Alive && !p2Alive)
        {
            DetermineWinner();
        }
    }

    private void DetermineWinner()
    {
        int p1Score = 0;
        int p2Score = 0;

        // Sahnedeki tüm bloklarý bul
        BlockController[] allBlocks = FindObjectsOfType<BlockController>();

        foreach (BlockController block in allBlocks)
        {
            // Sadece baþarýlý bir þekilde platformda duranlarý say (isLanded)
            if (block.isLanded)
            {
                if (block.playerID == 1) p1Score++;
                else if (block.playerID == 2) p2Score++;
            }
        }

        Debug.Log("=== OYUN BÝTTÝ! ===");
        if (p1Score > p2Score) Debug.Log("KAZANAN: 1. OYUNCU! (Skor: " + p1Score + " - " + p2Score + ")");
        else if (p2Score > p1Score) Debug.Log("KAZANAN: 2. OYUNCU! (Skor: " + p2Score + " - " + p1Score + ")");
        else Debug.Log("BERABERE! (Skor: " + p1Score + " - " + p2Score + ")");
    }
}