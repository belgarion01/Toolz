using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class Renew : EditorWindow
{
    [MenuItem("Window/Sprite + RenewImporter")]
    public static void ShowWindow()
    {
        Renew window = GetWindow<Renew>("Sprite Importer");
    }

    private string spriteFolderPath;
    private string spriteFilesPath;
    private string normalFolderPath;

    private void OnGUI()
    {
        #region Selection des dossiers
        if (spriteFolderPath != null) GUILayout.Label(Path.GetFileName(spriteFolderPath), EditorStyles.helpBox);
        spriteFolderPath = EditorUtility.OpenFolderPanel("Choisir le dossier où se trouve les sprites", "", "");
        if (normalFolderPath != null) GUILayout.Label(Path.GetFileName(normalFolderPath), EditorStyles.helpBox);
        normalFolderPath = EditorUtility.OpenFolderPanel("Choisir le dossier où se trouve les normals", "", "");
        #endregion

    }
}
