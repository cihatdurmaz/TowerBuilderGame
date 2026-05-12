using UnityEngine;

public class BlockController : MonoBehaviour
{
    [Header("Blok Ayarlarý")]
    [SerializeField] private float gridSize = 0.5f;
    [SerializeField] private float fallSpeed = 3f;

    // YENÝ: Bu bloðun hangi oyuncuya ait olduðunu ve üreticisini tutan deðiþkenler
    [HideInInspector] public int playerID = 1;
    [HideInInspector] public BlockSpawner mySpawner;

    private Rigidbody2D rb;
    [HideInInspector] public bool isLanded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        if (isLanded) return;

        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // --- YENÝ: OYUNCU GÝRÝÞLERÝ AYRILDI ---
        if (playerID == 1) // 1. OYUNCU (A ve D)
        {
            if (Input.GetKeyDown(KeyCode.A))
                transform.position += new Vector3(-gridSize, 0, 0);
            else if (Input.GetKeyDown(KeyCode.D))
                transform.position += new Vector3(gridSize, 0, 0);
        }
        else if (playerID == 2) // 2. OYUNCU (Sol ve Sað Ok)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                transform.position += new Vector3(-gridSize, 0, 0);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                transform.position += new Vector3(gridSize, 0, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isLanded) return;

        if (collision.contacts[0].normal.y > 0.5f)
        {
            isLanded = true;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.gravityScale = 1f;

            // Kamera takibini þimdilik iptal ediyoruz (Çünkü 2 kule olacak, kamerayý sabit tutmak parti oyunlarý için daha iyidir)

            // YENÝ: Sadece KENDÝ spawner'ýna yeni blok üretmesini söylüyor.
            if (mySpawner != null)
            {
                mySpawner.SpawnBlock();
            }
        }
    }
}