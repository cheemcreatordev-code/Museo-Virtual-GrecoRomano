using UnityEngine;
using UnityEditor;
using System.IO;

public class ExtraerAnimationClipMixamo
{
    [MenuItem("Tools/Mixamo/Extraer Animation Clips")]
    static void ExtraerClips()
    {
        Object[] seleccion = Selection.objects;

        if (seleccion.Length == 0)
        {
            EditorUtility.DisplayDialog(
                "Extraer Animation Clips",
                "Selecciona un FBX de Mixamo en el Project.",
                "Aceptar"
            );
            return;
        }

        int cantidad = 0;

        foreach (Object objeto in seleccion)
        {
            string ruta = AssetDatabase.GetAssetPath(objeto);

            if (string.IsNullOrEmpty(ruta) || !ruta.ToLower().EndsWith(".fbx"))
                continue;

            Object[] assets = AssetDatabase.LoadAllAssetsAtPath(ruta);

            foreach (Object asset in assets)
            {
                AnimationClip clip = asset as AnimationClip;

                if (clip == null)
                    continue;

                // Ignorar el clip interno de preview
                if (clip.name == "__preview__")
                    continue;

                string carpeta = Path.GetDirectoryName(ruta);

                string nombreClip = LimpiarNombre(clip.name);

                string rutaAnim = Path.Combine(
                    carpeta,
                    nombreClip + ".anim"
                );

                // Evitar sobrescribir accidentalmente
                rutaAnim = AssetDatabase.GenerateUniqueAssetPath(rutaAnim);

                AnimationClip nuevoClip = new AnimationClip();

                EditorUtility.CopySerialized(clip, nuevoClip);

                AssetDatabase.CreateAsset(nuevoClip, rutaAnim);

                cantidad++;

                Debug.Log(
                    "Animation Clip extraído: " +
                    AssetDatabase.GetAssetPath(nuevoClip)
                );
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog(
            "Extracción terminada",
            "Se extrajeron " + cantidad + " Animation Clip(s).",
            "Aceptar"
        );
    }

    static string LimpiarNombre(string nombre)
    {
        foreach (char caracter in Path.GetInvalidFileNameChars())
        {
            nombre = nombre.Replace(caracter.ToString(), "_");
        }

        return nombre;
    }

    [MenuItem("Tools/Mixamo/Extraer Animation Clips", true)]
    static bool ValidarSeleccion()
    {
        return Selection.objects != null &&
               Selection.objects.Length > 0;
    }
}