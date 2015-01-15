using UnityEngine;
using System.Collections.Generic;


namespace UnityTranslation
{
    namespace Internal
    {
        public static class Utils
        {
            /// <summary>
            /// Checks the name of the token.
            /// </summary>
            /// <returns><c>true</c>, if token name is correct, <c>false</c> otherwise.</returns>
            /// <param name="tokenName">Token name.</param>
            /// <param name="tagName">Tag name.</param>
            /// <param name="tokenNames">List of token names.</param>
            public static bool checkTokenName(string tokenName, string tagName, List<string> tokenNames)
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
        }
    }
}

