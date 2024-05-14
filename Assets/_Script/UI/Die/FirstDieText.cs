using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FirstDieText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float stayTime = 0.5f;
    [SerializeField] private float randomTime = 1f;

    private State state = State.Org;
    private enum State
    {
        Org,
        Question,
        Random
    }
    private string orgText = "DIED ";
    private string questionText = "DIED?";
    private string randomString = "";

    private string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private float startTime;

    private bool goNormal = false;
    private bool goQuestion = false;


    void Start()
    {
        ChangeStateOrg();
    }

    void Update()
    {
        if (state == State.Org)
        {
            if(Time.unscaledTime > startTime + stayTime)
            {
                ChangeStateRandom();
            }
        }
        else if (state == State.Question)
        {
            if (Time.unscaledTime > startTime + stayTime)
            {
                ChangeStateRandom();
            }
        }
        else if (state == State.Random)
        {
            DoRandom();
            if (Time.unscaledTime > startTime + randomTime)
            {
                if (goNormal)
                {
                    ChangeStateOrg();
                }
                else if (goQuestion) 
                { 
                    ChangeStateQuestion();
                }
            }
        }
    }

    private void ChangeStateOrg()
    {
        startTime = Time.unscaledTime;
        text.text = orgText;
        state = State.Question;

        goNormal = false;
        goQuestion = true;
    }

    private void ChangeStateQuestion()
    {
        startTime = Time.unscaledTime;
        text.text = questionText;
        state = State.Question;

        goNormal = true;
        goQuestion = false;
    }

    private void ChangeStateRandom()
    {
        startTime = Time.unscaledTime;
        state = State.Random;
    }

    private void DoRandom()
    {
        string _randomString = "";
        for (int i = 0; i < 4; i++)
        {
            _randomString += alphabet[Random.Range(0, alphabet.Length)];
        }
        text.text = randomString + _randomString;
    }
}
