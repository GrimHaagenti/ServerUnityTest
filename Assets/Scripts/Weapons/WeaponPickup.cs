using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField]
    private Weapon weaponToPick;

    private PhotonView pv;
    public Weapon PickWeapon { get { return weaponToPick; } }

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Character>(out Character chara))
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
