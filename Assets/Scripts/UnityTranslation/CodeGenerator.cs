#if UNITY_EDITOR

// Use this definition to generate "Languages.cs" and "PluralsRules.cs" from CLDR.
// Please become Unity Translation developer and commit your changes to https://github.com/Gris87/UnityTranslation
#define I_AM_UNITY_TRANSLATION_DEVELOPER

// Use this definition if you want to force code generation
// #define FORCE_CODE_GENERATION



using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;



namespace UnityTranslation
{
    public static class CodeGenerator
    {
#if I_AM_UNITY_TRANSLATION_DEVELOPER
        private static bool previouslyGeneratedLanguages          = false;
        private static bool previouslyGeneratedPluralsRules       = false;
#endif

        private static bool previouslyGeneratedR                  = false;
        private static bool previouslyGeneratedAvailableLanguages = false;



        /// <summary>
        /// Generates source code required for UnityTranslation
        /// </summary>
        public static void generate()
        {
            checkPreviouslyGeneratedFiles();

#if I_AM_UNITY_TRANSLATION_DEVELOPER
            generateLanguages();
            generatePluralsRules();
#endif

            generateStringsXml();
            generateR();
            generateAvailableLanguages();

            // TODO: Generate Translator
        }

        /// <summary>
        /// Checks the previously generated files.
        /// </summary>
        private static void checkPreviouslyGeneratedFiles()
        {
            if (isApplicationRebuilded())
            {
#if I_AM_UNITY_TRANSLATION_DEVELOPER
                previouslyGeneratedLanguages          = checkPreviouslyGeneratedFile("Language.cs");
                previouslyGeneratedPluralsRules       = checkPreviouslyGeneratedFile("PluralsRules.cs");
#endif

                previouslyGeneratedR                  = checkPreviouslyGeneratedFile("R.cs");
                previouslyGeneratedAvailableLanguages = checkPreviouslyGeneratedFile("AvailableLanguages.cs");
            }
        }

        /// <summary>
        /// Verifies that application rebuilded.
        /// </summary>
        /// <returns><c>true</c>, if application rebuilded, <c>false</c> otherwise.</returns>
        private static bool isApplicationRebuilded()
        {
            bool res = false;

            string binaryFile = Application.dataPath + "/../Library/ScriptAssemblies/Assembly-CSharp.dll";
            string tempFile   = Application.temporaryCachePath + "/UnityTranslation/Assembly-CSharp.dll";

            if (File.Exists(binaryFile))
            {
                byte[] newBinary = File.ReadAllBytes(binaryFile);

                if (File.Exists(tempFile))
                {
                    byte[] oldBinary = File.ReadAllBytes(tempFile);

                    res = !oldBinary.SequenceEqual(newBinary);
                }
                else
                {
                    res = true;
                }

                if (res)
                {
                    // Debug.Log("Application rebuilded");
                    File.WriteAllBytes(tempFile, newBinary);
                }
            }

            return res;
        }

