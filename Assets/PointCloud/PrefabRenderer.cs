using System;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine;
using UnityEngine.UI;

public class PrefabRenderer : MonoBehaviour {
    private GameObject soundCubePrefab;
    private List<GameObject> soundCubes = new List<GameObject>();

    public PrefabRenderer(GameObject prefab){
        soundCubePrefab = prefab;
    }

    private void GeneratePrefab() {
        Debug.Log("generate prefab");
        GameObject spawnedObject = Instantiate(soundCubePrefab, new Vector3(0,0,0), Quaternion.identity);
        spawnedObject.SetActive(false); 
        // TODO: might have audio issues
        soundCubes.Add(spawnedObject);  
    }

    public void RenderPoints(List<Vector3> objs) {
        while (soundCubes.Count < objs.Count) {
            this.GeneratePrefab();
        }
        for (int i = 0; i < Math.Min(soundCubes.Count, objs.Count); i++) {
            // render cube
            soundCubes[i].transform.position = objs[i];
            if (!soundCubes[i].activeSelf) {
                Debug.Log("set active");
                soundCubes[i].SetActive(true);
            }
            soundCubes[i].GetComponent<AudioSource>().mute = false;
        }
        for (int i = Math.Min(soundCubes.Count, objs.Count); i < soundCubes.Count; i++) {
            // disable cube
            /*if (soundCubes[i].activeSelf) {
                Debug.Log("set inactive");
                soundCubes[i].SetActive(false);
            }*/
            soundCubes[i].GetComponent<AudioSource>().mute = true;
        }
    }
}