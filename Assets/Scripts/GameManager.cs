using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager manager = null;

    public int maviScore = 0;
    public int sariScore = 0;

    public int matchTime = 300; //5 minutes
    private float remainingTime;

    public int maviLivesMax = 5;
    private int maviLives;

    public int sariLivesMax = 5;
    private int sariLives;

    public int MaviLives
    {
        get
        {
            return maviLives;
        }

        set
        {
            maviLives = value;
            HUD.hud.maviLives.text =  maviLives + "×" ;
        }
    }

    public int SariLives
    {
        get
        {
            return sariLives;
        }

        set
        {
            sariLives = value;
            HUD.hud.sariLives.text = "×" + sariLives ;
        }

    }

    public float dieWaitTime = 3;

    public GameObject mavikedi;
    public GameObject sarikedi;

    public Transform maviRespawn;
    public Transform sariRespawn;

    public float winWaitTime = 5;

    public float dieHeight = -60;

    private void Awake()
    {
        if (manager == null)
            manager = this;
        else if (manager != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

    }

    // Use this for initialization
    void Start () {

        SariLives = sariLivesMax;
        MaviLives = maviLivesMax;
		
	}
	
	// Update is called once per frame
	void Update () {
        
		
	}

    public void PlayerDied(bool mavi)
    {
        StartCoroutine(PlayerDiedCo(mavi));
    }

    public IEnumerator PlayerDiedCo(bool mavi)
    {
        if(mavi && MaviLives <= 0)
        {
            //sari wins
            StartCoroutine(Win(false));
            yield break;
        }
        else if(!mavi && SariLives <= 0)
        {
            //mavi wins
            StartCoroutine(Win(true));
            yield break;
        }


        HUD.hud.kiyimText.enabled = true;
        HUD.hud.kiyimText.color = mavi ? HUD.hud.sari : HUD.hud.mavi;


        yield return new WaitForSeconds(3);

        //respawn player
        if (mavi)
        {

            Instantiate(mavikedi, maviRespawn.position, maviRespawn.rotation);
            MaviLives--;
        }
        else
        {
            Instantiate(sarikedi, sariRespawn.position, sariRespawn.rotation);
            SariLives--;
        }



        HUD.hud.kiyimText.enabled = false;
    }

    public IEnumerator Win(bool mavi)
    {

        HUD.hud.kazandiText.enabled = true;
        HUD.hud.kazandiText.color = mavi ? HUD.hud.mavi : HUD.hud.sari;
        HUD.hud.kazandiText.text = mavi ? "MAVI KAZANDI" : "SARI KAZANDI";

        yield return new WaitForSeconds(winWaitTime);

        Restart();
    }

    void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        
        SceneManager.LoadScene(scene.name);
    }
}
