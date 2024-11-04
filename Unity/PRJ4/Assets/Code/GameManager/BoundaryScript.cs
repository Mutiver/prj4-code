using UnityEngine;

public class BoundaryScript : MonoBehaviour
{
    //UNTESTABLE
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Ensures that projectiles that miss target gets destroyed
        if (collision.gameObject.CompareTag("Enemy")) return;
        Destroy(collision.gameObject);
    }
}
