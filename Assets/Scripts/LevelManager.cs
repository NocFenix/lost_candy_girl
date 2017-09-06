using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public GameObject waterDeathParticle;
    public GameObject playerEnemyHitParticle;
    public GameObject chestCollectParticle;
    public GameObject mainCamera;
    public Text livesText;
    public float respawnDelay;
    public AudioClip[] audioClips;

    private PlayerController player;
    private ScoreManager scoreManager;
    private GameObject currentCheckPoint;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        livesText.text = "";
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Player falls into a WaterHazard
    /// </summary>
    public void PlayerWaterDeath()
    {
        StartCoroutine("RespawnPlayerCo");
        Instantiate(waterDeathParticle, player.transform.position, player.transform.rotation);
        GetComponent<AudioSource>().clip = audioClips[3];
        GetComponent<AudioSource>().Play();
        player.LoseHitPoint();
        player.FreezePlayer(true);
        player.ShowPlayer(false);
    }

    /// <summary>
    /// Player gets hurt
    /// </summary>
    public void PlayerHurt()
    {
        if (!player.isDead)
        {
            player.SetDead(true);
            player.LoseHitPoint();
            player.PlayHurtSound();
            Instantiate(playerEnemyHitParticle, player.transform.position, player.transform.rotation);
            StartCoroutine("RespawnPlayerCo");
        }
    }

    public IEnumerator RespawnPlayerCo()
    {
        yield return new WaitForSeconds(respawnDelay);
        player.ShowPlayer(true);
        if (player.lives > 0)
        {
            player.transform.position = currentCheckPoint.transform.position;
            player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            player.SetDead(false);
            player.FreezePlayer(false);
        }
        else
        {
            mainCamera.GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().clip = audioClips[5];
            GetComponent<AudioSource>().Play();
            livesText.text = "Game Over!\nPress R to Restart.";
            player.allowMovement = false;
            player.transform.position = currentCheckPoint.transform.position;
        }
    }

    public void CollectCoin()
    {
        GetComponent<AudioSource>().clip = audioClips[1];
        GetComponent<AudioSource>().Play();
        ScoreManager.AddPoints(10);
    }

    public void CollectChest()
    {
        Instantiate(chestCollectParticle, player.transform.position, player.transform.rotation);
        GetComponent<AudioSource>().clip = audioClips[0];
        GetComponent<AudioSource>().Play();
        ScoreManager.AddPoints(100);
    }

    public void BeatLevel()
    {
        player.EnterDoor();
        mainCamera.GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = audioClips[4];
        GetComponent<AudioSource>().Play();
        livesText.text = "Level Complete!";
    }

    public void SetCurrentCheckPoint(GameObject checkPoint)
    {
        if (currentCheckPoint != null)
        {
            if (currentCheckPoint.transform.position != checkPoint.transform.position)
            {
                currentCheckPoint.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Art/Graveyard_Tiles/Objects/Sign");
                currentCheckPoint = checkPoint;
                currentCheckPoint.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Art/Graveyard_Tiles/Objects/ArrowSign");
                GetComponent<AudioSource>().clip = audioClips[2];
                GetComponent<AudioSource>().Play();
                livesText.text = "Checkpoint Reached!";
                StartCoroutine("CheckPointReachedCo");
            }
        }
        else
        {
            currentCheckPoint = checkPoint;
            currentCheckPoint.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Art/Graveyard_Tiles/Objects/ArrowSign");
        }
    }

    public IEnumerator CheckPointReachedCo()
    {
        yield return new WaitForSeconds(respawnDelay);
        livesText.text = "";
    }
}
