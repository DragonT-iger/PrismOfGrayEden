using UnityEngine;

public class KeyScript : MonoBehaviour
{
    [SerializeField] ShelfScript shelfScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            shelfScript.isKeyAquired = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}