using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicMechanics : MonoBehaviour
{
    [SerializeField] Viktor viktorScript;
    [SerializeField] EnemyWeak enemyWeakScript;
    public float music = 1;
    public AudioSource[] SoundFX;

    public bool hasBuff1, hasBuff2, hasBuff3;

    bool upReg , downReg , leftReg , rightReg ;
    int notesCounter = 0;
    int musicInstrument = 0;

    // Notes Player
    [Header("Player")]
    [SerializeField] Image upButton;
    [SerializeField] Image rightButton;
    [SerializeField] Image downButton;
    [SerializeField] Image leftButton;

    // Instrument 1
    [Header("Instrument 1")]
    [SerializeField] Image upNote1;
    [SerializeField] Image downNote1;
    [SerializeField] Image upNote2;

    // Instrument 2
    [Header("Instrument 2")]
    [SerializeField] Image downNote2;
    [SerializeField] Image downNote3;
    [SerializeField] Image upNote3;

    // Instrument 3
    [Header("Instrument 3")]
    [SerializeField] Image rightNote1;
    [SerializeField] Image downNote4;
    [SerializeField] Image upNote4;

    // Instrument-Notes Relation
    [Header("Instrument-Notes Relation")]
    [SerializeField] GameObject NotesInstrument1;
    [SerializeField] GameObject NotesInstrument2;
    [SerializeField] GameObject NotesInstrument3;

    // Buff/Debuff Stats
    [Header("Buff/Debuff Stats")]
    [SerializeField] float enemyDebuffTime;

    void Start()
    {
        NotesInstrument1.SetActive(true);
        NotesInstrument2.SetActive(false);
        NotesInstrument3.SetActive(false);
        hasBuff1 = false;
        hasBuff2 = false;
        hasBuff3 = false;
    }

    void Update()
    {
        PlayMusic();

        if (Input.GetKeyDown(KeyCode.P))
        {
            ChangeInstrument();
        }

        if (musicInstrument % 3 == 0)
        {
            MusicChecker1();
        }
        else if (musicInstrument % 3 == 1)
        {
            MusicChecker2();
        }
        else
        {
            MusicChecker3();
        }
        //Debug.Log(musicInstrument);
    }

    private void RegReset()
    {
        if (upReg || downReg || leftReg || rightReg)
        {
            upReg = false;
            downReg = false;
            leftReg = false;
            rightReg = false;
        }
    }

    private void ChangeInstrument()
    {
        musicInstrument += 1;

        if (musicInstrument % 3 == 0)
        {
            notesCounter = 0;
            NotesInstrument1.SetActive(true);
            NotesInstrument2.SetActive(false);
            NotesInstrument3.SetActive(false);
        }
        else if (musicInstrument % 3 == 1)
        {
            notesCounter = 0;
            NotesInstrument1.SetActive(false);
            NotesInstrument2.SetActive(true);
            NotesInstrument3.SetActive(false);
        }
        else
        {
            notesCounter = 0;
            NotesInstrument1.SetActive(false);
            NotesInstrument2.SetActive(false);
            NotesInstrument3.SetActive(true);
        }
    }

    private void MusicChecker1()
    {
        upReg = Input.GetKeyDown(KeyCode.H);
        rightReg = Input.GetKeyDown(KeyCode.M);
        downReg = Input.GetKeyDown(KeyCode.N);
        leftReg = Input.GetKeyDown(KeyCode.B);

        string[] refArray = { "Up","Down" ,"Up"};

        if (upReg && notesCounter == 0) 
        {
            upNote1.color = Color.blue;
            notesCounter+=1;
            RegReset();
        }

        if (downReg && notesCounter == 1)
        {
            downNote1.color = Color.blue;
            notesCounter += 1;
            RegReset();
        }
        else if (notesCounter == 1 && (upReg || leftReg || rightReg))
        {
            upNote1.color = Color.yellow;
            downNote1.color = Color.yellow;
            upNote2.color = Color.yellow;
            notesCounter = 0;
            RegReset();
            StartCoroutine(MusicTime1());
        }

        if (upReg && notesCounter == 2)
        {
            upNote2.color = Color.blue;
            // BUFF 1
            hasBuff1 = true;
            RegReset();
            StartCoroutine(MusicTime1());
            notesCounter = 0;
        }
        else if (notesCounter == 2 && (downReg || leftReg || rightReg))
        {
            upNote1.color = Color.yellow;
            downNote1.color = Color.yellow;
            upNote2.color = Color.yellow;
            notesCounter = 0;
            RegReset() ;
            StartCoroutine(MusicTime1());
        }
    }

    private void MusicChecker2()
    {
        upReg = Input.GetKeyDown(KeyCode.H);
        rightReg = Input.GetKeyDown(KeyCode.M);
        downReg = Input.GetKeyDown(KeyCode.N);
        leftReg = Input.GetKeyDown(KeyCode.B);

        if (downReg && notesCounter == 0)
        {
            downNote2.color = Color.blue;
            notesCounter += 1;
            RegReset();
        }

        if (downReg && notesCounter == 1)
        {
            downNote3.color = Color.blue;
            notesCounter += 1;
            RegReset();
        }
        else if (notesCounter == 1 && (upReg || leftReg || rightReg))
        {
            downNote2.color = Color.yellow;
            downNote3.color = Color.yellow;
            upNote3.color = Color.yellow;
            notesCounter = 0;
            RegReset();
            StartCoroutine(MusicTime2());
        }

        if (upReg && notesCounter == 2)
        {
            upNote3.color = Color.blue;
            // BUFF 2
            hasBuff2 = true;
            RegReset();
            StartCoroutine(MusicTime2());
            notesCounter = 0;
        }
        else if (notesCounter == 2 && (downReg || leftReg || rightReg))
        {
            downNote2.color = Color.yellow;
            downNote3.color = Color.yellow;
            upNote3.color = Color.yellow;
            notesCounter = 0;
            StartCoroutine(MusicTime2());
            RegReset();
        }
    }

    private void MusicChecker3()
    {
        upReg = Input.GetKeyDown(KeyCode.H);
        rightReg = Input.GetKeyDown(KeyCode.M);
        downReg = Input.GetKeyDown(KeyCode.N);
        leftReg = Input.GetKeyDown(KeyCode.B);

        if (rightReg && notesCounter == 0)
        {
            rightNote1.color = Color.blue;
            notesCounter += 1;
            RegReset();
        }

        if (downReg && notesCounter == 1)
        {
            downNote4.color = Color.blue;
            notesCounter += 1;
            RegReset();
        }
        else if (notesCounter == 1 && (upReg || leftReg || rightReg))
        {
            rightNote1.color = Color.yellow;
            downNote4.color = Color.yellow;
            upNote4.color = Color.yellow;
            notesCounter = 0;
            RegReset();
            StartCoroutine(MusicTime3());
        }

        if (upReg && notesCounter == 2)
        {
            upNote4.color = Color.blue;
            // BUFF 3
            hasBuff3 = true;
            RegReset();
            StartCoroutine(MusicTime3());
            notesCounter = 0;
        }
        else if (notesCounter == 2 && (downReg || leftReg || rightReg))
        {
            rightNote1.color = Color.yellow;
            downNote4.color = Color.yellow;
            upNote4.color = Color.yellow;
            notesCounter = 0;
            RegReset();
            StartCoroutine(MusicTime3());
        }
    }

    private void PlayMusic()
    {
        // Press Listener
        if (Input.GetKeyDown(KeyCode.H))
        {
            //SoundFX[0].Play();
            upButton.color = Color.green;
        }
        else if (Input.GetKeyUp(KeyCode.H))
        {
            upButton.color = Color.white;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            //SoundFX[1].Play();
            rightButton.color = Color.green;
        }
        else if (Input.GetKeyUp(KeyCode.M))
        {
            rightButton.color = Color.white;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            //SoundFX[2].Play();
            downButton.color = Color.green;
        }
        else if (Input.GetKeyUp(KeyCode.N))
        {
            downButton.color = Color.white;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            //SoundFX[3].Play();
            leftButton.color = Color.green;
        }
        else if (Input.GetKeyUp(KeyCode.B))
        {
            leftButton.color = Color.white;
        }
    }

    IEnumerator MusicTime1()
    {
        
            yield return new WaitForSeconds(0.5f);
            upNote1.color = Color.red;
            downNote1.color = Color.red;
            upNote2.color = Color.red;

        
    }

    IEnumerator MusicTime2()
    {

            yield return new WaitForSeconds(0.5f);
            downNote2.color = Color.red;
            downNote3.color = Color.red;
            upNote3.color = Color.red;

        
    }
    IEnumerator MusicTime3()
    {

            yield return new WaitForSeconds(0.5f);
            rightNote1.color = Color.red;
            downNote4.color = Color.red;
            upNote4.color = Color.red;


    }
}