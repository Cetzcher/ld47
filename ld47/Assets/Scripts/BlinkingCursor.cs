using UnityEngine;
using System.Collections;
 
public class BlinkingCursor : MonoBehaviour {      
 
	private float m_TimeStamp;
	private bool cursor = false;
	public string cursorChar = "";
 
        void Update() {
             if (Time.time - m_TimeStamp >= 1)
       		{
       			m_TimeStamp = Time.time;
       			if (cursor == false)
       			{
                    cursor = true;
                    cursorChar = " \u2588";
       			}
       			else
       			{
       				cursor = false;
                    cursorChar = "";
       			}
       		}
        }
}