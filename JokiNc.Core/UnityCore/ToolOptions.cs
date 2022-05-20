using System;

namespace JokiNc.Core.UnityCore
{
    public class ToolOptions
    {
        /// <summary>
        /// Measured in mm/min
        /// </summary>
        public float FeedRate;

        /// <summary>
        /// Measured in mm/s
        /// </summary>
        public float FeedRatePerSecond
        {
            get => FeedRate * 0.0167f;
        }

        public float DiagonalSpeed
        {
            get
            {
                var toPower = Math.Pow(FeedRatePerSecond, 2);
                var diagonalSpeed = (float) Math.Sqrt(2 * toPower);

                return diagonalSpeed;
            }
        }
        
        public float SpindleSpeed;
        public ToolMovementType MovementType;
        public CoordinateSystem MeasuringSystem;
        public static ToolOptions Default
        {
            get
            {
                var to = new ToolOptions();
                to.FeedRate = 200;
                to.SpindleSpeed = 1000;

                return to;
            }
        }

        private ToolOptions() {}
    }
}