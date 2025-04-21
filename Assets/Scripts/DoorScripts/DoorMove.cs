using System.Collections;
using UnityEngine;

public class DoorMove : MonoBehaviour
{
    private Vector3 pivotOffset = new Vector3(-1f, 0f, 0f);
    private bool isOpen = false;
    private float rotationAngle = -80f;
    private float rotationSpeed = 2f;
    private Coroutine currentRotationCoroutine = null;

    public bool Lock = false; // 🔥 ここ！個別Lockを持たせる！

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen && !Lock)
        {
            if (currentRotationCoroutine != null)
            {
                StopCoroutine(currentRotationCoroutine);
            }
            currentRotationCoroutine = StartCoroutine(RotateDoor(true));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isOpen && !Lock)
        {
            if (currentRotationCoroutine != null)
            {
                StopCoroutine(currentRotationCoroutine);
            }
            currentRotationCoroutine = StartCoroutine(RotateDoor(false));
        }
    }

    IEnumerator RotateDoor(bool open)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = transform.rotation * Quaternion.Euler(0f, 0f, open ? rotationAngle : -rotationAngle);

        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            transform.position = transform.position + transform.rotation * pivotOffset - transform.rotation * pivotOffset;
            yield return null;
        }

        isOpen = open;
        currentRotationCoroutine = null;
    }
}
