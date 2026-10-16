using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    GameInputActions _gameInputActions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        #region Singleton for easy acces to InputManager

        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }
        _gameInputActions = new GameInputActions();

        #endregion
    }


    private void OnEnable()
    {
        _gameInputActions.Enable();
    }

    private void OnDisable()
    {
        _gameInputActions.Disable();
    }

    
    //____________________________________________________Get Input Data_____________________________________________________
    public bool OnSailSet()
    {
        return _gameInputActions.Player.SetSail.IsPressed();
    }

    public bool OnSailFolded()
    {
        return _gameInputActions.Player.ReefSail.IsPressed();
    }

    public bool OnTurnLeft()
    {
        return _gameInputActions.Player.MoveLeft.IsPressed();
    }

    public bool OnTurnRight()
    {
        return _gameInputActions.Player.MoveRight.IsPressed();
    }

}
