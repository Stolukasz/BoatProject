using UnityEngine;

public class BoatController : MonoBehaviour
{
    [Header("SailModel")]
    public SkinnedMeshRenderer sailMesh;
    private int sailBlendShapeIndex = 0;

    [Header("Boat Sail Opening Data")]
    public float sailOpenSpeed = 5f;
    public float sailRotationSpeed = 30f;
    public float maxBoatSpeed = 10f;


    [Header("Main Sail Data")]
    [SerializeField, Range(-45f, 45f)]
    private float sailAngle = 0f;

    [SerializeField, Range(0f, 100f)]
    private float sailValue = 100f;


    [Header("Boat Speed Data")]
    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float waterDrag = 0.5f;
    [SerializeField] private float turnStrength = 30f;

    private float boatSpeed = 0f;

    void Update()
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
        float sailDeployment = (100 - sailValue) / 100f; 
        float targetSpeed = maxBoatSpeed * sailDeployment;



        //boat accelerate if its speed is lover than target or slow down if its higher
        if (boatSpeed < targetSpeed)
        {
            boatSpeed = Mathf.MoveTowards(boatSpeed, targetSpeed, acceleration * Time.deltaTime);
        }
        else
        {        
            boatSpeed = Mathf.MoveTowards(boatSpeed, targetSpeed, waterDrag * Time.deltaTime);
        }

        //Move forward with calculated boatSpeed
        transform.position += transform.forward * boatSpeed * Time.deltaTime;
    }


    //_____________________________________________________Calculate Boat Rotation and make it rotate_________________________________________________________________________
    private void BoatRotation()
    {
        float steering = sailAngle / 45f;
        float speedFactor = boatSpeed / Mathf.Max(maxBoatSpeed, 0.01f);
        float turnRate = steering * turnStrength * speedFactor;


        transform.Rotate(0f, turnRate * Time.deltaTime, 0f, Space.Self);
    }


    //___________________________________Simple Use of InputManager To Check if button iss pressed and used to give boat command______________________________________________
    private void BoatControlManagment()
    {
        //Update sail folding variable then check if it reached max or min value
        if (InputManager.instance.OnSailSet())
        {
            sailValue -= sailOpenSpeed * Time.deltaTime;
        }
        else if (InputManager.instance.OnSailFolded())
        {
            sailValue += sailOpenSpeed * Time.deltaTime;
        }

        sailValue = Mathf.Clamp(sailValue, 0f, 100f);

        //Update sail direction variable then check if it reached max or min value
        if (InputManager.instance.OnTurnRight())
        {
            sailAngle += sailRotationSpeed * Time.deltaTime;
        }
        else if (InputManager.instance.OnTurnLeft())
        {
            sailAngle -= sailRotationSpeed * Time.deltaTime;
        }

        sailAngle = Mathf.Clamp(sailAngle, -45f, 45f);
    }


    //______________________________________________Updating model of sail. Update model shapekey and sail rotation____________________________________________________________
    private void UpdateSailVisual()
    {
        if (sailMesh != null)
        {
            sailMesh.SetBlendShapeWeight(sailBlendShapeIndex, sailValue);

            sailMesh.transform.localRotation = Quaternion.Euler(90f, sailAngle, 90f);
        }
    }

    //________________________________________________________________Updating UI of sail spread and sail direction__________________________________________________________
    private void UpdateUI()
    {
        UIController.Instance.ShowSailValue( 100f - sailValue );
        UIController.Instance.ShowSailDirection(sailAngle);

    }
}
