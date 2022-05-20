namespace JokiNc.Core.UnityCore
{
    public class ToolOrientation
    {
        public bool X;
        public bool Y;
        public bool Z;

        public ToolOrientation()
        {
            X = true;
            Y = true;
            Z = true;
        }

        public void Invert(WorldAxis axis)
        {
            switch (axis)
            {
                case WorldAxis.X:
                {
                    X = !X;
                    break;
                }
                case WorldAxis.Y:
                {
                    Y = !Y;
                    break;
                }
                case WorldAxis.Z:
                {
                    Z = !Z;
                    break;
                }
            }
        }

        public void FaceForwards(WorldAxis axis)
        {
            switch (axis)
            {
                case WorldAxis.X:
                {
                    X = true;
                    break;
                }
                case WorldAxis.Y:
                {
                    Y = true;
                    break;
                }
                case WorldAxis.Z:
                {
                    Z = true;
                    break;
                }
            }
        }

        public void FaceBackwards(WorldAxis axis)
        {
            switch (axis)
            {
                case WorldAxis.X:
                {
                    X = false;
                    break;
                }
                case WorldAxis.Y:
                {
                    Y = false;
                    break;
                }
                case WorldAxis.Z:
                {
                    Z = false;
                    break;
                }
            }
        }
    }
}