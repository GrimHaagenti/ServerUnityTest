using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "w_Revolver_Asset", menuName = "Revolver")]
public class Revolver_Weapon : Weapon
{
    [SerializeField]
    float bulletSpeed = 20;

    public override void Fire(int dir)
    {

        GameObject bullet = PhotonNetwork.Instantiate("bullet", weapon_S.Muzzle.transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetVelocity(dir, bulletSpeed, bulletDMG);
        //bullet.transform.position = MuzzleTransform.transform.TransformPoint(MuzzleTransform.transform.position);
        Destroy(bullet, 3f);
    }
}
