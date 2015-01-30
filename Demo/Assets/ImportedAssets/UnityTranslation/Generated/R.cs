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
            /// <para>Greeting</para>
            /// <para>Value:</para>
            ///   <para>Hello</para>
            /// </summary>
            hello
            ,
            /// <summary>
            /// <para>Goodbye</para>
            /// <para>Value:</para>
            ///   <para>Goodbye</para>
            /// </summary>
            goodbye
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
            /// <para>List of fruits</para>
            /// <para>Value:</para>
            ///   <para>- Item 1:</para>
            ///     <para>// This fruit is red</para>
            ///     <para>Apple</para>
            ///   <para>- Item 2:</para>
            ///     <para>// This fruit is orange</para>
            ///     <para>Orange</para>
            ///   <para>- Item 3:</para>
            ///     <para>// This fruit is yellow</para>
            ///     <para>Banana</para>
            /// </summary>
            fruits
            ,
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
            /// <para>Plurals for files</para>
            /// <para>Value:</para>
            ///   <para>- Zero:</para>
            ///     <para>// 0 files</para>
            ///     <para>no files</para>
            ///   <para>- One:</para>
            ///     <para>// 1 file</para>
            ///     <para>{0} file</para>
            ///   <para>- Two:</para>
            ///     <para>// 2 files</para>
            ///     <para>{0} files</para>
            ///   <para>- Few:</para>
            ///     <para>// 3 files</para>
            ///     <para>{0} files</para>
            ///   <para>- Many:</para>
            ///     <para>// 26 files</para>
            ///     <para>{0} files</para>
            ///   <para>- Other:</para>
            ///     <para>// 3219 files</para>
            ///     <para>{0} files</para>
            /// </summary>
            files
            ,
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
                /// Section ID for "test_section.xml" file.
                /// </summary>
                TestSection
                ,
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
                  "test_section.xml" // TestSection
            };

            /// <summary>
            /// Container for all tokens specified in "Assets/Resources/res/values/test_section.xml" file.
            /// </summary>
            public static class TestSection
            {
                /// <summary>
                /// Enumeration of all string tags in "Assets/Resources/res/values/test_section.xml"
                /// </summary>
                public enum strings
                {
                    /// <summary>
                    /// <para>Section name</para>
                    /// <para>Value:</para>
                    ///   <para>My section</para>
                    /// </summary>
                    name
                    ,
                    /// <summary>
                    /// Total amount of strings.
                    /// </summary>
                    Count // Should be last
                }

                /// <summary>
                /// Enumeration of all string-array tags in "Assets/Resources/res/values/test_section.xml"
                /// </summary>
                public enum array
                {
                    /// <summary>
                    /// Total amount of string-arrays.
                    /// </summary>
                    Count // Should be last
                }

                /// <summary>
                /// Enumeration of all plurals tags in "Assets/Resources/res/values/test_section.xml"
                /// </summary>
                public enum plurals
                {
                    /// <summary>
                    /// Total amount of plurals.
                    /// </summary>
                    Count // Should be last
                }
            }
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
                      { "hello"  , (int)R.strings.hello   }
                   , { "goodbye", (int)R.strings.goodbye }
                }
                ,
                new Dictionary<string, int> // array
                {
                      { "fruits", (int)R.array.fruits }
                }
                ,
                new Dictionary<string, int> // plurals
                {
                      { "files", (int)R.plurals.files }
                }
            }
            ,
            new Dictionary<string, int>[] // TestSection
            {
                new Dictionary<string, int> // strings
                {
                      { "name", (int)R.sections.TestSection.strings.name }
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
