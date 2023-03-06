using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Character : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("Stats")]
    private float baseValue = 460;

    private int Max_hp = 0;
    private int current_hp = 0;
    private float speed = 600f;
    private float jumpForce = 600f;
    private float bullet_dmg = 0;
    private float bullet_size = 0;
    


    Race characterRace;

    [Header("Is Online Switch")]
    [SerializeField]
    private bool isOnline = false;


    //WEAPON
    [Header("To be replaced - projectile")]
    [SerializeField]
    private Weapon m_weapon;
    [SerializeField]
    private GameObject WeaponPrefab;
    
    //COMPONENTS
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private GameObject WeaponParent;

    //PHOTON
    private PhotonView pv;

    private float desiredMovementAxis = 0f;
    private int rotationInt = 1;

    private int damageReceived = 0;


    //Enemy
    private Vector3 enemyPosition = Vector3.zero;
    private float enemyRotAxis = 0;
    private int enemyHp = 0;
    
    public void InitCharacter(int raceID)
    {
        characterRace = GameManager._GAME_MANAGER._GAME_DATA.races[raceID];
        Max_hp = characterRace.max_hp;
        current_hp = characterRace.max_hp;
        string sp = "1." + characterRace.speed;
        speed = float.Parse(sp);
        string jp = "1." + characterRace.jump_force;
        jumpForce = float.Parse(jp);
        bullet_dmg = characterRace.dmg;
        bullet_size = characterRace.bullet_size;
        isOnline = true;

    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        pv = GetComponent<PhotonView>();

        PhotonNetwork.SendRate = 20;
        PhotonNetwork.SerializationRate = 20;

        WeaponPrefab =  m_weapon.OnPickup(WeaponParent);
    }

    private void PickupWeapon(Weapon newWeapon)
    {
        Destroy(WeaponPrefab);
        m_weapon = newWeapon;
        WeaponPrefab = m_weapon.OnPickup(WeaponParent);
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
        if (enemyHp < 0)
        {
            SendToLobby();
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(desiredMovementAxis * Time.fixedDeltaTime * speed, rb.velocity.y);
    }

    private void RotateSprite(float dir)
    {
       if(dir > 0) {
            spriteRenderer.flipX = false;
            if (rotationInt != 1) {
                WeaponParent.transform.localPosition = new Vector3(WeaponParent.transform.localPosition.x * -1, WeaponParent.transform.localPosition.y, WeaponParent.transform.localPosition.z);
            }
            rotationInt = 1;
            m_weapon.RotationSprite(rotationInt);

        }
        else if(dir < 0) {
            spriteRenderer.flipX = true;
            if (rotationInt != -1)
            {
                WeaponParent.transform.localPosition = new Vector3(WeaponParent.transform.localPosition.x * -1, WeaponParent.transform.localPosition.y, WeaponParent.transform.localPosition.z);
            }
            rotationInt = -1;
            m_weapon.RotationSprite(rotationInt);
        }
    }

    private void CheckInputs()
    {
        desiredMovementAxis = InputManager._INPUT_MANAGER.HorizontalInput.x;

        RotateSprite(desiredMovementAxis);

        RaycastHit2D? hit = null;
        int LayerMask = 1 << 6;
        hit = Physics2D.Linecast(transform.position, transform.position + (-transform.up * 1f), LayerMask);
        Debug.DrawLine(transform.position, transform.position + (-transform.up * 1f));
        if (InputManager._INPUT_MANAGER.GetJumpButtonPressed() && hit.Value.collider != null )
        {
            rb.AddForce(new Vector2(0f, jumpForce));
        }

        if (InputManager._INPUT_MANAGER.GetShootButtonPressed()) 
        { 
            McShooty(); 
        }
    }
    private void SmoothReplicate()
    {
        transform.position = Vector3.Lerp(transform.position, enemyPosition, Time.deltaTime*20);
        RotateSprite(enemyRotAxis);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (isOnline)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(desiredMovementAxis);
                stream.SendNext(current_hp);
            }
            else if (stream.IsReading)
            {
                enemyPosition = (Vector3)stream.ReceiveNext();
                enemyRotAxis = (float)stream.ReceiveNext();
                enemyHp = (int)stream.ReceiveNext();
            }
        }
    }

    public void McShooty()
    {
        if (isOnline)
        {
            m_weapon.Fire(rotationInt);
        }
        else
        {
            m_weapon.Fire(rotationInt);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<WeaponPickup>(out WeaponPickup newWeapon))
        {
            PickupWeapon(newWeapon.PickWeapon);
        }
        else if (collision.gameObject.TryGetComponent<Bullet>(out Bullet colBullet)){
            damageReceived = colBullet.dmg;
            Damage();
        }
        

    }
    public void Damage()
    {
        pv.RPC("NetworkDamage", RpcTarget.All);
    }

    [PunRPC]
    public void NetworkDamage()
    {
        current_hp -= damageReceived;
        if (current_hp < 0)
        {
            SendToLobby();
        } 
    }


    public void SendToLobby()
    {
        pv.RPC("NetworkSendToLobby", RpcTarget.All);
    }

    [PunRPC]
    public void NetworkSendToLobby()
    {
        GameManager._GAME_MANAGER.GoBackToLobby();
    }
}

