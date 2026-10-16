using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance { get; private set; }
    GameInputActions gameInputActions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        #region Singleton for easy acces to InputManager

        if (instance != null)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
        }
        gameInputActions = new GameInputActions();

        #endregion
    }


    private void OnEnable()
    {
        gameInputActions.Enable();
    }

    private void OnDisable()
    {
        gameInputActions.Disable();
    }

    
    //____________________________________________________Get Input Data_____________________________________________________
    public bool OnSailSet()
    {
        return gameInputActions.Player.SetSail.IsPressed();
    }

    public bool OnSailFolded()
    {
        return gameInputActions.Player.ReefSail.IsPressed();
    }

    public bool OnTurnLeft()
    {
        return gameInputActions.Player.MoveLeft.IsPressed();
    }

    public bool OnTurnRight()
    {
        return gameInputActions.Player.MoveRight.IsPressed();
    }

}
