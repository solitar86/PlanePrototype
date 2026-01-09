using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class PlaneAudioHandler : MonoBehaviour
{
    private AudioSource _propellerAudioSource;
    private PlaneController_3 _planeController;
    [SerializeField] private float minPitch, maxPitch, minVolume, maxVolume;

    private void Awake()
    {
        _propellerAudioSource = GetComponent<AudioSource> ();
        _propellerAudioSource.playOnAwake = false;
        _planeController = GetComponent<PlaneController_3> ();  
    }

    private void Update()
    {
        float lerpAmount = _planeController.ThrustNormalized;

        if(lerpAmount > 0)
        {
            if (!_propellerAudioSource.isPlaying) _propellerAudioSource.Play();

            _propellerAudioSource.pitch = Mathf.Lerp(minPitch, maxPitch, lerpAmount);
            _propellerAudioSource.volume = Mathf.Lerp(minVolume, maxVolume, lerpAmount);
        }
        else
        {
            _propellerAudioSource.Pause();
        }

    }
}
