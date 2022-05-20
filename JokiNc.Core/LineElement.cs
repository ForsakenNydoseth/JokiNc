using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using JokiNc.Core.Processing.DefaultActions;
using UnityEngine;

namespace JokiNc.Core
{
    public class LineElement
    {
        public string Content;
        public string Id;
        public double? Value;

        public Line Context;
        public LineElement Last;
        public LineElement Next;

        public ElementType ElementType;

        internal LineElement()
        {
        }
        
        internal LineElement(string content)
        {
            Content = content;
            FillId();
            EnsureIsCorrectType();
        }

        private void FillId()
        {
            StringBuilder builder = new StringBuilder();
            var index = 0;
            for (int i = 0; i < Content.Length; i++)
            {
                var current = Content[i];
                if (char.IsLetter(current))
                {
                    builder.Append(current);
                }
                else
                {
                    Id = builder.ToString();
                    builder.Clear();
                    index = i;

                    break;
                }
            }

            if (double.TryParse(Content.Substring(index), out double val))
            {
                Value = val;
            }
        }

        public void EnsureIsCorrectType()
        {
            if (Constants.MappedIds.ContainsKey(Content))
            {
                var elementType = Constants.MappedIds[Content];
                ElementType = elementType;
            }
            else if (Id != null && Constants.MappedIds.ContainsKey(Id))
            {
                var elementType = Constants.MappedIds[Id];
                ElementType = elementType;
            }
            else
            {
                throw new ArgumentException(
                    "LineElement's Id value does not refer to any Id in the 'Constants.MappedIds' dictionary.");
            }

            FillId();
        }

        public bool TryRun()
        {
            try
            {
                var action = Defaults.DefaultMapping[Content];
                action(this);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                return false;
            }
        }
        /// <summary>
        /// Finds the first element in the current context (<see cref="Line"/>) that matches the name provided.
        /// If the <see cref="name"/> provided does not exist within all of the available set of elements, the function will return null.
        /// </summary>
        /// <param name="name">The name of the element defined in <see cref="Constants"/> class under the variable 'MappedIds'.</param>
        /// <returns>The first element found in the current context (<see cref="Line"/>) that matches the <see cref="name"/> provided.</returns>
        public LineElement FindByName(string name)
        {
            name = name.ToUpperInvariant();
            if (!Constants.UnMappedIds.Contains(name))
            {
                return null;
            }

            return this.FindFirst().FindMany(100).FirstOrDefault(x => x.Content.Contains(name));
        }

        /// <summary>
        /// Finds the first element in the current context (<see cref="Line"/>).
        /// </summary>
        /// <returns>The first element in the current context.</returns>
        public LineElement FindFirst()
        {
            if (Last == null)
            {
                return this;
            }

            LineElement current = Last;
            while (current?.Last != null)
            {
                current = current.Last;
            }

            return current;
        }

        /// <summary>
        /// Finds all elements that are situated between the current element and the next available element of <see cref="ElementType"/>.
        /// Not including either one in the output.
        /// </summary>
        /// <param name="type">The type of the next element to search for.</param>
        /// <returns>An array of items that contains all elements situated between the element this function is invoked on, and the next element of type <see cref="ElementType"/>.</returns>
        public LineElement[] FindUntil(ElementType type)
        {
            if (Next.ElementType == type)
            {
                return new LineElement[0];
            }

            List<LineElement> elements = new List<LineElement>();
            elements.Add(Next);
            var current = elements.Last();
            while (current.Next != null)
            {
                var next = current.Next;
                if (next != null && next.ElementType == type)
                {
                    break;
                }

                elements.Add(next);
                current = next;
            }

            return elements.ToArray();
        }

        public IEnumerable<LineElement> FindAll()
        {
            LineElement current = this;

            yield return current;

            while (current.Next != null)
            {
                var next = current.Next;

                yield return next;
                current = next;
            }
        }
        /// <summary>
        /// Finds num elements in front of the current element, if the leading (next) element is null at any point, the function will return immediately.
        /// </summary>
        /// <param name="num">The number of elements to try to find.</param>
        /// <returns>A collection of num elements found in front of the current element.</returns>
        public IEnumerable<LineElement> FindMany(int num)
        {
            LineElement current = Next;
            for (int i = 0; i < num; i++)
            {
                if (current != null)
                {
                    yield return current;
                    current = current.Next;
                }
                else
                {
                    break;
                }
            } 
        }
        /// <summary>
        /// Finds an element of the specified <see cref="ElementType"/> in the current context (<see cref="Line"/>).
        /// </summary>
        /// <param name="type">The type of element to search for.</param>
        /// <param name="forwards">Whether to go forwards or backwards in the search.</param>
        /// <returns>The found element if it exists, or null otherwise.</returns>
        public LineElement Find(ElementType type, bool forwards = true)
        {
            return Find(this, type, forwards);
        }
        public static LineElement Find(LineElement element, ElementType type, bool forwards = true)
        {
            if (element is null)
            {
                return null;
            }
            
            if (element.ElementType == type)
            {
                return element;
            }

            return forwards ? element.Next.Find(type, true) : element.Last.Find(type, false);
        }
        public static LineElement[] FromStringArray(string[] strings)
        {
            var arr = new LineElement[strings.Length];
            var indexNext = -1;
            for (int i = strings.Length - 1; i >= 0; i--)
            {
                if (string.IsNullOrEmpty(strings[i]))
                {
                    continue;
                }
                var element = new LineElement(strings[i]);
                arr[i] = element;
                if (indexNext != -1)
                {
                    element.Next = arr[indexNext];
                }
                indexNext = i;
            }

            //arr = arr.Where(x => x != null).ToArray();
            for (int i = 0; i < arr.Length; i++)
            {
                var element = arr[i];
                var index = i - 1;
                if (index < arr.Length && index >= 0)
                {
                    element.Last = arr[index];
                }
            }

            var line = new Line(arr);

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i].Context = line;
            }
            
            return arr;
        }

        public static LineElement[] FromString(string s)
        {
            if (s.Contains("  "))
            {
                s = s.Replace("  ", string.Empty);
            }
            return FromStringArray(s.Split(' '));
        }

        public override string ToString()
        {
            return Content;
        }
    }
}