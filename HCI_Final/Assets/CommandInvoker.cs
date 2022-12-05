using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Windows.Speech;

// DEBUGGING
using UnityEngine.UI;
using TMPro;

public class CommandInvoker : MonoBehaviour
{
    public static DictationRecognizer DR;
    public SpeechSystemStatus status;

    [Header("Debugging")]
    public TMP_Text inputDisplay = null;

    void Start() 
    {
        VoiceCommands.Initialize();

        DR = new DictationRecognizer();
        DR.DictationResult += onDictationResult;
        DR.DictationHypothesis += onDictationHypothesis;
       
        StartCoroutine(ControlVoiceRecog());

        Debug.Log("VCS Successfully Initialized.\n" + VoiceCommands.Commands.Count + " commands recognized.");
    }

    private void onDictationResult(string text, ConfidenceLevel confidence) {
        
        StartCoroutine(ShowDictationResult(text));

        string[] resultArr = text.Split(' ');
        int keywordOrder = 0;
        string prevKeyword = "";
        for (int i = 0; i < resultArr.Length; i++) {                        
            if (VoiceCommands.Commands.ContainsKey(resultArr[i])) {
                keywordOrder++;
                VoiceCommands.Commands[resultArr[i]].DynamicInvoke(keywordOrder, prevKeyword);    // Pass in the order that the word was detected
                prevKeyword = resultArr[i];
            }
        }
    }

    private IEnumerator ShowDictationResult(string text) 
    {
        inputDisplay.enabled = true;
        inputDisplay.text = text;
        yield return new WaitForSeconds(3.0f);
        inputDisplay.text = "";
        inputDisplay.enabled = false;
        
    }

    private void onDictationHypothesis(string text) {
        //Debug.Log(text);
    }

    IEnumerator ControlVoiceRecog() 
    {
        while (true) 
        {       
            status = DR.Status;

            if (DR.Status == SpeechSystemStatus.Stopped) 
            {
                DR.Start();
            }

            yield return new WaitForSeconds(1);
        }
    }
}
