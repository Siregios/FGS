using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(AttackManager))]
public class AttackManagerEditor : Editor {

    private Vector3 tempVector;

    public void OnSceneGUI()
    {
        AttackManager script = target as AttackManager;
        AttackManager.Attack[] a = script.attackList;
        for(int i = 0; i < a.Length; i++)
        {
            if (a[i].debug)
            {
                for (int j = 0; j < a[i].hitBoxes.Length; j++)
                {
                    tempVector = new Vector3(a[i].hitBoxes[j].origin.x * script.transform.localScale.x, a[i].hitBoxes[j].origin.y, 0);
                    var verts = new Vector3[] {script.transform.position + tempVector + new Vector3(a[i].hitBoxes[j].size.x, a[i].hitBoxes[j].size.y, 0),
                    script.transform.position + tempVector + new Vector3(a[i].hitBoxes[j].size.x, -a[i].hitBoxes[j].size.y, 0),
                    script.transform.position + tempVector + new Vector3(-a[i].hitBoxes[j].size.x, -a[i].hitBoxes[j].size.y, 0),
                    script.transform.position + tempVector + new Vector3(-a[i].hitBoxes[j].size.x, a[i].hitBoxes[j].size.y, 0)};
                    Handles.DrawSolidRectangleWithOutline(verts, new Color(1, 0, 0, 0.2f), new Color(0, 0, 0, 1));
                }
            }
        }
    }
}
