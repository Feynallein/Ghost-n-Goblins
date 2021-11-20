using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingText : MonoBehaviour {
    [SerializeField] float _BlinkDuration;

    private void OnEnable() {
        StartCoroutine(BlinkCoroutine(GetComponent<Text>()));
    }

    IEnumerator BlinkCoroutine(Text text) {
        float elaspedTime = 0f;
        while (true) {
            elaspedTime += Time.deltaTime;
            if (elaspedTime > _BlinkDuration) {
                elaspedTime = 0f;
                text.enabled = !text.enabled;
            }
            yield return null;
        }
    }
}