        /// <summary>
        /// Checks the previously generated file.
        /// </summary>
        /// <returns><c>true</c>, if file generated in previous build, <c>false</c> otherwise.</returns>
        /// <param name="filename">Name of file.</param>
        private static bool checkPreviouslyGeneratedFile(string filename)
        {
            bool res = false;

            string generatedFile = Application.dataPath + "/Scripts/UnityTranslation/Generated/" + filename;
            string tempFile      = Application.temporaryCachePath + "/UnityTranslation/"         + filename;

            if (File.Exists(generatedFile))
            {
                string newText = File.ReadAllText(generatedFile, Encoding.UTF8);

                if (File.Exists(tempFile))
                {
                    string oldText = File.ReadAllText(tempFile, Encoding.UTF8);

                    res = (oldText != newText);
                }
                else
                {
                    res = true;
                }

                if (res)
                {
                    // Debug.Log("File \"" + filename + "\" regenerated in previous build");
                    File.WriteAllText(tempFile, newText, Encoding.UTF8);
                }
            }

            return res;
        }

#if I_AM_UNITY_TRANSLATION_DEVELOPER
        /// <summary>
        /// Generates Languages.cs file
        /// </summary>
        private static void generateLanguages()
        {
            string cldrLanguagesFile = Application.dataPath           + "/../3rd_party/CLDR/json-full/main/en/languages.json";
            string tempLanguagesFile = Application.temporaryCachePath + "/UnityTranslation/languages.json";

            string targetFile = Application.dataPath + "/Scripts/UnityTranslation/Generated/Language.cs";

            byte[] cldrFileBytes = null;

            #region Check that Languages.cs is up to date
            if (!File.Exists(cldrLanguagesFile))
            {
                Debug.LogError("File \"" + cldrLanguagesFile + "\" not found");

                return;
            }

            cldrFileBytes = File.ReadAllBytes(cldrLanguagesFile);

#if !FORCE_CODE_GENERATION
            if (
                File.Exists(targetFile)
                &&
                File.Exists(tempLanguagesFile)
               )
            {
                byte[] tempFileBytes = File.ReadAllBytes(tempLanguagesFile);

                if (cldrFileBytes.SequenceEqual(tempFileBytes))
                {
                    return;
                }
            }
#endif
            #endregion

            #region Generating Languages.cs file
            Debug.Log("Generating \"Languages.cs\" file");

            string cldrFileText = Encoding.UTF8.GetString(cldrFileBytes);
            JSONObject json = new JSONObject(cldrFileText);

            if (json.type == JSONObject.Type.OBJECT)
            {
                int maxCodeLength = 0;
                int maxNameLength = 0;

                List<string> languageCodes = new List<string>();
                List<string> languageEnums = new List<string>();
                List<string> languageNames = new List<string>();

                #region Parse JSON
                bool good = true;

                json.GetField("main", delegate(JSONObject mainJson)
                {
                    mainJson.GetField("en", delegate(JSONObject enJson)
                    {
                        enJson.GetField("localeDisplayNames", delegate(JSONObject localeDisplayNamesJson)
                        {
                            localeDisplayNamesJson.GetField("languages", delegate(JSONObject languagesJson)
                            {
                                for (int i = 0; i < languagesJson.list.Count; ++i)
                                {
                                    if (languagesJson.keys[i] == "root")
                                    {
                                        continue;
                                    }

                                    JSONObject languageJson = languagesJson.list[i];

                                    string temp = "";

                                    string[] tokens = languageJson.str.Split(' ', '.', '-');

                                    for (int j = 0; j < tokens.Length; ++j)
                                    {
                                        if (tokens[j].Length > 0)
                                        {
                                            temp += tokens[j].Substring(0, 1).ToUpper() + tokens[j].Substring(1).ToLower();
                                        }
                                    }

                                    string languageEnum = "";

                                    for (int j = 0; j < temp.Length; ++j)
                                    {
                                        char ch = temp[j];

                                        if (
                                            (ch >= 'a') && (ch <= 'z')
                                            ||
                                            (ch >= 'A') && (ch <= 'Z')
                                            )
                                        {
                                            languageEnum += ch;
                                        }
                                        else
                                        if (ch == 700) // if (ch == 'ʼ')
                                        {
                                            // Nothing
                                        }
                                        else
                                        if (ch == 229) // if (ch == 'å')
                                        {
                                            languageEnum += 'a';
                                        }
                                        else
                                        if (ch == 231) // if (ch == 'ç')
                                        {
                                            languageEnum += 'c';
                                        }
                                        else
                                        if (ch == 252) // if (ch == 'ü')
                                        {
                                            languageEnum += 'u';
                                        }
                                        else
                                        if (ch == 245) // if (ch == 'õ')
                                        {
                                            languageEnum += 'o';
                                        }
                                        else
                                        {
                                            Debug.LogError("Unhandled character \"" + ch + "\" (" + (int)ch + ") in \"" + temp + "\" while parsing \"CLDR/json-full/main/en/languages.json\" file");
                                        }
                                    }

                                    string languageCode = languagesJson.keys[i];
                                    string languageName = languageJson.str;

                                    if (languageCode.Length > maxCodeLength)
                                    {
                                        maxCodeLength = languageCode.Length;
                                    }

                                    if (languageName.Length > maxNameLength)
                                    {
                                        maxNameLength = languageName.Length;
                                    }

                                    languageCodes.Add(languageCode);
                                    languageEnums.Add(languageEnum);
                                    languageNames.Add(languageName);
                                }
                            },
                            delegate(string name)
                            {
                                good = false;

                                Debug.LogError("Entry \"" + name + "\" not found in \"" + cldrLanguagesFile + "\"");
                            });
                        },
                        delegate(string name)
                        {
                            good = false;

                            Debug.LogError("Entry \"" + name + "\" not found in \"" + cldrLanguagesFile + "\"");
                        });
                    },
                    delegate(string name)
                    {
                        good = false;

                        Debug.LogError("Entry \"" + name + "\" not found in \"" + cldrLanguagesFile + "\"");
                    });
                },
                delegate(string name)
                {
                    good = false;

                    Debug.LogError("Entry \"" + name + "\" not found in \"" + cldrLanguagesFile + "\"");
                });

                if (!good)
                {
                    return;
                }
                #endregion

                string res = "// This file generated from \"CLDR/json-full/main/en/languages.json\" file.\n" +
                             "\n" +
                             "\n" +
                             "\n" +
                             "namespace UnityTranslation\n" +
                             "{\n" +
                             "    /// <summary>\n" +
                             "    /// Language. This enumeration contains list of supported languages.\n" +
                             "    /// </summary>\n" +
                             "    public enum Language\n" +
                             "    {\n" +
                             "        /// <summary>\n" +
                             "        /// Default language. Equivalent for English.\n" +
                             "        /// </summary>\n" +
                             "        Default\n";

                for (int i = 0; i < languageNames.Count; ++i)
                {
                    res += "        , \n" +
                           "        /// <summary>\n" +
                           "        /// " + languageNames[i] + ". Code: " + languageCodes[i] + "\n" +
                           "        /// </summary>\n" +
                           "        " + languageEnums[i] + "\n";
                }

                res += "        ,\n" +
                       "        Count // Should be last\n" +
                       "    }\n" +
                       "\n" +
                       "    /// <summary>\n" +
                       "    /// This class provides methods for converting language code to Language enum and Language enum to language code\n" +
                       "    /// </summary>\n" +
                       "    public static class LanguageCode\n" +
                       "    {\n" +
                       "        /// <summary>\n" +
                       "        /// Array of language codes for each Language enum value.\n" +
                       "        /// </summary>\n" +
                       "        public static readonly string[] codes = new string[]\n" +
                       "        {\n" +
                       string.Format("              {0,-" + (maxCodeLength + 2) + "} // Default\n", "\"\"");

                for (int i = 0; i < languageCodes.Count; ++i)
                {
                    res += string.Format("            , {0,-" + (maxCodeLength + 2) + "} // {1}\n", "\"" + languageCodes[i] + "\"", languageEnums[i]);
                }

                res += "        };\n" +
                       "\n" +
                       "        /// <summary>\n" +
                       "        /// Converts Language enum value to language code\n" +
                       "        /// </summary>\n" +
                       "        /// <returns>Language code.</returns>\n" +
                       "        /// <param name=\"language\">Language enum value</param>\n" +
                       "        public static string languageToCode(Language language)\n" +
                       "        {\n" +
                       "            return codes[(int)language];\n" +
                       "        }\n" +
                       "\n" +
                       "        /// <summary>\n" +
                       "        /// Converts language code to Language enum value\n" +
                       "        /// </summary>\n" +
                       "        /// <returns>Language enum value.</returns>\n" +
                       "        /// <param name=\"code\">Language code</param>\n" +
                       "        public static Language codeToLanguage(string code)\n" +
                       "        {\n" +
                       "            for (int i = 0; i < (int)Language.Count; ++i)\n" +
                       "            {\n" +
                       "                if (codes[i] == code)\n" +
                       "                {\n" +
                       "                    return (Language)i;\n" +
                       "                }\n" +
                       "            }\n" +
                       "\n" +
                       "            return Language.Count;\n" +
                       "        }\n" +
                       "    }\n" +
                       "\n" +
                       "    /// <summary>\n" +
                       "    /// This class provides methods for converting language name to Language enum and Language enum to language name\n" +
                       "    /// </summary>\n" +
                       "    public static class LanguageName\n" +
                       "    {\n" +
                       "        /// <summary>\n" +
                       "        /// Array of language names for each Language enum value.\n" +
                       "        /// </summary>\n" +
                       "        public static readonly string[] names = new string[]\n" +
                       "        {\n" +
                        string.Format("              {0,-" + (maxNameLength + 2) + "} // Default\n", "\"\"");

                for (int i = 0; i < languageNames.Count; ++i)
                {
                    res += string.Format("            , {0,-" + (maxNameLength + 2) + "} // {1}\n", "\"" + languageNames[i] + "\"", languageEnums[i]);
                }

                res += "        };\n" +
                       "\n" +
                       "        /// <summary>\n" +
                       "        /// Converts Language enum value to language name\n" +
                       "        /// </summary>\n" +
                       "        /// <returns>Language name.</returns>\n" +
                       "        /// <param name=\"language\">Language enum value</param>\n" +
                       "        public static string languageToName(Language language)\n" +
                       "        {\n" +
                       "            return names[(int)language];\n" +
                       "        }\n" +
                       "\n" +
                       "        /// <summary>\n" +
                       "        /// Converts language name to Language enum value\n" +
                       "        /// </summary>\n" +
                       "        /// <returns>Language enum value.</returns>\n" +
                       "        /// <param name=\"name\">Language name</param>\n" +
                       "        public static Language nameToLanguage(string name)\n" +
                       "        {\n" +
                       "            for (int i = 0; i < (int)Language.Count; ++i)\n" +
                       "            {\n" +
                       "                if (names[i] == name)\n" +
                       "                {\n" +
                       "                    return (Language)i;\n" +
                       "                }\n" +
                       "            }\n" +
                       "\n" +
                       "            return Language.Count;\n" +
                       "        }\n" +
                       "    }\n" +
                       "}\n";

                File.WriteAllText(targetFile, res, Encoding.UTF8);
            }
            else
            {
                Debug.LogError("Incorrect file format in \"" + cldrLanguagesFile + "\"");

                return;
            }

            Debug.Log("Caching CLDR languages file in \"" + tempLanguagesFile + "\"");
            Directory.CreateDirectory(Application.temporaryCachePath + "/UnityTranslation");
            File.WriteAllBytes(tempLanguagesFile, cldrFileBytes);
            #endregion
        }

