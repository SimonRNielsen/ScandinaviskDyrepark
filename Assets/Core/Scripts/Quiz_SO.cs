using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object that holds a list of questions and associated answers for a certain language or difficulty
/// </summary>
[CreateAssetMenu(fileName = "Quiz_SO", menuName = "Scriptable Objects/Quiz", order = 0)]
public class Quiz_SO : ScriptableObject
{

    /// <summary>
    /// Language-option for the quiz
    /// </summary>
    [Header("Quiz Data")]
    [Tooltip("Language the questions are in")] public LanguageOptions Language;
    /// <summary>
    /// Difficulty-option for the quiz
    /// </summary>
    [Tooltip("Approximated difficulty of quiz")] public QuizDifficulty Difficulty;
    /// <summary>
    /// List of questions and answers
    /// </summary>
    [Tooltip("List of questions")] public List<QuestionEntry> Questions = new List<QuestionEntry>();

}

[Serializable]
public class QuestionEntry
{

    #region Fields

    [SerializeField, Tooltip("Associated map for question")] public MapAssociation associatedMap;
    [Space]
    [SerializeField, TextArea, Tooltip("Type in the question")] private string question;
    [Space]
    [SerializeField, Tooltip("Type in 3 different answering options")] private AnswerArray answers = new AnswerArray();
    [Space]
    [SerializeField, Tooltip("Select which of the 3 is the correct answer")] private QuestionOptions correctAnswer;
    [Space]
    [SerializeField, Tooltip("Select picture to display in question, can only display either this or text")] private Sprite picture;
    [Space]
    [Tooltip("Option to enforce displaying both picture and text")] public bool displayBoth;

    #endregion
    #region Properties

    /// <summary>
    /// Get property for the question
    /// </summary>
    public string Question { get => question; }
    /// <summary>
    /// Get property for the array of answers
    /// </summary>
    public string[] Answers { get => answers.Options; }
    /// <summary>
    /// Get property for the correct answer cast to int from "QuestionOptions"-enum
    /// </summary>
    public int CorrectAnswer { get => (int)correctAnswer; }
    /// <summary>
    /// Get property for the picture related to the answers
    /// </summary>
    public Sprite Picture { get => picture; }

    #endregion
}

[Serializable]
public class AnswerArray
{

    #region Fields

    [SerializeField] private string option1;
    [SerializeField] private string option2;
    [SerializeField] private string option3;

    #endregion
    #region Properties

    /// <summary>
    /// Retrieves strings as an array
    /// </summary>
    public string[] Options
    {
        get
        {

            return new string[3] { option1, option2, option3 };

        }
    }

    #endregion

}

/// <summary>
/// Enum for Language option, currently containing Dansk/danish = 0, English = 1, Deutsch/german = 2
/// </summary>
public enum LanguageOptions 
{
    Dansk,
    English,
    Deutsch
}

/// <summary>
/// Enum for Difficulty option, currently containing Easy = 0, Normal = 1, Hard = 2
/// </summary>
public enum QuizDifficulty
{
    Easy,
    Normal,
    Hard
}

/// <summary>
/// Enum for Question options, currently containing Option1 = 0, Option2 = 1, Option3 = 2
/// </summary>
public enum QuestionOptions
{
    Option1,
    Option2, 
    Option3
}