using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Character : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("Stats")]
    [SerializeField]
    private float speed = 100f;
    [SerializeField]
    private float jumpForce = 200f;

    [Header("Is Online Switch")]
    [SerializeField]
    private bool isOnline = false;

    [Header("To be replaced - projectile")]
    [SerializeField]
    private GameObject projectile;

    private Rigidbody2D rb;
    private PhotonView pv;

    private float desiredMovementAxis = 0f;
    private Vector3 enemyPosition = Vector3.zero; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pv = GetComponent<PhotonView>();

        PhotonNetwork.SendRate = 20;
        PhotonNetwork.SerializationRate = 20;
    }

    private void Update()
    {
        if (pv.IsMine)
        {
            CheckInputs();
        }
        else
        {
            if (isOnline)
            {
                SmoothReplicate();
            }
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(desiredMovementAxis * Time.fixedDeltaTime * speed, rb.velocity.y);
    }


    private void CheckInputs()
    {
        desiredMovementAxis = Input.GetAxisRaw("Horizontal");


        if (Input.GetButtonDown("Jump") && Mathf.Approximately(rb.velocity.y, 0))
        {
            rb.AddForce(new Vector2(0f, jumpForce));
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) { McShooty(); }
    }
    private void SmoothReplicate()
    {
        transform.position = Vector3.Lerp(transform.position, enemyPosition, Time.deltaTime*20);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (isOnline)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
            }
            else if (stream.IsReading)
            {
                enemyPosition = (Vector3)stream.ReceiveNext();
            }
        }
    }

    public void McShooty()
    {
        if (isOnline)
        {
            PhotonNetwork.Instantiate("Bullet", transform.position + new Vector3(1f, 0f, 0f), Quaternion.identity);
        }
        else
        {
            Instantiate(projectile, transform.position + new Vector3(1f, 0f, 0f), Quaternion.identity);
        }
    }


    public void Damage()
    {
        pv.RPC("NetworkDie", RpcTarget.All);
    }

    [PunRPC]
    public void NetworkDie()
    {
        Destroy(this.gameObject);
    }

}

