using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Songs : MonoBehaviour {

	public static List<string> song1;
	public static AudioSource posnetek;
	public static int recordingIndex = 0;
	public static int songIndex = 0;
	public static bool comparing = false;
	public static bool firstNoteFound = false;
	public static List<string> tempNote;
	public static int songIndicesIndex = 0;
	public static List<int> recordingScore;
	public static bool songEnd = false;
	public static bool isGood = false;

	public static int tempIndexCounter = 0;
	public static int newIndex = 0;
	public static List<bool> playedNotesIsOk;
	public static List<string> burstNote;
	public static int burstIndex = 0;

	public static string currentNote = "";
	public static bool noteFound = false;
	public static int noteCounter = 0;
	public static int newKuzaPaziIndex = 0;

	public static string[] playedData;
	public static string[] tempoData;
	public static string[] noteData;
	public static string[] lestvica = {"C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B", "note not detected"};

	public static string[] songArray;

	// vsi toni so brez številk, zaradi slabe implementacije zaznavanja frekvenc
	// zato se primerja samo tone katerekoli oktave

	// zaznani toni originala, jih je 100
	public static string[] kuzapazi = {
		"note not detected", "C", "C", "C", "C", "C", "C", "C", "C", "C",
		"C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "D", "D",
		"D", "D", "D", "D", "D", "D", "D", "D", "D", "D", "D", "D", "D", "D",
		"D", "D", "D", "D", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E",
		"D", "D", "D", "D", "D", "D", "D", "D", "D", "D", "D", "C", "C", "C",
		"C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C", "C",
		"C", "C", "C", "C", "C", "note not detected", "note not detected",
		"note not detected", "note not detected", "note not detected",
		"note not detected", "note not detected", "note not detected",
		"note not detected", "note not detected", "note not detected",
		"note not detected", "note not detected", "note not detected",
		"note not detected", "note not detected"
	};

	// po vrsti razvrščeni različni toni
	//public static string[] kuzapaziNotes = { "C", "D", "E", "D", "C" };
	// indeksi začetkov istoležečih tonov iz tabele kuzapaziNotes
	// spodnja tabela ima +1 dolžino od zgornje
	// zaradi končnega indeksa, ki označje konec igranja (zadnjo zaznano noto)
	//public static int[] kuzapaziIndices = { 1, 22, 42, 52, 63, 85 };

	// better solution:
	// po vrsti razvrščene igrane note
	public static string[] kuzapaziNotes = {
		"C", "C",
		"C", "C",
		"D", "D",
		"D", "D",
		"E", "E",
		"D", "D",
		"C", "C",
		"C", "X",
	};

	// indeksi začetkov igranih not na istoležečih poziciajh
	// iz tabele kuzapaziNotes
	// zadnji indeks je zadnja zaznana nota
	// po tem indeksu je tišina
	public static int[] kuzapaziIndices = {
		1,  6,  11, 16,
		22, 27, 32, 37,
		42, 47,
		52, 57,
		63, 68, 73, 78,
		85
	};

	// 'iksi' so pavze, tam so zato da spawnNotes pravilno dela
	// vsaka vrstica predstavlja eno sekundo
	public static string[] cuksejeozenilNotes = {
		"C", "D", "E", "F",
		"G", "X", "G", "X",
		"A", "X", "A", "X",
		"G", "X", "X", "X",
		"A", "X", "A", "X",
		"G", "X", "X", "X",
		"F", "F", "F", "F",
		"E", "X", "E", "X",
		"D", "X", "D", "X",
		"G", "X", "X", "X",
		"F", "F", "F", "F",
		"E", "X", "E", "X",
		"D", "X", "D", "X",
		"C", "X", "X", "X",
	};

	public static string[] pluginbabyNotes = {
		"A#", "X", "B", "X", "C#", "X", "A#", "X",
		"B", "X", "C#", "X", "D", "X", "B", "X",
		"C#", "X", "D", "X", "E", "X", "F#", "G",
		"F#", "X", "E", "X", "D", "X", "C#", "X",
		"B", "X", "C#", "X", "D", "X", "B", "X",
		"C#", "X", "D", "X", "E", "X", "C#", "X",
		"D", "X", "E", "X", "F#", "X", "F#", "G",
		"F#", "X", "F", "X", "F#", "X", "F", "X",
		"F#", "X", "D", "X", "A", "X", "F#", "X",
		"A", "X", "D", "X", "F#", "X", "G#", "X",
		"A", "X", "G#", "X", "F#", "X", "F", "X",
		"F#", "X", "D", "X", "C#", "X", "D", "X",
		"A#", "X", "B", "X", "C#", "X", "A#", "X",
		"B", "X", "C#", "X", "D", "X", "B", "X",
		"C#", "X", "D", "X", "E", "X", "F#", "G",
		"F#", "X", "E", "X", "D", "X", "C#", "X",
		"B", "X", "X", "X", "X", "X", "X", "X",
		"X", "X", "X", "X", "X", "X", "X", "X",
	};


	void Start () {

		song1 = new List<string> ();
		posnetek = GetComponent<AudioSource>();
		tempNote = new List<string> ();
		recordingScore = new List<int> ();
		playedNotesIsOk = new List<bool>();
		burstNote = new List<string> ();
	}

	void Update() {
	
		/*
		psevdokoda:
		if (comparing) {

			če še ni blo nč druzga razn 'note not detected'
				-> skip

			else
				'nabiraj' zaznane note do naslednje note v tabeli kuzapaziIndices
				ustavi se ko indeks zanke pride do naslednjega indeksa v tej tabeli
				primerjaj posneto in igrano tabelo na tem intervalu
				po nekem kriteriju določi, če je igran interval pravilen
				mogoče če je več kot pol enakih elementov?
				končni score ubistvu sestavljen iz pravilnosti igranih not (intervalov)
		}
		*/
		/*if (comparing) {
		
			if (firstNoteFound) {
			
				if (songIndex >= kuzapaziIndices [songIndicesIndex]) {
				
					// new note, compare!
					CompareInterval();
				}
				else {
				
					// add note to tempNote
					tempNote.Add(song1[recordingIndex]);
					songIndex++;
				}
			}
			else {
			
				if (song1 [recordingIndex] != "note not detected") {

					tempNote.Add(song1[recordingIndex]);
					firstNoteFound = true;
					songIndex = kuzapaziIndices [songIndicesIndex];
					songIndicesIndex++;
				}
			}
			recordingIndex++;
		}*/
	}

	public static void Compare() {
	
		string[] zaigrano = song1.ToArray ();
		int l1 = kuzapazi.Length;
		int l2 = zaigrano.Length;
		float score = 0;
		int orgIdx = -1;
		int idx = -1;

		int length = Mathf.Min (l1, l2);

		for (int i = 0; i < length; i++) {
		
			if (orgIdx == -1 && kuzapazi [i] != "note not detected") {

				orgIdx = i;
			}

			if (idx == -1 && zaigrano [i] != "note not detected") {

				idx = i;
			}

			if (orgIdx != -1 && idx != -1) {
			
				break;
			}
		}

		if (orgIdx == -1) {
		
			orgIdx = 0;
		}

		if (idx == -1) {
		
			idx = 0;
		}

		int n = orgIdx;
		int m = idx;

		for (int i = 0; i < length; i++) {
		
			if (zaigrano [m].Substring (0, 1) == kuzapazi [n].Substring (0, 1)) {

				score++;
			}

			n++;
			m++;

			if (n >= length || m >= length) {

				break;
			}
		}

		/*for (int i = 0; i < length; i++) {
		
			if (zaigrano [i].Substring (0, 1) == kuzapazi [i].Substring (0, 1)) {
			
				score++;
			}
		}*/

		float result = (score / (length - orgIdx)) * 100;

		Debug.Log (score + " " + length + " " + orgIdx + " " + result);
		songScore.DisplayResult("Score: " + ((int) result));
	}

	public static void ClearRecording() {

		//song1.Clear ();
		song1 = new List<string> ();
		recordingIndex = 0;
		songIndex = 0;
		songIndicesIndex = 0;
		firstNoteFound = false;
		songEnd = false;
		newIndex = 0;
		playedNotesIsOk = new List<bool> ();
		burstNote = new List<string> ();
		burstIndex = 0;

		newKuzaPaziIndex = 0;
		noteCounter = 0;
		currentNote = "";
		noteFound = false;
	}

	public void PlaySong() {

		posnetek = GetComponents<AudioSource>()[PlayerPrefs.GetInt("currentsong", 0)];
		posnetek.volume = 0.75f;
		posnetek.Play ();
	}

	public static void CompareNote(string note) {
		
		//Debug.Log ("CompareNote called");

		// dodaj bool spremenljivko za 'first note found'
		/*if (true) {


		}*/

		/*if (kuzapazi [songIndex].Substring (0, 1) == song1 [recordingIndex].Substring (0, 1)) {

			songScore.DisplayResult ("Good");
		}
		else {
		
			songScore.DisplayResult ("Oops");
		}
		recordingIndex++;
		songIndex++;*/

		if (!songEnd) {
			
			// iskanje prve zaznane note ubistvu nepotrebno,
			// ker bo itak vizualizacija pravilnih not,
			// ki bodo kazale točno kdaj začet
			// odstopanje je pač late start and as such odbitek
			// pri ritmičnem ocenjevanju
			
			// (!firstNoteFound && note != "note not detected")
			if (!firstNoteFound) {
		
				firstNoteFound = true;
				songIndex = kuzapaziIndices [songIndicesIndex];
				songIndicesIndex++;
				//Debug.Log ("first note found! huzzah!");
			}

			if (firstNoteFound) {

				if (songIndex >= kuzapaziIndices [songIndicesIndex]) {

					//Debug.Log ("compare thy interval, sire!");
					// new note, compare!
					CompareInterval ();
				} //else {

					// add note to tempNote
					//Debug.Log ("alas, another note is aquired");
					tempNote.Add (note);
					songIndex++;
				//}
			}
			//Debug.Log (" ");
		}
		/*else {

			if (note != "note not detected") {

				tempNote.Add(note);
				firstNoteFound = true;
				songIndex = kuzapaziIndices [songIndicesIndex];
				songIndicesIndex++;
			}
		}*/
	}

	public static void CompareInterval() {

		//Debug.Log ("CompareInterval called");

		int score = 0;
		int length = tempNote.Count;

		for (int i = 0; i < length; i++) {

			if (tempNote [i].Substring (0, 1) == kuzapaziNotes[songIndicesIndex - 1]) {

				score++;
			}
			//tempIndexCounter++;
		}
		
		if (score > 0) {

			isGood = true;
			//Debug.Log ("'twas good m'lard " + songIndicesIndex);
		}
		else {

			isGood = false;
			//Debug.Log ("jajaja los lowes "  + songIndicesIndex);
		}

		//songScore.DisplayResult("VERY NAISU " + songIndicesIndex + " " + kuzapaziNotes[songIndicesIndex-1]);
		//Debug.Log ("this score: " + score + " tmpIdx: " + tempIndexCounter);
		recordingScore.Add (score);
		songIndicesIndex++;
		tempNote = new List<string> ();

		if (songIndicesIndex >= kuzapaziIndices.Length) {

			//Debug.Log ("song has ended??");
			songEnd = true;
		}
	}

	public static void cmp(string note){

		if (newIndex < kuzapaziNotes.Length) {	

			Debug.Log (note);

			if (kuzapaziNotes [newIndex] == note.Substring (0, 1)) {

				//Debug.Log ("good");
				playedNotesIsOk.Add(true);
			}
			else {

				//Debug.Log ("bad");
				playedNotesIsOk.Add(false);
			}
			newIndex++;
		}
	}

	public static void burstCompare() {

		if (burstIndex < kuzapaziNotes.Length) {

			int score = 0;

			for (int i = 0; i < burstNote.Count; i++) {

				if (burstNote [i].Substring (0, 1) == kuzapaziNotes [burstIndex]) {

					score++;
				}
			}
			
			//Debug.Log (kuzapaziNotes[burstIndex] + " " + score);
			
			// currently shitty condition, make it better
			if (score > (burstNote.Count) / 2) {

				playedNotesIsOk.Add (true);
			} else {

				playedNotesIsOk.Add (false);
			}

			burstIndex++;
			burstNote = new List<string> ();
			//Debug.Log ("end burst");
		}
	}

	public static void passiveCompare(string note) {

		if (newKuzaPaziIndex < kuzapaziNotes.Length) {

			if (!noteFound && note != "note not detected") {

				currentNote = note;
				noteFound = true;
				noteCounter++;
			} else {

				if (note == currentNote) {

					noteCounter++;
					// večje ali enako tri, na nek način minimalno število, da je 'ziher'
					if (noteCounter >= 3) {

						Debug.Log ("note played is " + note);
						noteCounter = 0;
						// trenutna implementacija čaka da krogec pride do tarče
						// šele nato se primerno obarva
						// spremeni da se obarva takoj ko se naredi spodnja evaluacija
						// glej .txt za psevdokodo
						if (note.Substring (0, 1) == kuzapaziNotes [newKuzaPaziIndex]) {

							playedNotesIsOk.Add (true);
						} else { 

							playedNotesIsOk.Add (false);
						}
						newKuzaPaziIndex++;
					}
				} else {

					noteCounter = 0;
					currentNote = "";
					noteFound = false;
				}
			}
		}
	}

	public static void CompareSimple() {

		int length = Mathf.Min (songArray.Length, song1.Count);
		int score = 0;

		for (int i = 0; i < length; i++) {

			if (songArray [i].Substring (0, 1) == song1 [i].Substring (0, 1)) {

				score += 10;
			}
		}

		float maxScore = length * 10;
		int songIdx = PlayerPrefs.GetInt ("currentsong", 0);
		string songName = playerStats.songNames [songIdx];
		int starCount = PlayerPrefs.GetInt (songName);
		
		if (score >= (int)(maxScore * 0.75)) {

			if (starCount < 3) {

				PlayerPrefs.SetInt (songName, 3);
				playerStats.songStars [songIdx] = 3;
			}

			stars.Set (3);
			//Debug.Log ("3");
		}
		else if (score >= (int)(maxScore * 0.5)) {

			if (starCount < 2) {

				PlayerPrefs.SetInt (songName, 2);
				playerStats.songStars [songIdx] = 2;
			}

			stars.Set (2);
			//Debug.Log ("2");
		}
		else if (score >= (int)(maxScore * 0.33)) {

			if (starCount < 1) {

				PlayerPrefs.SetInt (songName, 1);
				playerStats.songStars [songIdx] = 1;
			}

			stars.Set (1);
			//Debug.Log ("1");
		}
		else {

			stars.Set (0);
		}

		songScore.DisplayResult("Score: " + score);
		//stars.Refresh ();
		stars.Enable ();
	}

	public static void startCompare(int song) {

		if (song == 0) {

			songArray = (string[]) kuzapazi.Clone ();
		}
		else if (song == 1) {

			// add other array
		}
		else if (song == 2) {

			// add other array
		}

		CompareSimple ();
	}

	public static void compareSelector(int song) {

		int noteSize = 1;

		if (song == 0) {

			songArray = (string[]) kuzapaziNotes.Clone ();
			//noteSize = 8;
		}
		else if (song == 1) {

			songArray = (string[]) cuksejeozenilNotes.Clone ();
			//noteSize = 4;
		}
		else if (song == 2) {

			songArray = (string[]) pluginbabyNotes.Clone ();
			//noteSize = 2;
		}

		// noteSize odslej ne bo odvisen od pesmi, temveč od bpm-ja
		// GetFrequency zajame 16x na sekundo
		// torej je noteSize za 60 bpm 16
	    // za 120 je 8
		// za 90 pa 12
		// 16x na sekundo je morda odveč, lahko manjkrat, če bo zato boljši performance

		int currentTempo = PlayerPrefs.GetInt ("tempo", 60);

		if (currentTempo == 60) {

			noteSize = 16;
		}
		else if (currentTempo == 90) {

			noteSize = 12;
		}
		else if (currentTempo == 120) {

			noteSize = 8;
		}
		
		compareAdvanced (noteSize);
	}

	public static void compareAdvanced(int noteSize) {

		int length = song1.Count;
		int score = 0;
		int tempNoteSize = noteSize;
		int noteIdx = 0;
		string tempNote = "";
		string note = "";
		int tempScore = 0;
		int interval = 1;

		bool firstInterval = true;
		bool rushing = false;
		bool dragging = false;
		int rushCounter = 0;
		int dragCounter = 0;
		int lagCounter = 0;
		int tempoScore = 0;
		bool lagging = false;
		string tempoNote = "";

		playedData = new string[songArray.Length];
		tempoData = new string[songArray.Length];
		noteData = new string[songArray.Length];
		int playedDataIndex = 0;
		int tempoDataIndex = 0;
		int noteDataIndex = 0;

		for (int i = noteSize*4 + 8; i < length; i++) {

			if (songArray [noteIdx] == "X") {

				note = tempNote;
			}
			else {

				note = songArray [noteIdx];
				tempNote = note;
			}

			// preveri, če je ton dolg en ali dva znaka (npr. 'C' ali 'C#')
			if (song1 [i].Length > 2 && song1[i] != "note not detected") {

				interval = 2;
			}
			else {

				interval = 1;
			}

			/*
			psevdo-sih koda za preverjanje tempa
			
			v taprvem intervalu preveri, ali prehiteva ali zamuja
			-> če je prvih par napačnih (oz. note not detected) in šele nato pravilne -> zamuja
			-> če je prvih par pravilnih nato pa v istem intervalu do konca napačne (naslednje pravilne) -> prehiteva
			glede na eno ali drugo določi prehiteva = false/true
			če zamuja (prehiteva = false)
			-> v intervalu išči, če bo zaigral naslednjo pravilno noto
			   (ubistvu ni nujno da je pravilna - čim zaigra drugo noto prehitr je to prehitevanje)
			   -> je pa nujno da ni zaigrana trenutna pravilna nota, ker to pomen da še zmeri zamuja
			   SEPRAVI
               V INTERVALU IŠČI NOTO, KI NI TRENUTNO PRAVILNA, NITI NI PREJŠNJA
			   če jo -> prehiteva = true
			če prehiteva (prehiteva = true)
			-> v intervalu išči katerokoli drugo noto
			   če so v celem intervalu vse enake -> prehiteva = false

			točkovanje

			če zamuja
			-> število vseh - število napačnih not, ubistvu isto kot je za navadno preverjanje pravilnosti
			če prehiteva
			-> število vseh - število napačnih not prejšnjega intervala, ki so pravilne v trenutnem

			naredi nov list, v katerega za vsako igrano noto zapišeš:
			- ali je bila pravilna
			- katera nota je bila zaigrana (ni nujno) or is it?
			- ali je bila prezgodnja ali prepozna
			za to nared več listov al pa magar vse podatke v en string sprav
			do note:
			vse note, ki niso draggane so rushane
			če je pri rushanju lagCounter == noteSize, potem sploh ni blo rushan ampak kul
			če zaznaš rushanje, morš to zapisat za naslednjo noto, ne trenutno
			(glej listke)
			*/

			// v prvem intervalu odkrij, če prehiteva ali zamuja
			if (firstInterval && song1 [i].Substring (0, interval) != note) {

				//dragCounter++;
				//lagCounter++;
				dragging = true;
				firstInterval = false;
				lagging = true;
				tempoNote = note;
			}
			else if (firstInterval && song1 [i].Substring (0, interval) == note) {

				//rushCounter++;
				//lagCounter++;
				rushing = true;
				firstInterval = false;
				lagging = true;
			}
			//firstInterval = false;
			else if (lagging && dragging && song1 [i].Substring (0, interval) != tempoNote) {

				//dragging = false;
				lagging = false;
			}
			else if (lagging && rushing && song1 [i].Substring (0, interval) != note) {

				//rushing = false;
				lagging = false;
			}
			
			if (lagging) {

				lagCounter++;
			}

			if (song1 [i].Substring (0, interval) == note) {
				
				// niti ni važnen številčen score, važne so zvezdice
				tempScore++;
			}

			// mogoče dodeli score na vsak note, in ne na vsak zapis?
			
			tempNoteSize--;

			if (tempNoteSize == 0) {

				if (tempScore >= noteSize / 2) {

					score += 10;
					tempScore = 0;
					//playedData [playedDataIndex] += "good";
					noteData [noteDataIndex] = "good";
				}
				else {

					//playedData [playedDataIndex] += "nope";
					noteData [noteDataIndex] = "bad";
				}

				noteDataIndex++;
				noteIdx++;
				tempNoteSize = noteSize;

				//firstInterval = false;
				if (rushing) {

					tempoScore += lagCounter;

					if (lagCounter == noteSize) {

						//playedData [playedDataIndex] += "good";
						if (tempoData [tempoDataIndex] != "rush") {
						
							tempoData [tempoDataIndex] = "good";
						}
					}
					else {

						//playedData [playedDataIndex + 1] += "rush";
						if (tempoDataIndex + 1 < tempoData.Length) {
						
							tempoData [tempoDataIndex + 1] = "rush";
						}
					}
				}
				else if (dragging) {

					tempoScore += noteSize - lagCounter;
					//playedData [playedDataIndex] += "drag";
					if (tempoData [tempoDataIndex] != "rush") {
					
						tempoData [tempoDataIndex] = "drag";
					}
				}
				// else se nikoli ne zgodi, ker če ne dragga, potem rusha
				else {

					tempoScore += noteSize;
					//playedData [playedDataIndex] += "good";
					if (tempoData [tempoDataIndex] != "rush") {
					
						tempoData [tempoDataIndex] = "good";
					}
				}
				//playedDataIndex++;
				tempoDataIndex++;
				lagCounter = 0;
				rushing = false;
				dragging = false;
				lagging = false;
				firstInterval = true;

				// find most common note (aka note played)
				// condition should be correct, no?
				string currentNoteCompare = "";
				int[] playedNoteCounter = new int[lestvica.Length]; 
				for (int j = i - noteSize + 1; j < i; j++) {

					if (song1 [j].Length == 2) {

						currentNoteCompare = song1 [i].Substring (0, 2);
					}
					else if (song1 [j].Length == 1) {

						currentNoteCompare = song1 [i].Substring (0, 1);
					}
					else {

						currentNoteCompare = song1 [i];
					}

					for (int k = 0; k < lestvica.Length; k++) {

						if (currentNoteCompare == lestvica [k]) {

							playedNoteCounter [k]++;
						}
					}
				}

				int currentIndex = 0;
				int currentMax = 0;
				for (int j = 0; j < playedNoteCounter.Length; j++) {

					if (playedNoteCounter [j] > currentMax) {

						currentMax = playedNoteCounter [j];
						currentIndex = j;
					}
				}
				playedData [playedDataIndex] = lestvica [currentIndex];
				playedDataIndex++;

				if (noteIdx >= songArray.Length) {

					break;
				}
			}
		}

		//float maxScore = length * 10;
		//float maxScore = songArray.Length * 10;
		float maxScore = Recorder.songNoteSums[PlayerPrefs.GetInt("currentsong", 0)] * noteSize;
		int songIdx = PlayerPrefs.GetInt ("currentsong", 0);
		string songName = playerStats.songNames [songIdx];
		int starCount = PlayerPrefs.GetInt (songName);

		if (score >= (int)(maxScore * 0.75)) {

			if (starCount < 3) {

				PlayerPrefs.SetInt (songName, 3);
				playerStats.songStars [songIdx] = 3;
			}

			stars.Set (3);
			//Debug.Log ("3");
		}
		else if (score >= (int)(maxScore * 0.5)) {

			if (starCount < 2) {

				PlayerPrefs.SetInt (songName, 2);
				playerStats.songStars [songIdx] = 2;
			}

			stars.Set (2);
			//Debug.Log ("2");
		}
		else if (score >= (int)(maxScore * 0.33)) {

			if (starCount < 1) {

				PlayerPrefs.SetInt (songName, 1);
				playerStats.songStars [songIdx] = 1;
			}

			stars.Set (1);
			//Debug.Log ("1");
		}
		else {

			stars.Set (0);
		}

		songScore.DisplayResult("Score: " + score);
		//stars.Refresh ();
		stars.Enable ();
	}
}
