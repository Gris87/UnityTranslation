﻿// This file generated from xml files in "Assets/Resources/res/values".



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
            ///     <para>// 5 files</para>
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
                    Count // Should be last
                }

                /// <summary>
                /// Enumeration of all string-array tags in "Assets/Resources/res/values/test_section.xml"
                /// </summary>
                public enum array
                {
                    Count // Should be last
                }

                /// <summary>
                /// Enumeration of all plurals tags in "Assets/Resources/res/values/test_section.xml"
                /// </summary>
                public enum plurals
                {
                    Count // Should be last
                }
            }
        }
    }
}
