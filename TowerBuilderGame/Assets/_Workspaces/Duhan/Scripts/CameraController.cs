using UnityEngine;

public class CameraController : MonoBehaviour
{
    private string gameMode;
    private float startY;

    [Header("Takip Ayarlarý")]
    public float offsetY = 0f;
    public float smoothSpeed = 5f;

    [Header("Üreticiler (Spawner)")]
    public Transform spawnerP1;
    public Transform spawnerP2;
    private float spawnerOffset;

    void Start()
    {
        gameMode = PlayerPrefs.GetString("GameMode", "HeightMode");
        startY = transform.position.y;

        // Oyun baþlarken Spawner'ýn kameradan ne kadar yukarýda olduðunu hesapla
        if (spawnerP1 != null)
            spawnerOffset = spawnerP1.position.y - startY;
    }

    void LateUpdate()
    {
        // Sadece "En Yükseðe Çýk" modunda kamerayý hareket ettir
        if (gameMode == "HeightMode")
        {
            float highestBlockY = startY;

            BlockController[] blocks = FindObjectsOfType<BlockController>();
            foreach (BlockController block in blocks)
            {
                if (block.isLanded && block.transform.position.y > highestBlockY)
                {
                    highestBlockY = block.transform.position.y;
                }
            }

            float targetY = highestBlockY + offsetY;

            if (targetY > transform.position.y)
            {
                Vector3 targetPos = new Vector3(transform.position.x, targetY, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);

                // Üreticileri de kamerayla ayný hizayý koruyarak yukarý taþý
                if (spawnerP1 != null) spawnerP1.position = new Vector3(spawnerP1.position.x, transform.position.y + spawnerOffset, spawnerP1.position.z);
                if (spawnerP2 != null) spawnerP2.position = new Vector3(spawnerP2.position.x, transform.position.y + spawnerOffset, spawnerP2.position.z);
            }
        }
    }
}