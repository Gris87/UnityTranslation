// This file generated from xml files in "Assets/Resources/res/values".
using System.Collections.Generic;



namespace UnityTranslation
{
    /// <summary>
    /// Container for all tokens specified in xml files in "Assets/Resources/res/values".
    /// </summary>
    public static class R
    {
        /// <summary>
        /// Enumeration of all string tags in "Assets/Resources/res/values/strings.xml"
        /// </summary>
        public enum strings
        {
            /// <summary>
            /// <para>Application name</para>
            /// <para>Value:</para>
            ///   <para>Application name</para>
            /// </summary>
            app_name
            ,
            /// <summary>
            /// Total amount of strings.
            /// </summary>
            Count // Should be last
        }

        /// <summary>
        /// Enumeration of all string-array tags in "Assets/Resources/res/values/strings.xml"
        /// </summary>
        public enum array
        {
            /// <summary>
            /// Total amount of string-arrays.
            /// </summary>
            Count // Should be last
        }

        /// <summary>
        /// Enumeration of all plurals tags in "Assets/Resources/res/values/strings.xml"
        /// </summary>
        public enum plurals
        {
            /// <summary>
            /// Total amount of plurals.
            /// </summary>
            Count // Should be last
        }

        /// <summary>
        /// Container for dynamically loadable tokens specified in non strings.xml files.
        /// </summary>
        public static class sections
        {
            /// <summary>
            /// Section ID. This enumeration contains list of dynamically loadable sections.
            /// </summary>
            public enum SectionID
            {
                /// <summary>
                /// Total amount of sections.
                /// </summary>
                Count // Should be last
            }

            /// <summary>
            /// Names of xml files for each section.
            /// </summary>
            public static readonly string[] xmlFiles = new string[]
            {
            };
        }

        /// <summary>
        /// <para>Container for all token IDs in strings.xml (index 0) and in another sections</para>
        /// <para>Each element of tokenIds is an array with 3 elements inside:</para>
        /// <para>0: strings tokens</para>
        /// <para>1: array tokens</para>
        /// <para>2: plurals tokens</para>
        /// </summary>
        public static readonly Dictionary<string, int>[][] tokenIds = new Dictionary<string, int>[][]
        {
            new Dictionary<string, int>[] // Global
            {
                new Dictionary<string, int> // strings
                {
                      { "app_name", (int)R.strings.app_name }
                }
                ,
                new Dictionary<string, int> // array
                {
                }
                ,
                new Dictionary<string, int> // plurals
                {
                }
            }
        };
    }
}
