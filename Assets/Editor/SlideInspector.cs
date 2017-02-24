using UnityEngine;
using System.Collections;
using UnityEditor;
using VRTK;

[CustomEditor(typeof(Slide_Interactable), true)]
public class SlideInspector : Editor {
    public override void OnInspectorGUI() {
        Slide_Interactable slide = (Slide_Interactable)target;

        EditorGUILayout.LabelField("Slide Properties", EditorStyles.boldLabel);

        slide.boltSpeed = EditorGUILayout.FloatField("Bolt Speed", slide.boltSpeed);
        slide.travelDistance = EditorGUILayout.FloatField("Travel Distance", slide.travelDistance);
        slide.slideStop = EditorGUILayout.FloatField("Slide Stop Distance", slide.slideStop);
        slide.unchamberDistance = EditorGUILayout.FloatField("Unchamber Distance", slide.unchamberDistance);
        slide.loadDistance = EditorGUILayout.FloatField("Load Distance", slide.loadDistance);
        slide.direction = (Slide_Interactable.SlideDir) EditorGUILayout.EnumPopup("Travel Direction", slide.direction);
        slide.gun = EditorGUILayout.ObjectField("Parent Weapon", slide.gun, typeof(Gun_Base), true) as Gun_Base;
        slide.flipeSide = EditorGUILayout.Toggle("Flip Direction", slide.flipeSide);

        EditorGUILayout.Space();
    }
}