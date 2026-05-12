using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BlockController block = collision.GetComponent<BlockController>();
        if (block != null)
        {
            // Bloðun kime ait olduðunu bul ve GameManager'a bildir!
            GameManager.instance.EliminatePlayer(block.playerID);
        }
    }
}