        /// <summary>
        /// Generates PluralsRules.cs file
        /// </summary>
        private static void generatePluralsRules()
        {
            string cldrPluralsFile = Application.dataPath           + "/../3rd_party/CLDR/json-full/supplemental/plurals.json";
            string tempPluralsFile = Application.temporaryCachePath + "/UnityTranslation/plurals.json";

            string targetFile = Application.dataPath + "/Scripts/UnityTranslation/Generated/PluralsRules.cs";

            byte[] cldrFileBytes = null;

            #region Check that PluralsRules.cs is up to date
            if (!File.Exists(cldrPluralsFile))
            {
                Debug.LogError("File \"" + cldrPluralsFile + "\" not found");

                return;
            }

            cldrFileBytes = File.ReadAllBytes(cldrPluralsFile);

            #if !FORCE_CODE_GENERATION
            if (
                !previouslyGeneratedLanguages
                &&
                File.Exists(targetFile)
                &&
                File.Exists(tempPluralsFile)
               )
            {
                byte[] tempFileBytes = File.ReadAllBytes(tempPluralsFile);

                if (cldrFileBytes.SequenceEqual(tempFileBytes))
                {
                    return;
                }
            }
            #endif
            #endregion

            #region Generating PluralsRules.cs file
            Debug.Log("Generating \"PluralsRules.cs\" file");

            string cldrFileText = Encoding.UTF8.GetString(cldrFileBytes);
            JSONObject json = new JSONObject(cldrFileText);

            if (json.type == JSONObject.Type.OBJECT)
            {
                List<List<Language>>                      languages = new List<List<Language>>();
                List<Dictionary<PluralsQuantity, string>> plurals   = new List<Dictionary<PluralsQuantity, string>>();

                #region Parse JSON
                bool good = true;

                json.GetField("supplemental", delegate(JSONObject supplementalJson)
                {
                    supplementalJson.GetField("plurals-type-cardinal", delegate(JSONObject pluralsJson)
                    {
                        for (int i = 0; i < pluralsJson.list.Count; ++i)
                        {
                            if (pluralsJson.keys[i] == "root")
                            {
                                continue;
                            }

                            JSONObject languageJson = pluralsJson.list[i];

                            List<Language> languagesList = new List<Language>();

                            for (int j = 0; j < LanguageCode.codes.Length; ++j)
                            {
                                if (
                                    LanguageCode.codes[j] == pluralsJson.keys[i]
                                    ||
                                    LanguageCode.codes[j].StartsWith(pluralsJson.keys[i] + "-")
                                   )
                                {
                                    languagesList.Add((Language)j);
                                }
                            }

                            if (languagesList.Count == 0)
                            {
                                Debug.LogWarning("Unexpected language code \"" + pluralsJson.keys[i] + "\" in \"" + cldrPluralsFile + "\". Skipped");

                                continue;
                            }

                            Dictionary<PluralsQuantity, string> languagePlurals = new Dictionary<PluralsQuantity, string>();

                            for (int j = 0; j < languageJson.list.Count; ++j)
                            {
                                PluralsQuantity quantity;

                                if (languageJson.keys[j] == "pluralRule-count-zero")
                                {
                                    quantity = PluralsQuantity.Zero;
                                }
                                else
                                if (languageJson.keys[j] == "pluralRule-count-one")
                                {
                                    quantity = PluralsQuantity.One;
                                }
                                else
                                if (languageJson.keys[j] == "pluralRule-count-two")
                                {
                                    quantity = PluralsQuantity.Two;
                                }
                                else
                                if (languageJson.keys[j] == "pluralRule-count-few")
                                {
                                    quantity = PluralsQuantity.Few;
                                }
                                else
                                if (languageJson.keys[j] == "pluralRule-count-many")
                                {
                                    quantity = PluralsQuantity.Many;
                                }
                                else
                                if (languageJson.keys[j] == "pluralRule-count-other")
                                {
                                    quantity = PluralsQuantity.Other;
                                }
                                else
                                {
                                    good = false;

                                    Debug.LogError("Unexpected plurals \"" + languageJson.keys[j] + "\" found for language code \"" + pluralsJson.keys[i] + "\" in \"" + cldrPluralsFile + "\"");

                                    break;
                                }

                                languagePlurals[quantity] = languageJson.list[j].str;
                            }

                            if (!good)
                            {
                                break;
                            }

                            int index = -1;

                            for (int j = 0; j < plurals.Count; ++j)
                            {
                                index = j;

                                for (int k = 0; k < (int)PluralsQuantity.Count; ++k)
                                {
                                    string leftString;
                                    string rightString;

                                    plurals[j].TryGetValue(     (PluralsQuantity)k, out leftString);
                                    languagePlurals.TryGetValue((PluralsQuantity)k, out rightString);

                                    if (leftString == null && rightString == null)
                                    {
                                        continue;
                                    }

                                    if (
                                        leftString != null && rightString == null
                                        ||
                                        leftString == null && rightString != null
                                       )
                                    {
                                        index = -1;
                                        break;
                                    }

                                    int charIndex = leftString.IndexOf('@');

                                    if (charIndex >= 0)
                                    {
                                        leftString = leftString.Substring(0, charIndex).Trim();
                                    }

                                    charIndex = rightString.IndexOf('@');

                                    if (charIndex >= 0)
                                    {
                                        rightString = rightString.Substring(0, charIndex).Trim();
                                    }

                                    if (leftString != rightString)
                                    {
                                        index = -1;
                                        break;
                                    }
                                }

                                if (index >= 0)
                                {
                                    break;
                                }
                            }

                            if (index >= 0)
                            {
                                languages[index].AddRange(languagesList);
                            }
                            else
                            {
                                languages.Add(languagesList);
                                plurals.Add(languagePlurals);
                            }
                        }
                    },
                    delegate(string name)
                    {
                        good = false;

                        Debug.LogError("Entry \"" + name + "\" not found in \"" + cldrPluralsFile + "\"");
                    });
                },
                delegate(string name)
                {
                    good = false;

                    Debug.LogError("Entry \"" + name + "\" not found in \"" + cldrPluralsFile + "\"");
                });

                if (!good)
                {
                    return;
                }

                #region Move English plurals to the beginning
                for (int i = 0; i < languages.Count; ++i)
                {
                    if (languages[i].Contains(Language.English))
                    {
                        List<Language>                      englishLanguages = languages[i];
                        Dictionary<PluralsQuantity, string> englishPlurals   = plurals[i];

                        languages.RemoveAt(i);
                        plurals.RemoveAt(i);

                        languages.Insert(0, englishLanguages);
                        plurals.Insert(0, englishPlurals);

                        break;
                    }
                }
                #endregion

                #endregion

                string res = "// This file generated from \"CLDR/json-full/supplemental/plurals.json\" file.\n" +
                             "using System;\n" +
                             "\n" +
                             "\n" +
                             "\n" +
                             "namespace UnityTranslation\n" +
                             "{\n" +
                             "    /// <summary>\n" +
                             "    /// Container for all plurals rules for each language.\n" +
                             "    /// </summary>\n" +
                             "    public static class PluralsRules\n" +
                             "    {\n" +
                             "        public delegate PluralsQuantity PluralsFunction(double quantity);\n" +
                             "\n" +
                             "        /// <summary>\n" +
                             "        /// Array of functions for getting PluralsQuantity.\n" +
                             "        /// </summary>\n" +
                             "        public static readonly PluralsFunction[] pluralsFunctions = new PluralsFunction[]\n" +
                             "        {\n" +
                             "              pluralsDefaultFunction  // Default\n";

                for (int i = 1; i < (int)Language.Count; ++i)
                {
                    Language language = (Language)i;

                    int index = -1;

                    for (int j = 0; j < languages.Count; ++j)
                    {
                        if (languages[j].Contains(language))
                        {
                            index = j;
                            break;
                        }
                    }

                    if (index >= 0)
                    {
                        if (index == 0)
                        {
                            res += "            , pluralsDefaultFunction  // " + language + "\n";
                        }
                        else
                        {
                            res += string.Format("            , {0,-23} // {1}\n", "pluralsFunction" + index, language);
                        }
                    }
                    else
                    {
                        res += "            , pluralsFallbackFunction // " + language + "\n";
                    }
                }

                res += "        };\n" +
                       "\n" +
                       "        /// <summary>\n" +
                       "        /// Fallback function for any language without plurals rules that just return PluralsQuantity.Other.\n" +
                       "        /// </summary>\n" +
                       "        /// <returns>PluralsQuantity.Other.</returns>\n" +
                       "        /// <param name=\"quantity\">Quantity.</param>\n" +
                       "        private static PluralsQuantity pluralsFallbackFunction(double quantity)\n" +
                       "        {\n" +
                       "            return PluralsQuantity.Other;\n" +
                       "        }\n";

                for (int i = 0; i < languages.Count; ++i)
                {
                    string functionName = (i == 0)? "pluralsDefaultFunction" : ("pluralsFunction" + i);

                    res += "\n" +
                           "        /// <summary>\n";

                    if (i == 0)
                    {
                        res += "        /// <para>Default function for languages that has the same plurals rules as for English</para>\n";
                    }
                    else
                    {
                        res += "        /// <para>Some function for getting PluralsQuantity</para>\n";
                    }

                    res += "        /// <para>Used in languages:</para>\n";

                    for (int j = 0; j < languages[i].Count; ++j)
                    {
                        res += "        /// <para>" + languages[i][j] + "</para>\n";
                    }

                    res += "        /// </summary>\n" +
                           "        /// <returns>PluralsQuantity related to provided quantity.</returns>\n" +
                           "        /// <param name=\"quantity\">Quantity.</param>\n" +
                           "        private static PluralsQuantity " + functionName + "(double quantity)\n" +
                           "        {\n";

                    List<string> additionalLines = new List<string>();
                    List<string> conditions      = new List<string>();

                    for (int j = 0; j < (int)PluralsQuantity.Other; ++j)
                    {
                        string pluralsCondition;

                        if (plurals[i].TryGetValue((PluralsQuantity)j, out pluralsCondition))
                        {
                            conditions.Add("            if (" + transformPluralsCondition(pluralsCondition, ref additionalLines) + ") // " + pluralsCondition + "\n" +
                                           "            {\n" +
                                           "                return PluralsQuantity." + (PluralsQuantity)j + ";\n" +
                                           "            }");
                        }
                    }

                    if (additionalLines.Count > 0)
                    {
                        for (int j = 0; j < additionalLines.Count; ++j)
                        {
                            res += "            " + additionalLines[j] + "\n";
                        }

                        res += "\n";
                    }

                    for (int j = 0; j < conditions.Count; ++j)
                    {
                        res += conditions[j] + "\n" +
                               "\n";
                    }

                    res += "            return PluralsQuantity.Other;\n" +
                           "        }\n";
                }

                res += "    }\n" +
                       "}\n";

                File.WriteAllText(targetFile, res, Encoding.UTF8);
            }
            else
            {
                Debug.LogError("Incorrect file format in \"" + cldrPluralsFile + "\"");

                return;
            }

            Debug.Log("Caching CLDR plurals file in \"" + tempPluralsFile + "\"");
            Directory.CreateDirectory(Application.temporaryCachePath + "/UnityTranslation");
            File.WriteAllBytes(tempPluralsFile, cldrFileBytes);
            #endregion
        }

