using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

namespace Majoras
{
    public class SpriteImporterV2 : EditorWindow
    {
        [MenuItem("Window/Sprite Importer V2")]
        public static void ShowWindow()
        {
            GetWindow<SpriteImporterV2>("Sprite Importer V2");
        }

        string folderPath;
        string[] filesPath;
        List<PreMaterial> preMaterials = new List<PreMaterial>();

        private void OnGUI()
        {

            #region Selection du dossier
            if (folderPath != null) GUILayout.Label(Path.GetFileName(folderPath));

            if (GUILayout.Button("Select"))
            {
                folderPath = EditorUtility.OpenFolderPanel("Choose the folder to import", "", "");
            }

            if (folderPath == null) return;
            #endregion

            if (GUILayout.Button("Import"))
            {
                string folderName = Path.GetFileName(folderPath);
                filesPath = Directory.GetFiles(folderPath);

                foreach (string filePath in filesPath)
                {
                    //if (Path.GetFileNameWithoutExtension(filePath).EndsWith("_spr")) //LAAA
                    if (Path.GetFileNameWithoutExtension(filePath).StartsWith("spr_"))
                    {
                        string fileName = Path.GetFileName(filePath);
                        string crunchedFileName = Path.GetFileNameWithoutExtension(filePath);
                        //crunchedFileName = crunchedFileName.Remove(Path.GetFileNameWithoutExtension(filePath).Length-4); //LAAAA
                        crunchedFileName = crunchedFileName.Substring(4, Path.GetFileNameWithoutExtension(filePath).Length - 4);
                        Debug.Log(crunchedFileName);
                        //Créer le dossier Sprites si il existe pas
                        if (!AssetDatabase.IsValidFolder("Assets/Sprites")) AssetDatabase.CreateFolder("Assets", "Sprites");
                        AssetDatabase.Refresh();

                        //Copier / Coller le sprite dans Sprites/ LeNomDuDossier
                        if (!AssetDatabase.IsValidFolder("Assets/Sprites/" + folderName)) AssetDatabase.CreateFolder("Assets/Sprites", folderName);
                        FileUtil.CopyFileOrDirectory(filePath, "Assets/Sprites/" + Path.GetFileName(folderPath) + "/" + Path.GetFileName(filePath));
                        AssetDatabase.Refresh();
                        //Vérifie si un prematerial a déjà ce nom. Si oui, donne le sprite. Si non, créer un matérial avec le nom et le sprite puis l'ajoute à la liste.
                        if (GetPreMaterial(crunchedFileName) != null)
                        {
                            GetPreMaterial(crunchedFileName).sprite = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Sprites/" + folderName + "/" + Path.GetFileName(filePath), typeof(Texture2D));
                        }
                        else
                        {
                            PreMaterial mat = CreatePreMaterial(crunchedFileName);
                            mat.sprite = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Sprites/" + folderName + "/" + Path.GetFileName(filePath), typeof(Texture2D));
                            preMaterials.Add(mat);
                        }
                        AssetDatabase.Refresh();
                        TextureImporter importer = AssetImporter.GetAtPath("Assets/Sprites/" + folderName + "/" + Path.GetFileName(filePath)) as TextureImporter;
                        importer.textureType = TextureImporterType.Sprite;
                    }
                    //else if (Path.GetFileNameWithoutExtension(filePath).EndsWith("_normal")) //LAAAAA
                    else if (Path.GetFileNameWithoutExtension(filePath).StartsWith("normal_"))
                    {
                        string fileName = Path.GetFileName(filePath);
                        string crunchedFileName = Path.GetFileNameWithoutExtension(filePath);
                        //crunchedFileName = crunchedFileName.Remove(Path.GetFileNameWithoutExtension(filePath).Length - 7); //LAAAAAA
                        crunchedFileName = crunchedFileName.Substring(7, Path.GetFileNameWithoutExtension(filePath).Length - 7);
                        Debug.Log(crunchedFileName);
                        //Créer le dossier Sprites si il existe pas
                        if (!AssetDatabase.IsValidFolder("Assets/Textures")) AssetDatabase.CreateFolder("Assets", "Textures");
                        AssetDatabase.Refresh();

                        //Copier / Coller le sprite dans Sprites/ SonNom
                        if (!AssetDatabase.IsValidFolder("Assets/Textures/" + folderName)) AssetDatabase.CreateFolder("Assets/Textures", folderName);
                        FileUtil.CopyFileOrDirectory(filePath, "Assets/Textures/" + Path.GetFileName(folderPath) + "/" + Path.GetFileName(filePath));
                        AssetDatabase.Refresh();
                        //Vérifie si un prematerial a déjà ce nom. Si oui, donne le sprite. Si non, créer un matérial avec le nom et le sprite puis l'ajoute à la liste.

                        if (GetPreMaterial(crunchedFileName) != null)
                        {
                            GetPreMaterial(crunchedFileName).normal = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Textures/" + folderName + "/" + Path.GetFileName(filePath), typeof(Texture2D));
                        }
                        else
                        {
                            PreMaterial mat = CreatePreMaterial(crunchedFileName);
                            mat.normal = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Textures/" + folderName + "/" + Path.GetFileName(filePath), typeof(Texture2D));
                            preMaterials.Add(mat);
                        }
                        AssetDatabase.Refresh();
                        TextureImporter importer = AssetImporter.GetAtPath("Assets/Textures/" + folderName + "/" + Path.GetFileName(filePath)) as TextureImporter;
                        importer.textureType = TextureImporterType.NormalMap;

                    }
                }
                if (!AssetDatabase.IsValidFolder("Assets/Materials")) AssetDatabase.CreateFolder("Assets", "Materials");
                foreach (PreMaterial prematerial in preMaterials)
                {
                    if (prematerial.sprite != null && prematerial.normal != null && prematerial.name != null)
                    {
                        AssetDatabase.CreateAsset(CreateNormalMaterial(prematerial), "Assets/Materials/" + prematerial.name + "_mat.mat");
                    }
                }
                Reset();
            }
        }

        private void Reset()
        {
            folderPath = null;
            filesPath = null;
            preMaterials.Clear();
        }

        private Material CreateNormalMaterial(PreMaterial premat)
        {
            Material mat = new Material(Shader.Find("Legacy Shaders/Transparent/Bumped Diffuse"));
            mat.SetTexture("_MainTex", premat.sprite);
            mat.SetTexture("_BumpMap", premat.normal);
            return mat;
        }

        private PreMaterial CreatePreMaterial(string name)
        {
            PreMaterial mat = new PreMaterial();
            mat.name = name;
            return mat;
        }

        private PreMaterial GetPreMaterial(string assetName)
        {
            if (preMaterials.Count > 0)
            {
                foreach (PreMaterial premat in preMaterials)
                {
                    if (premat.name == assetName)
                    {
                        return premat;
                    }
                }
            }
            return null;
        }

        private void AddSpriteToPreMaterial(Texture2D sprite)
        {

        }
    }

    public class PreMaterial
    {
        public string name;
        public Texture2D sprite;
        public Texture2D normal;
    }
}
