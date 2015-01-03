using UnityEngine.Events;



namespace UnityTranslation
{
    public static class Translator
    {
        private static Language   mLanguage              = Language.Default;
        private static UnityEvent mLanguageChangedAction = null;



        #region Properties

        #region language
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
                    mLanguage = value;
                    mLanguageChangedAction.Invoke();
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

        public static void addLanguageChangedListener(UnityAction listener)
        {
            mLanguageChangedAction.AddListener(listener);
            listener.Invoke();
        }

        public static void removeLanguageChangedListener(UnityAction listener)
        {
            mLanguageChangedAction.RemoveListener(listener);
        }

        public static string getString(R.strings id)
        {
            // TODO: Implement UnityTranslation.getString()

            return "";
        }

        public static string[] getStringArray(R.array id)
        {
            // TODO: Implement UnityTranslation.getStringArray()

            return null;
        }

        public static string getQuantityString(R.plurals id, int quantity)
        {
            // TODO: Implement UnityTranslation.getQuantityString()

            return "";
        }
    }
}
