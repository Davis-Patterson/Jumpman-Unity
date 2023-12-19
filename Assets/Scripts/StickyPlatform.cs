using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && gameObject.activeInHierarchy)
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            // Check if the platform is active before changing the parent
            if (gameObject.activeInHierarchy)
            {
                collision.gameObject.transform.SetParent(null);
            }
            else
            {
                // Optionally, defer the SetParent call if needed
                StartCoroutine(DeferredSetParent(collision.transform));
            }
        }
    }

    private IEnumerator DeferredSetParent(Transform playerTransform)
    {
        // Wait until the next frame
        yield return null;

        // Check again if the platform is active
        if (gameObject.activeInHierarchy)
        {
            playerTransform.SetParent(null);
        }
        // else handle the case where the platform is still inactive
    }
}
