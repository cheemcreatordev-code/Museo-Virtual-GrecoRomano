using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apuntado : MonoBehaviour
{
    public KeyCode InputApuntado;

    public Transform PosicionOriginal;
    public Transform PosicionApuntado;
    public Transform PosicionNormalizado;

    [Range(0, 100)] public float SpeedTranslation;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Apuntar();
    }

    public void Apuntar()
    {
        if (Input.GetKey(InputApuntado))
        {
            PosicionOriginal.position = Vector3.Lerp(PosicionOriginal.position, PosicionApuntado.position, SpeedTranslation * Time.deltaTime);
        }
        else
        {
            PosicionOriginal.position = Vector3.Lerp(PosicionOriginal.position, PosicionNormalizado.position, SpeedTranslation * Time.deltaTime);  
        }
    }
}