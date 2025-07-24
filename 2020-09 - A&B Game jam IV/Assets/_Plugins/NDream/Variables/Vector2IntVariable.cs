using UnityEngine;

namespace NDream
{
    [CreateAssetMenu(fileName = "Vector2Variable", menuName = "Framework/Variables/Vector2Int")]
    public class Vector2IntVariable : BaseVariable<Vector2Int>
    {
        public int x
        {
            get => Value.x;
            set => this.x = value;
        }

        public int y
        {
            get => Value.y;
            set => this.y = value;
        }

        public void ApplyChange(Vector2Int value)
            => Value += value;
    }
}