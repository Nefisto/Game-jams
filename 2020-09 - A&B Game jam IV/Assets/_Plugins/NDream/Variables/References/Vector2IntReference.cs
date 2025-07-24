using UnityEngine;

namespace NDream
{
    [System.Serializable]
    public class Vector2IntReference
    {
        public bool UseConstant = true;
        public Vector2Int ConstantValue;
        public Vector2IntVariable Variable;


        #region Constructors

        public Vector2IntReference()
        { }

        public Vector2IntReference(Vector2Int value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        #endregion

        #region Properties

        public Vector2Int Value
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

        public int x
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

        public int y
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

        public static implicit operator Vector2Int(Vector2IntReference reference)
            => reference.Value;
    }
}