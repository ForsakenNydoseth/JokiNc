using System.Collections.Generic;
using System.Linq;

namespace JokiNc.Core
{
    public sealed class Line
    {
        public Dictionary<string, LineElement> Elements;
        public string Content;
        public Line(string s)
        {
            Content = s;
            Elements = LineElement.FromStringArray(s.Split(' ')).ToDictionary(x => x.Content, y => y);
        }

        public Line(LineElement[] array)
        {
            Content = string.Join(" ", array.Select(x => x.Content));
            Elements = array.ToDictionary(x => x.ElementType == ElementType.Parameter ? x.Id : x.Content, y => y);
        }
    }
}