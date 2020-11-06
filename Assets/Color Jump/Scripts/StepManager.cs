
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepManager : MonoBehaviour
{
    public Step stepPrefab;
    public GameObject dummyStepPrefab;

    public GameObject coinPrefab;
    public GameObject starPrefab;

    GameManager gameManager;

    Dictionary<int, GameObject> stepObjects;
    Dictionary<int, GameObject> dummyStepObjects;
    Dictionary<int, GameObject> itemObjects;

    int stepIndex = 1;

    enum StepType
    {
        Normal = 0,
        Short,
        Suddenly,
        ShortSuddenly,
        Dummy,
        Move,
    }

    enum ItemType
    {
        None = 0,
        Coin,
        Star,
    }

    void Start ()
    {
        gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();

        DestroyStepAndItems ();

        for (int i = 0; i < 4; i++) {
            MakeNewStep ();
        }
    }

    public void MakeNewStep ()
    {
        DestroyStepAndItem ();

        var type = GetRandom<StepType> ();
        if (stepIndex < 5 || gameManager.isStar) {
            type = StepType.Normal;
        }

        var randomPosx = stepIndex == 1 ? 0 : Random.Range (-4, 5);
        Vector2 pos = new Vector2 (randomPosx, stepIndex * 4);
        var newStep = Instantiate (stepPrefab, pos, Quaternion.identity);
        newStep.transform.SetParent (transform);
        newStep.gameObject.name = stepIndex.ToString ();
        stepObjects.Add (stepIndex, newStep.gameObject);

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
            CreateDummyStep (newStep);
            break;
        case StepType.Move:
            SetMove (newStep);
            break;
        }

        MakeItem ();

        stepIndex++;
    }

    public void MakeItem ()
    {
        var type = GetRandom<ItemType> ();
        if (stepIndex < 5 || (gameManager.isStar && type == ItemType.Star)) {
            type = ItemType.None;
        }

        switch (type) {
        case ItemType.None:
            break;
        case ItemType.Coin:
            CreateCoin ();
            break;
        case ItemType.Star:
            CreateStar ();
            break;
        }
    }

    void SetShort (Step newStep)
    {
        var scale = newStep.transform.localScale;
        newStep.transform.localScale = new Vector3 (0.75f, scale.y, scale.z);
    }

    IEnumerator SetSuddenly (Step newStep)
    {
        var spriteRenderer = newStep.GetComponent<SpriteRenderer> ();
        spriteRenderer.color = new Color (1f, 1f, 1f, 0f);
        yield return new WaitForSeconds (1.1f);
        spriteRenderer.color = new Color (1f, 1f, 1f, 1f);
    }

    void CreateDummyStep (Step newStep)
    {
        var isDummyLeft = Random.Range (0, 2) == 1;
        int randomPosLeft = Random.Range (-4, 0);
        int randomPosRight = Random.Range (1, 5);
        Vector2 posLeft = new Vector2 (randomPosLeft, stepIndex * 4);
        Vector2 posRight = new Vector2 (randomPosRight, stepIndex * 4);
        GameObject newDummyStep = Instantiate (dummyStepPrefab, isDummyLeft ? posLeft : posRight, Quaternion.identity);
        newDummyStep.transform.SetParent (transform);
        newDummyStep.gameObject.name = stepIndex.ToString ();
        dummyStepObjects.Add (stepIndex, newDummyStep);
        newStep.transform.localPosition = isDummyLeft ? posRight : posLeft;
    }

    void SetMove (Step newStep)
    {
        newStep.StartMove ();
    }

    void CreateCoin ()
    {
        var randomPosx = Random.Range (-4, 5);
        Vector2 pos = new Vector2 (randomPosx, stepIndex * 4 + 2);
        GameObject coin = Instantiate (coinPrefab, pos, Quaternion.identity);
        coin.transform.SetParent (transform);
        itemObjects.Add (stepIndex, coin);
    }

    void CreateStar ()
    {
        var randomPosx = Random.Range (-4, 5);
        Vector2 pos = new Vector2 (randomPosx, stepIndex * 4 + 2);
        GameObject star = Instantiate (starPrefab, pos, Quaternion.identity);
        star.transform.SetParent (transform);
        itemObjects.Add (stepIndex, star);
    }

    void DestroyStepAndItem ()
    {
        if (stepObjects.ContainsKey (stepIndex - 10)) {
            Destroy (stepObjects[stepIndex - 10]);
            stepObjects.Remove (stepIndex - 10);
        }

        if (dummyStepObjects.ContainsKey (stepIndex - 10)) {
            Destroy (dummyStepObjects[stepIndex - 10]);
            dummyStepObjects.Remove (stepIndex - 10);
        }

        if (itemObjects.ContainsKey (stepIndex - 10)) {
            Destroy (itemObjects[stepIndex - 10]);
            itemObjects.Remove (stepIndex - 10);
        }
    }

    void DestroyStepAndItems ()
    {
        if (stepObjects == null) {
            stepObjects = new Dictionary<int, GameObject> ();
        } else {
            foreach (var obj in stepObjects) {
                Destroy (obj.Value);
            }
            stepObjects.Clear ();
            stepObjects = new Dictionary<int, GameObject> ();
        }

        if (dummyStepObjects == null) {
            dummyStepObjects = new Dictionary<int, GameObject> ();
        } else {
            foreach (var obj in dummyStepObjects) {
                Destroy (obj.Value);
            }
            dummyStepObjects.Clear ();
            dummyStepObjects = new Dictionary<int, GameObject> ();
        }

        if (itemObjects == null) {
            itemObjects = new Dictionary<int, GameObject> ();
        } else {
            foreach (var obj in itemObjects) {
                Destroy (obj.Value);
            }
            itemObjects.Clear ();
            itemObjects = new Dictionary<int, GameObject> ();
        }
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
