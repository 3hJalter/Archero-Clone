using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject effect;
    private bool _isThroughDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (_isThroughDoor) return;
        LevelManager.Ins.OpenNextLevelUI();
        _isThroughDoor = true;
    }

    public void OnInit()
    {
        _isThroughDoor = false;
        CloseDoor();
    }

    public void OpenDoor()
    {
        door.SetActive(false);
        effect.SetActive(true);
    }

    private void CloseDoor()
    {
        door.SetActive(true);
        effect.SetActive(false);
    }
}
