using UnityEngine;
using System.Collections;
using UnityEditor;
using VRTK;

[CustomEditor(typeof(Gun_Base), true)]
public class GunInspector : Editor {
    public override void OnInspectorGUI() {
        Gun_Base gun = (Gun_Base)target;

        EditorGUILayout.LabelField("Gun Parts", EditorStyles.boldLabel);
        gun.slide = EditorGUILayout.ObjectField("Slide", gun.slide, typeof(Slide_Interactable), true) as Slide_Interactable;
        gun.magWell = EditorGUILayout.ObjectField("Magazine Well", gun.magWell, typeof(VRTK_SnapDropZone), true) as VRTK_SnapDropZone;
        gun.trigger = EditorGUILayout.ObjectField("Trigger", gun.trigger, typeof(Transform), true) as Transform;
        gun.muzzle = EditorGUILayout.ObjectField("Muzzle", gun.muzzle, typeof(GameObject), true) as GameObject;
        gun.ejectionPort = EditorGUILayout.ObjectField("Ejection Port", gun.ejectionPort, typeof(GameObject), true) as GameObject;
        gun.chamberedBullet = EditorGUILayout.ObjectField("Chambered Round", gun.chamberedBullet, typeof(GameObject), true) as GameObject;
        gun.bulletPrefab = EditorGUILayout.ObjectField("Bullet Prefab", gun.bulletPrefab, typeof(GameObject), true) as GameObject;
        gun.shellPrefab = EditorGUILayout.ObjectField("Shell Prefab", gun.shellPrefab, typeof(GameObject), true) as GameObject;

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Gun Properties", EditorStyles.boldLabel);
        gun.fullAuto = EditorGUILayout.Toggle("Full auto", gun.fullAuto);
        gun.magRelease = EditorGUILayout.Toggle("Magazine Release", gun.magRelease);
        gun.slideRelease = EditorGUILayout.Toggle("Slide Release", gun.slideRelease);
        gun.bulletSpeed = EditorGUILayout.FloatField("Bullet velocity", gun.bulletSpeed);
    }
}