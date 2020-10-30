
using System.Collections;
using UnityEngine;

public class StepManager : MonoBehaviour
{

    public GameObject stepPrefab;
    public GameObject dummyStepPrefab;

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
        int randomPosx = Random.Range (-4, 5);
        Vector2 pos = new Vector2 (randomPosx, stepIndex * 4);
        GameObject newDummyStep = Instantiate (dummyStepPrefab, pos, Quaternion.identity);
        newDummyStep.transform.SetParent (transform);
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
