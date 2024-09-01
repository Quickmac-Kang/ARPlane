using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{
    public GameObject funiturePrefab;
    public Text logText; // UI Text�� ������ ����
    MarkerManager markerMng;
    GameObject currentObject;

    // Start is called before the first frame update
    void Start()
    {
        markerMng = GetComponent<MarkerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (markerMng.marker.activeInHierarchy && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                //logText.text = $"��ġ ������: ��ġ - {touch.position} / {markerMng.marker.transform.position} / {markerMng.marker.transform.rotation}";
                ObjectMove();
            }
            else if (Input.touchCount == 1 && touch.phase == TouchPhase.Moved)
            {
                ObjectRotation();
            }
            else if (Input.touchCount == 2)
            {
                ObjectScale();
            }
        }
    }

    void ObjectMove()
    {
        if (currentObject == null)
        {
            currentObject = Instantiate(funiturePrefab, markerMng.marker.transform.position,
                                   markerMng.marker.transform.rotation);
        }
        else
        {
            if (Vector3.Distance(
                currentObject.transform.position, markerMng.marker.transform.position) > 1.0f)
            {
                currentObject.transform.SetPositionAndRotation(markerMng.marker.transform.position,
                                       currentObject.transform.rotation);
            }

        }
    }

    void ObjectRotation()
    {
        Touch touch = Input.GetTouch(0);
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // �浹�� ������Ʈ�� Tag�� Ȯ��
            if (hit.collider.CompareTag("MyObject"))
            {
                Vector3 deltaPos = touch.deltaPosition;
                if (currentObject != null)
                {
                    currentObject.transform.Rotate(currentObject.transform.up, deltaPos.x * -1.0f * 0.1f);
                }

            }
        }
    }

    void ObjectScale()
    {
        // �� �հ����� ����Ͽ� ������ ����
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        // �� ��ġ�� ���� ��ġ�� �����մϴ�.
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        // ���� �����Ӱ� ���� �����ӿ��� �� ��ġ ������ ���� �Ÿ��� ���̸� ����մϴ�.
        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        // �Ÿ� ���̸� ����մϴ�.
        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        // �Ÿ� ���̿� ���� �������� �����մϴ�.
        if (currentObject != null)
        {
            float scaleFactor = deltaMagnitudeDiff * 0.01f; // ������ ���� �ӵ�
            currentObject.transform.localScale -= new Vector3(scaleFactor, scaleFactor, scaleFactor);

            // �������� �ʹ� �۰ų� Ŀ���� �ʵ��� �����մϴ�.
            currentObject.transform.localScale = new Vector3(
                Mathf.Clamp(currentObject.transform.localScale.x, 0.1f, 10.0f),
                Mathf.Clamp(currentObject.transform.localScale.y, 0.1f, 10.0f),
                Mathf.Clamp(currentObject.transform.localScale.z, 0.1f, 10.0f)
            );
        }
    }
}
