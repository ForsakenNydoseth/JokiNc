using System.Collections.Generic;

namespace JokiNc.Core
{
    internal static class Constants
    {
        public static char[] Separators =
        {
            ' '
        };

        public static Dictionary<string, char[]> Functions = new Dictionary<string, char[]>()
        {
            {
                "ATRANS", new char[]
                {
                    'X', 'Y', 'Z'
                }
            },
            {
                "TRANS", new char[]
                {
                    'X', 'Y', 'Z'
                }
            },
            {
                "G54", new char[0]
            },
            {
                "G0", new char[]
                {
                    'X', 'Y', 'Z'
                }
            },
            {
                "G1", new char[]
                {
                    'X', 'Y', 'Z'
                }
            },
            {
                "G2", new char[]
                {
                    'X', 'Y', 'I', 'J'
                }
            },
            {
                "G3", new char[]
                {
                    'X', 'Y', 'I', 'J'
                }
            }
        };

        public static string[] Modulars =
        {
            "F",
            "S"
        };

        public static char[] Parameters =
        {
            'X',
            'Y',
            'Z',
            'I',
            'J'
        };

        public static string[] UnMappedIds = new[]
        {
            "ATRANS",
            "TRANS",
            "G0",
            "G1",
            "G2",
            "G3",
            "X",
            "Y",
            "Z",
            "I",
            "J",
            "S",
            "F"
        };
        public static Dictionary<string, ElementType> MappedIds = new Dictionary<string, ElementType>()
        {
            {"ATRANS", ElementType.Function},
            {"TRANS", ElementType.Function},
            {"G0", ElementType.Modular},
            {"G1", ElementType.Modular},
            {"G2", ElementType.Function},
            {"G3", ElementType.Function},
            {"G54", ElementType.Function},
            {"X", ElementType.Parameter},
            {"Y", ElementType.Parameter},
            {"Z", ElementType.Parameter},
            {"I", ElementType.Parameter},
            {"J", ElementType.Parameter},
            {"S", ElementType.Function},
            {"F", ElementType.Function}
        };
    }
}