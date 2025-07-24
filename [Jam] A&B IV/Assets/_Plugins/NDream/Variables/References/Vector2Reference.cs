using UnityEngine;

namespace NDream
{
    [System.Serializable]
    public class Vector2Reference
    {
        public bool UseConstant = true;
        public Vector2 ConstantValue;
        public Vector2Variable Variable;


        #region Constructors

        public Vector2Reference()
        { }

        public Vector2Reference(Vector2 value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        #endregion

        #region Properties

        public Vector2 Value
        {
            get => UseConstant ? ConstantValue : Variable.Value;
            set
            {
                if (UseConstant)
                    ConstantValue = value;
                else
                    Variable.Value = value;
            }
        }

        public float x
        {
            get => Value.x;
            set
            {
                if (UseConstant)
                    ConstantValue.x = value;
                else
                    Variable.x = value;
            }
        }

        public float y
        {
            get => Value.y;
            set
            {
                if (UseConstant)
                    ConstantValue.y = value;
                else
                    Variable.y = value;
            }
        }

        #endregion

        public static implicit operator Vector2(Vector2Reference reference)
            => reference.Value;
    }
}