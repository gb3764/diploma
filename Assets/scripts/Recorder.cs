using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using B83.MathHelpers;

public class Recorder : MonoBehaviour {

	public static AudioSource posnetek;
	// samplerate in arraySize naj bosta 44100 in 8192, no further experimenting needed
	public static int samplerate = 44100; // last value: 44100
	public static int arraySize = 8192; // last value: 8192

	public static bool tuning = false;

	public static AudioClip intro;
	public static bool introWait = false;
	public static float introTime;
	public static float introTimeTemp;

	public static bool savingSong = false;
	public static float frequency;
	public static float savingSongTime;

	// dejanskih not na sekundo je 1/notesPerSecond
	// 0.0625 da 16 samplov na sekundo, kar je več kot dovolj
	// če je slab performance, zvečaj to cifro (16 na sekundo je a bit much)
	public static float notesPerSecond = 0.0625f;
	// 1/bmp = število not na sekundo
	public static float bpm = 0.5f;

	public static int burstNoteCounter = 0;

	int recordingTime = 10;
	// TEMPO CHANGE --------------------------------------------------------------------------------------- TEMPO CHANGE
	// trenutne vrednosti so za 120 bpm
	// dodaj še za 60 in 90 (treba še posnet)
	int[] recordingTimes = {9, 15, 19};
	// PLACEHOLDERS
	int[] recordingTimes60 = {18, 30, 38};
	int[] recordingTimes90 = {14, 23, 29};
	int[] recordingTimes120 = {9, 15, 19};

	// nova tabela 'toni' ima samo tone, ki jih lahko zaigraš na kitari s standardno uglasitvijo EADGBE
	public static string[] toni = {
		"E2", "F2", "F#2", "G2", "G#2", "A2", "A#2", "B2", "C3", "C#3", "D3", "D#3",
		"E3", "F3", "F#3", "G3", "G#3", "A3", "A#3", "B3", "C4", "C#4", "D4", "D#4",
		"E4", "F4", "F#4", "G4", "G#4", "A4", "A#4", "B4", "C5", "C#5", "D5", "D#5",
		"E5", "F5", "F#5", "G5", "G#5", "A5", "A#5", "B5", "C6"
	};

	/*public static string[] toni = {
		"E2", "F2", "F#2", "G2", "G#2", "A2", "A#2", "B2", "C3", "C#3", "D3", "D#3",
		"E3", "F3", "F#3", "G3", "G#3", "A3", "A#3", "B3", "C4", "C#4", "D4", "D#4",
		"E4", "F4", "F#4", "G4", "G#4", "A4", "A#4", "B4", "C5", "C#5", "D5", "D#5",
		"E5", "F5", "F#5", "G5", "G#5", "A5", "A#5", "B5", "C6", "C#6", "D6", "D#6",
		"E6", "F6", "F#6", "G6", "G#6", "A6", "A#6", "B6", "C7"
	};*/

	// nova tabela 'frekvence' ima prave frekvence tonov
	public static float[] frekvence = {
		82.407f,  87.307f,  92.499f,  97.999f,  103.826f, 110f, 116.541f, 123.471f, 130.813f, 138.591f, 146.832f, 155.563f,
		164.814f, 174.614f, 184.997f, 195.998f, 207.652f, 220f, 233.082f, 246.942f, 261.626f, 277.183f, 293.665f, 311.127f,
		329.628f, 349.228f, 369.994f, 391.995f, 415.305f, 440f, 466.164f, 493.883f, 523.251f, 554.365f, 587.330f, 622.254f,
		659.255f, 698.456f, 739.989f, 783.991f, 830.609f, 880f, 932.328f, 987.767f, 1046.502f
	};

	//zadnjih pet izračunanih, ne izmerjenih
	/*public static int[] frekvence = {
	    75,   80,   86,   91,   96,   102,  107,  113,  121, 126,  134,  142,
		150,  158,  169,  180,  191,  201,  212,  226,  239, 253,  269,  285,
		301,  320,  336,  355,  376,  401,  425,  452,  479, 506,  535,  567,
		602,  640,  680,  718,  761,  805,  855,  906,  958, 1009, 1073, 1135,
		1205, 1273, 1348, 1431, 1510, 1600, 1710, 1810, 1950
	};*/

	public static int[] slack = {
	    2, 3, 2, 2, 3, 2, 3, 3, 2, 3, 4, 4,
		4, 4, 5, 5, 5, 5, 6, 6, 7, 7, 8, 8, 8, 8, 8,
		10, 11, 12, 12, 13, 13, 14, 15, 17, 18, 18,
		18, 20, 21, 22, 23, 24, 25, 26, 29, 32, 33,
		35, 36, 37, 42, 45, 48, 52, 55
	};

