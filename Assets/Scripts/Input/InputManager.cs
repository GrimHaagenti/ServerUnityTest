using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //SINGLETON
    public static InputManager _INPUT_MANAGER = null;

    private InputM inputM;


    // VALUES
    public Vector2 HorizontalInput = Vector2.zero;
    private float timeSinceShoot = 0.1f;
    private float timeSinceJump = 0.1f;

    private void Awake()
    {
        if(_INPUT_MANAGER != null && _INPUT_MANAGER != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            inputM = new InputM();
            inputM.Gameplay.Enable();

            inputM.Gameplay.Move.performed += H_AxisUpdate;
            inputM.Gameplay.Jump.performed += JumpButtonPressed;
            inputM.Gameplay.Shoot.performed += ShootButtonPressed;


            _INPUT_MANAGER = this;
            DontDestroyOnLoad(this);
        }

    }

    private void Update()
    {
        timeSinceJump += Time.deltaTime;
        timeSinceShoot += Time.deltaTime;
        InputSystem.Update();
    }


    private void H_AxisUpdate(InputAction.CallbackContext context)
    {
        HorizontalInput = context.ReadValue<Vector2>();
    }

    private void JumpButtonPressed(InputAction.CallbackContext context)
    {
        timeSinceJump = 0f;
    }
    private void ShootButtonPressed(InputAction.CallbackContext context)
    {
        timeSinceShoot = 0f;
    }
    
    public bool GetJumpButtonPressed()
    {
        return timeSinceJump == 0f;
    }
    public bool GetShootButtonPressed()
    {
        return timeSinceShoot == 0f;
    }


}
