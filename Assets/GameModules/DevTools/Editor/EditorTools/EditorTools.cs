using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TFPlay.EditorTools
{
    public static class EditorTools
    {
        [MenuItem("Tools/Editor Tools/Set Selected Position &z")]
        public static void SetSelectedPosition()
        {
            if (Selection.activeObject != null)
            {
                (Selection.activeObject as GameObject).transform.localPosition = Vector3.zero;
                (Selection.activeObject as GameObject).transform.localRotation = Quaternion.identity;
            }
        }

        [MenuItem("Tools/Editor Tools/TogleGameObjectActive &x")]
        public static void TogleGameObjectActive()
        {
            GameObject go = Selection.activeObject as GameObject;
            if (go != null)
            {
                go.SetActive(!go.activeSelf);
            }
        }

        [MenuItem("CONTEXT/Transform/Reset And Save Child Position")]
        public static void ResetAndSaveChildPos(MenuCommand command)
        {
            const string ADD_KEY_FOR_UNDO = "ResetAndSaveChildPos_Parrent";

            Transform parrent = (Transform)command.context;
            Undo.RecordObject(parrent, ADD_KEY_FOR_UNDO);

            List<Transform> allChilds = new List<Transform>();
            for (int i = 0; i < parrent.childCount; i++)
                allChilds.Add(parrent.GetChild(i));

            foreach (var item in allChilds)
            {
                Undo.RecordObject(item, item.GetInstanceID() + ADD_KEY_FOR_UNDO);
                item.SetParent(null);
            }

            parrent.localPosition = Vector3.zero;
            parrent.localRotation = Quaternion.Euler(Vector3.zero);
            parrent.localScale = Vector3.one;

            foreach (var item in allChilds)
                item.SetParent(parrent);
        }
    }
}