	void Start () {

		posnetek = GetComponent<AudioSource>();
		intro = posnetek.clip;
		// tist -1.0f je SUPER GRD FIX
		// tam je zato, da začne snemat 'takoj' po koncu odštevanja (intro-ta)
		// TEMPO CHANGE --------------------------------------------------------------------------------------- TEMPO CHANGE
		// introTime naj bo tabela treh časov za vsak tempo
		// za 120 bpm je očitno idealno intro.length - 1.35f
		introTime = intro.length - 1.35f;

		/*
		int dspBufferLength, numBuffers;
		AudioSettings.GetDSPBufferSize( out dspBufferLength, out numBuffers );

		int numberOfMics = Microphone.devices.Length;
		int minFreqs = -1;
		int maxFreqs = -1;

		//AudioSettings.outputSampleRate = 44100;
		Microphone.GetDeviceCaps(Microphone.devices[0], out minFreqs, out maxFreqs);
		print ( "AudioSettings.ouputSampleRate: " + AudioSettings.outputSampleRate + "; audio buffer size: " + dspBufferLength +"; numBuffers: "+numBuffers + "; numberOfMics: "+ numberOfMics  + "; minFrequency: " + minFreqs + "; maxFrequency: "+ maxFreqs + "; Is microphone started?" + Microphone.IsRecording(null)+ "; audio.clip.frequency: " + posnetek.clip.frequency );
		*/
	}

	void Update() {

		if (introWait) {
		
			introTimeTemp -= Time.deltaTime;

			if (introTimeTemp <= 0.0f) {
			
				introWait = false;
				StartMic ();
			}
		}
		else if (savingSong) {

			savingSongTime -= Time.deltaTime;

			if (savingSongTime <= 0.0f) {

				savingSong = false;
				CancelInvoke ("SaveNote");
				//CancelInvoke("BurstNote");
				//WriteSong ();
				//Songs.startCompare(PlayerPrefs.GetInt("currentsong", 0));
				Songs.compareSelector(PlayerPrefs.GetInt("currentsong", 0));
				//WriteAndCompareSongs ();
				Target.image.enabled = false;
			} 
			/*else {
			
				SaveNote();	
			}*/

		}
	}

	public static void TestMicOn() {

		posnetek.clip = Microphone.Start (null, true, 10, samplerate);
		posnetek.loop = true;
		posnetek.volume = 0.01f;
		while (!(Microphone.GetPosition (null) > 0)) {
		}
		posnetek.Play ();
	}

	public static void TestGetData() {

		/*
		possible parameter changes:
		- arraySize (must be power of 2, try 16384)
		- samplerate
		- FFTWindow (now simplest/fastest Rectangular, try more complex like BlackmanHarris)
		- for loop condition - arraySize/2
		- frequency calculation - i * samplerate / arraySize
		*/

		/*float[] data = new float[arraySize];
		posnetek.GetSpectrumData (data, 0, FFTWindow.Rectangular);

		for (int i = 0; i < arraySize; i++) {
		
			float raw = data [i];
			float freq = ((float) i) * (((float) samplerate) / 2) / ((float) arraySize);
			string note = GetNote ((int) freq);
			string line = "" + raw + " " + freq;

			fileWriter.writeLine (line);
		}*/

		float frequency = 0f;
		float[] data = new float[arraySize];
		posnetek.GetSpectrumData (data, 0, FFTWindow.BlackmanHarris);

		float max = 0.0f;
		int index = 0;
		// arraySize/2, ker baje druga polovica neuporabna
		for (int j = 1; j < arraySize/2; j++) {

			if (max < data [j]) {

				max = data [j];
				index = j;
			}
		}

		float tempIndex = index;
		float tempSampleRate = samplerate / 2;
		float tempArraySize = arraySize;

		frequency = tempIndex * tempSampleRate / tempArraySize;

		Result.WriteLineResult ("unity: " + frequency);

		float[] data2 = new float[arraySize * 2];
		Complex[] cmplx = new Complex[arraySize * 2];
		posnetek.GetOutputData (data2, 0);

		for (int i = 0; i < data2.Length; i++) {
		
			cmplx [i] = new Complex (data2[i], 0);
		}

		FFT.CalculateFFT (cmplx, false);

		max = 0.0f;
		index = 0;
		for (int j = 1; j < cmplx.Length/2; j++) {

			if (max < 4+(float)cmplx[j].magnitude*2) {

				max = 4+(float)cmplx[j].magnitude*2;
				index = j;
			}
		}

		tempIndex = index;
		tempSampleRate = samplerate / 2;
		tempArraySize = arraySize;

		frequency = tempIndex * tempSampleRate / tempArraySize;

		Result.WriteLineResult ("FFT: " + frequency);
	}

