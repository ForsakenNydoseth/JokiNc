namespace JokiNc.Core.UnityCore
{
    public static class UnitHelper
    {
        public static float ToRPM(this float radPerSec)
        {
            return radPerSec * 9.552000000000001f;
        }

        public static float ToRadiansPerSecond(this float rpm)
        {
            return rpm / 9.552000000000001f;
        }
    }
}