        /// <summary>
        /// Transforms the plurals condition in C# syntax.
        /// </summary>
        /// <returns>Plurals condition in C# syntax.</returns>
        /// <param name="condition">Plurals condition.</param>
        /// <param name="additionalLines">Additional lines if needed.</param>
        private static string transformPluralsCondition(string condition, ref List<string> additionalLines)
        {
            int index = condition.IndexOf('@');

            if (index >= 0)
            {
                condition = condition.Substring(0, index).Trim();
            }

            condition = condition.Replace("or",  " || ").Replace("and", " && ");
            condition = condition.Replace("%",   " % " ).Replace("=",   " == ").Replace("! ==",   " != ");
            condition = condition.Replace(", ",  ","   ).Replace(" ,",  ",");
            condition = condition.Replace(".. ", ".."  ).Replace(" ..", "..");

            do
            {
                string oldCondition = condition;

                condition = condition.Replace("  ", " ");

                if (condition == oldCondition)
                {
                    break;
                }
            } while (true);

            bool containsN = condition.Contains("n");
            bool containsI = condition.Contains("i");
            bool containsV = condition.Contains("v");
            bool containsW = condition.Contains("w");
            bool containsF = condition.Contains("f");
            bool containsT = condition.Contains("t");

            if (
                containsN
                ||
                containsI
                ||
                containsV
                ||
                containsW
                ||
                containsF
                ||
                containsT
               )
            {
                string newAdditionalLine = "double n = Math.Abs(quantity);       // absolute value of the source number (integer and decimals)";

                if (!additionalLines.Contains(newAdditionalLine))
                {
                    additionalLines.Add(newAdditionalLine);
                }

                if (containsI)
                {
                    newAdditionalLine = "int    i = (int)Math.Floor(n);       // integer digits of n";

                    if (!additionalLines.Contains(newAdditionalLine))
                    {
                        additionalLines.Add(newAdditionalLine);
                    }
                }

                if (
                    containsV
                    ||
                    containsW
                    ||
                    containsF
                    ||
                    containsT
                   )
                {
                    newAdditionalLine = "int    v = 3;                        // number of visible fraction digits in n, with trailing zeros";

                    if (!additionalLines.Contains(newAdditionalLine))
                    {
                        additionalLines.Add(newAdditionalLine);
                        additionalLines.Add("int    f = ((int)(n * 1000)) % 1000; // visible fractional digits in n, with trailing zeros");
                        additionalLines.Add("");
                        additionalLines.Add("if (f == 0)");
                        additionalLines.Add("{");
                        additionalLines.Add("    v = 0;");
                        additionalLines.Add("}");
                        additionalLines.Add("else");
                        additionalLines.Add("{");
                        additionalLines.Add("    while ((f > 0) && (f % 10 == 0))");
                        additionalLines.Add("    {");
                        additionalLines.Add("        f /= 10;");
                        additionalLines.Add("        --v;");
                        additionalLines.Add("    }");
                        additionalLines.Add("}");
                    }

                    if (
                        containsW
                        ||
                        containsT
                       )
                    {
                        if (containsW)
                        {
                            newAdditionalLine = "int    w = v;                        // number of visible fraction digits in n, without trailing zeros";

                            if (!additionalLines.Contains(newAdditionalLine))
                            {
                                additionalLines.Add("");
                                additionalLines.Add(newAdditionalLine);
                            }
                        }

                        if (containsT)
                        {
                            newAdditionalLine = "int    t = f;                        // visible fractional digits in n, without trailing zeros";

                            if (!additionalLines.Contains(newAdditionalLine))
                            {
                                additionalLines.Add("");
                                additionalLines.Add(newAdditionalLine);
                            }
                        }
                    }
                }
            }

            if (condition.Contains("n % 1000000"))
            {
                string newAdditionLine = "double n_mod_1000000 = n % 1000000;";

                if (!additionalLines.Contains(newAdditionLine))
                {
                    additionalLines.Add(newAdditionLine);
                }

                condition = condition.Replace("n % 1000000", "n_mod_1000000");
            }

            if (condition.Contains("n % 100"))
            {
                string newAdditionLine = "double n_mod_100 = n % 100;";

                if (!additionalLines.Contains(newAdditionLine))
                {
                    additionalLines.Add(newAdditionLine);
                }

                condition = condition.Replace("n % 100", "n_mod_100");
            }

            if (condition.Contains("n % 10"))
            {
                string newAdditionLine = "double n_mod_10 = n % 10;";

                if (!additionalLines.Contains(newAdditionLine))
                {
                    additionalLines.Add(newAdditionLine);
                }

                condition = condition.Replace("n % 10", "n_mod_10");
            }

            if (condition.Contains("i % 100"))
            {
                string newAdditionLine = "int i_mod_100 = i % 100;";

                if (!additionalLines.Contains(newAdditionLine))
                {
                    additionalLines.Add(newAdditionLine);
                }

                condition = condition.Replace("i % 100", "i_mod_100");
            }

            if (condition.Contains("i % 10"))
            {
                string newAdditionLine = "int i_mod_10 = i % 10;";

                if (!additionalLines.Contains(newAdditionLine))
                {
                    additionalLines.Add(newAdditionLine);
                }

                condition = condition.Replace("i % 10", "i_mod_10");
            }

            if (condition.Contains("f % 100"))
            {
                string newAdditionLine = "int f_mod_100 = f % 100;";

                if (!additionalLines.Contains(newAdditionLine))
                {
                    additionalLines.Add(newAdditionLine);
                }

                condition = condition.Replace("f % 100", "f_mod_100");
            }

            if (condition.Contains("f % 10"))
            {
                string newAdditionLine = "int f_mod_10 = f % 10;";

                if (!additionalLines.Contains(newAdditionLine))
                {
                    additionalLines.Add(newAdditionLine);
                }

                condition = condition.Replace("f % 10", "f_mod_10");
            }

            if (condition.Contains("t % 100"))
            {
                string newAdditionLine = "int t_mod_100 = t % 100;";

                if (!additionalLines.Contains(newAdditionLine))
                {
                    additionalLines.Add(newAdditionLine);
                }

                condition = condition.Replace("t % 100", "t_mod_100");
            }

            if (condition.Contains("t % 10"))
            {
                string newAdditionLine = "int t_mod_10 = t % 10;";

                if (!additionalLines.Contains(newAdditionLine))
                {
                    additionalLines.Add(newAdditionLine);
                }

                condition = condition.Replace("t % 10", "t_mod_10");
            }

            List<string> conditionTokens = new List<string>(condition.Split(' '));

            if (conditionTokens.Count == 0)
            {
                Debug.LogError("Impossible to transform condition \"" + condition + "\"");

                return "false";
            }

            for (int i = 0; i < conditionTokens.Count; ++i)
            {
                if (
                    conditionTokens[i].Contains(',')
                    ||
                    conditionTokens[i].Contains("..")
                   )
                {
                    if (i < 2)
                    {
                        Debug.LogError("Impossible to transform condition \"" + condition + "\". Incorrect syntax");

                        return "false";
                    }

                    string varName      = conditionTokens[i - 2];
                    string operatorType = conditionTokens[i - 1];
                    string rangeList    = conditionTokens[i];

                    if (operatorType != "==" && operatorType != "!=")
                    {
                        Debug.LogError("Impossible to transform condition \"" + condition + "\". Incorrect operator \"" + operatorType + "\"");

                        return "false";
                    }

                    string[] separator = new string[] { ".." };

                    List<string[]> ranges = new List<string[]>();

                    foreach (string rangeEnum in rangeList.Split(','))
                    {
                        ranges.Add(rangeEnum.Split(separator, StringSplitOptions.None));
                    }

                    string replacement = "";

                    if (operatorType == "==")
                    {
                        for (int j = 0; j < ranges.Count; ++j)
                        {
                            if (j > 0)
                            {
                                replacement += " || ";
                            }

                            if (ranges[j].Length == 1)
                            {
                                replacement += varName + " == " + ranges[j][0];
                            }
                            else
                            if (ranges[j].Length == 2)
                            {
                                replacement += "(" + varName + " >= " + ranges[j][0] + " && " + varName + " <= " + ranges[j][1] + ")";
                            }
                            else
                            {
                                Debug.LogError("Impossible to transform condition \"" + condition + "\". Incorrect range \"" + rangeList + "\"");

                                return "false";
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < ranges.Count; ++j)
                        {
                            if (j > 0)
                            {
                                replacement += " && ";
                            }

                            if (ranges[j].Length == 1)
                            {
                                replacement += varName + " != " + ranges[j][0];
                            }
                            else
                            if (ranges[j].Length == 2)
                            {
                                replacement += "(" + varName + " < " + ranges[j][0] + " || " + varName + " > " + ranges[j][1] + ")";
                            }
                            else
                            {
                                Debug.LogError("Impossible to transform condition \"" + condition + "\". Incorrect range \"" + rangeList + "\"");

                                return "false";
                            }
                        }
                     }

                    if (ranges.Count > 1)
                    {
                        replacement = "(" + replacement + ")";
                    }

                    i -= 2;

                    conditionTokens.RemoveAt(i);
                    conditionTokens.RemoveAt(i);

                    conditionTokens[i] = replacement;
                }
            }

            string res = "";

            for (int i = 0; i < conditionTokens.Count; ++i)
            {
                if (i > 0)
                {
                    res += " ";
                }

                res += conditionTokens[i];
            }

            return res;
        }
#endif

        /// <summary>
        /// Generates Assets/Resources/res/values/strings.xml file if absent
        /// </summary>
        private static void generateStringsXml()
        {
            string valuesFolder   = Application.dataPath + "/Resources/res/values";
            string stringsXmlFile = valuesFolder + "/strings.xml";

            if (!File.Exists(stringsXmlFile))
            {
                Debug.Log("Generating \"Assets/Resources/res/values/strings.xml\" file");

                Directory.CreateDirectory(valuesFolder);

                File.WriteAllText(stringsXmlFile
                                  , "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
                                    "<resources>\n" +
                                    "\n" +
                                    "    <!-- Application name -->\n" +
                                    "    <string name=\"app_name\">Application name</string>\n" +
                                    "\n" +
                                    "</resources>"
                                  , Encoding.UTF8);

                Debug.LogWarning("Resource \"Assets/Resources/res/values/strings.xml\" generated. Please rebuild your application. You have to switch focus to another application to let Unity check that project structure was changed.");
            }
        }

        /// <summary>
        /// Generates R.cs file for all specified tokens
        /// </summary>
        private static void generateR()
        {
            string stringsXmlFile = Application.dataPath + "/Resources/res/values/strings.xml";

            if (File.Exists(stringsXmlFile))
            {
                string rFilePath = null;

                #region Search for R.cs file
                DirectoryInfo assetsFolder = new DirectoryInfo(Application.dataPath);
                FileInfo[] foundFiles = assetsFolder.GetFiles("R.cs", SearchOption.AllDirectories);

                if (foundFiles.Length > 0)
                {
                    rFilePath = foundFiles[0].FullName;
                }
                else
                {
                    DirectoryInfo[] foundDirs = assetsFolder.GetDirectories("UnityTranslation", SearchOption.AllDirectories);

                    for (int i = 0; i < foundDirs.Length; ++i)
                    {
                        if (File.Exists(foundDirs[i].FullName + "/Translator.cs"))// TODO: AbstractTranslator
                        {
                            rFilePath = foundDirs[i].FullName + "/Generated/R.cs";

                            break;
                        }
                    }

                    if (rFilePath == null)
                    {
                        rFilePath = Application.dataPath + "/R.cs";

                        Debug.LogError("Unexpected behaviour for getting path to \"R.cs\" file");
                    }
                }
                #endregion

                // TODO: Sections

                string tempStringsXmlFile = Application.temporaryCachePath + "/UnityTranslation/values/strings.xml";

                string targetFile = rFilePath.Replace('\\', '/');

                byte[] xmlFileBytes = File.ReadAllBytes(stringsXmlFile);

                #region Check that R.cs is up to date
                #if !FORCE_CODE_GENERATION
                if (
#if I_AM_UNITY_TRANSLATION_DEVELOPER
                    !previouslyGeneratedLanguages
                    &&
#endif
                    File.Exists(targetFile)
                    &&
                    File.Exists(tempStringsXmlFile)
                   )
                {
                    byte[] tempFileBytes = File.ReadAllBytes(tempStringsXmlFile);

                    if (xmlFileBytes.SequenceEqual(tempFileBytes))
                    {
                        return;
                    }
                }
                #endif
                #endregion

                #region Generating R.cs file
                Debug.Log("Generating \"R.cs\" file");

                List<string>                              stringNames               = new List<string>();
                List<string>                              stringComments            = new List<string>();
                List<string>                              stringValues              = new List<string>();

                List<string>                              stringArrayNames          = new List<string>();
                List<string>                              stringArrayComments       = new List<string>();
                List<List<string>>                        stringArrayValuesComments = new List<List<string>>();
                List<List<string>>                        stringArrayValues         = new List<List<string>>();

                List<string>                              pluralsNames              = new List<string>();
                List<string>                              pluralsComments           = new List<string>();
                List<Dictionary<PluralsQuantity, string>> pluralsValuesComments     = new List<Dictionary<PluralsQuantity, string>>();
                List<Dictionary<PluralsQuantity, string>> pluralsValues             = new List<Dictionary<PluralsQuantity, string>>();

                #region Parse strings.xml file
                bool good = true;

                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(new MemoryStream(xmlFileBytes, false));
                    reader.WhitespaceHandling = WhitespaceHandling.None;

                    bool resourcesFound = false;

                    while (reader.Read())
                    {
                        if (reader.Name == "resources")
                        {
                            resourcesFound = true;

                            string lastComment = null;

                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Comment)
                                {
                                    lastComment = reader.Value.Trim();
                                }
                                else
                                if (reader.NodeType == XmlNodeType.Element)
                                {
                                    if (reader.Name == "string")
                                    {
                                        string tokenName = reader.GetAttribute("name");

                                        if (!checkTokenName(tokenName, reader.Name, stringNames))
                                        {
                                            good = false;

                                            break;
                                        }

                                        stringNames.Add(tokenName);
                                        stringComments.Add(lastComment);
                                        stringValues.Add(reader.ReadString());

                                        lastComment = null;
                                    }
                                    else
                                    if (reader.Name == "string-array")
                                    {
                                        string tokenName = reader.GetAttribute("name");

                                        if (!checkTokenName(tokenName, reader.Name, stringArrayNames))
                                        {
                                            good = false;

                                            break;
                                        }

                                        List<string> values         = new List<string>();
                                        List<string> valuesComments = new List<string>();

                                        string lastValueComment = null;

                                        while (reader.Read())
                                        {
                                            if (reader.NodeType == XmlNodeType.Comment)
                                            {
                                                lastValueComment = reader.Value.Trim();
                                            }
                                            else
                                            if (reader.NodeType == XmlNodeType.Element)
                                            {
                                                if (reader.Name == "item")
                                                {
                                                    valuesComments.Add(lastValueComment);
                                                    values.Add(reader.ReadString());

                                                    lastValueComment = null;
                                                }
                                                else
                                                {
                                                    good = false;

                                                    Debug.LogError("Unexpected tag <" + reader.Name + "> found in tag <string-array>");

                                                    break;
                                                }
                                            }
                                            else
                                            if (reader.NodeType == XmlNodeType.EndElement)
                                            {
                                                if (reader.Name == "string-array")
                                                {
                                                    break;
                                                }
                                            }
                                        }

                                        if (!good)
                                        {
                                            break;
                                        }

                                        stringArrayNames.Add(tokenName);
                                        stringArrayComments.Add(lastComment);
                                        stringArrayValues.Add(values);
                                        stringArrayValuesComments.Add(valuesComments);

                                        lastComment = null;
                                    }
                                    else
                                    if (reader.Name == "plurals")
                                    {
                                        string tokenName = reader.GetAttribute("name");

                                        if (!checkTokenName(tokenName, reader.Name, pluralsNames))
                                        {
                                            good = false;

                                            break;
                                        }

                                        Dictionary<PluralsQuantity, string> values         = new Dictionary<PluralsQuantity, string>();
                                        Dictionary<PluralsQuantity, string> valuesComments = new Dictionary<PluralsQuantity, string>();

                                        string lastValueComment = null;

                                        while (reader.Read())
                                        {
                                            if (reader.NodeType == XmlNodeType.Comment)
                                            {
                                                lastValueComment = reader.Value.Trim();
                                            }
                                            else
                                            if (reader.NodeType == XmlNodeType.Element)
                                            {
                                                if (reader.Name == "item")
                                                {
                                                    PluralsQuantity quantity = PluralsQuantity.Count; // Nothing

                                                    string quantityValue = reader.GetAttribute("quantity");

                                                    if (quantityValue == null)
                                                    {
                                                        good = false;

                                                        Debug.LogError("Attribute \"quantity\" not found for tag <item> in tag <plurals> with name \"" + tokenName + "\" in \"Assets/Resources/res/values/strings.xml\"");

                                                        break;
                                                    }

                                                    if (quantityValue == "")
                                                    {
                                                        good = false;

                                                        Debug.LogError("Attribute \"quantity\" empty for tag <item> in tag <plurals> with name \"" + tokenName + "\" in \"Assets/Resources/res/values/strings.xml\"");

                                                        break;
                                                    }

                                                    if (quantityValue == "zero")
                                                    {
                                                        quantity = PluralsQuantity.Zero;
                                                    }
                                                    else
                                                    if (quantityValue == "one")
                                                    {
                                                        quantity = PluralsQuantity.One;
                                                    }
                                                    else
                                                    if (quantityValue == "two")
                                                    {
                                                        quantity = PluralsQuantity.Two;
                                                    }
                                                    else
                                                    if (quantityValue == "few")
                                                    {
                                                        quantity = PluralsQuantity.Few;
                                                    }
                                                    else
                                                    if (quantityValue == "many")
                                                    {
                                                        quantity = PluralsQuantity.Many;
                                                    }
                                                    else
                                                    if (quantityValue == "other")
                                                    {
                                                        quantity = PluralsQuantity.Other;
                                                    }
                                                    else
                                                    {
                                                        good = false;

                                                        Debug.LogError("Unknown attribute \"quantity\" value \"" + quantityValue + "\" for tag <item> in tag <plurals> with name \"" + tokenName + "\" in \"Assets/Resources/res/values/strings.xml\"");

                                                        break;
                                                    }

                                                    valuesComments[quantity] = lastValueComment;
                                                    values[quantity]         = reader.ReadString();

                                                    lastValueComment = null;
                                                }
                                                else
                                                {
                                                    good = false;

                                                    Debug.LogError("Unexpected tag <" + reader.Name + "> found in tag <plurals>");

                                                    break;
                                                }
                                            }
                                            else
                                            if (reader.NodeType == XmlNodeType.EndElement)
                                            {
                                                if (reader.Name == "plurals")
                                                {
                                                    break;
                                                }
                                            }
                                        }

                                        if (!good)
                                        {
                                            break;
                                        }

                                        pluralsNames.Add(tokenName);
                                        pluralsComments.Add(lastComment);
                                        pluralsValues.Add(values);
                                        pluralsValuesComments.Add(valuesComments);

                                        lastComment = null;
                                    }
                                    else
                                    {
                                        good = false;

                                        Debug.LogError("Unexpected tag <" + reader.Name + "> found in tag <resources>");

                                        break;
                                    }
                                }
                            }

                            break;
                        }
                    }

                    if (!resourcesFound)
                    {
                        good = false;

                        Debug.LogError("Tag <resources> not found in \"Assets/Resources/res/values/strings.xml\"");
                    }
                }
                catch (Exception /*e*/)
                {
                    good = false;

                    Debug.LogError("Exception occured while parsing \"Assets/Resources/res/values/strings.xml\"");
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }

