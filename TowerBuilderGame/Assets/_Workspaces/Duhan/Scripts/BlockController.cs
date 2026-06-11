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

        // Hem bilgisayar testleri için klavye açýk kalýyor hem de yeni fonksiyonlarý çaðýrýyoruz
        if (playerID == 1)
        {
            if (Input.GetKeyDown(KeyCode.A)) MoveLeft();
            else if (Input.GetKeyDown(KeyCode.D)) MoveRight();
        }
        else if (playerID == 2)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) MoveLeft();
            else if (Input.GetKeyDown(KeyCode.RightArrow)) MoveRight();
        }
    }

    // YENÝ: Mobil butonlarýn da tetikleyebilmesi için hareketleri dýþa (public) açtýk
    public void MoveLeft()
    {
        if (!isLanded) transform.position += new Vector3(-gridSize, 0, 0);
    }

    public void MoveRight()
    {
        if (!isLanded) transform.position += new Vector3(gridSize, 0, 0);
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