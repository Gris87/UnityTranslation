using UnityEngine;
using UnityEngine.UI;
using UnityTranslation;



public class LanguageButtonScript : MonoBehaviour
{
	private static LanguageButtonScript selectedButton = null;

	public Language language;



	// Use this for initialization
	void Start()
	{
		if (language == Translator.language)
		{
			selectedButton = this;

			transform.GetChild(0).GetComponent<Text>().color = Color.red;
		}

		GetComponent<Button>().onClick.AddListener(OnClick);
	}

	public void OnClick()
	{
		Translator.language = language;

		if (selectedButton != null)
		{
			selectedButton.transform.GetChild(0).GetComponent<Text>().color = Color.black;
		}

		selectedButton = this;

		transform.GetChild(0).GetComponent<Text>().color = Color.red;
	} 
}
