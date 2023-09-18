using UnityEngine;
using UnityEngine.Serialization;

public class UICanvas : MonoBehaviour
{
    //public bool IsAvoidBackKey = false;
    [FormerlySerializedAs("IsDestroyOnClose")] public bool isDestroyOnClose;
    // private Animator _mAnimator;
    // private bool _mIsInit = false;
    // private float _mOffsetY = 0;

    protected RectTransform MRectTransform;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        MRectTransform = GetComponent<RectTransform>();
        // _mAnimator = GetComponent<Animator>();

        //float ratio = (float)Screen.height / (float)Screen.width;

        //// xu ly tai tho
        //if (ratio > 2.1f)
        //{
        //    Vector2 leftBottom = m_RectTransform.offsetMin;
        //    Vector2 rightTop = m_RectTransform.offsetMax;
        //    rightTop.y = -100f;
        //    m_RectTransform.offsetMax = rightTop;
        //    leftBottom.y = 0f;
        //    m_RectTransform.offsetMin = leftBottom;
        //    m_OffsetY = 100f;
        //}
        //m_IsInit = true;
    }

    public virtual void Setup()
    {
        UIManager.Ins.AddBackUI(this);
        UIManager.Ins.PushBackAction(this, BackKey);
    }

    protected virtual void BackKey()
    {

    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
        //anim
    }

    public virtual void Close()
    {
        UIManager.Ins.RemoveBackUI(this);
        //anim
        gameObject.SetActive(false);
        if (isDestroyOnClose) Destroy(gameObject);

    }
}
