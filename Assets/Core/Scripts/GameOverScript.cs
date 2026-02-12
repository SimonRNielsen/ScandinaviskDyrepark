using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    [SerializeField, Tooltip("The scene that the restart button should load")] private string restartLevelName;
    private Label gameOverLabel;
    private Label totalCollectedLabel;
    private Label totalCollectedValueLabel;
    private Button restartButton;
    private LanguageStrings_SO languageStrings;
    private CollectibleDataSO memory;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    private void OnEnable()
    {
        var doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;
        gameOverLabel = root.Q<Label>("GameOverLabel");
        totalCollectedLabel = root.Q<Label>("TotalCollectedLabel");
        totalCollectedValueLabel = root.Q<Label>("TotalCollectedValueLabel");
        restartButton = root.Q<Button>("RestartButton");
        restartButton.clicked += RestartGame;

        languageStrings = Resources.Load<LanguageStrings_SO>("LanguageStrings_SO");
        gameOverLabel.text = languageStrings.GetString("GameOverLabel");
        totalCollectedLabel.text = languageStrings.GetString("TotalCollectedLabel");
        restartButton.text = languageStrings.GetString(restartButton.name);

        memory = Resources.Load<CollectibleDataSO>("CollectibleDataSO");
        totalCollectedValueLabel.text = memory.CollectibleCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void RestartGame()
    {
        memory.CollectibleCount = 0;
        SceneManager.LoadSceneAsync(restartLevelName, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("GameOverScene");
    }
}
