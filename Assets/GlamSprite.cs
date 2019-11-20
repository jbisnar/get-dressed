using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlamSprite : MonoBehaviour
{

    public Sprite nude;
    public Sprite pants;
    public Sprite shoes;
    public Sprite suit;
    public Sprite suitopen;
    public Sprite bowtie;
    public Sprite bowtieopen;
    Glam_Movement player;
    SpriteRenderer srend;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponentInParent<Glam_Movement>();
        srend = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.gliding)
        {
            if (player.swingtie)
            {
                srend.sprite = bowtieopen;
            } else
            {
                srend.sprite = suitopen;
            }
        } else if (player.swingtie)
        {
            srend.sprite = bowtie;
        } else if (player.glidesuit)
        {
            srend.sprite = suit;
        } else if (player.wallshoes)
        {
            srend.sprite = shoes;
        } else if (player.slidepants)
        {
            srend.sprite = pants;
        } else
        {
            srend.sprite = nude;
        }

        if (player.temp.x > 0)
        {
            srend.flipX = false;
        } else if (player.temp.x < 0)
        {
            srend.flipX = true;
        }

        if (player.sliding)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0,0,90));
            transform.localPosition = new Vector3(0, -.18f, 0);
            if (player.temp.x > 0)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
            } else
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
            }
        } else
        {
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            transform.localPosition = Vector3.zero;
        }
    }
}
