using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon: ScriptableObject
{
    [SerializeField]
    protected GameObject gun_Prefab;
    [SerializeField]
    protected GameObject bullet_Prefab;

    protected int bulletDMG;
    protected int bulletScale ;
    protected WeaponScript weapon_S;

    protected Vector3 localMuzzlePos;
    public GameObject OnPickup(GameObject parent)
    {
        GameObject weaponObj = Instantiate(gun_Prefab, parent.transform);
        WeaponScript ws = weaponObj.GetComponent<WeaponScript>();
        weapon_S = ws;
        localMuzzlePos = weapon_S.Muzzle.transform.localPosition;
        bullet_Prefab = ws.Bullet;

        return weaponObj;
    }

    public void RotationSprite(int rotate)
    {
        if (rotate > 0)
        {
            weapon_S.spr.flipX = false;
            weapon_S.Muzzle.transform.localPosition = new Vector3(localMuzzlePos.x, localMuzzlePos.y, localMuzzlePos.z);
        }
        if (rotate < 0)
        {
            weapon_S.spr.flipX = true;
            weapon_S.Muzzle.transform.localPosition = new Vector3(-localMuzzlePos.x, localMuzzlePos.y, localMuzzlePos.z);
        }
    }
    public abstract void Fire(int dir);

    
}
