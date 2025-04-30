using Audio;
using UnityEngine;

public class PlayerBeam : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    private void OnTriggerEnter(Collider other) {
        AudioManager.Instance.Play2DAudio(AudioEvent.DestroyEnemy);
        Destroy(other.gameObject);
    }
}