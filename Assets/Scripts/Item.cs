using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//some item types i came up with
public enum ItemType { Melee, Gun, Gadget, Sundry }

public class Item : MonoBehaviour {

    public Vector2 holdPos;

    public ItemType item;

    public bool maviOwned;
    public Inventory inventoryScript;

    public Transform itemTrans;

    public ItemSpawner itemSpawnerScript;

    public Rigidbody2D rb;
    public Collider2D col;
    public float despawnTime = 3;

    public bool dead = false;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Respawn()
    {
        itemSpawnerScript.Respawn();
    }

    //when the ammo depletes, or the item's use has been used
    //drop the item in front of everything but through the floor and terminate it after a while
    public void Drop()
    {
        StartCoroutine(DropCo());
    }

    IEnumerator DropCo()
    {
        inventoryScript.currentItem = null;
        inventoryScript.currentItemScript = null;
        inventoryScript.currentGunScript = null;

        transform.parent = null;

        rb.isKinematic = false;
        rb.gravityScale = 1;

        col.enabled = false;

        rb.AddTorque(50);

        dead = true;

        //fallinggggggggggg

        yield return new WaitForSeconds(despawnTime);


        Destroy(gameObject);


    }
}
