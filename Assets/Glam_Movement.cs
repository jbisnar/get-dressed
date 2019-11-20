using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glam_Movement : MonoBehaviour
{
    float gravNormal = 15f;
    float gravJump = 15f;
    float gravDown = 15f;
    float gravWall = 0f;
    float gravGlide = 2f;
    float velWalk = 7.5f;
    float accelWalk = 10f;
    float accelSwitch = 40f;
    float accelSlow = 30f;
    float velAir = 3f;
    float accelAir = 10f;
    float accelAirSwitch = 15f;
    float accelAirSlow = 5f;
    float velSlide = 3f;
    float accelSlide = 10f;
    float accelSlideSwitch = 15f;
    float accelSlideSlow = 5f;
    float velGlide = 1f;
    float velJumpGround = 5f;
    float velJumpWallH = 4f;
    float velJumpWallV = 4f;
    float velWallSlideDown = 3f;

    public bool slidepants = false;
    public bool wallshoes = false;
    public bool glidesuit = false;
    public bool swingtie = false;

    float savedVel = 0;
    float savedVelAir = 0;
    float savedVelWallKick = 0;
    public bool gliding = false;
    public bool sliding = false;
    public GameObject standCollide;
    public GameObject slideCollide;

    public bool grounded = false;
    public bool jumping = false;
    public bool walledL = false;
    public bool wallslideL = false;
    public bool walledR = false;
    public bool wallslideR = false;
    bool cantslide = false;
    bool cantstand = false;
    float walljumpgrace = .05f;
    float walljumpcontrol = .5f;
    float perfectkickgrace = .1f;
    float walljumpgracetime;
    float walljumpcontroltime;
    float perfectkicktime;
    public LayerMask layerGround;
    public Vector2 temp;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        temp = transform.GetComponent<Rigidbody2D>().velocity;
        grounded = Physics2D.OverlapArea(new Vector2(transform.position.x - .16f, transform.position.y - .36f),
            new Vector2(transform.position.x + .16f, transform.position.y - .38f), layerGround);
        if ((!walledL && !walledR) && (Physics2D.OverlapArea(new Vector2(transform.position.x - .2f, transform.position.y + .36f),
            new Vector2(transform.position.x - .18f, transform.position.y - .30f), layerGround) || Physics2D.OverlapArea(new Vector2(transform.position.x + .18f, transform.position.y + .36f),
            new Vector2(transform.position.x + .2f, transform.position.y - .30f), layerGround)))
        {
            perfectkicktime = Time.time + perfectkickgrace;
        }
        walledL = Physics2D.OverlapArea(new Vector2(transform.position.x - .2f, transform.position.y + .36f),
            new Vector2(transform.position.x - .18f, transform.position.y - .30f), layerGround) && !sliding;
        wallslideL = walledL && Input.GetAxisRaw("Horizontal") < 0;
        walledR = Physics2D.OverlapArea(new Vector2(transform.position.x + .18f, transform.position.y + .36f),
            new Vector2(transform.position.x + .2f, transform.position.y - .30f), layerGround) && !sliding;
        wallslideR = walledR && Input.GetAxisRaw("Horizontal") > 0;
        cantslide = Physics2D.OverlapArea(new Vector2(transform.position.x - .34f, transform.position.y - .2f),
            new Vector2(transform.position.x + .34f, transform.position.y - .34f), layerGround) || !slidepants;
        cantstand = Physics2D.OverlapArea(new Vector2(transform.position.x - .16f, transform.position.y + .34f),
            new Vector2(transform.position.x + .16f, transform.position.y - .34f), layerGround);
        if (walledL || walledR)
        {
            walljumpgracetime = Time.time + walljumpgrace;
        }

        if (Input.GetAxisRaw("Vertical") < 0 && !cantslide)
        {
            if (!sliding)
            {
                slideCollide.gameObject.SetActive(true);
                standCollide.gameObject.SetActive(false);
            }
            sliding = true;
        } else if (!cantstand)
        {
            if (sliding)
            {
                standCollide.gameObject.SetActive(true);
                slideCollide.gameObject.SetActive(false);
            }
            sliding = false;
        }

        //HORIZONTAL CONTROL
        if (Input.GetAxisRaw("Horizontal") == 0)
        { //Slow down
            if (Mathf.Abs(temp.x) < accelSlow * Time.deltaTime)
            {
                temp.x = 0;
            }
            else if (temp.x > 0)
            {
                if (grounded)
                {
                    if (sliding)
                    {
                        temp.x -= accelSlideSlow * Time.deltaTime;
                    }
                    else
                    {
                        temp.x -= accelSlow * Time.deltaTime;
                    }
                }
                else if (Time.time > walljumpcontroltime)
                {
                    temp.x -= accelAirSlow * Time.deltaTime;
                }
            }
            else
            {
                if (grounded)
                {
                    if (sliding)
                    {
                        temp.x += accelSlideSlow * Time.deltaTime;
                    }
                    else
                    {
                        temp.x += accelSlow * Time.deltaTime;
                    }
                }
                else if (Time.time > walljumpcontroltime)
                {
                    temp.x += accelAirSlow * Time.deltaTime;
                }
            }
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        { //Right
            if (walledR)
            {
                temp.x = 0;
            }
            else if (grounded)
            {
                if (sliding)
                {
                    if (temp.x > velSlide)
                    {
                        temp.x = temp.x;
                    }
                    else if (temp.x < 0)
                    {
                        temp.x += accelSlideSwitch * Time.deltaTime;
                    }
                    else
                    {
                        temp.x += accelSlide * Time.deltaTime;
                    }
                }
                else if (temp.x > velWalk)
                {
                    temp.x = velWalk;
                }
                else if (temp.x < 0)
                {
                    temp.x += accelSwitch * Time.deltaTime;
                }
                else
                {
                    temp.x += accelWalk * Time.deltaTime;
                }
                savedVelAir = temp.x;
            }
            else
            {
                if (Time.time < walljumpcontroltime)
                {
                    temp.x = temp.x;
                }
                else if (temp.x > velAir)
                {
                    temp.x = temp.x;
                }
                else if (temp.x < 0)
                {
                    temp.x += accelAirSwitch * Time.deltaTime;
                }
                else
                {
                    temp.x += accelAir * Time.deltaTime;
                }
            }
        }
        else
        { //Left
            if (walledL)
            {
                temp.x = 0;
            }
            else if (grounded)
            {
                if (sliding)
                {
                    if (temp.x < -velSlide)
                    {
                        temp.x = temp.x;
                    }
                    else if (temp.x > 0)
                    {
                        temp.x -= accelSwitch * Time.deltaTime;
                    }
                    else
                    {
                        temp.x -= accelWalk * Time.deltaTime;
                    }
                }
                else if (temp.x < -velWalk)
                {
                    temp.x = -velWalk;
                }
                else if (temp.x > 0)
                {
                    temp.x -= accelSwitch * Time.deltaTime;
                }
                else
                {
                    temp.x -= accelWalk * Time.deltaTime;
                }
            }
            else
            {
                if (Time.time < walljumpcontroltime)
                {
                    temp.x = temp.x;
                }
                else if (temp.x < -velAir)
                {
                    temp.x = temp.x;
                }
                else if (temp.x > 0)
                {
                    temp.x -= accelAirSwitch * Time.deltaTime;
                }
                else
                {
                    temp.x -= accelAir * Time.deltaTime;
                }
            }
        }
        if (temp.x != 0)
        {
            savedVel = temp.x;
        }

        //GRAVITY
        if (grounded)
        {
            jumping = false;
            gliding = false;
            temp.y = 0;
            if (Input.GetAxisRaw("Vertical") > 0 && !cantstand)
            {
                temp.y = velJumpGround;
                jumping = true;
            }
        }
        else if (walledL)
        {
            jumping = false;
            if (Input.GetKeyDown("w") && wallshoes)
            {
                if (savedVel < -velJumpWallH && Time.time < perfectkicktime)
                {
                    temp.x = -savedVel;
                } else {
                    temp.x = velJumpWallH;
                }
                temp.y = velJumpWallV;
                jumping = true;
                walljumpcontroltime = Time.time + walljumpcontrol;
            }
            if (wallslideL && temp.y < -velWallSlideDown)
            {
                temp.y = -velWallSlideDown;
            }
            else if (Input.GetAxisRaw("Vertical") > 0 && temp.y > 0)
            {
                temp.y -= gravJump * Time.deltaTime;
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                jumping = false;
                temp.y -= gravDown * Time.deltaTime;
            }
            else
            {
                jumping = false;
                temp.y -= gravNormal * Time.deltaTime;
            }
        }
        else if (walledR)
        {
            jumping = false;
            if (Input.GetKeyDown("w") && wallshoes)
            {
                if (savedVel > velJumpWallH && Time.time < perfectkicktime)
                {
                    temp.x = -savedVel;
                }
                else
                {
                    temp.x = -velJumpWallH;
                }
                temp.y = velJumpWallV;
                jumping = true;
                walljumpcontroltime = Time.time + walljumpcontrol;
            }
            if (wallslideR && temp.y < -velWallSlideDown)
            {
                temp.y = -velWallSlideDown;
            }
            else if (Input.GetKey("w") && temp.y > 0)
            {
                temp.y -= gravJump * Time.deltaTime;
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                jumping = false;
                temp.y -= gravDown * Time.deltaTime;
            }
            else
            {
                jumping = false;
                temp.y -= gravNormal * Time.deltaTime;
            }
        }
        else if (Time.time < walljumpgracetime)
        {
            jumping = false;
            if (Input.GetKeyDown("w") && wallshoes)
            {
                //temp.x = -velJumpWallH;
                temp.y = velJumpWallV;
                jumping = true;
                walljumpcontroltime = Time.time + walljumpcontrol;
            }
        }
        else
        {
            if (Input.GetKey("w") && temp.y > 0 && jumping)
            {
                temp.y -= gravJump * Time.deltaTime;
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                jumping = false;
                temp.y -= gravDown * Time.deltaTime;
            }
            else
            {
                jumping = false;
                temp.y -= gravNormal * Time.deltaTime;
                if (Input.GetKeyDown("w") && glidesuit)
                {
                    gliding = true;
                } else if (Input.GetKeyUp("w"))
                {
                    gliding = false;
                }
            }

            if (gliding)
            {
                if (temp.y < -velGlide)
                {
                    temp.y = -velGlide;
                } else if (temp.y < 0)
                {
                    temp.y -= gravGlide * Time.deltaTime;
                }
            }
        }
        transform.GetComponent<Rigidbody2D>().velocity = temp;
    }
}
