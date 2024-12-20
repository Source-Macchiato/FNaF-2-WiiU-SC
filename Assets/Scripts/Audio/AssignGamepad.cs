using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class AssignGamepad : MonoBehaviour {

	void Start () {
		foreach(var audio in GetComponentsInChildren<AudioSource>())
		{
			WiiU.AudioSourceOutput.Assign(audio, WiiU.AudioOutput.TV | WiiU.AudioOutput.GamePad);
		}
	}
}
