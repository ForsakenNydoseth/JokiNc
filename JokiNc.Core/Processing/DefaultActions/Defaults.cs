using System;
using System.Collections.Generic;
using System.Linq;
using JokiNc.Core.UnityCore;
using UnityEngine;

namespace JokiNc.Core.Processing.DefaultActions
{
    public static class Defaults
    {
        public static Action<LineElement> DefaultMoveAction = caller =>
        {
            var parameters = caller.FindUntil(ElementType.Function);
            foreach (var parameter in parameters)
            {
                ToolController.Instance.Move((float)parameter.Value!, parameter.Id switch
                {
                    "X" => WorldAxis.X,
                    "Y" => WorldAxis.Y,
                    "Z" => WorldAxis.Z,
                    _ => throw new Exception("No valid axis specified at 'DefaultMoveAction(caller)'!")
                }, ToolController.Instance.Options.MeasuringSystem);
                
                Debug.Log($"Executed 'Move(float, axis)' with ({parameter.Value!}), [{parameter.Id}]");
            }
        };

        public static readonly Action<LineElement> DefaultCall = caller =>
        {
            Console.WriteLine($"Default call called from caller '{caller.Content}' in context '{caller.Context.Content}'");
        };
        
        public static Dictionary<string, Action<LineElement>> DefaultMapping = new Dictionary<string, Action<LineElement>>()
        {
            {"G54", DefaultCall},
            {"TRANS", caller =>
            {
                var parameters = caller.FindUntil(ElementType.Function);
                float[] array = new float[3];
                foreach (var parameter in parameters)
                {
                    var id = parameter.Id;
                    switch (id)
                    {
                        case "X":
                        {
                            array[0] = (float)parameter.Value!;
                            break;
                        }
                        case "Y":
                        {
                            array[1] = (float)parameter.Value!;
                            break;
                        }       
                        case "Z":
                        {
                            array[2] = (float)parameter.Value!;
                            break;
                        }
                    }
                }
                ToolController.Instance.WorkingObjOptions.TransOptions = new TransOptions(array[0], array[1], array[2]);
            }},
            {"G0", DefaultMoveAction},
            {"G1", DefaultMoveAction}
        };
    }
}