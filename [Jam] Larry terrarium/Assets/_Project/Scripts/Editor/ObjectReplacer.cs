using System.Linq;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Editor
{
    public class ObjectReplacer : EditorWindow
    {
        [MenuItem("Game/Tools/ObjectReplacer")]
        private static void ShowWindow()
        {
            var window = GetWindow<ObjectReplacer>();
            window.titleContent = new GUIContent("Object Replacer");
            window.Show();
        }

        private string customName = "";
        private GameObject root;
        private GameObject newPrefab;
        private int amountToChange = -1;
        private Vector3 positionOffset = Vector3.zero;
        private Vector3 rotationOffset = Vector3.zero;
        private bool shouldDestroyOldObject = true;

        private void OnGUI()
        {
            EditorGUILayout.HelpBox("Used to substitute objects that start with a string with some object", MessageType.Info);

            customName = EditorGUILayout.TextField("Custom name:", customName);
            root = (GameObject)EditorGUILayout.ObjectField("Research center root:", root, typeof(GameObject), true);
            newPrefab = (GameObject)EditorGUILayout.ObjectField("New prefab:", newPrefab, typeof(GameObject), false);
            shouldDestroyOldObject = EditorGUILayout.Toggle("Should destroy old objects:", shouldDestroyOldObject);
            amountToChange = EditorGUILayout.IntField("Amount of objects to change: ", amountToChange);
            positionOffset = EditorGUILayout.Vector3Field("Position off set:", positionOffset);
            rotationOffset = EditorGUILayout.Vector3Field("Rotation off set:", rotationOffset);
            
            if (GUILayout.Button("Replace"))
                Replace();
        }

        private void Replace()
        {
            if (root == null || newPrefab == null)
                return;

            var chosenName = string.IsNullOrWhiteSpace(customName) 
                ? newPrefab.name : customName;
            var cloneCounter = 0;
            
            var iterator = root
                .transform
                .Cast<Transform>()
                .Where(t => (t.name.StartsWith(chosenName + " ") || t.name.StartsWith(chosenName + "(") || t.name == chosenName)) // avoid subnames - changes in Wall_01 can't perform on Wall_018 
                            // && t.GetComponent<Piece>() == null)
                .Take(amountToChange < 0 ? int.MaxValue : amountToChange)
                .ToList();
            foreach (var child in iterator)
            {
                var instance = (GameObject)PrefabUtility.InstantiatePrefab(newPrefab, child.parent);
                
                UpdateInstanceTransform(instance, child);
                UpdateInstanceName(instance, cloneCounter);
                UpdateInstanceSiblingIndex(child, instance);
                instance.SetActive(child.gameObject.activeInHierarchy);
                
                Undo.RegisterCreatedObjectUndo(instance, "Created new object");
                
                if (shouldDestroyOldObject)
                    Undo.DestroyObjectImmediate(child.gameObject);
                
                cloneCounter++;
            }
        }

        private static void UpdateInstanceName (GameObject instance, int cloneCounter)
            => instance.name += $" ({cloneCounter:00})";

        private static void UpdateInstanceSiblingIndex (Transform child, GameObject instance)
        {
            var siblingIndex = child.GetSiblingIndex();
            instance.transform.SetSiblingIndex(siblingIndex);
        }

        private void UpdateInstanceTransform (GameObject instance, Transform child)
        {
            instance.transform.position = child.position + positionOffset;
            var targetRotation = Quaternion.Euler(child.rotation.eulerAngles + rotationOffset);
            instance.transform.rotation = targetRotation;
            instance.transform.localScale = child.localScale;
        }
    }
}