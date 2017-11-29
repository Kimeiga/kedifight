using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float maxHealth = 100;
    private float health;

    public bool mavi;

    public float Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;

            float  deathColor = health.Remap(100, 0, 0.5f, 1);

            //make em whiter when it gets closer to dying
            deathRen.material.SetColor("_TintColor", new Color(deathColor, deathColor, deathColor));

            if (mavi)
            {
                HUD.hud.maviHP.text = health + "+";
            }
            else
            {

                HUD.hud.sariHP.text = "+" + health;
            }
        }

    }

    public SpriteRenderer deathRen;

    Inventory inventoryScript;
    PlatformController pConScript;
    Collider2D col;
    Rigidbody2D rb;


    // Use this for initialization
    void Start () {

        Health = maxHealth;

        inventoryScript = GetComponent<Inventory>();
        pConScript = GetComponent<PlatformController>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        
	}
	
	// Update is called once per frame
	void Update () {
		
        if(Health <= 0)
        {
            //you died oh geez
            //have the cat fall off the screen and shit
            //disable boxcollider and platformcontroller and inventory

            inventoryScript.enabled = false;
            pConScript.enabled = false;
            col.enabled = false;
            rb.freezeRotation = false;
            
            rb.AddTorque(-30);

            StartCoroutine(DestroyAfterTime(3));
            
            GameManager.manager.PlayerDied(mavi);
            this.enabled = false;
        }

        if(transform.position.y < GameManager.manager.dieHeight)
        {
            Health = 0;
        }

	}


    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }
    
}
