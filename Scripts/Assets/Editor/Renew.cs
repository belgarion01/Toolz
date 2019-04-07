using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System;

public class Renew : EditorWindow
{
    [MenuItem("Window/Sprite + RenewImporter")]
    public static void ShowWindow()
    {
        Renew window = GetWindow<Renew>("Sprite Importer");
    }

    private string spriteFolderPath;
    private string[] spriteFilesPath;

    private string normalFolderPath;
    private string[] normalFilesPath;

    private Texture2D[] sprites;
    private Texture2D[] normals;


    bool createNewFolder = false;

    int numberOfLoadedFiles = 0;
    int numberOfUnloadedFiles = 0;

    List<PreMaterialo> preMaterials = new List<PreMaterialo>();

    string filesDirectory;

    bool loadFinished = false;

    private void OnGUI()
    {
        string projectMaterialsFolderPath = AssetDatabase.IsValidFolder("Assets/Materials") ? "Assets/Materials" : "";
        string projectSpritesFolderPath = AssetDatabase.IsValidFolder("Assets/Sprites") ? "Assets/Sprites" : "";
        string projectNormalsFolderPath = AssetDatabase.IsValidFolder("Assets/Textures") ? "Assets/Textures" : "";

        #region Selection des dossiers
        if (spriteFolderPath != null) GUILayout.Label(Path.GetFileName(spriteFolderPath), EditorStyles.helpBox);
        if (GUILayout.Button("Importer sprites")){
            spriteFolderPath = EditorUtility.OpenFolderPanel("Choisir le dossier où se trouve les sprites", projectSpritesFolderPath, "");
        }

        if (normalFolderPath != null) GUILayout.Label(Path.GetFileName(normalFolderPath), EditorStyles.helpBox);
        if (GUILayout.Button("Importer normals"))
        {
            normalFolderPath = EditorUtility.OpenFolderPanel("Choisir le dossier où se trouve les normals", projectNormalsFolderPath, "");
        }
        if ((spriteFolderPath == null || spriteFolderPath == "") || (normalFolderPath == null || normalFolderPath == "")) return;
        #endregion

        if (GUILayout.Button("Charger les fichier"))
        {
            spriteFilesPath = Directory.GetFiles(spriteFolderPath, "*.png");
            string[] spriteRelativeFilesPath = new string[spriteFilesPath.Length];
            for (int i = 0; i < spriteFilesPath.Length; i++) {
                string relativePath = "Assets"+spriteFilesPath[i].Substring(Application.dataPath.Length);
                spriteRelativeFilesPath[i] = relativePath;
            }

            normalFilesPath = Directory.GetFiles(normalFolderPath, "*.png");
            string[] normalRelativeFilesPath = new string[normalFilesPath.Length];
            for (int i = 0; i < normalFilesPath.Length; i++)
            {
                string relativePath = "Assets" + normalFilesPath[i].Substring(Application.dataPath.Length);
                normalRelativeFilesPath[i] = relativePath;
            }


            for (int i = 0; i < spriteFilesPath.Length; i++)
            {
                string crunchedFileName = Path.GetFileNameWithoutExtension(spriteFilesPath[i]);
                crunchedFileName = crunchedFileName.Substring(4, Path.GetFileNameWithoutExtension(spriteFilesPath[i]).Length - 4);
                PreMaterialo premat = CreatePreMaterial(crunchedFileName);
                premat.sprite = (Texture2D)AssetDatabase.LoadAssetAtPath(spriteRelativeFilesPath[i], typeof(Texture2D));
                preMaterials.Add(premat);
            }
            for (int i = 0;i<normalFilesPath.Length;i++) {
                string crunchedFileName = Path.GetFileNameWithoutExtension(normalFilesPath[i]);
                crunchedFileName = crunchedFileName.Substring(7, Path.GetFileNameWithoutExtension(normalFilesPath[i]).Length - 7);
                
                bool gotALover = false;
                foreach (PreMaterialo premat in preMaterials) {
                    if (premat.name == crunchedFileName)
                    {
                        premat.normal = (Texture2D)AssetDatabase.LoadAssetAtPath(normalRelativeFilesPath[i], typeof(Texture2D));
                        gotALover = true;
                    }
                }
                if (!gotALover)
                {
                    numberOfUnloadedFiles++;
                }
            }
            loadFinished = true;
        }
        if (!loadFinished) return;
        GUILayout.Label("Création de " + preMaterials.Count + " fichiers");
        GUILayout.Label("Nombre de fichiers échoués : " + numberOfUnloadedFiles);
        if (GUILayout.Button("Créer les matériaux")) {
            filesDirectory = EditorUtility.OpenFolderPanel("Choisir le dossier où placer les matériaux", projectMaterialsFolderPath, "");
            
        }
        if (filesDirectory != null && filesDirectory != "")
        {
            string filesDirectoryRelativePath = "Assets" + filesDirectory.Substring(Application.dataPath.Length);
            foreach (PreMaterialo premat in preMaterials)
            {
                Debug.Log(premat.normal);
                if (premat.name != null && premat.sprite != null && premat.normal != null)
                {
                    Debug.Log("C'est passé");
                    AssetDatabase.CreateAsset(CreateNormalMaterial(premat), filesDirectoryRelativePath + "/" + premat.name + "_mat.mat");                  
                }
            }
            AssetDatabase.Refresh();
            Reset();
        }
    }

    private void Reset()
    {
        spriteFolderPath = null;
        normalFolderPath = null;
        spriteFilesPath = null;
        normalFilesPath = null;
        filesDirectory = null;
        numberOfLoadedFiles = 0;
        numberOfUnloadedFiles = 0;
        loadFinished = false;
        preMaterials.Clear();
    }

    private Material CreateNormalMaterial(PreMaterialo premat)
    {
        Material mat = new Material(Shader.Find("Legacy Shaders/Transparent/Bumped Diffuse"));
        mat.SetTexture("_MainTex", premat.sprite);
        mat.SetTexture("_BumpMap", premat.normal);
        return mat;
    }

    private PreMaterialo CreatePreMaterial(string name)
    {
        PreMaterialo mat = new PreMaterialo();
        mat.name = name;
        return mat;
    }

    private PreMaterialo GetPreMaterial(string assetName)
    {
        if (preMaterials.Count > 0)
        {
            foreach (PreMaterialo premat in preMaterials)
            {
                if (premat.name == assetName)
                {
                    return premat;
                }
            }
        }
        return null;
    }
}

public class PreMaterialo
{
    public string name;
    public Texture2D sprite;
    public Texture2D normal;
}
