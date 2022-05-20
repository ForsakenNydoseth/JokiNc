using System;

namespace JokiNc.Core.UnityCore
{
    public enum ToolMovementType
    {
        /// <summary>
        /// Using the differential of two points in a 3D space, we increment the object's transform position until it reaches the destination 3D point.
        /// This is rather slow, but accomplishes the job easily on the code writer's side.
        /// </summary>
        [Obsolete] Legacy,
        /// <summary>
        /// Using the distance between two points in a 3D space, we apply a Vector3 force behind the object (or in front if the force is negative, but this happens automatically by Unity)
        /// in a velocity adding manner so the force applied is equal to the velocity gained on the object.
        /// </summary>
        VelocityBased,
        /// <summary>
        /// Using the distance between two points in a 3D space, we apply a Vector3 force behind the object (or in front if the force is negative, but this happens automatically by Unity)
        /// in a force based manner, meaning the velocity gained on the object is equal to the formula: 'V = sqrt((0.5 * mass * force) / distance)'
        /// </summary>
        [Obsolete] ForceBased
        
    }
}