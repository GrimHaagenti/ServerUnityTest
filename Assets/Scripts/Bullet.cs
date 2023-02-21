using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;
    private Rigidbody2D rb;

    private PhotonView pv;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pv = GetComponent<PhotonView>();
    }


    private void Start()
    {
        rb.velocity = new Vector2(speed, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        pv.RPC("NetworkDestroy", RpcTarget.All);
    }

    [PunRPC]
    public void NetworkDestroy()
    {
        Destroy(this.gameObject);
    }

}