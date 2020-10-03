﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TypeWriterEffect : MonoBehaviour {

	public float delay = 0.1f;
	public string fullText;
	private string currentText = "";

	// Use this for initialization
	void Start () {
		StartCoroutine(ShowText());
	}
	
	IEnumerator ShowText(){
        fullText = fullText + " ";
        var textCarret = true;
		for(int i = 0; i < fullText.Length; i++){
			currentText = fullText.Substring(0,i);
            var delayAddition = 0.0f;
            if (i != 0)
            {
                if (currentText.Substring(currentText.Length-1).Equals('/'))
                {
                    currentText = currentText.Substring(0, i-1);
                    delayAddition = 0.5f;
                }
            }
            if (i % 6 == 0 && (i!=fullText.Length-1))
                textCarret = !textCarret;
            if (textCarret)
                currentText += 	" \u2588";
			this.GetComponent<TMPro.TextMeshProUGUI>().text = currentText;
			yield return new WaitForSeconds(delay + delayAddition);
		}
	}
}