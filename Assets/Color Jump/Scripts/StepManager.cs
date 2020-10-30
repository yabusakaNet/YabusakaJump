using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepManager : MonoBehaviour
{

    public GameObject stepPrefab;

    float hueValue;
    int stepIndex = 1;
    const float initialStepWidth = 1.5f;
    const float limitStepWidth = 0.5f;

    void Start ()
    {
        InitColor ();


        for (int i = 0; i < 4; i++) {
            MakeNewStep ();
        }
    }


    void InitColor ()
    {
        hueValue = Random.Range (0, 10) / 10.0f;
        Camera.main.backgroundColor = Color.HSVToRGB (hueValue, 0.6f, 0.8f);
    }




    public void MakeNewStep ()
    {

        int randomPosx;
        if (stepIndex == 1)
            randomPosx = 0;
        else
            randomPosx = Random.Range (-4, 5);

        Vector2 pos = new Vector2 (randomPosx, stepIndex * 4);
        GameObject newStep = Instantiate (stepPrefab, pos, Quaternion.identity);
        newStep.transform.SetParent (transform);

        SetWidth (newStep);

        stepIndex++;
    }

    void SetWidth (GameObject newStep)
    {
        var scale = newStep.transform.localScale;
        var width = initialStepWidth - (stepIndex * 0.01f);
        if (width < limitStepWidth) {
            width = limitStepWidth;
        }
        newStep.transform.localScale = new Vector3 (width, scale.y, scale.z);
    }
}
