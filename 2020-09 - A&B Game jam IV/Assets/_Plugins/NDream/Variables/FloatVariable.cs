using UnityEngine;

namespace NDream
{
    [CreateAssetMenu(fileName = "FloatVariable", menuName = "Framework/Variables/Float")]
    public class FloatVariable : BaseVariable<float>
    {
        public void ApplyChange(float value)
            => Value += value;
    }
}