using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public GameObject currentItem ;
    public Item currentItemScript;
    public Gun currentGunScript;
    public Transform itemTrans;

    public PlatformController controller;
    Player playerScript;

    public bool touchingItem = false;

    bool justGrabbed = false;

	// Use this for initialization
	void Start () {

        controller = GetComponent<PlatformController>();
        playerScript = GetComponent<Player>();

	}
	
	// Update is called once per frame
	void Update () {

        if (controller.rightPlayer && Input.GetButtonDown("Right Interact") ||
            !controller.rightPlayer && Input.GetButtonDown("Left Interact"))
        {

            //pressed down when yer not touching an item
            //shoot if you have a gun
            if (currentItem && !justGrabbed)
            {

                if (currentItemScript.item == ItemType.Gun)
                {
                    currentGunScript.fireCommand = true;
                }
            }
            
        }

        if (currentGunScript)
        {
            if (playerScript.mavi)
            {
                HUD.hud.maviAmmo.text = currentGunScript.Ammo + "#";
                HUD.hud.maviAmmo.enabled = true;

            }
            else
            {

                HUD.hud.sariAmmo.text = "#" + currentGunScript.Ammo;
                HUD.hud.sariAmmo.enabled = true;
            }
        }
        else
        {
            if (playerScript.mavi)
                HUD.hud.maviAmmo.enabled = false;
            else
                HUD.hud.sariAmmo.enabled = false;
        }

        justGrabbed = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("WeaponTrigger") && other.transform.parent.gameObject != currentItem)
        {

            touchingItem = true;
            
            if ((controller.rightPlayer && Input.GetButtonDown("Right Interact") ||
                !controller.rightPlayer && Input.GetButtonDown("Left Interact")) && !currentItem)
            {
                Item itemScript = other.transform.root.GetComponent<Item>();
                if (itemScript.dead)
                {
                    return;
                }

                justGrabbed = true;

                //grab an item
                currentItem = other.transform.root.gameObject;

                currentItem.transform.parent = itemTrans;

                currentItemScript = itemScript;
                if (currentItemScript.item == ItemType.Gun)
                {
                    currentGunScript = currentItem.GetComponent<Gun>();
                }

                currentItem.transform.localPosition = Vector3.zero + (Vector3)currentItemScript.holdPos;

                if (controller.facingRight){

                    currentItem.transform.localRotation = Quaternion.identity;
                }
                else
                {

                    currentItem.transform.localRotation = Quaternion.Euler(0,180,0);

                    //this flips the shot lines, so we must position them behind the gun
                    if (currentGunScript)
                    {
                        currentGunScript.FlipLines();
                    }
                }


                Rigidbody2D itemRigid = currentItem.GetComponent<Rigidbody2D>();

                itemRigid.isKinematic = true;
                itemRigid.gravityScale = 0;


                currentItemScript.maviOwned = playerScript.mavi ? true : false;
                currentItemScript.itemTrans = itemTrans;

                //tell it's item Spawner that you grabbed it
                currentItemScript.Respawn();

                currentItemScript.inventoryScript = this;
            }

        }
        else
        {
            touchingItem = false;
        }
    }
}
