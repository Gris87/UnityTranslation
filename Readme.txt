UnityTranslation
===========
UnityTranslation is a localization system that looks the same as Android localization system.

Main advantages:
- Based on CLDR 26.0.1
- Supporting 610 languages
- Supporting plurals
- Language hierarchy. Language en contains common translations for en-US and for en-GB while en-US and en-GB has some language specific translations
- Dynamically loadable/unloadable sections with tokens
- Code generator that provide source code to user with the best performance

Demo:              http://gris.ucoz.ru/UnityModules/UnityTranslate/Web/UnityTranslation.html
Unity Asset Store: http://u3d.as/bex

Description:

All translatable tokens should be provided in Assets/Resources/res/values/strings.xml file
Please follow link below for xml file format description:
http://developer.android.com/guide/topics/resources/string-resource.html

If you want to add new language in your application just create the same xml file in Assets/Resources/res/values-CODE,
where CODE is a language code with 2-3 letters length.
Please check LanguageCode class in Language.cs file to get list of language codes.

Run your application to recreate AvailableLanguages.file with the set of specified languages in Assets/Resources/res folder.

!!!WARNING!!!
Please note that code generator works only when you are using UnityTranslation somewhere.

All set of tokens are stored in R.cs file.



Example:
    using UnityTranslation;

    string hello = Translator.getString(R.strings.hello_world);

It is also possible to use string format arguments in localization.

    using UnityTranslation;

    string hello = Translator.getString(R.strings.hello_my_dear, "friend"); // Where R.strings.hello_my_dear = "Hello, my dear {0}!"



Plurals:

Some languages has specific rules for localizable string according to provided quantity.
To get more information about plurals please follow link below:
http://developer.android.com/guide/topics/resources/string-resource.html#Plurals



Example:
    using UnityTranslation;
    
    int amountOfDogs = 3;

    string dogs = Translator.getQuantityString(R.plurals.dogs, amountOfDogs, amountOfDogs);



Sections:

Section is a set of tokens that might be loaded/unloaded in Runtime. You have to create xml file with some name in Assets/Resources/res/value folder.
Code generator will try to convert xml file name to section ID.
Please note that xml file name should be different from strings.xml.
To use section tokens just provide token ID specified in R.sections.SECTION_NAME.



Example:
    using UnityTranslation;
    
    string[] fruits = Translator.getStringArray(R.sections.MySection.array.fruits);



It will load section tokens automatically if it's not loaded yet.
If you want to load/unload section manually you have to call Translator.LoadSection and Translator.UnloadSection methods.



Example:
    using UnityTranslation;
    
    Translator.LoadSection(R.sections.SectionID.MySection);
    Translator.UnloadSection(R.sections.SectionID.MySection);
    


Language changed listeners:

You can provide listener to handle language changed event in your application.
Please do not forget to remove listener when it is not needed.



Example:
    using UnityTranslation;
    
    void Start()
    {
        mText = GetComponent<Text>();

        Translator.addLanguageChangedListener(OnLanguageChanged);
    }

    void OnDestroy()
    {
        Translator.removeLanguageChangedListener(OnLanguageChanged);
    }

    public void OnLanguageChanged()
    {
        mText.text = Translator.getString(id);
    }
    
!!!WARNING!!!
When you call addLanguageChangedListener it will automatically invoke listener method!



Please feal free to contact with me if you meet some errors.
e-mail: gris87@yandex.ru



Links:

Site:              http://gris.ucoz.ru/index/unitytranslation/0-15
Unity Asset Store: http://u3d.as/bex
GitHub:            https://github.com/Gris87/UnityTranslation

See also:

Strings.xml file format: http://developer.android.com/guide/topics/resources/string-resource.html
Plurals:                 http://developer.android.com/guide/topics/resources/string-resource.html#Plurals