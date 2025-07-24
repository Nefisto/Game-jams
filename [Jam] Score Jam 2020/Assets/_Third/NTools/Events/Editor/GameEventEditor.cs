using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameEvent))]
public class GameEventEditor : GameEventBaseEditor<GameEvent, GameEventListener>
{
    protected override GameEvent GetTarget()
        => target as GameEvent;

    protected override void Raise(GameEvent eventToRaise)
        => eventToRaise.Raise();

    // ! ISSO É UMA GAMBIARRA, SÓ RODA NO EDITOR PORTANTO NAO TEM (MTO) PROBLEMA, QUANTO TIVE MAIS TEMPO MELHORAR ISSO PFRV
    // ! UM BOM CAMINHO TALVEZ SEJA USANDO OS GUIDS DOS ASSETS, MAS NAO SEI COMO FAZER NO MOMENTO ATUAL
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (!Application.isPlaying)
            return;

        var listeners = RemoveObjectsFromSamePrefab(GetTarget().GetListeners());

        GUILayout.Label("Listeners list:", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical();

            foreach (var list in listeners)
            {
                EditorGUILayout.ObjectField(list, list.GetType(), true);

                var response = list.Response;
                for (int i = 0; i < response.GetPersistentEventCount(); i++)
                    GUILayout.Label($"   - {response.GetPersistentMethodName(i)}");
            }
        
        EditorGUILayout.EndVertical();
    }
}