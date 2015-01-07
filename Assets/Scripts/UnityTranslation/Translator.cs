using UnityEngine;
using UnityEngine.Events;



namespace UnityTranslation
{
	// TODO: Summary
    public static class Translator
    {
        private static Language   mLanguage              = Language.Default;
        private static UnityEvent mLanguageChangedAction = null;



        #region Properties

        #region language
		// TODO: Summary
        public static Language language
        {
            get
            {
                return mLanguage;
            }

            set
            {
                if (mLanguage != value)
                {
					if (AvailableLanguages.list.ContainsKey(value))
					{
						mLanguage = value;

						// TODO: Reload tokens

						mLanguageChangedAction.Invoke();
					}
					else
					{
						Debug.LogError("Impossible to change language to " + value + " because it's not specified in \"Assets/Resources/res\" folder");
					}
                }
            }
        }
        #endregion

        #endregion



        static Translator()
        {
#if UNITY_EDITOR
            CodeGenerator.generate();
#endif

            mLanguageChangedAction = new UnityEvent();
        }

		// TODO: Summary
        public static void addLanguageChangedListener(UnityAction listener)
        {
            mLanguageChangedAction.AddListener(listener);
            listener.Invoke();
        }

		// TODO: Summary
        public static void removeLanguageChangedListener(UnityAction listener)
        {
            mLanguageChangedAction.RemoveListener(listener);
        }

		// TODO: Summary
        public static string getString(R.strings id)
        {
            // TODO: Implement UnityTranslation.getString()

            return "";
        }

		// TODO: Summary
        public static string[] getStringArray(R.array id)
        {
            // TODO: Implement UnityTranslation.getStringArray()

            return null;
        }

		// TODO: Summary
        public static string getQuantityString(R.plurals id, int quantity)
        {
            // TODO: Implement UnityTranslation.getQuantityString()

            return "";
        }
    }
}
