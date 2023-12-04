using System.Collections.Generic;
using UnityEngine;

public class CellInstantiator : MonoBehaviour
{
    public GameObject cellPrefab;
    public List<AudioCell> audioCells;
    public List<AudioClip> soundBank;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject newObject = Instantiate(cellPrefab) as GameObject;
            AudioCell newAudioCell = newObject.AddComponent<AudioCell>();
            newAudioCell.Create(soundBank[Random.Range(0, 10)], Random.Range(0.5f, 1), Random.Range(0.4f, 5), 1);
            
            audioCells.Add(newAudioCell);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            AudioCell cellToDestroy = audioCells[0];
            StartCoroutine(cellToDestroy.FadeAndDestroy());
            audioCells.Remove(cellToDestroy);
        }
    }
}