﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TypeWriterEffect : MonoBehaviour {

	public float delay = 0.1f;
	public string fullText;
	public string currentText = "";
    private bool finished = false;
    public BlinkingCursor blinkingCursor;

	// Use this for initialization
	public void init () {
		StartCoroutine(ShowText());
	}
	
	IEnumerator ShowText(){
        fullText = fullText + " ";
		for(int i = 0; i < fullText.Length; i++){
			currentText = fullText.Substring(0,i);
            var delayAddition = 0.0f;
            if (i != 0)
            {
                if (currentText.Substring(currentText.Length-1).Equals("/"))
                {
                    currentText = currentText.Substring(0, i-1);
                    fullText = fullText.Substring(0,i-1) + fullText.Substring(i);
                    delayAddition = 0.2f;
                }
            }
            if (i == fullText.Length -1)
                finished = true;
			this.GetComponent<TMPro.TextMeshProUGUI>().text = currentText + blinkingCursor.cursorChar;
			yield return new WaitForSeconds(delay + delayAddition);
		}
	}

    void Update() {
        if (finished)
            this.GetComponent<TMPro.TextMeshProUGUI>().text = currentText + blinkingCursor.cursorChar;
    }
}