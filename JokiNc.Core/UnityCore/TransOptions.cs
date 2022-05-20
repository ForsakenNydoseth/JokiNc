using System;

namespace JokiNc.Core.UnityCore
{
    public class TransOptions
    {
        private float _x;
        private float _y;
        private float _z;
        public float X
        {
            get => _x;
            set
            {
                _x = value;
                OnTransChanged?.Invoke(this, null);
            }
        }
        public float Y   
        {
            get => _y;
            set
            {
                _y = value;
                OnTransChanged?.Invoke(this, null);
            }
        }
        public float Z 
        {
            get => _z;
            set
            {
                _z = value;
                OnTransChanged?.Invoke(this, null);
            }
        }

        public TransOptions(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public event EventHandler OnTransChanged;
    }
}