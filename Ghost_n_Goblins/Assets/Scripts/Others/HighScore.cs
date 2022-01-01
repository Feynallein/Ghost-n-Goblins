using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

static public class HighScore {

    static public string[] GetCroppedText(TextAsset file) {
        return file.text.Split(new char[] { ':', '\n' });
    }

    static public List<string> GetHighScore(TextAsset highScoreFile, int textWidth) {
        List<string> finalOutput = new List<string>();
        string[] croppedText = GetCroppedText(highScoreFile);
        int neededLength;
        for (int i = 0; i < croppedText.Length; i+=2) {
            neededLength = textWidth - (croppedText[i].Length + croppedText[i + 1].Length);
            finalOutput.Add(croppedText[i] + new string('.', neededLength) + croppedText[i + 1]);
        }
        return finalOutput;
    }

    static public void WriteHighScore(TextAsset highScoreFile, int newScore, string name) {
        string[] croppedText = GetCroppedText(highScoreFile);
        if (newScore < int.Parse(croppedText[croppedText.Length - 1])) return;
        int i = 1;
        while (newScore < int.Parse(croppedText[i])) i += 2;
        croppedText[i] = newScore.ToString();
        croppedText[i - 1] = name;

        StreamWriter sr = new StreamWriter("Assets/Other/HighScore.txt");

       for(int j = 0; j < croppedText.Length; j += 2) {
            sr.WriteLine(croppedText[j] + ':' + croppedText[j + 1]);
        }
        sr.Close();
    }
}
