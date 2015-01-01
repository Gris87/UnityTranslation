#if UNITY_EDITOR

// Use this definition to generate Languages.cs and PluralsRules.cs from
#define I_AM_UNITY_TRANSLATION_DEVELOPER
// TODO: GITHUB Link

// Use this definition if you want to force code generation
// #define FORCE_CODE_GENERATION



using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;



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
			Debug.Log("Generating Languages.cs file");

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

				string res = "// This file generated from CLDR/json-full/main/en/languages.json file.\n" +
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
					res += "        , \n"+
						   "        /// <summary>\n"+
						   "        /// " + languageNames[i] + ".\n"+
						   "        /// </summary>\n"+
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
                       "}\n";

				File.WriteAllText(targetFile, res);
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
			Debug.Log("Generating PluralsRules.cs file");
		}
#endif

		private static void generateStringsXml()
		{
			Debug.Log("Generating res/values/strings.xml file"); // TODO: Full path
		}

		private static void generateR()
		{
			Debug.Log("Generating R.cs file");
		}
	}
}
#endif