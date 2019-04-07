using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

namespace Majoras
{
    public class SpriteNormalImporter : EditorWindow
    {
        [MenuItem("Window/Sprite + Normal Importer")]
        public static void ShowWindow()
        {
            SpriteNormalImporter window = GetWindow<SpriteNormalImporter>("Sprite Importer");
        }

        //string folderPath;
        //string[] filesPath;
        List<PreMaterial> preMaterials = new List<PreMaterial>();
        bool customePixelsPerUnit = true;
        int pixelsPerUnit;
        bool isPixelArt = true;
        List<File> files;
        Folder materialsDirectoryFolder;
        Folder spriteOriginFolder;
        Folder normalOriginFolder;
        Folder spriteDirectoryFolder;
        Folder normalDirectoryFolder;

        private void OnGUI()
        {

            #region Selection des dossiers (Origine et Destination)
            #region Dossiers sprite
            if (spriteOriginFolder != null) GUILayout.Label(spriteOriginFolder.name, EditorStyles.helpBox);
            
            if (GUILayout.Button("Origine des sprites"))
            {
                string folderPath = EditorUtility.OpenFolderPanel("Selectionner le dossier où se trouve les sprites", "", ""); 
                if (folderPath != null && folderPath != "")
                {
                    spriteOriginFolder = new Folder(folderPath);
                }
            }

            if (GUILayout.Button("Destination des sprites")) {
                string projectSpritesFolderPath = AssetDatabase.IsValidFolder("Assets/Sprites") ? "Assets/Sprites" : "";
                string folderPath = EditorUtility.OpenFolderPanel("Selectionner le dossier où importer les sprites", projectSpritesFolderPath, "");
                spriteDirectoryFolder = new Folder(folderPath);
            }
            #endregion
            #region Dossier normals
            if (GUILayout.Button("Dossier des normals"))
            {
                string folderPath = EditorUtility.OpenFolderPanel("Selectionner le dossier où se trouve les normals", "", ""); 
                if (folderPath != null && folderPath != "")
                {
                    normalOriginFolder = new Folder(folderPath);
                }
            }

            if (GUILayout.Button("Destination des normals"))
            {
                string projectNormalsFolderPath = AssetDatabase.IsValidFolder("Assets/Textures") ? "Assets/Textures" : "";
                string folderPath = EditorUtility.OpenFolderPanel("Selectionner le dossier où importer les normals", projectNormalsFolderPath, "");
                normalDirectoryFolder = new Folder(folderPath);
            }
            #endregion
            #endregion

            if (spriteOriginFolder == null || normalOriginFolder == null || spriteDirectoryFolder == null || normalDirectoryFolder == null) return;

            #region Paramètres supplémentaires
            isPixelArt = EditorGUILayout.Toggle("Assets en Pixel Art", isPixelArt); //Ptetre
            customePixelsPerUnit = EditorGUILayout.Toggle("Valeur de Pixel Per Unit", customePixelsPerUnit);
            if (customePixelsPerUnit)
            {
                pixelsPerUnit = EditorGUILayout.IntField("Valeur de PixelPerUnit", pixelsPerUnit);
            }
            #endregion


            if (GUILayout.Button("Importer"))
            {
                string projectMaterialsFolderPath = AssetDatabase.IsValidFolder("Assets/Materials") ? "Assets/Materials" : "";
                string folderPath = EditorUtility.OpenFolderPanel("Selectionner le dossier où vous voulez créer vos matériaux", projectMaterialsFolderPath, "");
                materialsDirectoryFolder = new Folder(folderPath);

                string folderName = Path.GetFileName(folderPath);//A ENLEVER

                string[] filesPath = Directory.GetFiles(folderPath);//
                foreach (string filePath in filesPath) {
                    files.Add(new File(filePath));
                }

                foreach (File file in files) {
                    //Créer le dossier Sprite
                    //Créer un dossier dans Sprites au nom du fichier
                    //C'est créer par Marie et storer dans spriteDirectoryFolder
                    //Copier les fichiers dans ce dossier
                    FileUtil.CopyFileOrDirectory(file.fullPath, spriteDirectoryFolder.relativePath + "/" + file.fullName);
                    //Vérifie si un preMaterial porte son nom
                        //Si oui, va dedans en tant que sprite
                        //Si non, créer un preMaterial portant son nom et son sprite
                    //Change les paramètres d'importation

                    //Meme chose pour les normals

                    //Créer le dossier Materials
                    //Vérifie si les preMaterials sont bien remplies
                        //Si oui, les créer dans le fichier portant le nom du dossier
                    
                    //Reset
                }

                foreach (string filePath in filesPath)
                {
                    if (Path.GetFileNameWithoutExtension(filePath).StartsWith("spr_"))
                    {
                        string fileName = Path.GetFileName(filePath);
                        string crunchedFileName = Path.GetFileNameWithoutExtension(filePath);
                        crunchedFileName = crunchedFileName.Substring(4, Path.GetFileNameWithoutExtension(filePath).Length - 4);
                        Debug.Log(crunchedFileName);
                        //Créer le dossier Sprites si il existe pas
                        if (!AssetDatabase.IsValidFolder("Assets/Sprites")) AssetDatabase.CreateFolder("Assets", "Sprites");
                        AssetDatabase.Refresh();

                        //Copier / Coller le sprite dans Sprites/ LeNomDuDossier
                        if (!AssetDatabase.IsValidFolder("Assets/Sprites/" + folderName)) AssetDatabase.CreateFolder("Assets/Sprites", folderName); //A ENLEVER
                        FileUtil.CopyFileOrDirectory(filePath, "Assets/Sprites/" + Path.GetFileName(folderPath) + "/" + Path.GetFileName(filePath)); //
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
                        if (customePixelsPerUnit) {
                            importer.spritePixelsPerUnit = pixelsPerUnit;
                                }
                        if (isPixelArt) {
                            importer.filterMode = FilterMode.Point;
                        }
                    }
                    else if (Path.GetFileNameWithoutExtension(filePath).StartsWith("normal_"))
                    {
                        string fileName = Path.GetFileName(filePath);
                        string crunchedFileName = Path.GetFileNameWithoutExtension(filePath);
                        crunchedFileName = crunchedFileName.Substring(7, Path.GetFileNameWithoutExtension(filePath).Length - 7);
                        //Créer le dossier Sprites si il existe pas
                        if (!AssetDatabase.IsValidFolder("Assets/Textures")) AssetDatabase.CreateFolder("Assets", "Textures");
                        AssetDatabase.Refresh();

                        //Copier / Coller le sprite dans Sprites/ SonNom
                        if (!AssetDatabase.IsValidFolder("Assets/Textures/" + folderName)) AssetDatabase.CreateFolder("Assets/Textures", folderName); //A ENLEVER
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
                        if (isPixelArt) {
                            importer.filterMode = FilterMode.Point;
                        }
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
            /*
            folderPath = null;
            filesPath = null;
            preMaterials.Clear();
            */
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
    }

    public class PreMaterial
    {
        public string name;
        public Texture2D sprite;
        public Texture2D normal;
    }

    public class File {
        public string fullName;
        public string nameWithoutExtension;
        public string crunchedName;
        public string fullPath;
        public string relativePath;

        public enum TypesOfFile { Null, Sprite, Normal };
        public TypesOfFile fileType;

        public File(string path) {
            fullPath = path;
            fullName = Path.GetFileName(path);
            nameWithoutExtension = Path.GetFileNameWithoutExtension(path);
            if (nameWithoutExtension.StartsWith("spr_")){
                fileType = TypesOfFile.Sprite;
                crunchedName = nameWithoutExtension.Substring(4, nameWithoutExtension.Length - 4);
            }
            else if (nameWithoutExtension.StartsWith("normal_")){
                fileType = TypesOfFile.Normal;
                crunchedName = nameWithoutExtension.Substring(7, nameWithoutExtension.Length - 7);
            }
            relativePath = "Assets" + fullPath.Substring(Application.dataPath.Length);
        }
    }

    public class Folder {
        public string name;
        public string fullPath;
        public string relativePath;

        public Folder(string path) {
            fullPath = path;
            relativePath = "Asset"+fullPath.Substring(Application.dataPath.Length);
            name = Path.GetFileName(path);
        }
    }
}
