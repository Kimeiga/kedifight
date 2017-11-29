using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    [HideInInspector]
    public bool canFire = true;

    [HideInInspector]
    public bool fireCommand = false;

    public int ammo;
    public int maxAmmo;
    public float damage;

    Item itemScript;    

    public LayerMask maviFireMask;
    public LayerMask sariFireMask;

    public Transform fireTrans;

    public LineRenderer shotLine;
    public LineRenderer glowLine;

    public float shotLineLast = 0.2f;
    

    public float kickback;
    float kickbackAcc;
    public float kickRecovery = 0.5f;

    public SpriteRenderer deathRen;

    public int Ammo
    {
        get
        {
            return ammo;
        }

        set
        {

            ammo = value;

            float ammoF = ammo;

            float deathColor = ammoF.Remap(maxAmmo, 0, 1f, 0);

            //make em whiter when it gets closer to dying
            deathRen.color = new Color(deathColor, deathColor, deathColor);

            if(ammo <= 0)
            {
                itemScript.Drop();
            }
        }
    }

    public float shotForce = 10;

    // Use this for initialization
    void Start () {
        itemScript = GetComponent<Item>();

        Ammo = maxAmmo;

        shotLine.enabled = false;
        glowLine.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {


        if(fireCommand && Ammo > 0 && canFire)
        {
            //push back the player who shot this gun with recoil
            itemScript.inventoryScript.controller.recoilForce = -fireTrans.forward * shotForce * 0.3f;


            RaycastHit2D hit;


            if (itemScript.maviOwned)
            {
                hit = Physics2D.Raycast(fireTrans.position, fireTrans.forward, 9999, maviFireMask);
            }
            else
            {
                hit = Physics2D.Raycast(fireTrans.position, fireTrans.forward, 9999, sariFireMask);
            }
            

            Debug.DrawRay(fireTrans.position, fireTrans.forward);


            //turn on shot lines

            shotLine.enabled = true;
            glowLine.enabled = true;


            if (hit.collider != null)
            {

                //extend lines to point

                Vector3 hitPointLocal = fireTrans.InverseTransformPoint(hit.point);
                Vector3 shotHitPointLocal = new Vector3(0, hitPointLocal.y, hitPointLocal.z);

                shotLine.SetPosition(1, shotHitPointLocal);
                glowLine.SetPosition(1, shotHitPointLocal);

                //did we hit a player?
                if (hit.collider.CompareTag("Player"))
                {
                    //decrease her health
                    Player player = hit.collider.GetComponent<Player>();
                    player.Health -= damage;


                    //try something else if it's a player
                    PlatformController pConScript = hit.collider.GetComponent<PlatformController>();
                    pConScript.shotForce = (fireTrans.forward * shotForce);
                    pConScript.shot = true;
                }

                //push it with physiscs
                if (hit.collider.GetComponent<Rigidbody2D>())
                {
                    Rigidbody2D rb2D = hit.collider.GetComponent<Rigidbody2D>();
                    //rb2D.AddForceAtPosition(fireTrans.forward * shotForce, hit.point, ForceMode2D.Impulse);
                    rb2D.AddForce(fireTrans.forward * shotForce, ForceMode2D.Impulse);

                }
                

            }
            else
            {
                //we didn't hit anything

                //extend line to infinity
                shotLine.SetPosition(1, new Vector3(0, 0, 100));
                glowLine.SetPosition(1, new Vector3(0, 0, 100));
            }

            //turn off lines after a while
            StartCoroutine(ShotLine());



            LeanTween.cancel(gameObject);
            kickbackAcc -= kickback;

            fireCommand = false;

            Ammo--;

        }


        if(itemScript.itemTrans)
            itemScript.itemTrans.localPosition =  new Vector3( -kickbackAcc, itemScript.itemTrans.localPosition.y, itemScript.itemTrans.localPosition.z);

        
        LeanTween.value(gameObject, kickbackAcc, 0, kickRecovery).setOnUpdate((float val) => { kickbackAcc = val; });

    }

    IEnumerator ShotLine()
    {
        yield return new WaitForSeconds(shotLineLast);
        shotLine.enabled = false;
        glowLine.enabled = false;

    }

    public void FlipLines()
    {
        shotLine.transform.position = new Vector3(shotLine.transform.position.x, shotLine.transform.position.y, -shotLine.transform.position.z);
        glowLine.transform.position = new Vector3(glowLine.transform.position.x, glowLine.transform.position.y, -glowLine.transform.position.z);
    }
}
