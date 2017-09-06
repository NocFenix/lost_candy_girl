using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectObjectController : MonoBehaviour
{
    public LevelManager levelManager;

    // Use this for initialization
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            switch (tag)
            {
                case "Coin":
                    levelManager.CollectCoin();
                    Destroy(gameObject);
                    break;
                case "Chest":
                    levelManager.CollectChest();
                    Destroy(gameObject);
                    break;
                case "Exit":
                    levelManager.BeatLevel();
                    break;
                default:
                    break;
            }
        }
    }

}
