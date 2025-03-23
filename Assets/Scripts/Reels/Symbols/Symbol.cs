using UnityEngine;

public class Symbol : MonoBehaviour
{
    [SerializeField] public GameObject spriteNode;
    [SerializeField] public GameObject spineNode;
    public AudioClip _audioClip;
    private int _symbolID;
    [HideInInspector] public bool isSpecialSymbol;

    [SerializeField] private SymbolsAnimationController symbolsAnimationController;

    public int SymbolID { get => _symbolID; set => _symbolID = value; }

    private void Start()
    {
        HideWin();
    }

    public void ShowWin(PayLineState paylinestate)
    {
        spriteNode.SetActive(false);
        spineNode.SetActive(true);

        if (symbolsAnimationController != null)
        {
            symbolsAnimationController.ManageSymbolAnimForPaylineState(paylinestate, IsSpecialSymbol());
        }
    }

    private bool IsSpecialSymbol()
    {
        var systemSetting = ReelManager.Instance.SystemConfig;
        return SymbolID == systemSetting.ScatterId || SymbolID == systemSetting.BonusId;
    }

    public void HideWin()
    {
        spriteNode.SetActive(true);
        spineNode.gameObject.SetActive(false);
    }

    public void SetSpineSortingOrder(int setTo)
    {
        MeshRenderer mr = spineNode.GetComponent<MeshRenderer>();
        mr.sortingOrder = setTo;
    }
}
