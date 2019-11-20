using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    public Sprite pickupsprite;
    public int upgrade;
    Glam_Movement player;
    public LayerMask playerLayer;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().size = new Vector2(3f,3f);
        gameObject.GetComponent<SpriteRenderer>().sprite = pickupsprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 8)
        { //Player
            player = col.gameObject.GetComponent<Glam_Movement>();
            if (upgrade == 0)
            {
                player.slidepants = true;
            } else if (upgrade == 1)
            {
                player.wallshoes = true;
            } else if (upgrade == 2)
            {
                player.glidesuit = true;
            } else if (upgrade == 3)
            {
                player.swingtie = true;
            }
        }
        GameObject.Destroy(gameObject);
    }
}
