using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [Header("UI Toolkit")]
    [SerializeField] private PanelRenderer panelRenderer;


    //____________________UI references__________________________
    private Label sailOpeningLabel;
    private Label sailRotationLabel;
    private Button closeTutorialButton;
    private VisualElement tutorial;



    private void Awake()
    {
        #region Singleton for easy acces to UI

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        #endregion

    }


    
    private void OnEnable()
    {
        if (panelRenderer == null)
        {
            panelRenderer = GetComponent<PanelRenderer>();
        }

        if (panelRenderer != null)
        {
            panelRenderer.RegisterUIReloadCallback(OnUIReload);
        }
    }

    private void OnDisable()
    {
        if (panelRenderer != null)
        {
            panelRenderer.UnregisterUIReloadCallback(OnUIReload);
        }
    }

    private void OnUIReload(PanelRenderer renderer, VisualElement root)
    {
        //_________________________________________Set UI Data___________________________________________________________
        sailOpeningLabel = root.Q<Label>("sail-opening-label");
        sailRotationLabel = root.Q<Label>("sail-rotation-label");

        closeTutorialButton = root.Q<Button>("CloseTutorial");
        tutorial = root.Q<VisualElement>("Tutorial");



        //_________________________________________Asign Button Function___________________________________________________________
        if (closeTutorialButton != null && tutorial != null)
        {
            closeTutorialButton.clicked -= CloseTutorial;
            closeTutorialButton.clicked += CloseTutorial;

        }
        else
        {
            Debug.LogError("Cannot Find closeTutorialButton or tutorial object");
        }


    }

    

    //____________________________________________________________________Boat UI Methods_________________________________________________________________________

    //__________shows the level of sail deployment_______________
    public void ShowSailValue(float value)
    {
        if (sailOpeningLabel != null)
        {
            sailOpeningLabel.text = "Sail opening: " + Mathf.RoundToInt(value)+"%";
        }
    }

    //_________________shows sail direction_____________________
    public void ShowSailDirection(float direction)
    {
        if (sailRotationLabel != null)
        {
            sailRotationLabel.text = "Sail rotation: " + Mathf.RoundToInt(direction) + " °";
        }
    }


    //_______________________________________________________Other private block of data to make thins estetic______________________________________________________________

    private void CloseTutorial()
    {
        tutorial.style.display = DisplayStyle.None;
    }
}