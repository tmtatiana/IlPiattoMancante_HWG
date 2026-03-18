using UnityEngine;
using System.Collections;

public class ChefSpawner : MonoBehaviour
{
    public GameObject Chef;
    public float SpawnTimer;

    //this is where the audio variable is stored
    public AudioClip ChefVoiceLine;
    private AudioSource ChefAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnChef());

    //storing audio references
        ChefAudio = GetComponent<AudioSource>();
        ChefAudio.playOnAwake = true;
    }

    private IEnumerator SpawnChef()
    {
        yield return new WaitForSeconds(SpawnTimer);
        Object.Instantiate(Chef);
    }
}