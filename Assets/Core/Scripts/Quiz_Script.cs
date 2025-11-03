using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Quiz_Script : MonoBehaviour
{

    [SerializeField, Tooltip("Array of quizs to be added, do not add more than one of each language or difficulty to a total of 9")] private Quiz_SO[] addedQuizes;
    [SerializeField, Range(-4f, 10f), Tooltip("Time to close resultwindow is 5 seconds minus this parameter to a minimum of 1 second and a maximum of 15 seconds")] private float closeTimeParameter = 0f;
    [SerializeField, Tooltip("Reward time to add"), Min(5)] private float timeReward = 5f;
    private Dictionary<(LanguageOptions, QuizDifficulty), Quiz_SO> quizs;
    private QuizMemory quizMemory;
    private Quiz_SO quiz;
    private VisualElement picture;
    private Label question;
    private Button option1, option2, option3;
    private bool closingQuiz = false;
    private bool buttonsEnabled = false;
    private const float defaultCloseTime = 5f;
    private const float wrongAnswer = -1f;
    private float closingIn;
    private int questionIndex;
    private string result = string.Empty;
    private List<QuestionEntry> relevantQuestions;

    /// <summary>
    /// Get/Set property for "language" option
    /// </summary>
    public LanguageOptions Language
    {

        get
        {

            if (quizMemory != null)
                return quizMemory.Language;

            return LanguageOptions.Dansk;

        }

    }
    /// <summary>
    /// Get/Set property for "difficulty" option
    /// </summary>
    public QuizDifficulty Difficulty
    {

        get
        {

            if (quizMemory != null)
                return quizMemory.Difficulty;

            return QuizDifficulty.Easy;

        }

    }
    /// <summary>
    /// Combined default close time and slidable parameter
    /// </summary>
    public float CloseTime { get => defaultCloseTime + closeTimeParameter; }

    /// <summary>
    /// Runs initialization logic
    /// </summary>
    private void OnEnable()
    {

        Time.timeScale = 0;

        CheckQuizAndMemory();

        AssignLabelAndButtons();

        InitiateQuiz();

    }

    /// <summary>
    /// Runs backup unassigning of buttons
    /// </summary>
    private void OnDisable()
    {

        if (buttonsEnabled)
            DisableButtons();

        Time.timeScale = 1;

    }

    /// <summary>
    /// Updates countdown on textfield
    /// </summary>
    void Update()
    {

        if (closingQuiz)
        {
            closingIn -= Time.unscaledDeltaTime;
            question.text = result + $"\n\n{(int)closingIn}s";
        }

    }

    /// <summary>
    /// Initiates logic for correct answer
    /// </summary>
    private void CorrectAnswer()
    {

        if (buttonsEnabled)
            DisableButtons();

        switch (quiz.Language) //Gives feedback dependant on language selected
        {
            case LanguageOptions.Dansk:
                question.text = "Rigtigt svar";
                break;
            case LanguageOptions.English:
                question.text = "Correct Answer";
                break;
            case LanguageOptions.Deutsch:
                question.text = "Richtige antwort";
                break;
        }

        quizMemory.CorrectAnswer?.Invoke(timeReward);

        StartCoroutine(CloseQuiz());

    }

    /// <summary>
    /// Initiates logic for wrong answer
    /// </summary>
    private void WrongAnswer()
    {

        if (buttonsEnabled)
            DisableButtons();

        switch (quiz.Language) //Gives feedback dependant on language selected
        {
            case LanguageOptions.Dansk:
                question.text = "Forkert svar";
                break;
            case LanguageOptions.English:
                question.text = "Wrong answer";
                break;
            case LanguageOptions.Deutsch:
                question.text = "Falsche antwort";
                break;
        }

        quizMemory.CorrectAnswer?.Invoke(wrongAnswer);

        StartCoroutine(CloseQuiz());

    }

    /// <summary>
    /// Gets a new question, and (re)sets parameters as needed + assigns actions to buttons + flags them as enabled
    /// </summary>
    private void InitiateQuiz()
    {

        if (quiz == null || question == null) //Checks conditions are in order
        {

            if (quiz == null)
                Debug.Log("No quiz found at InitiateQuiz");

            if (question == null)
                Debug.Log("No label for question found at InitiateQuiz");

            return;

        }

        closingQuiz = false;
        closingIn = CloseTime;

        if (relevantQuestions.Count == quizMemory.previousQuestions.Count) //Resets memory if all questions have been answered 
            quizMemory.previousQuestions.Clear();

        do
        {

            questionIndex = Random.Range(0, relevantQuestions.Count); //Gives a random index number inside bounds

        } while (quizMemory.previousQuestions.Contains(questionIndex)); //Loops until a new index is found
        quizMemory.previousQuestions.Add(questionIndex); //Adds int so question can be skipped in favor of others

        //Displays question and related answers on label and buttons
        if (relevantQuestions[questionIndex].Picture != null && relevantQuestions[questionIndex].displayBoth)
        {

            picture.style.backgroundImage = new StyleBackground(relevantQuestions[questionIndex].Picture);
            question.text = relevantQuestions[questionIndex].Question;

        }
        else if (relevantQuestions[questionIndex].Picture != null)
        {

            question.text = string.Empty;
            picture.style.backgroundImage = new StyleBackground(relevantQuestions[questionIndex].Picture);

        }
        else
        {

            question.text = relevantQuestions[questionIndex].Question;
            picture.style.backgroundImage = new StyleBackground();

        }
        option1.text = relevantQuestions[questionIndex].Answers[(int)QuestionOptions.Option1];
        option2.text = relevantQuestions[questionIndex].Answers[(int)QuestionOptions.Option2];
        option3.text = relevantQuestions[questionIndex].Answers[(int)QuestionOptions.Option3];

        switch (relevantQuestions[questionIndex].CorrectAnswer) //Assigns actions to buttons dependant on "CorrectAnswer", which is Option# minus 1 to account for 0-indexation
        {
            case 0:
                option1.clicked += CorrectAnswer;
                option2.clicked += WrongAnswer;
                option3.clicked += WrongAnswer;
                break;
            case 1:
                option2.clicked += CorrectAnswer;
                option1.clicked += WrongAnswer;
                option3.clicked += WrongAnswer;
                break;
            case 2:
                option3.clicked += CorrectAnswer;
                option2.clicked += WrongAnswer;
                option1.clicked += WrongAnswer;
                break;
        }

        buttonsEnabled = true;

    }

    /// <summary>
    /// Removes actionbinding for buttons + flags them as disabled
    /// </summary>
    private void DisableButtons()
    {

        switch (relevantQuestions[questionIndex].CorrectAnswer) //Unassigns actions from buttons in same manner as they were assigned
        {
            case 0:
                option1.clicked -= CorrectAnswer;
                option2.clicked -= WrongAnswer;
                option3.clicked -= WrongAnswer;
                break;
            case 1:
                option2.clicked -= CorrectAnswer;
                option1.clicked -= WrongAnswer;
                option3.clicked -= WrongAnswer;
                break;
            case 2:
                option3.clicked -= CorrectAnswer;
                option2.clicked -= WrongAnswer;
                option1.clicked -= WrongAnswer;
                break;
        }

        buttonsEnabled = false;

    }

    /// <summary>
    /// Coroutine for closing quiz-scene
    /// </summary>
    /// <returns>Queued actions</returns>
    private IEnumerator CloseQuiz()
    {

        result = question.text; //Maintains string
        closingQuiz = true;

        yield return new WaitForSecondsRealtime(CloseTime);

        yield return SceneManager.UnloadSceneAsync(gameObject.scene);

    }

    /// <summary>
    /// Checks for valid persistant data
    /// </summary>
    private void CheckQuizAndMemory()
    {

        if (quizMemory == null)
            quizMemory = Resources.Load<QuizMemory>("QuizMemory_SO");

        if (quizs == null || quizs.Count == 0) //Populates quizs as needed
        {

            quizs = new Dictionary<(LanguageOptions, QuizDifficulty), Quiz_SO>();
            foreach (Quiz_SO entry in addedQuizes)
            {

                if (entry != null && quizs.TryAdd((entry.Language, entry.Difficulty), entry)) { }
                else
                    Debug.Log("Multiple same language and difficulty quizs");

            }

        }

        if (quizs.TryGetValue((Language, Difficulty), out Quiz_SO foundQuiz))
            quiz = foundQuiz;
        else if (quizs.TryGetValue((Language, QuizDifficulty.Easy), out Quiz_SO alternativeQuiz))
            quiz = alternativeQuiz;
        else
        {

            Debug.Log("No valid quiz found for combined difficulty and language");
            quiz = quizs.Values.FirstOrDefault(); //Tries getting any quiz if none were found that matched language and difficulty

            if (quiz == null)
                Debug.Log("No quizs found");

        }

        List<QuestionEntry> list = quiz.Questions.FindAll(x => x.associatedMap == quizMemory.CurrentMap);

        if (list.Count > 0)
            relevantQuestions = list;
        else
            relevantQuestions = quiz.Questions;

    }

    /// <summary>
    /// Assigns fields to be manipulated through logic
    /// </summary>
    private void AssignLabelAndButtons()
    {

        if (question != null && picture != null && option1 != null && option2 != null && option3 != null)
            return;

        var root = GetComponent<UIDocument>().rootVisualElement;

        if (root != null)
        {

            question = root.Q<Label>("textbox");
            picture = root.Q<VisualElement>("picture");
            option1 = root.Q<Button>("option1");
            option2 = root.Q<Button>("option2");
            option3 = root.Q<Button>("option3");

        }
        else
            Debug.Log("No quiz-element found");

    }

}