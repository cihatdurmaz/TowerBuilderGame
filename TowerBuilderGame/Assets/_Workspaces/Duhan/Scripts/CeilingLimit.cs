using UnityEngine;

public class CeilingLimit : MonoBehaviour
{
    // OnTriggerEnter yerine OnTriggerStay kullanýyoruz. 
    // Çünkü blok içinden geçerken deðil, içinde "kaldýðýnda ve yerleþtiðinde" yakalamak istiyoruz.
    private void OnTriggerStay2D(Collider2D collision)
    {
        BlockController block = collision.GetComponent<BlockController>();

        // Eðer bu objede BlockController varsa VE blok artýk yere/kuleye oturmuþsa (isLanded == true)
        if (block != null && block.isLanded)
        {
            GameManager.instance.EliminatePlayer(block.playerID);
        }
    }
}