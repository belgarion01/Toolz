using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SpriteImporter : EditorWindow
{
    [MenuItem("Window/Sprite Importer")]
    public static void ShowWindow() {
        GetWindow<SpriteImporter>("Sprite Importer");
    }

    private string _path;
    private Texture2D spriteSheet;
    private Texture2D normalMap;

    private string[] filePaths;
    private List<string> Files;

    private void OnGUI()
    {
        if (GUILayout.Button("Select"))
        {
            _path = EditorUtility.OpenFolderPanel("Choose folder", "", "");
        }

        if (_path != null)
        {
            GUILayout.Label(Path.GetFileName(_path));
        }

        if (GUILayout.Button("Import"))
        {
            if (_path != null)
            {
                string folderName = Path.GetFileName(_path);
                //FileUtil.CopyFileOrDirectory(_path, "Assets/Sprites/"+Path.GetFileName(_path));
                filePaths = Directory.GetFiles(_path);
                if (filePaths.Length > 0)
                {
                    foreach (string fileP in filePaths)
                    {
                        if (Path.GetFileNameWithoutExtension(fileP).EndsWith("_spr"))
                        {
                            CreateSpriteAssetInFolder(fileP, folderName);
                            AssetDatabase.Refresh();
                            spriteSheet = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Sprites/" + folderName + "/" + Path.GetFileName(fileP), typeof(Texture2D));
                            TextureImporter importer = AssetImporter.GetAtPath("Assets/Sprites/" + folderName + "/" + Path.GetFileName(fileP)) as TextureImporter;
                            importer.textureType = TextureImporterType.Sprite;
                        }
                        if (Path.GetFileNameWithoutExtension(fileP).EndsWith("_normal"))
                        {
                            CreateNormalAssetInFolder(fileP, folderName);
                            AssetDatabase.Refresh();
                            normalMap = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Textures/" + folderName + "/" + Path.GetFileName(fileP), typeof(Texture2D));
                            TextureImporter importer = AssetImporter.GetAtPath("Assets/Textures/" + folderName + "/" + Path.GetFileName(fileP)) as TextureImporter;
                            importer.textureType = TextureImporterType.NormalMap;
                        }
                    }
                    AssetDatabase.Refresh();
                    AssetDatabase.CreateAsset(CreateNormalMaterial(), "Assets/"+folderName+"_mat.mat");
                }
                AssetDatabase.Refresh();
                
            }

        }

        if (spriteSheet)
        {
            Texture2D myTexture = AssetPreview.GetAssetPreview(spriteSheet);
            GUILayout.Label(myTexture);
            //EditorGUI.DrawPreviewTexture(new Rect(0, 0, 15f, 15f), spriteSheet);
        }
        //Debug
        /*if (filePaths.Length > 0)
        {
            string number = filePaths.Length.ToString();
            GUILayout.Label(number);
        }*/
    }

    private void CreateSpriteAssetInFolder(string filePath, string foldName) {
        if (!AssetDatabase.IsValidFolder("Assets/Sprites/" + foldName))
        {
            AssetDatabase.CreateFolder("Assets/Sprites", foldName);
        }
        FileUtil.CopyFileOrDirectory(filePath, "Assets/Sprites/" + foldName + "/" + Path.GetFileName(filePath));
        spriteSheet = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Sprites/" + foldName + "/" + Path.GetFileName(filePath), typeof(Texture2D));
    }

    private void CreateNormalAssetInFolder(string filePath, string foldName)
    {
        if (!AssetDatabase.IsValidFolder("Assets/Textures/" + foldName))
        {
            AssetDatabase.CreateFolder("Assets/Textures", foldName);
        }
        FileUtil.CopyFileOrDirectory(filePath, "Assets/Textures/" + foldName + "/" + Path.GetFileName(filePath));
        normalMap = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Textures/" + foldName + "/" + Path.GetFileName(filePath), typeof(Texture2D));
    }

    private Material CreateNormalMaterial() {
        Material mat = new Material(Shader.Find("Legacy Shaders/Bumped Diffuse"));
        mat.SetTexture("_MainTex", spriteSheet);
        mat.SetTexture("_BumpMap", normalMap);
        return mat;
    }
}