	/*public static void TestGetData() {
	
		float[] data = new float[arraySize];
		float[] topTen = new float[10];
		float[] topTenFreq = new float[10];
		string[] topTenStr = new string[10];
		float frequency = 0;

		posnetek.GetSpectrumData (data, 0, FFTWindow.Rectangular);

		// find 10 biggest values
		for (int i = 0; i < topTen.Length; i++) {
		
			float max = 0;
			int index = 0;

			// set some value for comparison
			for (int k = 0; k < topTen.Length; k++) {
			
				if (data [k] != 0) {
				
					max = data [k];
					break;
				}
			}

			// find max
			for (int j = 0; j < arraySize / 2; j++) {
			
				if (max < data [j] && data[j] != 0) {
				
					max = data [j];
					index = j;
				}
			}

			topTen [i] = max;
			topTenFreq[i] = (float) index * ((float) samplerate)/2 / (float) arraySize;
			topTenStr [i] = GetNote ((int) topTenFreq[i]);

			data [index] = 0;
		}

		Result.TestDisplayResult (topTen, topTenFreq, topTenStr);
	}*/

	public static void Record () {

		Songs.ClearRecording ();
		songScore.DisplayResult ("");
		stars.Disable ();
		posnetek.clip = intro;
		posnetek.volume = 1.0f;
		Play ();

		// hard kodirano za kuža pazi NOT ANYMORE

		Target.Countdown ();

		//noteSpawner.kuzaPazi ();
		noteSpawner.Spawn(PlayerPrefs.GetInt("currentsong", 0));
		// end hard code

		introTimeTemp = introTime;
		introWait = true;
		//Invoke ("StartMic", 2);
	}

	void StartMic() {
	
		//posnetek.clip = Microphone.Start (null, false, recordingTimes[PlayerPrefs.GetInt("currentsong", 0)], samplerate);

		int tempRecTime = 0;
		int tempo = PlayerPrefs.GetInt("tempo", 60);

		if (tempo == 60) {
		
			tempRecTime = recordingTimes60 [PlayerPrefs.GetInt ("currentsong", 0)];
		}
		else if (tempo == 90) {

			tempRecTime = recordingTimes90 [PlayerPrefs.GetInt ("currentsong", 0)];
		}
		else if (tempo == 120) {

			tempRecTime = recordingTimes120 [PlayerPrefs.GetInt ("currentsong", 0)];
		}

		posnetek.clip = Microphone.Start (null, false, tempRecTime, samplerate);

		// dodano za real-time analizo
		posnetek.volume = 0.01f;
		while (!(Microphone.GetPosition (null) > 0)) {
		}

		//uporabljeno za analiziranje že posnetega kužapazija
		/*
		posnetek = Songs.posnetek;
		posnetek.volume = 1.0f;
		*/

		Play ();
		//savingSongTime = (float) recordingTime;
		//savingSongTime = (float) recordingTimes[PlayerPrefs.GetInt("currentsong", 0)];
		savingSongTime = (float) tempRecTime;
		savingSong = true;
		// end of dodano

		InvokeRepeating ("SaveNote", 0, notesPerSecond);
		//InvokeRepeating ("SaveNote", 0, bpm);
		//InvokeRepeating("BurstNote", 0, bpm);

		Debug.Log ("recording has started");

		//endSound.Play ((float) recordingTime);
		//endSound.Play ((float) recordingTimes[PlayerPrefs.GetInt("currentsong", 0)]);
		endSound.Play ((float) tempRecTime);
	}

	public static void StopRecord () {

		Microphone.End (null);
	}

	public static void Play () {
	
		posnetek.Play ();
	}

	public static float GetFrequency() {

		float frequency = 0f;
		float[] data = new float[arraySize];
		posnetek.GetSpectrumData (data, 0, FFTWindow.BlackmanHarris);

		float max = 0.0f;
		int index = 0;
		// arraySize/2, ker baje druga polovica neuporabna
		for (int j = 1; j < arraySize/2; j++) {

			if (max < data [j]) {

				max = data [j];
				index = j;
			}
		}

		float tempIndex = index;
		float tempSampleRate = samplerate / 2;
		float tempArraySize = arraySize;

		frequency = tempIndex * tempSampleRate / tempArraySize;

		return frequency;
	}

	/*
	opusti tabelo slack
	preciznost zaznavanja je 2.69 Hz, kar je problem pri nižjih frekvencah (pri B2 bi mogl bit že kul)
	 -> mogoče za prvih par nižjih tonov uporabi custom slack?
	za vse ostalo dej absolutno razliko med zaznanim tonom in (trenutnim tonom + 2.69 Hz)
	*/

