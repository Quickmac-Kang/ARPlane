using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MarkerManager : MonoBehaviour
{
    ARRaycastManager aRRaycastManager;
    GameObject markerObj;
    // Start is called before the first frame update
    void Start()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        markerObj = Instantiate(aRRaycastManager.raycastPrefab);
        markerObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlane();
    }

    Vector2 GetScreenCenterPos()
    {
        return new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
    }

    void CheckPlane()
    {
        List<ARRaycastHit> hitList = new List<ARRaycastHit>();
        if(aRRaycastManager.Raycast(GetScreenCenterPos(), hitList, TrackableType.Planes))
        {
            markerObj.SetActive(true);

            markerObj.transform.position = hitList[0].pose.position;
            markerObj.transform.rotation = hitList[0].pose.rotation;
            markerObj.transform.position += Vector3.up * 0.01f;
        }
        else
        {
            markerObj.SetActive(false);
        }
    }
    public GameObject marker { get { return markerObj; } private set { markerObj = value; } }
     
}
