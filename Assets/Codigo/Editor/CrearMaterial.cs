using UnityEngine;
using UnityEditor;

public static class CrearMaterial
{
    [MenuItem("Assets/Crear Material desde Textura &m")]
    public static void Crear()
    {
        Object[] seleccionados = Selection.objects;

        int creados = 0;

        foreach (Object seleccionado in seleccionados)
        {
            Texture2D textura = seleccionado as Texture2D;

            if (textura == null)
                continue;

            string rutaTextura = AssetDatabase.GetAssetPath(textura);

            string carpeta = System.IO.Path.GetDirectoryName(rutaTextura);
            string nombre = System.IO.Path.GetFileNameWithoutExtension(rutaTextura);

            string rutaMaterial = carpeta + "/" + nombre + ".mat";

            // Evita sobrescribir un material existente
            rutaMaterial = AssetDatabase.GenerateUniqueAssetPath(rutaMaterial);

            Material material = new Material(Shader.Find("Standard"));

            // Coloca la imagen como textura principal
            material.mainTexture = textura;

            AssetDatabase.CreateAsset(material, rutaMaterial);

            creados++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Se crearon " + creados + " materiales.");
    }

    [MenuItem("Assets/Crear Material desde Textura &m", true)]
    public static bool Validar()
    {
        // Permite usarlo si hay al menos una textura seleccionada
        foreach (Object seleccionado in Selection.objects)
        {
            if (seleccionado is Texture2D)
                return true;
        }

        return false;
    }
}