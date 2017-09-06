using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

    public LevelManager levelManager;
    //private SpriteRenderer sr;

    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        //sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
                levelManager.SetCurrentCheckPoint(gameObject);
        }
    }
}
