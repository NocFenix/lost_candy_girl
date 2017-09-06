using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour {

    public LevelManager levelManager;
    private bool isLocked = false;
    private int waitTime = 3;

    // Use this for initialization
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            switch (tag)
            {
                case "WaterHazard":
                    levelManager.PlayerWaterDeath();
                    break;
                case "Exit":
                    levelManager.BeatLevel();
                    break;
                case "Enemy":
                    if (!isLocked)
                    {
                        isLocked = true;
                        levelManager.PlayerHurt();
                        StartCoroutine("ResetLock");
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public IEnumerator ResetLock()
    {
        yield return new WaitForSeconds(waitTime);
        isLocked = false;
    }

}
