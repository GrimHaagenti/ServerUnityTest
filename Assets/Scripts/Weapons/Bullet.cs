using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   
    private Rigidbody2D rb;

    private PhotonView pv;
    public int dmg =0;

    private bool isOnline = true;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pv = GetComponent<PhotonView>();
    }


    

    public void SetVelocity(int dir, float speed, int dmg)
    {
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        if(dir < 0)
        {
            spr.flipX = true ;
        }
        else if(dir > 0){ spr.flipX = false; }

        rb.velocity = new Vector2(speed * dir, 0);
        this.dmg = dmg;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOnline && collision.TryGetComponent<Character>(out Character chara))
        {
            pv.RPC("NetworkDestroy", RpcTarget.All);
        }
    }

    [PunRPC]
    public void NetworkDestroy()
    {
        Destroy(this.gameObject);
    }

}