                if (!good)
                {
                    return;
                }
                #endregion

                string res = "// This file generated from \"Assets/Resources/res/values/strings.xml\" file.\n" +
                             "\n" +
                             "\n" +
                             "\n" +
                             "namespace UnityTranslation\n" +
                             "{\n" +
                             "    /// <summary>\n" +
                             "    /// Container for all tokens specified in \"Assets/Resources/res/values/strings.xml\"\n" +
                             "    /// </summary>\n" +
                             "    public sealed class R\n" +
                             "    {\n" +
                             "        /// <summary>\n" +
                             "        /// Enumeration of all string tags in \"Assets/Resources/res/values/strings.xml\"\n" +
                             "        /// </summary>\n" +
                             "        public enum strings\n" +
                             "        {\n";

                for (int i = 0; i < stringNames.Count; ++i)
                {
                    string[] separators = new string[] {"\n", "\\n"};

                    string[] commentLines = stringComments[i] == null ? null : stringComments[i].Split(separators, StringSplitOptions.None);
                    string[] valueLines   =                                    stringValues[i].Split(separators, StringSplitOptions.None);

                    res += "            /// <summary>\n";

                    if (commentLines != null)
                    {
                        for (int j = 0; j < commentLines.Length; ++j)
                        {
                            res += "            /// <para>" + commentLines[j] + "</para>\n";
                        }
                    }

                    res += "            /// <para>Value:</para>\n";

                    if (valueLines != null)
                    {
                        for (int j = 0; j < valueLines.Length; ++j)
                        {
                            res += "            ///   <para>" + valueLines[j] + "</para>\n";
                        }
                    }

                    res += "            /// </summary>\n" +
                           "            " + stringNames[i] + "\n" +
                           "            ,\n";
                }

