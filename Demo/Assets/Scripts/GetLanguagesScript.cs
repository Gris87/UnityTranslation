using UnityEngine;
using UnityEngine.UI;
using UnityTranslation;



public class GetLanguagesScript : MonoBehaviour
{
    public Button languageButtonPrefab;



    // Use this for initialization
    void Start()
    {
        if (Translator.language == Language.Default)
        {
            Translator.language = Language.English;
        }

        // Create language buttons
        foreach (Language language in AvailableLanguages.list.Keys)
        {
            // Lets hide default language because we have English language in our set
            if (language == Language.Default)
            {
                continue;
            }

            //***************************************************************************
            // Button GameObject
            //***************************************************************************
            #region Button GameObject
            GameObject languageButton = Instantiate(languageButtonPrefab.gameObject) as GameObject;

            languageButton.transform.SetParent(transform);
            languageButton.name = language.ToString();

            //===========================================================================
            // RectTransform Component
            //===========================================================================
            #region RectTransform Component
            RectTransform languageButtonTransform = languageButton.GetComponent<RectTransform>();

            languageButtonTransform.localScale         = new Vector3(1f, 1f, 1f);
            languageButtonTransform.anchoredPosition3D = new Vector3(0f, 0f, 0f);
            #endregion

            //===========================================================================
            // LanguageButtonScript Component
            //===========================================================================
            #region LanguageButtonScript Component
            LanguageButtonScript languageButtonScript = languageButton.GetComponent<LanguageButtonScript>();

            languageButtonScript.language = language;
            #endregion
            #endregion

            //***************************************************************************
            // Text GameObject
            //***************************************************************************
            #region Text GameObject
            GameObject languageButtonText = languageButton.transform.GetChild(0).gameObject;

            #region Text Component
            Text text = languageButtonText.GetComponent<Text>();
            text.text = LanguageName.LanguageToName(language);
            #endregion
            #endregion
        }

        //===========================================================================
        // RectTransform Component
        //===========================================================================
        #region RectTransform Component
        RectTransform scrollAreaContentTransform = GetComponent<RectTransform>();

        scrollAreaContentTransform.anchoredPosition3D = new Vector3(0f, 0f, 0f);
        scrollAreaContentTransform.sizeDelta          = new Vector2(0f, transform.childCount * 30);
        #endregion
    }
}
