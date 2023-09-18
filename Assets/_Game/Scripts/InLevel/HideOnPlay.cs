using UnityEngine;

public class HideOnPlay : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        gameObject.SetActive(false);
    }
}
