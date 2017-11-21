using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	float frequency = 0f;
	bool helper = true;

	void Update() {
	
		/*if (Recorder.tuning) {
		
			GetFrequency ();
		}*/
		if (Recorder.tuning) {
		
			if (helper) {

				InvokeRepeating ("GetFrequency", 0, Recorder.notesPerSecond);
				helper = false;
			}
		}
		else {
		
			if (!helper) {
			
				CancelInvoke ("GetFrequency");
				helper = true;
			}
		}
	}

	public void TestMicOn() {

		Recorder.TestMicOn ();
	}

	public void TestGetData() {
	
		/*
		int dspBufferLength, numBuffers;
		AudioSettings.GetDSPBufferSize( out dspBufferLength, out numBuffers );

		int numberOfMics = Microphone.devices.Length;
		int minFreqs = -1;
		int maxFreqs = -1;

		Microphone.GetDeviceCaps(Microphone.devices[0], out minFreqs, out maxFreqs);
		print ( "AudioSettings.ouputSampleRate: " + AudioSettings.outputSampleRate + "; audio buffer size: " + dspBufferLength +"; numBuffers: "+numBuffers + "; numberOfMics: "+ numberOfMics  + "; minFrequency: " + minFreqs + "; maxFrequency: "+ maxFreqs + "; Is microphone started?" + Microphone.IsRecording(null)+ "; audio.clip.frequency: " + Recorder.posnetek.clip.frequency );
		*/

		// tole vrne čudne rezultate
		Recorder.TestGetData ();

		// tole vrne 'pravilne' rezultate
		/*float[] arr1 = {1.107f};
		float[] arr2 = new float[1];
		string[] arr3 = new string[1];
		arr2[0] = (float) Recorder.GetFrequency ();
		arr3[0] = Recorder.GetNote ((int) arr2[0]);
		Result.TestDisplayResult (arr1, arr2, arr3);*/
	}

	public void Tune() {

		Recorder.Tune ();
	}

	public void Quit () {
	
		Application.Quit ();
	}

	public void Record () {

		Recorder.Record ();
	}

	public void StopRecord () {

		Recorder.StopRecord ();
	}

	public void Play () {

		Recorder.Play ();
	}

	public void GetFrequency () {
	
		frequency = Recorder.GetFrequency ();
		string result = "" + frequency;
		string note = Recorder.GetNote (frequency);
		Result.DisplayResult (result, note);
	}

	public void SaveSong () {
	
		Recorder.SaveSong ();
	}

	public void ToMainMenu() {

		SceneManager.LoadScene ("menu", LoadSceneMode.Single);
	}

	public void ToSongMenu() {
	
		SceneManager.LoadScene ("song select", LoadSceneMode.Single);
	}

	public void ToKuzaPazi() {

		SceneManager.LoadScene ("kuzapazi", LoadSceneMode.Single);
		PlayerPrefs.SetInt ("currentsong", 0);
	}

	public void ToCukSeJeOzenil() {

		SceneManager.LoadScene ("cuksejeozenil", LoadSceneMode.Single);
		PlayerPrefs.SetInt ("currentsong", 1);
	}

	public void ToPlugInBaby() {

		SceneManager.LoadScene ("pluginbaby", LoadSceneMode.Single);
		PlayerPrefs.SetInt ("currentsong", 2);
	}

	public void SetTempo60() {
	
		PlayerPrefs.SetInt ("tempo", 60);
	}

	public void SetTempo90() {

		PlayerPrefs.SetInt ("tempo", 90);
	}

	public void SetTempo120() {

		PlayerPrefs.SetInt ("tempo", 120);
	}
}
