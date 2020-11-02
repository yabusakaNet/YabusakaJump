
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepManager : MonoBehaviour
{

    public GameObject stepPrefab;
    public GameObject dummyStepPrefab;

    Dictionary<int, GameObject> dummyStepObjects;

    float hueValue;
    int stepIndex = 1;

    enum StepType
    {
        Normal = 0,
        Short,
        Suddenly,
        ShortSuddenly,
        Dummy,
    }

    void Start ()
    {
        DestroyDummySteps ();

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
        DestroyDummyStep ();

        var randomPosx = stepIndex == 1 ? 0 : Random.Range (-4, 5);
        Vector2 pos = new Vector2 (randomPosx, stepIndex * 4);
        GameObject newStep = Instantiate (stepPrefab, pos, Quaternion.identity);
        newStep.transform.SetParent (transform);

        var type = GetRandom<StepType> ();
        if (stepIndex < 5) {
            type = StepType.Normal;
        }

        switch (type) {
        case StepType.Normal:
            break;
        case StepType.Short:
            SetShort (newStep);
            break;
        case StepType.Suddenly:
            StartCoroutine (SetSuddenly (newStep));
            break;
        case StepType.ShortSuddenly:
            StartCoroutine (SetSuddenly (newStep));
            SetShort (newStep);
            break;
        case StepType.Dummy:
            SetDummyStep (newStep);
            break;
        }

        stepIndex++;
    }

    void SetShort (GameObject newStep)
    {
        var scale = newStep.transform.localScale;
        newStep.transform.localScale = new Vector3 (0.75f, scale.y, scale.z);
    }

    IEnumerator SetSuddenly (GameObject newStep)
    {
        var spriteRenderer = newStep.GetComponent<SpriteRenderer> ();
        spriteRenderer.color = new Color (1f, 1f, 1f, 0f);
        yield return new WaitForSeconds (1.1f);
        spriteRenderer.color = new Color (1f, 1f, 1f, 1f);
    }

    void SetDummyStep (GameObject newStep)
    {
        var isDummyLeft = Random.Range (0, 2) == 1;
        int randomPosLeft = Random.Range (-4, 0);
        int randomPosRight = Random.Range (1, 5);
        Vector2 posLeft = new Vector2 (randomPosLeft, stepIndex * 4);
        Vector2 posRight = new Vector2 (randomPosRight, stepIndex * 4);
        GameObject newDummyStep = Instantiate (dummyStepPrefab, isDummyLeft ? posLeft : posRight, Quaternion.identity);
        newDummyStep.transform.SetParent (transform);
        dummyStepObjects.Add (stepIndex, newDummyStep);
        newStep.transform.localPosition = isDummyLeft ? posRight : posLeft;
    }

    void DestroyDummyStep ()
    {
        if (dummyStepObjects.ContainsKey (stepIndex - 5)) {
            Destroy (dummyStepObjects[stepIndex - 5]);
            dummyStepObjects.Remove (stepIndex - 5);
        }
    }

    void DestroyDummySteps ()
    {
        if (dummyStepObjects == null) {
            dummyStepObjects = new Dictionary<int, GameObject> ();
            return;
        }
        foreach (var obj in dummyStepObjects) {
            Destroy (obj.Value);
        }
        dummyStepObjects.Clear ();
        dummyStepObjects = new Dictionary<int, GameObject> ();
    }

    public int GetTypeNum<T> () where T : struct
    {
        return System.Enum.GetValues (typeof (T)).Length;
    }

    public T GetRandom<T> () where T : struct
    {
        int no = Random.Range (0, GetTypeNum<T> ());
        return NoToType<T> (no);
    }

    public T NoToType<T> (int targetNo) where T : struct
    {
        return (T)System.Enum.ToObject (typeof (T), targetNo);
    }
}
