using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [Header("Oyuncu ve Üretim Ayarlarý")]
    public int playerID = 1;
    [SerializeField] private GameObject[] blockPrefabs;
    [SerializeField] private Transform spawnPoint;

    [HideInInspector] public bool canSpawn = true;

    // --- void Start() KISMI TAMAMEN SÝLÝNDÝ ---

    public void SpawnBlock()
    {
        if (!canSpawn) return;

        int randomIndex = Random.Range(0, blockPrefabs.Length);
        GameObject newBlock = Instantiate(blockPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);

        BlockController controller = newBlock.GetComponent<BlockController>();
        if (controller != null)
        {
            controller.playerID = this.playerID;
            controller.mySpawner = this;

            // YENÝ: Doðan bloðu GameManager'a bildir!
            if (this.playerID == 1) GameManager.instance.activeBlockP1 = controller;
            else if (this.playerID == 2) GameManager.instance.activeBlockP2 = controller;
        }
    }
}