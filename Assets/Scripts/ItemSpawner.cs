using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemSpawner : MonoBehaviour {

    public GameObject item;

    public TextMeshPro respawnTimeText;
    public int respawnDelay = 20;
    

    //when the item gets picked up, set the respawn timer.


	// Use this for initialization
	void Start () {
        //when game starts, release an item
        GameObject itemObj = Instantiate(item, transform.position, transform.rotation);

        Item itemScript = itemObj.GetComponent<Item>();
        itemScript.itemSpawnerScript = this;
        
        respawnTimeText.enabled = false;
	}

    public void Respawn()
    {
        StartCoroutine(RespawnItem());
    }
	
	IEnumerator RespawnItem()
    {
        float respawnTime = Time.time + respawnDelay;

        respawnTimeText.enabled = true;

        //have something execute every frame..............
        //while (true)
        //{
        //    int timeLeft = (int)(Time.time - respawnTime);
        //    respawnTimeText.text = timeLeft.ToString();
        //}

        yield return new WaitForSeconds(respawnDelay);


        GameObject itemObj = Instantiate(item, transform.position, transform.rotation);

        Item itemScript = itemObj.GetComponent<Item>();
        itemScript.itemSpawnerScript = this; 

    }


}
