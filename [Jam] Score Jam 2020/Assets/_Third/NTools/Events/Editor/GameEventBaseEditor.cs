using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public abstract class GameEventBaseEditor<T, K> : Editor
    where T : ScriptableObject // Game Event
    where K : MonoBehaviour // Game Event Listene
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        T e = GetTarget();
        
        if (GUILayout.Button("Raise"))
            Raise(e);
    }

    protected abstract T GetTarget();
    protected abstract void Raise(T eventToRaise);

    protected List<K> RemoveObjectsFromSamePrefab(List<K> listeners)
    {
        var dict = new Dictionary<string, K>();

        foreach (var list in listeners)
        {
            var show = list.name.Any(c => char.IsDigit(c)) ? list.name.Substring(0, list.name.Length-3) : list.name; 
            show = show.Trim();

            if (dict.ContainsKey(show))
                continue;
            
            dict[show] = list;
        }

        return dict.Values.ToList();
    }
}