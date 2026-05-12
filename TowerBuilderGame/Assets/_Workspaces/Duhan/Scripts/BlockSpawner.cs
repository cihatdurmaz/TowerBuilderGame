using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [Header("Oyuncu ve Üretim Ayarlarý")]
    public int playerID = 1;
    [SerializeField] private GameObject[] blockPrefabs;
    [SerializeField] private Transform spawnPoint;

    // YENÝ: Spawner aktif mi?
    [HideInInspector] public bool canSpawn = true;

    void Start()
    {
        SpawnBlock();
    }

    public void SpawnBlock()
    {
        // Eðer oyuncu elendiyse yeni blok üretme!
        if (!canSpawn) return;

        int randomIndex = Random.Range(0, blockPrefabs.Length);
        GameObject newBlock = Instantiate(blockPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);

        BlockController controller = newBlock.GetComponent<BlockController>();
        if (controller != null)
        {
            controller.playerID = this.playerID;
            controller.mySpawner = this;
        }
    }
}