                res += "            Count // Should be last\n" +
                       "        }\n" +
                       "\n" +
                       "        /// <summary>\n" +
                       "        /// Enumeration of all string-array tags in \"Assets/Resources/res/values/strings.xml\"\n" +
                       "        /// </summary>\n" +
                       "        public enum array\n" +
                       "        {\n";

                for (int i = 0; i < stringArrayNames.Count; ++i)
                {
                    string[] separators = new string[] {"\n", "\\n"};

                    string[] commentLines = stringArrayComments[i] == null ? null : stringArrayComments[i].Split(separators, StringSplitOptions.None);

                    res += "            /// <summary>\n";

                    if (commentLines != null)
                    {
                        for (int j = 0; j < commentLines.Length; ++j)
                        {
                            res += "            /// <para>" + commentLines[j] + "</para>\n";
                        }
                    }

                    res += "            /// <para>Value:</para>\n";

                    for (int j = 0; j < stringArrayValues[i].Count; ++j)
                    {
                        string[] valueCommentLines = stringArrayValuesComments[i][j] == null ? null : stringArrayValuesComments[i][j].Split(separators, StringSplitOptions.None);
                        string[] valueLines        =                                                  stringArrayValues[i][j].Split(separators, StringSplitOptions.None);

                        res += "            ///   <para>- Item " + (j + 1) + ":</para>\n";

                        if (valueCommentLines != null)
                        {
                            for (int k = 0; k < valueCommentLines.Length; ++k)
                            {
                                res += "            ///     <para>// " + valueCommentLines[k] + "</para>\n";
                            }
                        }

                        for (int k = 0; k < valueLines.Length; ++k)
                        {
                            res += "            ///     <para>" + valueLines[k] + "</para>\n";
                        }
                    }

                    res += "            /// </summary>\n" +
                           "            " + stringArrayNames[i] + "\n" +
                           "            ,\n";
                }

