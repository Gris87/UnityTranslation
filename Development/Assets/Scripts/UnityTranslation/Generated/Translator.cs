// This file generated from xml files in "Assets/Resources/res/values".
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
        #region Trasparent access to Internal.Translator public members

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
        /// Load tokens for specified section.
        /// </summary>
        /// <param name="section">Section ID.</param>
        public static void LoadSection(R.sections.SectionID section)
        {
            Internal.Translator.LoadSection(section, true);
        }

        /// <summary>
        /// Unload tokens for specified section.
        /// </summary>
        /// <param name="section">Section ID.</param>
        public static void UnloadSection(R.sections.SectionID section)
        {
            Internal.Translator.UnloadSection(section);
        }

        /// <summary>
        /// Determines if specified section is loaded.
        /// </summary>
        /// <returns><c>true</c> if section is loaded; otherwise, <c>false</c>.</returns>
        /// <param name="section">Section ID.</param>
        public static bool IsSectionLoaded(R.sections.SectionID section)
        {
            return Internal.Translator.IsSectionLoaded(section);
        }
        #endregion

        #region Generated code
        /// <summary>
        /// Return the string value associated with a particular resource ID.
        /// </summary>
        /// <returns>Localized string.</returns>
        /// <param name="id">String resource ID.</param>
        public static string getString(R.strings id)
        {
            if (
                Internal.Translator.tokens[0].selectedLanguage != null
                &&
                Internal.Translator.tokens[0].selectedLanguage.stringValues[(int)id] != null
               )
            {
                return Internal.Translator.tokens[0].selectedLanguage.stringValues[(int)id];
            }
            else
            {
                return Internal.Translator.tokens[0].defaultLanguage.stringValues[(int)id];
            }
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
            if (
                Internal.Translator.tokens[0].selectedLanguage != null
                &&
                Internal.Translator.tokens[0].selectedLanguage.stringArrayValues[(int)id] != null
               )
            {
                return Internal.Translator.tokens[0].selectedLanguage.stringArrayValues[(int)id];
            }
            else
            {
                return Internal.Translator.tokens[0].defaultLanguage.stringArrayValues[(int)id];
            }
        }

        /// <summary>
        /// Return the string necessary for grammatically correct pluralization of the given resource ID for the given quantity.
        /// </summary>
        /// <returns>Localized string.</returns>
        /// <param name="id">Plurals resource ID.</param>
        /// <param name="quantity">Quantity.</param>
        public static string getQuantityString(R.plurals id, double quantity)
        {
            string[]        pluralsValues;
            PluralsQuantity pluralsQuantity;

            if (
                Internal.Translator.tokens[0].selectedLanguage != null
                &&
                Internal.Translator.tokens[0].selectedLanguage.pluralsValues[(int)id] != null
               )
            {
                pluralsValues   = Internal.Translator.tokens[0].selectedLanguage.pluralsValues[(int)id];
                pluralsQuantity = PluralsRules.pluralsFunctions[(int)Internal.Translator.language](quantity);
            }
            else
            {
                pluralsValues   = Internal.Translator.tokens[0].defaultLanguage.pluralsValues[(int)id];
                pluralsQuantity = PluralsRules.pluralsFunctions[0](quantity);
            }

            for (int i = (int)pluralsQuantity ; i < (int)PluralsQuantity.Count; ++i)
            {
                if (pluralsValues[i] != null)
                {
                    return pluralsValues[i];
                }
            }

            for (int i = (int)pluralsQuantity - 1 ; i >= 0; --i)
            {
                if (pluralsValues[i] != null)
                {
                    return pluralsValues[i];
                }
            }

            return "";
        }

        /// <summary>
        /// Formats the string necessary for grammatically correct pluralization of the given resource ID for the given quantity, using the given arguments.
        /// </summary>
        /// <returns>Localized string.</returns>
        /// <param name="id">Plurals resource ID.</param>
        /// <param name="quantity">Quantity.</param>
        /// <param name="formatArgs">Format arguments.</param>
        public static string getQuantityString(R.plurals id, double quantity, params object[] formatArgs)
        {
            return string.Format(getQuantityString(id, quantity), formatArgs);
        }
        #endregion
    }
}
