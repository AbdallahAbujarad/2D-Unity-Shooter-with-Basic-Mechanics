using UnityEngine;

public class FollowMouseRotation : MonoBehaviour
{
    float lerpSpeed = 80;
    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0;
        transform.right = Vector2.Lerp(transform.right,new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y),lerpSpeed * Time.deltaTime);
    }
}
