using UnityEngine;
using UnityTranslation;
using System.Collections;



public class RunScript : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
		// Used just to initialize UnityTranslation Translator
		Debug.Log("Current language: " + Translator.language);
	}
}
