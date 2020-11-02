using UnityEngine;

public class WorldToHUDSprite : MonoBehaviour
{
    // Update is called once per frame
    protected virtual void LateUpdate()
    {
        if (Camera.main != null) {
            this.transform.rotation = Camera.main.transform.rotation;
        }
    }
}
