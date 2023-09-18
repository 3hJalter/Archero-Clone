using UnityEngine;

public class Skin : HMonoBehaviour
{
    [SerializeField] private Transform allBody;
    
    public void OnInit()
    {
        allBody.localPosition = Vector3.zero;
    }
}
