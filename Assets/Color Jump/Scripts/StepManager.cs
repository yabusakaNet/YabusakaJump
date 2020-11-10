
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StepManager : MonoBehaviour
{
    public Step stepPrefab;
    public GameObject dummyStepPrefab;

    public GameObject coinPrefab;
    public GameObject starPrefab;

    GameManager gameManager;
    GameDesignConstants gameDesignConstants;

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
        gameDesignConstants = GameDesignConstantsBehaviour.Instance.GameDesignConstants;

        DestroyStepAndItems ();

        for (int i = 0; i < 4; i++) {
            MakeNewStep ();
        }
    }

    public void MakeNewStep ()
    {
        DestroyStepAndItem ();

        var type = (int)StepType.Normal;
        if (stepIndex >= 5 && !gameManager.isStar) {
            var typeList = new List<int> ();
            typeList.Add (gameDesignConstants.NormalStepAppearanceProbability);
            if (gameManager.score >= gameDesignConstants.AppearShortStepScore) {
                typeList.Add (gameDesignConstants.ShortStepStepAppearanceProbability);
            }
            if (gameManager.score >= gameDesignConstants.AppearSuddenlyStepScore) {
                typeList.Add (gameDesignConstants.SuddenlyStepAppearanceProbability);
            }
            if (gameManager.score >= gameDesignConstants.AppearShortSuddenlyStepScore) {
                typeList.Add (gameDesignConstants.ShortSuddenlyStepAppearanceProbability);
            }
            if (gameManager.score >= gameDesignConstants.AppearDummyStepScore) {
                typeList.Add (gameDesignConstants.DummyStepAppearanceProbability);
            }
            if (gameManager.score >= gameDesignConstants.AppearMoveStepScore) {
                typeList.Add (gameDesignConstants.MoveStepAppearanceProbability);
            }
            type = GetRandomIndex (typeList);
        }

        var newStep = CreateStep (type);

        switch (type) {
        case (int)StepType.Normal:
        case (int)StepType.Short:
            break;
        case (int)StepType.Suddenly:
        case (int)StepType.ShortSuddenly:
            StartCoroutine (SetSuddenly (newStep));
            break;
        case (int)StepType.Dummy:
            CreateDummyStep (newStep);
            break;
        case (int)StepType.Move:
            SetMove (newStep);
            break;
        }

        MakeItem ();

        stepIndex++;
    }

    public int GetRandomIndex (List<int> weightTable)
    {
        var totalWeight = weightTable.Sum ();
        var value = Random.Range (1, totalWeight + 1);
        var retIndex = -1;
        for (var i = 0; i < weightTable.Count; ++i) {
            if (weightTable[i] >= value) {
                retIndex = i;
                break;
            }
            value -= weightTable[i];
        }
        return retIndex;
    }

    private Step CreateStep (int type)
    {
        var randomPosx = stepIndex == 1 ? 0 : Random.Range (-4, 5);
        if (gameManager.isStar) {
            randomPosx = Random.Range (-3, 4);
        }

        Vector2 pos = new Vector2 (randomPosx, stepIndex * 4);
        var newStep = Instantiate (stepPrefab, pos, Quaternion.identity);
        var scale = newStep.transform.localScale;
        var width = gameDesignConstants.NormalStepWidthUpperLimit;

        if (gameManager.isStar) {
            width = Random.Range (gameDesignConstants.StarEnableStepWidthLowerLimit, gameDesignConstants.StarEnableStepWidthUpperLimit);
        } else {
            switch (type) {
            case (int)StepType.Normal:
                width = Random.Range (gameDesignConstants.NormalStepWidthLowerLimit, gameDesignConstants.NormalStepWidthUpperLimit);
                break;
            case (int)StepType.Short:
                width = Random.Range (gameDesignConstants.ShortStepWidthLowerLimit, gameDesignConstants.ShortStepWidthUpperLimit);
                break;
            case (int)StepType.Suddenly:
                width = Random.Range (gameDesignConstants.SuddenlyStepWidthLowerLimit, gameDesignConstants.SuddenlyStepWidthUpperLimit);
                break;
            case (int)StepType.ShortSuddenly:
                width = Random.Range (gameDesignConstants.ShortStepWidthLowerLimit, gameDesignConstants.ShortStepWidthUpperLimit);
                break;
            case (int)StepType.Dummy:
                width = Random.Range (gameDesignConstants.DummyNormalStepWidthLowerLimit, gameDesignConstants.DummyNormalStepWidthUpperLimit);
                break;
            case (int)StepType.Move:
                width = Random.Range (gameDesignConstants.MoveStepWidthLowerLimit, gameDesignConstants.MoveStepWidthUpperLimit);
                break;
            }
        }

        newStep.transform.localScale = new Vector3 (width, scale.y, scale.z);

        newStep.transform.SetParent (transform);
        newStep.gameObject.name = stepIndex.ToString ();
        stepObjects.Add (stepIndex, newStep.gameObject);

        return newStep;
    }

    public void MakeItem ()
    {
        var type = ItemType.None;
        if (stepIndex >= 5 && !gameManager.isStar) {
            if (gameManager.score >= gameDesignConstants.AppearCoinScore && Random.Range (1, gameDesignConstants.CoinAppearanceProbability) == 1) {
                type = ItemType.Coin;
            } else if (gameManager.score >= gameDesignConstants.AppearStarScore && Random.Range (1, gameDesignConstants.StarAppearanceProbability) == 1) {
                type = ItemType.Star;
            }
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

    IEnumerator SetSuddenly (Step newStep)
    {
        var spriteRenderer = newStep.GetComponent<SpriteRenderer> ();
        spriteRenderer.color = new Color (1f, 1f, 1f, 0f);
        yield return new WaitForSeconds (1.1f);
        if (spriteRenderer != null) {
            spriteRenderer.color = new Color (1f, 1f, 1f, 1f);
        }
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
        var scale = newDummyStep.transform.localScale;
        var width = Random.Range (gameDesignConstants.DummyStepWidthLowerLimit, gameDesignConstants.DummyStepWidthUpperLimit);
        newDummyStep.transform.localScale = new Vector3 (width, scale.y, scale.z);
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
