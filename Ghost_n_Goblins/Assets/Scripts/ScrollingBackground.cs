using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour {
    [SerializeField] GameObject _Background;
    [SerializeField] float _Choke;
    Camera _MainCamera;
    Vector2 _ScreenBounds;

    private void Start() {
        _MainCamera = gameObject.GetComponent<Camera>();
        _ScreenBounds = _MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _MainCamera.transform.position.z));
        float BackgroundWidth = _Background.GetComponent<SpriteRenderer>().bounds.size.x + _Choke;
        int childsNeeded = (int)Mathf.Ceil(_ScreenBounds.x * 2 / BackgroundWidth);
        GameObject clone = Instantiate(_Background) as GameObject;
        for(int i = 0; i <= childsNeeded; i++) {
            GameObject c = Instantiate(clone) as GameObject;
            c.transform.SetParent(_Background.transform);
            c.transform.position = new Vector3(BackgroundWidth * i, _Background.transform.position.y, _Background.transform.position.z);
        }
        Destroy(clone);
        Destroy(_Background.GetComponent<SpriteRenderer>());
    }

    private void LateUpdate() {
        Transform[] children = _Background.GetComponentsInChildren<Transform>();
        if(children.Length > 1) {
            GameObject firstChild = children[1].gameObject;
            GameObject lastChild = children[children.Length - 1].gameObject;
            float halfObjectWidth = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x - choke;
            if(transform.position.x + _ScreenBounds.x > lastChild.transform.position.x + halfObjectWidth) {
                firstChild.transform.SetAsLastSibling();
                firstChild.transform.position = new Vector3(lastChild.transform.position.x + halfObjectWidth * 2, lastChild.transform.position.y, lastChild.transform.position.z);
            }
            else if(transform.position.x - _ScreenBounds.x < firstChild.transform.position.x - halfObjectWidth) {
                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector3(firstChild.transform.position.x - halfObjectWidth * 2, firstChild.transform.position.y, firstChild.transform.position.z);
            }
        }
    }
}
