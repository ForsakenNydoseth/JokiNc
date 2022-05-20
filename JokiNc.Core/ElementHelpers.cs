using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace JokiNc.Core
{
    public static class ElementHelpers
    {
        [Pure]
        public static bool IsAlone(this LineElement element)
        {
            return (element.Last is null) && (element.Next is null);
        }
        [Pure]
        public static bool IsFirst(this LineElement element)
        {
            return element.Last is null;
        }
        [Pure]
        public static bool IsLast(this LineElement element)
        {
            return element.Next is null;
        }
        
        public static IEnumerable<LineElement> MergeIntoFunction(this LineElement element)
        {
            if (!element.IsFirst())
            {
                throw new Exception("Cannot merge a non-first element into function!");
            }

            var lineElements = element.FindAll();
            var arrElements = lineElements.Prepend(new LineElement()
            {
                Content = "G1",
                Context = element.Context,
                ElementType = ElementType.Modular,
                Id = "G1",
                Last = null,
                Next = null
            }).ToArray();
            arrElements[0].Next = arrElements[1];
            arrElements[1].Last = arrElements[0];
            
            element.Context.Elements.Add(arrElements[0].Content, arrElements[0]);

            return arrElements;
        }
        public static string MergeIntoString(this IEnumerable<LineElement> elements)
        {
            StringBuilder builder = new StringBuilder();
            elements = elements.ToArray();
            foreach (var element in (LineElement[]) elements)
            {
                builder.Append(element.Content).Append(' ');
            }

            return builder.Remove(builder.Length - 1, 1).ToString();
        }
    }
}