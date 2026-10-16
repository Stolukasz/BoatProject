using UnityEngine;

public class BoatController : MonoBehaviour
{
    [Header("SailModel")]
    public SkinnedMeshRenderer sailMesh;
    private int _sailBlendShapeIndex = 0;

    [Header("Boat Sail Opening Data")]
    [SerializeField] float _sailOpenSpeed = 5f;
    [SerializeField] float _sailRotationSpeed = 30f;
    [SerializeField] float _maxBoatSpeed = 10f;


    [Header("Main Sail Data")]
    [SerializeField, Range(-45f, 45f)]
    private float _sailAngle = 0f;

    [SerializeField, Range(0f, 100f)]
    private float _sailValue = 100f;


    [Header("Boat Speed Data")]
    [SerializeField] private float _acceleration = 2f;
    [SerializeField] private float _waterDrag = 0.5f;
    [SerializeField] private float _turnStrength = 30f;

    private float _boatSpeed = 0f;

    private void Update()
    {
        BoatControlManagment();

        UpdateSailVisual();
        UpdateUI();

        BoatMovement();
        BoatRotation();
    }



    //____________________________________________________Calculate Boat Speed and make it move forward_______________________________________________________________________
    private void BoatMovement()
    {
        //calculate speed that boat can reach while sail is folded in current stage
        float sailDeployment = (100 - _sailValue) / 100f; 
        float targetSpeed = _maxBoatSpeed * sailDeployment;



        //boat accelerate if its speed is lover than target or slow down if its higher
        if (_boatSpeed < targetSpeed)
        {
            _boatSpeed = Mathf.MoveTowards(_boatSpeed, targetSpeed, _acceleration * Time.deltaTime);
        }
        else
        {        
            _boatSpeed = Mathf.MoveTowards(_boatSpeed, targetSpeed, _waterDrag * Time.deltaTime);
        }

        //Move forward with calculated boatSpeed
        transform.position += transform.forward * _boatSpeed * Time.deltaTime;
    }


    //_____________________________________________________Calculate Boat Rotation and make it rotate_________________________________________________________________________
    private void BoatRotation()
    {
        float steering = _sailAngle / 45f;
        float speedFactor = _boatSpeed / Mathf.Max(_maxBoatSpeed, 0.01f);
        float turnRate = steering * _turnStrength * speedFactor;


        transform.Rotate(0f, turnRate * Time.deltaTime, 0f, Space.Self);
    }


    //___________________________________Simple Use of InputManager To Check if button iss pressed and used to give boat command______________________________________________
    private void BoatControlManagment()
    {
        //Update sail folding variable then check if it reached max or min value
        if (InputManager.Instance.OnSailSet())
        {
            _sailValue -= _sailOpenSpeed * Time.deltaTime;
        }
        else if (InputManager.Instance.OnSailFolded())
        {
            _sailValue += _sailOpenSpeed * Time.deltaTime;
        }

        _sailValue = Mathf.Clamp(_sailValue, 0f, 100f);

        //Update sail direction variable then check if it reached max or min value
        if (InputManager.Instance.OnTurnRight())
        {
            _sailAngle += _sailRotationSpeed * Time.deltaTime;
        }
        else if (InputManager.Instance.OnTurnLeft())
        {
            _sailAngle -= _sailRotationSpeed * Time.deltaTime;
        }

        _sailAngle = Mathf.Clamp(_sailAngle, -45f, 45f);
    }


    //______________________________________________Updating model of sail. Update model shapekey and sail rotation____________________________________________________________
    private void UpdateSailVisual()
    {
        if (sailMesh != null)
        {
            sailMesh.SetBlendShapeWeight(_sailBlendShapeIndex, _sailValue);

            sailMesh.transform.localRotation = Quaternion.Euler(90f, _sailAngle, 90f);
        }
    }

    //________________________________________________________________Updating UI of sail spread and sail direction__________________________________________________________
    private void UpdateUI()
    {
        if (UIController.Instance == null)
            { return; }

        UIController.Instance.ShowSailValue( 100f - _sailValue );
        UIController.Instance.ShowSailDirection(_sailAngle);

    }
}
