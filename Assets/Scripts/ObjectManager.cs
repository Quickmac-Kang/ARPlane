using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{
    public GameObject funiturePrefab;
    public Text logText; // UI Text를 참조할 변수
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
                //logText.text = $"터치 감지됨: 위치 - {touch.position} / {markerMng.marker.transform.position} / {markerMng.marker.transform.rotation}";
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
            // 충돌한 오브젝트의 Tag를 확인
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
        // 두 손가락을 사용하여 스케일 조정
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        // 각 터치의 이전 위치를 저장합니다.
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        // 이전 프레임과 현재 프레임에서 두 터치 사이의 벡터 거리의 차이를 계산합니다.
        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        // 거리 차이를 계산합니다.
        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        // 거리 차이에 따라 스케일을 조정합니다.
        if (currentObject != null)
        {
            float scaleFactor = deltaMagnitudeDiff * 0.01f; // 스케일 조정 속도
            currentObject.transform.localScale -= new Vector3(scaleFactor, scaleFactor, scaleFactor);

            // 스케일이 너무 작거나 커지지 않도록 제한합니다.
            currentObject.transform.localScale = new Vector3(
                Mathf.Clamp(currentObject.transform.localScale.x, 0.1f, 10.0f),
                Mathf.Clamp(currentObject.transform.localScale.y, 0.1f, 10.0f),
                Mathf.Clamp(currentObject.transform.localScale.z, 0.1f, 10.0f)
            );
        }
    }
}
