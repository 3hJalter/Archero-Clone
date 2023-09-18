using UnityEngine;

public class Skin : HMonoBehaviour
{
    [SerializeField] private Transform allBody;
    
    public void OnInit()
    {
        allBody.localPosition = Vector3.zero;
    }
    
    // Fix Anim Not Reset Rotation Bug
    public void ResetBodyRotation()
    {
        allBody.localRotation = Quaternion.identity;
    }
}
