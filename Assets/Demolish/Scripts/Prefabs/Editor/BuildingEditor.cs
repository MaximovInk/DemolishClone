using UnityEditor;
using UnityEngine;

namespace MaximovInk
{
    [CustomEditor(typeof(Building))]
    public class BuildingEditor : Editor
    {
        private Building _target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (_target == null)
            {
                _target = target as Building;
            }

            if (GUILayout.Button("Make fractured from source"))
            {
                var go = new GameObject($"[Fractured]{_target.gameObject.name}");

                _target.gameObject.name = $"[Source]{_target.gameObject.name}";

                var fractured = go.AddComponent<FracturedObject>();
                fractured.SourceObject = _target.gameObject;

                go.transform.SetPositionAndRotation(_target.transform.position,_target.transform.rotation);
                Selection.activeGameObject = go;

                //_target.transform.SetParent(go.transform);
                _target.gameObject.SetActive(false);
                
            }
        }
    }
}
