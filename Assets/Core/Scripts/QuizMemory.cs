using System;
using System.Collections.Generic;
using UnityEngine;

public enum MapAssociation
{

    LevelOne,
    LevelTwo,
    LevelThree,
    LevelFour,
    LevelFive,
    LevelSix,
    LevelSeven

}

/// <summary>
/// Carries persistant data for which Quiz's have been displayed
/// </summary>
[CreateAssetMenu(fileName = "QuizMemory", menuName = "Scriptable Objects/QuizMemory")]
public class QuizMemory : ScriptableObject
{
    
    public List<int> previousQuestions;
    public Action<float> CorrectAnswer; //Rewards time bonus
    public Action CoinSoundTrigger;
    [SerializeField, Tooltip("Option to select language if removing user options")] private LanguageOptions language;
    [SerializeField, Tooltip("Option to select difficulty if removing user options")] private QuizDifficulty difficulty;

    public MapAssociation CurrentMap { get; set; }

    /// <summary>
    /// Language setting and default value
    /// </summary>
    public LanguageOptions Language { get => language; set => language = value; }

    /// <summary>
    /// Difficulty setting and default value
    /// </summary>
    public QuizDifficulty Difficulty { get => difficulty; set => difficulty = value; }

    /// <summary>
    /// Property to retrieve score
    /// </summary>
    public int TotalCollected { get; set; }

    /// <summary>
    /// Method to instantiate a new List<int>
    /// </summary>
    public void InitializeAndResetMemory()
    {

        previousQuestions = new List<int>();
        TotalCollected = 0;

    }

}

/// <summary>
/// Provides On-build run of method
/// </summary>
public class Startup
{

    /// <summary>
    /// Method runs before scenes are loaded
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitOnPlay()
    {

        QuizMemory so = Resources.Load<QuizMemory>("QuizMemory_SO");
        LanguageStrings_SO languageStrings_SO = Resources.Load<LanguageStrings_SO>("LanguageStrings_SO");
        CollectibleDataSO collectibleDataSO = Resources.Load<CollectibleDataSO>("CollectibleDataSO");

        if (so != null)
            so.InitializeAndResetMemory();

        if (languageStrings_SO != null)
            languageStrings_SO.Initialize();

        if (collectibleDataSO != null)
            collectibleDataSO.CollectibleCount = 0;

    }

}
