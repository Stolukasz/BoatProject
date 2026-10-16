using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [Header("UI Toolkit")]
    [SerializeField] private PanelRenderer _panelRenderer;


    //____________________UI references__________________________
    private Label _sailOpeningLabel;
    private Label _sailRotationLabel;
    private Button _closeTutorialButton;
    private VisualElement _tutorial;



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
        if (_panelRenderer == null)
        {
            _panelRenderer = GetComponent<PanelRenderer>();
        }

        if (_panelRenderer != null)
        {
            _panelRenderer.RegisterUIReloadCallback(OnUIReload);
        }
    }

    private void OnDisable()
    {
        if (_panelRenderer != null)
        {
            _panelRenderer.UnregisterUIReloadCallback(OnUIReload);
        }
    }

    private void OnUIReload(PanelRenderer renderer, VisualElement root)
    {
        //_________________________________________Set UI Data___________________________________________________________
        _sailOpeningLabel = root.Q<Label>("sail-opening-label");
        _sailRotationLabel = root.Q<Label>("sail-rotation-label");

        _closeTutorialButton = root.Q<Button>("CloseTutorial");
        _tutorial = root.Q<VisualElement>("Tutorial");



        //_________________________________________Asign Button Function___________________________________________________________
        if (_closeTutorialButton != null && _tutorial != null)
        {
            _closeTutorialButton.clicked -= CloseTutorial;
            _closeTutorialButton.clicked += CloseTutorial;

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
        if (_sailOpeningLabel != null)
        {
            _sailOpeningLabel.text = "Sail opening: " + Mathf.RoundToInt(value)+"%";
        }
    }

    //_________________shows sail direction_____________________
    public void ShowSailDirection(float direction)
    {
        if (_sailRotationLabel != null)
        {
            _sailRotationLabel.text = "Sail rotation: " + Mathf.RoundToInt(direction) + " °";
        }
    }


    //_______________________________________________________Other private block of data to make thins estetic______________________________________________________________

    private void CloseTutorial()
    {
        if (_tutorial != null)
        {
            _tutorial.style.display = DisplayStyle.None;
        }
    }
}