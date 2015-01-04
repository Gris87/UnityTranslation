#if UNITY_EDITOR

// Use this definition to generate "Languages.cs" and "PluralsRules.cs" from CLDR. Please become Unity Translation developer and commit your changes to https://github.com/Gris87/UnityTranslation
#define I_AM_UNITY_TRANSLATION_DEVELOPER

// Use this definition if you want to force code generation
#define FORCE_CODE_GENERATION



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
        public static void generate()
        {
#if I_AM_UNITY_TRANSLATION_DEVELOPER
            generateLanguages();
            generatePluralsRules();
#endif

            generateStringsXml();
            generateR();
        }

#if I_AM_UNITY_TRANSLATION_DEVELOPER
        private static void generateLanguages()
        {
            string cldrLanguagesFile = Application.dataPath           + "/../3rd_party/CLDR/json-full/main/en/languages.json";
            string tempLanguagesFile = Application.temporaryCachePath + "/languages.json";

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
                                    string temp = "";

                                    string[] tokens = languagesJson.list[i].str.Split(' ', '.', '-');

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

                                    languageCodes.Add(languagesJson.keys[i]);
                                    languageEnums.Add(languageEnum);
                                    languageNames.Add(languagesJson.list[i].str);
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
                       "    public static class LanguageCode\n" +
                       "    {\n" +
                       "        private static string[] mCodes = null;\n" +
                       "\n" +
                       "        static LanguageCode()\n" +
                       "        {\n" +
                       "            mCodes = new string[(int)Language.Count];\n" +
                       "\n" +
                       "            mCodes[(int)Language.Default] = \"\";\n";

                for (int i = 0; i < languageCodes.Count; ++i)
                {
                    res += "            mCodes[(int)Language." + languageEnums[i] + "] = \"" + languageCodes[i] + "\";\n";
                }

                res += "        }\n" +
                       "\n" +
                       "        public static string languageToCode(Language language)\n" +
                       "        {\n" +
                       "            return mCodes[(int)language];\n" +
                       "        }\n" +
                       "\n" +
                       "        public static Language codeToLanguage(string code)\n" +
                       "        {\n" +
                       "            for (int i = 0; i < (int)Language.Count; ++i)\n" +
                       "            {\n" +
                       "                if (mCodes[i] == code)\n" +
                       "                {\n" +
                       "                    return (Language)i;\n" +
                       "                }\n" +
                       "            }\n" +
                       "\n" +
                       "            return Language.Default;\n" +
                       "        }\n" +
                       "    }\n" +
					   "\n" +
					   "    public static class LanguageName\n" +
					   "    {\n" +
					   "        private static string[] mNames = null;\n" +
					   "\n" +
					   "        static LanguageName()\n" +
					   "        {\n" +
					   "            mNames = new string[(int)Language.Count];\n" +
					   "\n" +
					   "            mNames[(int)Language.Default] = \"\";\n";
				
				for (int i = 0; i < languageNames.Count; ++i)
				{
					res += "            mNames[(int)Language." + languageEnums[i] + "] = \"" + languageNames[i] + "\";\n";
				}
				
				res += "        }\n" +
					   "\n" +
					   "        public static string languageToName(Language language)\n" +
					   "        {\n" +
					   "            return mNames[(int)language];\n" +
					   "        }\n" +
					   "\n" +
					   "        public static Language nameToLanguage(string name)\n" +
					   "        {\n" +
					   "            for (int i = 0; i < (int)Language.Count; ++i)\n" +
					   "            {\n" +
					   "                if (mNames[i] == name)\n" +
					   "                {\n" +
					   "                    return (Language)i;\n" +
					   "                }\n" +
					   "            }\n" +
					   "\n" +
					   "            return Language.Default;\n" +
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
            File.WriteAllBytes(tempLanguagesFile, cldrFileBytes);
            #endregion
        }

        private static void generatePluralsRules()
        {
            Debug.Log("Generating \"PluralsRules.cs\" file");
        }
#endif

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
                        if (File.Exists(foundDirs[i].FullName + "/Translator.cs"))
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

                string tempStringsXmlFile = Application.temporaryCachePath + "/strings.xml";

                string targetFile = rFilePath.Replace('\\', '/');

                byte[] xmlFileBytes = File.ReadAllBytes(stringsXmlFile);

                #region Check that R.cs is up to date
                #if !FORCE_CODE_GENERATION
                if (
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

                good = false; // TODO: Remove it

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
                             "    /// Container for all tokens specified in Assets/Resources/res/values/strings.xml\n" +
                             "    /// </summary>\n" +
                             "    public sealed class R\n" +
                             "    {\n" +
                             "        /// <summary>\n" +
                             "        /// Enumeration of all string tags in Assets/Resources/res/values/strings.xml\n" +
                             "        /// </summary>\n" +
                             "        public enum strings\n" +
                             "        {\n" +
                             "            /// <summary>\n" +
                             "            /// <para>Value: Hello</para>\n" +
                             "            /// </summary>\n" +
                             "            hello\n" +
                             "        }\n" +
                             "\n" +
                             "        /// <summary>\n" +
                             "        /// Enumeration of all string-array tags in Assets/Resources/res/values/strings.xml\n" +
                             "        /// </summary>\n" +
                             "        public enum array\n" +
                             "        {\n" +
                             "            Count\n" +
                             "        }\n" +
                             "\n" +
                             "        /// <summary>\n" +
                             "        /// Enumeration of all plurals tags in Assets/Resources/res/values/strings.xml\n" +
                             "        /// </summary>\n" +
                             "        public enum plurals\n" +
                             "        {\n" +
                             "           Count\n" +
                             "        }\n" +
                             "    }\n" +
                             "}\n";
                // TODO: Fill with real data

                File.WriteAllText(targetFile, res, Encoding.UTF8);

                Debug.Log("Caching strings.xml file in \"" + tempStringsXmlFile + "\"");
                File.WriteAllBytes(tempStringsXmlFile, xmlFileBytes);
                #endregion
            }
            else
            {
                Debug.LogError("Resource \"Assets/Resources/res/values/strings.xml\" not found. UnityTranslation is not available for now.");
            }
        }

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

            if (tokenNames.Contains(tokenName))
            {
                Debug.LogError("Attribute \"name\" for tag <" + tagName + "> has duplicate value \"" + tokenName + "\" in \"Assets/Resources/res/values/strings.xml\"");

                return false;
            }

            return true;
        }
    }
}
#endif
