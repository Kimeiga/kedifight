using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour {

    public static HUD hud;

    public TextMeshProUGUI maviHP;
    public TextMeshProUGUI sariHP;
    public TextMeshProUGUI maviLives;
    public TextMeshProUGUI sariLives;

    public TextMeshProUGUI kiyimText;

    public Color mavi;
    public Color sari;

    public TextMeshProUGUI kazandiText;

    public TextMeshProUGUI maviAmmo;
    public TextMeshProUGUI sariAmmo;

    private void Awake()
    {
        if (hud == null)
            hud = this;
        else if (hud != this)
            Destroy(gameObject);

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
