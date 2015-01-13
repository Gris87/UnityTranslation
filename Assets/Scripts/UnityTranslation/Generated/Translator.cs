using UnityEngine.Events;



namespace UnityTranslation
{
    /// <summary>
    /// UnityTranslation Translator class that has methods for getting localized strings.
    /// Translator provide localization in the same way as in Android localization system.
    /// </summary>
    /// <seealso cref="http://developer.android.com/guide/topics/resources/string-resource.html"/>
    public static class Translator
    {
        #region Properties

        #region language
        /// <summary>
        /// Gets or sets currently used language.
        /// Please note that if you want to add new language you have to create values folder in Assets/Resources/res folder.
        /// Language code should be one of specified language codes in Language.cs
        /// </summary>
        /// <value>Current language.</value>
        public static Language language
        {
            get
            {
                return Internal.Translator.language;
            }

            set
            {
                Internal.Translator.language = value;
            }
        }
        #endregion

        #endregion



        /// <summary>
        /// Adds specified language changed listener and invoke it.
        /// </summary>
        /// <param name="listener">Language changed listener.</param>
        public static void addLanguageChangedListener(UnityAction listener)
        {
            Internal.Translator.addLanguageChangedListener(listener);
        }

        /// <summary>
        /// Removes specified language changed listener.
        /// </summary>
        /// <param name="listener">Language changed listener.</param>
        public static void removeLanguageChangedListener(UnityAction listener)
        {
            Internal.Translator.removeLanguageChangedListener(listener);
        }

        /// <summary>
        /// Return the string value associated with a particular resource ID.
        /// </summary>
        /// <returns>Localized string.</returns>
        /// <param name="id">String resource ID.</param>
        public static string getString(R.strings id)
        {
            // TODO: Implement UnityTranslation.getString()

            return "";
        }

        /// <summary>
        /// Return the string value associated with a particular resource ID, substituting the format arguments as defined in string.Format.
        /// </summary>
        /// <returns>Localized string.</returns>
        /// <param name="id">String resource ID.</param>
        /// <param name="formatArgs">Format arguments.</param>
        public static string getString(R.strings id, params object[] formatArgs)
        {
            return string.Format(getString(id), formatArgs);
        }

        /// <summary>
        /// Return the string array associated with a particular resource ID.
        /// </summary>
        /// <returns>Localized string array.</returns>
        /// <param name="id">String array resource ID.</param>
        public static string[] getStringArray(R.array id)
        {
            // TODO: Implement UnityTranslation.getStringArray()

            return null;
        }

        /// <summary>
        /// Return the string necessary for grammatically correct pluralization of the given resource ID for the given quantity.
        /// </summary>
        /// <returns>Localized string.</returns>
        /// <param name="id">Plurals resource ID.</param>
        /// <param name="quantity">Quantity.</param>
        public static string getQuantityString(R.plurals id, int quantity)
        {
            // TODO: Implement UnityTranslation.getQuantityString()

            return "";
        }

        /// <summary>
        /// Formats the string necessary for grammatically correct pluralization of the given resource ID for the given quantity, using the given arguments.
        /// </summary>
        /// <returns>Localized string.</returns>
        /// <param name="id">Plurals resource ID.</param>
        /// <param name="quantity">Quantity.</param>
        /// <param name="formatArgs">Format arguments.</param>
        public static string getQuantityString(R.plurals id, int quantity, params object[] formatArgs)
        {
            return string.Format(getQuantityString(id, quantity), formatArgs);
        }
    }
}