using UnityEngine;
using UnityEngine.Events;



namespace UnityTranslation
{
	namespace Internal
	{
		/// <summary>
		/// UnityTranslation internal Translator class that has methods for getting localized strings.
		/// Translator provide localization in the same way as in Android localization system.
		/// </summary>
		/// <seealso cref="http://developer.android.com/guide/topics/resources/string-resource.html"/>
		public static class Translator
		{
			private static Language   mLanguage              = Language.Default;
			private static UnityEvent mLanguageChangedAction = null;
			
			
			
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
					return mLanguage;
				}
				
				set
				{
					if (mLanguage != value)
					{
						// TODO: TryGetValue
						if (AvailableLanguages.list.ContainsKey(value))
						{
							mLanguage = value;
							
							// TODO: Reload custom tokens
							
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
				
				// TODO: Load default tokens
			}
			
			/// <summary>
			/// Adds specified language changed listener and invoke it.
			/// </summary>
			/// <param name="listener">Language changed listener.</param>
			public static void addLanguageChangedListener(UnityAction listener)
			{
				mLanguageChangedAction.AddListener(listener);
				listener.Invoke();
			}
			
			/// <summary>
			/// Removes specified language changed listener.
			/// </summary>
			/// <param name="listener">Language changed listener.</param>
			public static void removeLanguageChangedListener(UnityAction listener)
			{
				mLanguageChangedAction.RemoveListener(listener);
			}
		}
	}
}
