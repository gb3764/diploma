using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class fileWriter : MonoBehaviour {

	public static string path = "Assets/Resources/graphData.txt";

	public static void writeLine(string line) {
	
		StreamWriter writer = new StreamWriter(path, true);
		writer.WriteLine(line);
		writer.Close();
	}
}