	public static string GetNote(float frequency) {

		string result = "";

		int idx = 0;
		int length = toni.Length;
		while (result == "") {

			// zaznana frekvenca je previsoka
			if (idx >= length) {

				result = "note not detected";
				break;
			}

			// zaznana frekvenca je prenizka
			if (idx == 0) {

				if (frequency < frekvence [idx] - slack [idx]) {

					result = "note not detected";
					break;
				}
			}

			//pogoj samo za desno stran intervala
			if (Mathf.Abs(frekvence[idx] - frequency) < 2.69f) {

				result = toni[idx];
				break;
			}

			idx++;
		}

		return result;
	}

	/*public static string GetNote(float frequency) {

		string result = "";

		int idx = 0;
		int length = toni.Length;
		while (result == "") {

			// zaznana frekvenca je previsoka
			if (idx >= length) {

				result = "note not detected";
				break;
			}

			// zaznana frekvenca je prenizka
			if (idx == 0) {

				if (frequency < frekvence [idx] - slack [idx]) {

					result = "note not detected";
					break;
				}
			}

			//pogoj samo za desno stran intervala
			if (frequency <= frekvence [idx] - slack [idx]) {

				result = toni[idx];
				break;
			}

			idx++;
		}

		return result;
	}*/

	/*public static int GetFrequency() {

		int frequency = 0;
		float[] data = new float[arraySize];
		// prej BlackmanHarris, zdej Rectangular pa isto natančno lol
		posnetek.GetSpectrumData (data, 0, FFTWindow.Rectangular);

		// mogoče je pa 'max' prevelka cifra (mogoče so vse pod 0?)
		// set to first element of array
	    // -> vse vrednosti so pozitivne, no fix needed

		float max = 0.0f;
		int index = 0;
		// arraySize/2, ker baje druga polovica neuporabna
		for (int j = 1; j < arraySize/2; j++) {
		
			if (max < data [j]) {
			
				max = data [j];
				index = j;
			}
		}
		frequency = index * (samplerate/2) / arraySize;
		//frequency = index * samplerate / arraySize;

		return frequency;
	}*/

	/*public static string GetNote(int frequency) {
	
		string result = "";

		int idx = 0;
		int length = toni.Length;
		while (result == "") {

			if (idx >= length) {
			
				result = "note not detected";
				break;
			}

			if (idx == 0) {
			
				if (frequency < frekvence [idx] - slack [idx]) {

					result = "note not detected";
					break;
				}
			}

			//pogoj samo za desno stran intervala
			else if (frequency <= frekvence[idx] + slack[idx]) {

				result = toni[idx];
				break;
			}

			idx++;
		}

		return result;
	}*/

	public static void Tune() {
	
		if (!tuning) {

			// 10 sekund optimalno?
			posnetek.clip = Microphone.Start (null, true, 10, samplerate);
			posnetek.loop = true;
			//posnetek.mute = true;
			posnetek.volume = 0.01f;
			while (!(Microphone.GetPosition (null) > 0)) {
			}
			posnetek.Play ();
			tuning = true;
		}
		else {

			tuning = false;
			string result = "";
			string note = "";
			Result.DisplayResult (result, note);
		}
	}

	public static void SaveSong() {

		Play ();
		savingSongTime = posnetek.clip.length;
		savingSong = true;
	}

	public static void WriteSong() {

		// <= ?
		for (int i=0; i < Songs.song1.Count; i++) {

			Debug.Log (Songs.song1 [i]);
		}
	}

	public static void WriteAndCompareSongs() {
	
		int idx = 0;
		string tempNote = "";
		string note = "";
		int noteSize = 4;

		for (int i = 16; i < Songs.song1.Count; i++) {

			if (Songs.cuksejeozenilNotes [idx] == "X") {
			
				note = tempNote;
			} else {
			
				note = Songs.cuksejeozenilNotes [idx];
				tempNote = note;
			}

			noteSize--;
			if (noteSize == 0) {
			
				idx++;
				if (idx >= Songs.cuksejeozenilNotes.Length) {
				
					break;
				}
				noteSize = 4;
			}

			Debug.Log (Songs.song1 [i] + " " +  note);
		}
	}

	public void SaveNote() {

		if (savingSong) {
		
			frequency = GetFrequency ();
			string note = GetNote ((int) frequency);
			Songs.song1.Add (note);
			//Debug.Log ("note saved");
			//Songs.CompareNote (note);
			//Songs.cmp(note);
			//Songs.passiveCompare(note);
		}
	}

	public void BurstNote() {
	
		Debug.Log ("bam");
		burstNoteCounter = 0;
		InvokeRepeating ("SingleNote", 0, 0.01f);
	}

	public void SingleNote() {

		frequency = GetFrequency ();
		string note = GetNote ((int) frequency);
		Songs.burstNote.Add (note);
		burstNoteCounter++;

		if (burstNoteCounter >= 10) {
		
			CancelInvoke ("SingleNote");
			Songs.burstCompare ();
		}
	}
}