using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelSelectionScript : MonoBehaviour
{
    private Button bearLevelButton;
    private Button deerLevelButton;
    private Button eagleLevelButton;
    private Button ferretLevelButton;
    private Button foxLevelButton;
    private Button polarBearLevelButton;
    private Button wolfLevelButton;
    private TextField passWordTextField;
    private Label levelSelectLabel;
    [Header("Scenes")]
    [SerializeField, Tooltip("The name of the scene that should be used. Make sure to  type it exactly as it is shown in the project window in the unity editor")] private string bearLevelName;
    [SerializeField, Tooltip("The name of the scene that should be used. Make sure to  type it exactly as it is shown in the project window in the unity editor")] private string deerLevelName;
    [SerializeField, Tooltip("The name of the scene that should be used. Make sure to  type it exactly as it is shown in the project window in the unity editor")] private string eagleLevelName;
    [SerializeField, Tooltip("The name of the scene that should be used. Make sure to  type it exactly as it is shown in the project window in the unity editor")] private string ferretLevelName;
    [SerializeField, Tooltip("The name of the scene that should be used. Make sure to  type it exactly as it is shown in the project window in the unity editor")] private string foxLevelName;
    [SerializeField, Tooltip("The name of the scene that should be used. Make sure to  type it exactly as it is shown in the project window in the unity editor")] private string polarBearLevelName;
    [SerializeField, Tooltip("The name of the scene that should be used. Make sure to  type it exactly as it is shown in the project window in the unity editor")] private string wolfLevelName;
    [SerializeField, Tooltip("The name of the scene used for UI/HUD on the chosen level")] string UILevelName;
    [SerializeField, Tooltip("The name of the scene used for UI/HUD on the chosen level")] string UIDeer;
    [SerializeField, Tooltip("The name of the scene used for UI/HUD on the chosen level")] string UIEagle;
    [SerializeField, Tooltip("The opacity of buttons with no referenced scene"), Range(0, 1)] private float unappliedButtonOpacity = 0.1f;
    [SerializeField, Tooltip("The opacity of buttons with a scene, but not yet unlocked by the player"), Range(0, 1)] private float lockedButtonOpacity = 0.3f;
    [Header("Passwords")]
    [SerializeField, Tooltip("Password for unlocking the bear level")] private string bearPassword;
    [SerializeField, Tooltip("Password for unlocking the deer level")] private string deerPassword;
    [SerializeField, Tooltip("Password for unlocking the eagle level")] private string eaglePassword;
    [SerializeField, Tooltip("Password for unlocking the ferret level")] private string ferretPassword;
    [SerializeField, Tooltip("Password for unlocking the fox level")] private string foxPassword;
    [SerializeField, Tooltip("Password for unlocking the polar bear level")] private string polarBearPassword;
    [SerializeField, Tooltip("Password for unlocking the wolf level")] private string wolfPassword;
    private LanguageStrings_SO languageStrings;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void OnEnable()
    {
        //References for all the buttons in the UI document
        var doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;
        bearLevelButton = root.Q<Button>("BearLevelButton");
        deerLevelButton = root.Q<Button>("DeerLevelButton");
        eagleLevelButton = root.Q<Button>("EagleLevelButton");
        ferretLevelButton = root.Q<Button>("FerretLevelButton");
        foxLevelButton = root.Q<Button>("FoxLevelButton");
        polarBearLevelButton = root.Q<Button>("PolarBearLevelButton");
        wolfLevelButton = root.Q<Button>("WolfLevelButton");
        levelSelectLabel = root.Q<Label>("LevelSelectLabel");
        passWordTextField = root.Q<TextField>("PasswordTextInput");
        
       //Set languages of buttons:
        languageStrings = Resources.Load<LanguageStrings_SO>("LanguageStrings_SO");
        levelSelectLabel.text = languageStrings.GetString("LevelSelectLabel");
        bearLevelButton.text = languageStrings.GetString("BearLevelButton");
        deerLevelButton.text = languageStrings.GetString("DeerLevelButton");
        eagleLevelButton.text = languageStrings.GetString("EagleLevelButton");
        ferretLevelButton.text = languageStrings.GetString("FerretLevelButton");
        foxLevelButton.text = languageStrings.GetString("FoxLevelButton");
        polarBearLevelButton.text = languageStrings.GetString("PolarBearLevelButton");
        wolfLevelButton.text = languageStrings.GetString("WolfLevelButton");
        passWordTextField.textEdition.placeholder = languageStrings.GetString("PasswordTextInput");


       // DisableButtons(new Button[] { bearLevelButton, eagleLevelButton, ferretLevelButton, foxLevelButton, polarBearLevelButton, wolfLevelButton });


        //Actions added to buttons
        bearLevelButton.clicked += OnBearPressed;
        deerLevelButton.clicked += OnDeerPressed;
        eagleLevelButton.clicked += OnEaglePressed;
        ferretLevelButton.clicked += OnFerretPressed;
        foxLevelButton.clicked += OnFoxPressed;
        polarBearLevelButton.clicked += OnPolarBearPressed;
        wolfLevelButton.clicked += OnWolfPressed;
        passWordTextField.isDelayed = true;
        passWordTextField.RegisterCallback<ChangeEvent<string>>(changeEvent => CheckInputAgainstPasswordTextInput(changeEvent));
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Loads the appropriate scenes for the corresponding button (level and UI) and calls mehtods to unload the level selection scene. 
    /// </summary>
    private void OnBearPressed()
    {
        if (bearLevelName != string.Empty)
        {
            SceneManager.LoadSceneAsync(bearLevelName, LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync(UILevelName, LoadSceneMode.Additive);
            UnloadLevelSelectionScene();
        }

    }
    /// <summary>
    /// Loads the appropriate scenes for the corresponding button (level and UI) and calls mehtods to unload the level selection scene. 
    /// </summary>
    private void OnDeerPressed()
    {
        if (deerLevelName != string.Empty)
        {
            SceneManager.LoadSceneAsync(deerLevelName, LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync(UIDeer, LoadSceneMode.Additive);
            UnloadLevelSelectionScene();
        }
    }
    /// <summary>
    /// Loads the appropriate scenes for the corresponding button (level and UI) and calls mehtods to unload the level selection scene. 
    /// </summary>
    private void OnEaglePressed()
    {
        if (eagleLevelName != string.Empty)
        {
            SceneManager.LoadSceneAsync(eagleLevelName, LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync(UIEagle, LoadSceneMode.Additive);
            UnloadLevelSelectionScene();
        }
    }

    /// <summary>
    /// Loads the appropriate scenes for the corresponding button (level and UI) and calls mehtods to unload the level selection scene. 
    /// </summary>
    private void OnFerretPressed()
    {
        if (ferretLevelName != string.Empty)
        {
            SceneManager.LoadSceneAsync(ferretLevelName, LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync(UILevelName, LoadSceneMode.Additive);
            UnloadLevelSelectionScene();
        }
    }

    /// <summary>
    /// Loads the appropriate scenes for the corresponding button (level and UI) and calls mehtods to unload the level selection scene. 
    /// </summary>
    private void OnFoxPressed()
    {
        if (foxLevelName != string.Empty)
        {
            SceneManager.LoadSceneAsync(foxLevelName, LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync(UILevelName, LoadSceneMode.Additive);
            UnloadLevelSelectionScene();
        }
    }
    /// <summary>
    /// Loads the appropriate scenes for the corresponding button (level and UI) and calls mehtods to unload the level selection scene. 
    /// </summary>
    private void OnPolarBearPressed()
    {
        if (polarBearLevelName != string.Empty)
        {
            SceneManager.LoadSceneAsync(polarBearLevelName, LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync(UILevelName, LoadSceneMode.Additive);
            UnloadLevelSelectionScene();
        }
    }
    /// <summary>
    /// Loads the appropriate scenes for the corresponding button (level and UI) and calls mehtods to unload the level selection scene. 
    /// </summary>
    private void OnWolfPressed()
    {
        if (wolfLevelName != string.Empty)
        {
            SceneManager.LoadSceneAsync(wolfLevelName, LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync(UILevelName, LoadSceneMode.Additive);
            UnloadLevelSelectionScene();
        }
    }

    /// <summary>
    /// Unloads the LevelSelectionScene
    /// </summary>
    private void UnloadLevelSelectionScene()
    {
        SceneManager.UnloadSceneAsync("LevelSelectionScene");
    }

    /// <summary>
    /// Sets the opacities of the button, according to wether or not they have a scene they reference. 
    /// </summary>
    private void SetButtonOpacity(Button button)
    {
        switch (button.name)
        {
            case "BearLevelButton":
                if (bearLevelName == string.Empty)
                {
                    button.style.opacity = unappliedButtonOpacity;
                }
                else
                {
                    button.style.opacity = lockedButtonOpacity;
                }
                break;
            case "DeerLevelButton":
                if (deerLevelName == string.Empty)
                {
                    button.style.opacity = unappliedButtonOpacity;
                }
                else
                {
                    button.style.opacity = lockedButtonOpacity;
                }
                break;
            case "EagleLevelButton":
                if (eagleLevelName == string.Empty)
                {
                    button.style.opacity = unappliedButtonOpacity;
                }
                else
                {
                    button.style.opacity = lockedButtonOpacity;
                }
                break;
            case "FerretLevelButton":
                if (ferretLevelName == string.Empty)
                {
                    button.style.opacity = unappliedButtonOpacity;
                }
                else
                {
                    button.style.opacity = lockedButtonOpacity;
                }
                break;
            case "FoxLevelButton":
                if (foxLevelName == string.Empty)
                {
                    button.style.opacity = unappliedButtonOpacity;
                }
                else
                {
                    button.style.opacity = lockedButtonOpacity;
                }
                break;
            case "PolarBearLevelButton":
                if (polarBearLevelName == string.Empty)
                {
                    button.style.opacity = unappliedButtonOpacity;
                }
                else
                {
                    button.style.opacity = lockedButtonOpacity;
                }
                break;
            case "WolfLevelButton":
                if (wolfLevelName == string.Empty)
                {
                    button.style.opacity = unappliedButtonOpacity;
                }
                else
                {
                    button.style.opacity = lockedButtonOpacity;
                }
                break;
        }
    }

    /// <summary>
    /// Disable all buttons in the given array, and sets their opacites by callin SetButtonOpacity method.
    /// </summary>
    /// <param name="buttons">The array of butttons to disable</param>
    private void DisableButtons(Button[] buttons)
    {
        foreach (Button button in buttons)
        {
            button.SetEnabled(false);
            SetButtonOpacity(button);
        }
    }

    /// <summary>
    /// Checks if the input is equal to one of the passwords for unlocking scenes.
    /// If so, and if the scene is assigned to a button, the corrosponding button is enabled and opacity changed. 
    /// </summary>
    /// <param name="changeEvent">The changeevent that triggers the method</param>
    public void CheckInputAgainstPasswordTextInput(ChangeEvent<string> changeEvent)
    {
        if(changeEvent.newValue == bearPassword && bearLevelName != string.Empty)
        {
                    bearLevelButton.SetEnabled(true);
                    bearLevelButton.style.opacity = 1;
        }
        else if (changeEvent.newValue == deerPassword && deerLevelName != string.Empty)
        {
            deerLevelButton.SetEnabled(true);
            deerLevelButton.style.opacity = 1;
        }
        else if (changeEvent.newValue == eaglePassword && eagleLevelName != string.Empty)
        {
            eagleLevelButton.SetEnabled(true);
            eagleLevelButton.style.opacity = 1;
        }
        else if (changeEvent.newValue == ferretPassword && ferretLevelName != string.Empty)
        {
            ferretLevelButton.SetEnabled(true);
            ferretLevelButton.style.opacity = 1;
        }
        else if (changeEvent.newValue == foxPassword && foxLevelName != string.Empty)
        {
            foxLevelButton.SetEnabled(true);
            foxLevelButton.style.opacity = 1;
        }
        else if (changeEvent.newValue == polarBearPassword && polarBearLevelName != string.Empty)
        {
            polarBearLevelButton.SetEnabled(true);
            polarBearLevelButton.style.opacity = 1;
        }
        else if (changeEvent.newValue == wolfPassword && wolfLevelName != string.Empty)
        {
            wolfLevelButton.SetEnabled(true);
            wolfLevelButton.style.opacity = 1;
        }
        passWordTextField.SetValueWithoutNotify(string.Empty);
    }
}
