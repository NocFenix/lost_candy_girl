using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenAreaController : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(gameObject);
        if (other.name == "Player")
        {
            switch (tag)
            {
                case "HiddenArea":
                    var renderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
                    foreach (SpriteRenderer sr in renderers)
                        sr.enabled = false;

                    break;
                default:
                    break;
            }

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            switch (tag)
            {
                case "HiddenArea":
                    var renderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
                    foreach (SpriteRenderer sr in renderers)
                        sr.enabled = true;

                    break;
                default:
                    break;
            }

        }
    }

}