                res += "            Count // Should be last\n" +
                       "        }\n" +
                       "\n" +
                       "        /// <summary>\n" +
                       "        /// Enumeration of all plurals tags in \"Assets/Resources/res/values/strings.xml\"\n" +
                       "        /// </summary>\n" +
                       "        public enum plurals\n" +
                       "        {\n";

                for (int i = 0; i < pluralsNames.Count; ++i)
                {
                    string[] separators = new string[] {"\n", "\\n"};

                    string[] commentLines = pluralsComments[i] == null ? null : pluralsComments[i].Split(separators, StringSplitOptions.None);

                    res += "            /// <summary>\n";

                    if (commentLines != null)
                    {
                        for (int j = 0; j < commentLines.Length; ++j)
                        {
                            res += "            /// <para>" + commentLines[j] + "</para>\n";
                        }
                    }

                    res += "            /// <para>Value:</para>\n";

                    for (int j = 0; j < (int)PluralsQuantity.Count; ++j)
                    {
                        PluralsQuantity quantity = (PluralsQuantity)j;
                        string valueComment;
                        string value;

                        if (
                            pluralsValuesComments[i].TryGetValue(quantity, out valueComment)
                            &&
                            pluralsValues[i].TryGetValue(quantity, out value)
                           )
                        {
                            string[] valueCommentLines = valueComment == null ? null : valueComment.Split(separators, StringSplitOptions.None);
                            string[] valueLines        =                               value.Split(separators, StringSplitOptions.None);

                            res += "            ///   <para>- " + quantity + ":</para>\n";

                            if (valueCommentLines != null)
                            {
                                for (int k = 0; k < valueCommentLines.Length; ++k)
                                {
                                    res += "            ///     <para>// " + valueCommentLines[k] + "</para>\n";
                                }
                            }

                            for (int k = 0; k < valueLines.Length; ++k)
                            {
                                res += "            ///     <para>" + valueLines[k] + "</para>\n";
                            }
                        }
                    }

                    res += "            /// </summary>\n" +
                           "            " + pluralsNames[i] + "\n" +
                           "            ,\n";
                }

                res += "            Count // Should be last\n" +
                       "        }\n" +
                       "    }\n" +
                       "}\n";

                File.WriteAllText(targetFile, res, Encoding.UTF8);

