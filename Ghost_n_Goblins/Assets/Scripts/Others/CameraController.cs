using UnityEngine;
using EventsManager;

public class CameraController : SimpleGameStateObserver {
    [SerializeField] Transform _Target;
    [SerializeField] int _yOffset;

    Transform _MapBeginning;
    Transform _MapEnding;

    float _HalfWidthSize;

    Transform _Transform;
    Vector3 _InitPosition;

    GameObject _Background;
    Camera _MainCamera;

    #region Camera implementation
    protected override void Awake() {
        base.Awake();
        _Transform = transform;
        _InitPosition = _Transform.position;
    }

    void Start() {
        _HalfWidthSize = Camera.main.orthographicSize * Camera.main.aspect;
        _MainCamera = gameObject.GetComponent<Camera>();
    }

    void Update() {
        if (!GameManager.Instance.IsPlaying) return;
        if (_Target.position.x - _HalfWidthSize > _MapBeginning.position.x && _Target.position.x + _HalfWidthSize < _MapEnding.position.x)
            transform.position = new Vector3(_Target.position.x, _Target.position.y + _yOffset, transform.position.z);
    }

    private void LateUpdate() {
        // Updating the background if needed
        if (!GameManager.Instance.IsPlaying) return;
        Transform[] children = _Background.GetComponentsInChildren<Transform>();
        if (children.Length > 1) {
            GameObject firstChild = children[1].gameObject;
            GameObject lastChild = children[children.Length - 1].gameObject;
            float halfObjectWidth = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x;
            if (_MainCamera.transform.position.x > lastChild.transform.position.x + (halfObjectWidth / 4)) {
                firstChild.transform.SetAsLastSibling();
                firstChild.transform.position = new Vector3(lastChild.transform.position.x + halfObjectWidth * 2, lastChild.transform.position.y, lastChild.transform.position.z);
            }
            else if (_MainCamera.transform.position.x < firstChild.transform.position.x + (halfObjectWidth / 4)) {
                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector3(firstChild.transform.position.x - halfObjectWidth * 2, firstChild.transform.position.y, firstChild.transform.position.z);
            }
        }
    }
    #endregion

    #region Camera methods
    void InitializeChildren() {
        // Calculating and initializing how much backgrounds are needed
        float BackgroundWidth = _Background.GetComponent<SpriteRenderer>().bounds.size.x;
        int childsNeeded = 2;
        GameObject clone = Instantiate(_Background) as GameObject;
        for (int i = 0; i <= childsNeeded; i++) {
            GameObject c = Instantiate(clone) as GameObject;
            c.transform.SetParent(_Background.transform);
            c.transform.position = new Vector3(BackgroundWidth * i, _Background.transform.position.y, _Background.transform.position.z);
        }
        Destroy(clone);
        Destroy(_Background.GetComponent<SpriteRenderer>());
    }

    void SetUpMapPointsAndBackground(Transform mapBeginning, Transform mapEnding, GameObject background) {
        // Setting the points and the background
        _MapBeginning = mapBeginning;
        _MapEnding = mapEnding;
        _Background = background;
        InitializeChildren();
    }

    void ResetCamera() {
        _Transform.position = _InitPosition;
    }
    #endregion

    #region Event Callbacks
    protected override void GameMenu(GameMenuEvent e) {
        ResetCamera();
    }

    protected override void LevelReady(LevelReadyEvent e) {
        if (e.isNewGame) SetUpMapPointsAndBackground(e.eMapBeginning, e.eMapEnding, e.eBackground);
        else ResetCamera();
    }
    #endregion
}