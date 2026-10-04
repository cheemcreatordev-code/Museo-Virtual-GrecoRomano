using UnityEngine;
using UnityEditor;

public static class HerramientasSceneView
{
    // =========================================================
    // CREAR CÁMARA
    // =========================================================

    [MenuItem("Tools/Scene View/Crear Cámara")]
    static void CrearCamara()
    {
        SceneView vista = SceneView.lastActiveSceneView;

        if (vista == null)
            return;

        GameObject camara = new GameObject("Camera");

        camara.transform.position = vista.camera.transform.position;
        camara.transform.rotation = vista.camera.transform.rotation;

        camara.AddComponent<Camera>();

        Undo.RegisterCreatedObjectUndo(camara, "Crear Cámara");

        Selection.activeGameObject = camara;
    }


    // =========================================================
    // OBJETOS 3D
    // =========================================================

    [MenuItem("Tools/Scene View/Cube")]
    static void CrearCube()
    {
        CrearPrimitivo(PrimitiveType.Cube);
    }

    [MenuItem("Tools/Scene View/Sphere")]
    static void CrearSphere()
    {
        CrearPrimitivo(PrimitiveType.Sphere);
    }

    [MenuItem("Tools/Scene View/Capsule")]
    static void CrearCapsule()
    {
        CrearPrimitivo(PrimitiveType.Capsule);
    }

    [MenuItem("Tools/Scene View/Cylinder")]
    static void CrearCylinder()
    {
        CrearPrimitivo(PrimitiveType.Cylinder);
    }

    [MenuItem("Tools/Scene View/Plane")]
    static void CrearPlane()
    {
        CrearPrimitivo(PrimitiveType.Plane);
    }

    [MenuItem("Tools/Scene View/Quad")]
    static void CrearQuad()
    {
        CrearPrimitivo(PrimitiveType.Quad);
    }


    static void CrearPrimitivo(PrimitiveType tipo)
    {
        SceneView vista = SceneView.lastActiveSceneView;

        if (vista == null)
            return;

        GameObject objeto = GameObject.CreatePrimitive(tipo);

        objeto.transform.position = vista.camera.transform.position;
        objeto.transform.rotation = vista.camera.transform.rotation;

        Undo.RegisterCreatedObjectUndo(
            objeto,
            "Crear " + tipo.ToString()
        );

        Selection.activeGameObject = objeto;
    }


    // =========================================================
    // EMPTY
    // =========================================================

    [MenuItem("Tools/Scene View/Crear Empty")]
    static void CrearEmpty()
    {
        SceneView vista = SceneView.lastActiveSceneView;

        if (vista == null)
            return;

        GameObject empty = new GameObject("Empty");

        empty.transform.position = vista.camera.transform.position;
        empty.transform.rotation = vista.camera.transform.rotation;

        Undo.RegisterCreatedObjectUndo(
            empty,
            "Crear Empty"
        );

        Selection.activeGameObject = empty;
    }


    // =========================================================
    // POSICIONAR SELECCIONADO
    // =========================================================

    [MenuItem("Tools/Scene View/Posicionar seleccionado")]
    static void PosicionarSeleccionado()
    {
        SceneView vista = SceneView.lastActiveSceneView;

        if (vista == null)
            return;

        GameObject objeto = Selection.activeGameObject;

        if (objeto == null)
            return;

        Undo.RecordObject(
            objeto.transform,
            "Posicionar objeto en vista"
        );

        objeto.transform.position =
            vista.camera.transform.position;

        objeto.transform.rotation =
            vista.camera.transform.rotation;

        EditorUtility.SetDirty(objeto);
    }

    [MenuItem("Tools/Scene View/Posicionar seleccionado", true)]
    static bool ValidarPosicionar()
    {
        return Selection.activeGameObject != null;
    }
}