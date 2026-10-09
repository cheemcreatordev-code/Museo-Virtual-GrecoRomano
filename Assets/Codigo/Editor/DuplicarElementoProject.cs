
using UnityEngine;
using UnityEditor;
using System.IO;

public static class DuplicarElementoProject
{
    [MenuItem("Tools/Duplicar elemento del Project")]
    private static void DuplicarElemento()
    {
        Object[] seleccionados = Selection.objects;

        if (seleccionados == null || seleccionados.Length == 0)
        {
            Debug.LogWarning("Selecciona un elemento en Project.");
            return;
        }

        foreach (Object elemento in seleccionados)
        {
            string origen = AssetDatabase.GetAssetPath(elemento);

            if (string.IsNullOrEmpty(origen))
                continue;

            if (AssetDatabase.IsSubAsset(elemento))
            {
                Debug.LogWarning(
                    "No se puede duplicar un subelemento interno: "
                    + elemento.name);
                continue;
            }

            string carpeta;
            string nombre;
            string extension;

            if (AssetDatabase.IsValidFolder(origen))
            {
                carpeta = Path.GetDirectoryName(origen);
                nombre = Path.GetFileName(origen);
                extension = "";
            }
            else
            {
                carpeta = Path.GetDirectoryName(origen);
                nombre = Path.GetFileNameWithoutExtension(origen);
                extension = Path.GetExtension(origen);
            }

            string destino = AssetDatabase.GenerateUniqueAssetPath(
                Path.Combine(
                    carpeta,
                    nombre + " Copy" + extension
                ).Replace("\\", "/")
            );

            if (AssetDatabase.CopyAsset(origen, destino))
            {
                Debug.Log("Duplicado correctamente: " + destino);
            }
            else
            {
                Debug.LogError("Error al duplicar: " + origen);
            }
        }

        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/Duplicar elemento del Project", true)]
    private static bool ValidarSeleccion()
    {
        return Selection.objects != null
            && Selection.objects.Length > 0;
    }
}