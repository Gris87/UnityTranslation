// This file generated from xml files in "Assets/Resources/res/values".
using UnityEngine;
using UnityEngine.UI;



namespace UnityTranslation
{
    [RequireComponent(typeof(Text))]
    public class TextAutoTranslationTestSection : MonoBehaviour
    {
        public R.sections.TestSection.strings id;

        private Text mText;



        // Use this for initialization
        void Start()
        {
            mText = GetComponent<Text>();

            Translator.addLanguageChangedListener(OnLanguageChanged);
        }

        void OnDestroy()
        {
            Translator.removeLanguageChangedListener(OnLanguageChanged);
        }

        public void OnLanguageChanged()
        {
            mText.text = Translator.getString(id);
        }
   }
}