                Debug.Log("Caching strings.xml file in \"" + tempStringsXmlFile + "\"");
                Directory.CreateDirectory(Application.temporaryCachePath + "/UnityTranslation/values");
                File.WriteAllBytes(tempStringsXmlFile, xmlFileBytes);
                #endregion
            }
            else
            {
                Debug.LogError("Resource \"Assets/Resources/res/values/strings.xml\" not found. UnityTranslation is not available for now.");
            }
        }

        /// <summary>
        /// Checks the name of the token.
        /// </summary>
        /// <returns><c>true</c>, if token name is correct, <c>false</c> otherwise.</returns>
        /// <param name="tokenName">Token name.</param>
        /// <param name="tagName">Tag name.</param>
        /// <param name="tokenNames">List of token names.</param>
        private static bool checkTokenName(string tokenName, string tagName, List<string> tokenNames)
        {
            if (tokenName == null)
            {
                Debug.LogError("Attribute \"name\" not found for tag <" + tagName + "> in \"Assets/Resources/res/values/strings.xml\"");

                return false;
            }

            if (tokenName == "")
            {
                Debug.LogError("Attribute \"name\" empty for tag <" + tagName + "> in \"Assets/Resources/res/values/strings.xml\"");

                return false;
            }

            if (tokenName[0] >= '0' && tokenName[0] <= '9')
            {
                Debug.LogError("Attribute \"name\" for tag <" + tagName + "> starts with a digit (\"" + tokenName + "\") in \"Assets/Resources/res/values/strings.xml\"");

                return false;
            }

            for (int i = 0; i < tokenName.Length; ++i)
            {
                char ch = tokenName[i];

                if (
                    (ch < 'a' || ch > 'z')
                    &&
                    (ch < 'A' || ch > 'Z')
                    &&
                    (ch < '0' || ch > '9')
                    &&
                    ch != '_'
                   )
                {
                    Debug.LogError("Attribute \"name\" for tag <" + tagName + "> has unsupported character \"" + ch + "\" in value \"" + tokenName + "\" in \"Assets/Resources/res/values/strings.xml\"");

                    return false;
                }
            }

            if (tokenNames.Contains(tokenName))
            {
                Debug.LogError("Attribute \"name\" for tag <" + tagName + "> has duplicate value \"" + tokenName + "\" in \"Assets/Resources/res/values/strings.xml\"");

                return false;
            }

            return true;
        }

        /// <summary>
        /// Generates AvailableLanguages.cs file
        /// </summary>
        private static void generateAvailableLanguages()
        {
            List<string> valuesFolders       = new List<string>();
            string       valuesFoldersString = "";

            #region Getting list of languages in Assets/Resources/res
            string resFolder = Application.dataPath + "/Resources/res";

            DirectoryInfo   resDir = new DirectoryInfo(resFolder);
            DirectoryInfo[] dirs   = resDir.GetDirectories();

            for (int i = 0; i < dirs.Length; ++i)
            {
                string dirName = dirs[i].Name;

                if (dirName.StartsWith("values-"))
                {
                    string locale = dirName.Substring(7);

                    valuesFolders.Add(locale);
                    valuesFoldersString += locale + "\n";
                }
                else
                if (dirName != "values")
                {
                    Debug.LogError("Unexpected folder name \"" + dirName + "\" in \"Assets/Resources/res\"");

                    return;
                }
            }
            #endregion

            string tempValuesFolderFile = Application.temporaryCachePath + "/UnityTranslation/valuesFolders.txt";

            string targetFile = Application.dataPath + "/Scripts/UnityTranslation/Generated/AvailableLanguages.cs";

            #region Check that AvailableLanguages.cs is up to date
            #if !FORCE_CODE_GENERATION
            if (
#if I_AM_UNITY_TRANSLATION_DEVELOPER
                !previouslyGeneratedLanguages
                &&
#endif
                File.Exists(targetFile)
                &&
                File.Exists(tempValuesFolderFile)
               )
            {
                string tempFileText = File.ReadAllText(tempValuesFolderFile);

                if (valuesFoldersString == tempFileText)
                {
                    return;
                }
            }
            #endif
            #endregion

            #region Generating AvailableLanguages.cs file
            Debug.Log("Generating \"AvailableLanguages.cs\" file");

            string[]                     languageCodes     = LanguageCode.codes;
            Dictionary<Language, string> languageRealCodes = new Dictionary<Language, string>();

            for (int i = 0; i < valuesFolders.Count; ++i)
            {
                int index = -1;

                for (int j = 1; j < languageCodes.Length; ++j)
                {
                    string originalLanguageCode = languageCodes[j];
                    string languageCode         = originalLanguageCode;

                    int dashIndex = languageCode.IndexOf('-');

                    if (dashIndex >= 0)
                    {
                        languageCode = languageCode.Insert(dashIndex + 1, "r");
                    }

                    if (
                        valuesFolders[i].StartsWith(originalLanguageCode)
                        ||
                        valuesFolders[i].StartsWith(languageCode)
                       )
                    {
                        if (
                            index < 0
                            ||
                            languageCodes[j].Length > languageCodes[index].Length
                           )
                        {
                            index = j;
                        }
                    }
                }

                if (index < 0)
                {
                    Debug.LogError("Unknown language code \"" + valuesFolders[i] + "\" found in \"Assets/Resources/res\"");

                    return;
                }

                languageRealCodes[(Language)index] = valuesFolders[i];
            }

            int maxCodeLength     = 0;
            int maxRealCodeLength = 0;

            foreach (Language language in languageRealCodes.Keys)
            {
                string languageCode     = language.ToString();
                string languageRealCode = languageRealCodes[language];

                if (languageCode.Length > maxCodeLength)
                {
                    maxCodeLength = languageCode.Length;
                }

                if (languageRealCode.Length > maxRealCodeLength)
                {
                    maxRealCodeLength = languageRealCode.Length;
                }
            }

            string res = "// This file generated according to the list of \"Assets/Resources/res/values-*\" folders.\n" +
                         "using System.Collections.Generic;\n" +
                         "\n" +
                         "\n" +
                         "\n" +
                         "namespace UnityTranslation\n" +
                         "{\n" +
                         "    /// <summary>\n" +
                         "    /// Container for all languages specified in \"Assets/Resources/res\"\n" +
                         "    /// </summary>\n" +
                         "    public static class AvailableLanguages\n" +
                         "    {\n" +
                         "        /// <summary>\n" +
                         "        /// List of all languages specified in \"Assets/Resources/res\"\n" +
                         "        /// </summary>\n" +
                         "        public static readonly Dictionary<Language, string> list = new Dictionary<Language, string>\n" +
                         "        {\n" +
                         string.Format("              {{ Language.{0,-" + (maxCodeLength + 1) + "} {1,-" + (maxRealCodeLength + 2) + "} }} \n", "Default,", "\"\"");

            foreach (Language language in languageRealCodes.Keys)
            {
                res += string.Format("            , {{ Language.{0,-" + (maxCodeLength + 1) + "} {1,-" + (maxRealCodeLength + 2) + "} }} \n", language.ToString() + ",", "\"" + languageRealCodes[language] + "\"");
            }

            res += "        };\n" +
                   "    }\n" +
                   "}\n";

            File.WriteAllText(targetFile, res, Encoding.UTF8);

            Debug.Log("Caching valuesFolders.txt file in \"" + tempValuesFolderFile + "\"");
            Directory.CreateDirectory(Application.temporaryCachePath + "/UnityTranslation");
            File.WriteAllText(tempValuesFolderFile, valuesFoldersString);
            #endregion
        }
    }
}
#endif
