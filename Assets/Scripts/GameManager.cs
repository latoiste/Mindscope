using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    private Patient patient;
    private string disease;
    private Question[] questions;
    private static readonly System.Random rng = new();
    
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);

        // load patient dan question
        TextAsset patientJson = Resources.Load<TextAsset>("patients");
        if (patientJson == null)
        {
            Debug.LogError("patients.json file not found");
            return;
        }

        var patients = JsonUtility.FromJson<PatientWrapper>(patientJson.text).patients;
        var selectedPatient = patients[Random.Range(0, patients.Length)];

        var selectedDisease = selectedPatient.possibleDiseases[Random.Range(0, selectedPatient.possibleDiseases.Length)];

        var questionFilename = "questions";
        // var questionFilename = $"{selectedPatient.id}_{selectedDisease}";
        TextAsset questionJson = Resources.Load<TextAsset>(questionFilename);
        if (patientJson == null)
        {
            Debug.LogError($"question json file for {questionFilename} not found");
            return;
        }

        var questionList = JsonUtility.FromJson<QuestionWrapper>(questionJson.text)
            .questions
            .GroupBy(g => g.type)
            .ToDictionary(g => g.Key, g => g.ToArray());

        var specificQuestions = questionList.GetValueOrDefault(selectedDisease);
        var incorrectSpecificQuestions = questionList
            .Where(s => s.Key != selectedDisease && s.Key != "general")
            .SelectMany(s => s.Value)
            .ToArray();
        var generalQuestions = questionList.GetValueOrDefault("general");
        
        Shuffle(specificQuestions);
        Shuffle(incorrectSpecificQuestions);
        Shuffle(generalQuestions);

        if (selectedPatient == null)
        {
            Debug.LogError("Patient not selected");
            return;
        }

        if (selectedDisease == null)
        {
            Debug.LogError("Patient not selected");
            return;
        }

        if (specificQuestions == null
            || incorrectSpecificQuestions == null
            || generalQuestions == null)
        {
            Debug.LogError("Patient not selected");
            return;
        }

        patient = selectedPatient;
        disease = selectedDisease;
        questions = specificQuestions.Take(2)
            .Concat(incorrectSpecificQuestions.Take(2))
            .Concat(generalQuestions.Take(4))
            .ToArray();
    }

    public Patient GetSelectedPatient => patient;
    public string GetSelectedDisease => disease;
    public Question[] GetQuestions => questions;

    public void Diagnose(string outcome)
    {
        if (outcome == disease)
        {
            SceneManager.LoadScene("WinScreen");
        } else
        {
            SceneManager.LoadScene("LoseScreen");
        }
    }

    private void Shuffle<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }
}