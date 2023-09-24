using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class UICanvas : MonoBehaviour
{
    //public bool IsAvoidBackKey = false;
    [FormerlySerializedAs("IsDestroyOnClose")] public bool isDestroyOnClose;
    // private Animator _mAnimator;
    // private bool _mIsInit = false;
    // private float _mOffsetY = 0;
    private Vector2 _initPos ;
    
    private RectTransform _mRectTransform;

    private RectTransform MRectTransform
    {
        get
        {
            _mRectTransform = _mRectTransform ? _mRectTransform : gameObject.transform as RectTransform;
            return _mRectTransform;
        }
    }
    
    private void GetInitPos()
    {
        _initPos = _initPos != Vector2.zero ? MRectTransform.anchoredPosition : new Vector2(1080, 0);
    }
    
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        GetInitPos();
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
        RunOpenAnim();
    }

    protected virtual void RunOpenAnim()
    {
        MRectTransform.anchoredPosition = _initPos;
        MRectTransform.DOAnchorPos(Vector2.zero, 0.2f)
            .SetEase(Ease.Linear);
    }

    public virtual void Close()
    {
        UIManager.Ins.RemoveBackUI(this);
        //anim
        RunCloseAnim(OnClose);
    }

    private void OnClose()
    {
        gameObject.SetActive(false);
        if (isDestroyOnClose) Destroy(gameObject);
    }

    protected virtual void RunCloseAnim(Action onCompleteAction, float time = 0.2f)
    {
        MRectTransform.DOAnchorPos(-_initPos, time)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                onCompleteAction?.Invoke(); 
                MRectTransform.anchoredPosition = Vector2.zero;
            });
    }
    
}
