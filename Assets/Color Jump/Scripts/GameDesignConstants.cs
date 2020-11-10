using UnityEngine;

[CreateAssetMenu (menuName = "Yabusaka/Create GameDesignConstants")]
public class GameDesignConstants : ScriptableObject
{
    //ステップ幅
    [SerializeField]
    public float NormalStepWidthUpperLimit = 1.7f;
    [SerializeField]
    public float NormalStepWidthLowerLimit = 1.7f;

    [SerializeField]
    public float ShortStepWidthUpperLimit = 1.2f;
    [SerializeField]
    public float ShortStepWidthLowerLimit = 0.8f;

    [SerializeField]
    public float MoveStepWidthUpperLimit = 1.7f;
    [SerializeField]
    public float MoveStepWidthLowerLimit = 1.7f;

    [SerializeField]
    public float SuddenlyStepWidthUpperLimit = 1.7f;
    [SerializeField]
    public float SuddenlyStepWidthLowerLimit = 1.7f;

    [SerializeField]
    public float DummyStepWidthUpperLimit = 1.7f;
    [SerializeField]
    public float DummyStepWidthLowerLimit = 1.7f;
    [SerializeField]
    public float DummyNormalStepWidthUpperLimit = 1.7f;
    [SerializeField]
    public float DummyNormalStepWidthLowerLimit = 1.7f;

    [SerializeField]
    public float StarEnableStepWidthUpperLimit = 2.5f;
    [SerializeField]
    public float StarEnableStepWidthLowerLimit = 2.5f;


    //スター効果持続秒数
    [SerializeField]
    public float StarEffectiveSeconds = 5f;


    //各ステップ出現スコア数
    [SerializeField]
    public int AppearShortStepScore = 50;
    [SerializeField]
    public int AppearSuddenlyStepScore = 100;
    [SerializeField]
    public int AppearShortSuddenlyStepScore = 100;
    [SerializeField]
    public int AppearDummyStepScore = 200;
    [SerializeField]
    public int AppearMoveStepScore = 200;


    //各ステップ出現確率
    [SerializeField]
    public int NormalStepAppearanceProbability = 50;
    [SerializeField]
    public int ShortStepStepAppearanceProbability = 10;
    [SerializeField]
    public int SuddenlyStepAppearanceProbability = 10;
    [SerializeField]
    public int ShortSuddenlyStepAppearanceProbability = 10;
    [SerializeField]
    public int DummyStepAppearanceProbability = 10;
    [SerializeField]
    public int MoveStepAppearanceProbability = 10;


    //各アイテム出現スコア数
    [SerializeField]
    public int AppearCoinScore = 50;
    [SerializeField]
    public int AppearStarScore = 100;


    //各アイテム出現確率(Nステップに一回の割合で出現)
    [SerializeField]
    public int CoinAppearanceProbability = 10;
    [SerializeField]
    public int StarAppearanceProbability = 20;


    //ドラッグによるキャラクター移動量の調整係数
    [SerializeField]
    public float PlayerMoveMagnification = 2f;


}
