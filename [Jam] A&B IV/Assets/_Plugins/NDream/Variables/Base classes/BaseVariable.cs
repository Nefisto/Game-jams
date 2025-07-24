using UnityEngine;

namespace NDream
{
    public abstract class BaseVariable<T> : ScriptableObject
    {
#pragma warning disable 0414

        [SerializeField]
        [Multiline]
        private string DeveloperDescription = "";

#pragma warning restore 0414

        [Header("Will have an default value?")]
        public bool haveDefaultValue = false;

        [Header("Default/Current value")]
        [SerializeField]
        private T value;

        [Header("Debug")]
        [SerializeField]
        private T runTimeValue;

        public T Value
        {
            get => haveDefaultValue ? runTimeValue : value;
            set
            {
                if (haveDefaultValue)
                    runTimeValue = value;
                else
                    this.value = value;
            }
        }

        public void Reset()
            => Value = default(T);

        private void OnEnable()
        {
            if (haveDefaultValue)
                runTimeValue = value;
        }

        private void OnDisable()
        {
            if (haveDefaultValue)
                runTimeValue = value;
        }
    }
}