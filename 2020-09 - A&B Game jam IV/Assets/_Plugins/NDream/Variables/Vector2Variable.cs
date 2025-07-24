#pragma warning disable 0414

using UnityEngine;

namespace NDream
{
    [CreateAssetMenu(fileName = "Vector2", menuName = "Framework/Variables/Vector2")]
    public class Vector2Variable : BaseVariable<Vector2>
    {
        public float x
        {
            get => Value.x;
            set => this.x = value;
        }

        public float y
        {
            get => Value.y;
            set => this.y = value;
        }
        
        public void ApplyChange(Vector2 value)
            => Value += value;
    }
}