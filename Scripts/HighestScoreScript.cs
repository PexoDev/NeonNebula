using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class HighestScoreScript : MonoBehaviour {
    public bool SaveHighestScore(int NewScore)
    {
        
        if (NewScore > LoadHighestScore())
        {
            Debug.Log("NEW HIGHEST SCORE: " + NewScore);
            string filepath = Application.persistentDataPath + "/save.pexo";
            BinaryFormatter binform = new BinaryFormatter();
            FileStream savefile = File.Create(filepath);
            binform.Serialize(savefile, NewScore);
            savefile.Close();
            return true;
        }
        return false;
    }

    public int LoadHighestScore()
    {
        
        string filepath = Application.persistentDataPath + "/save.pexo";
        if (!File.Exists(filepath))
        {
            BinaryFormatter binform = new BinaryFormatter();
            FileStream savefile = File.Create(filepath);
            binform.Serialize(savefile, 0);
            savefile.Close();
            return LoadHighestScore();
        }

        else
        {
            BinaryFormatter binform = new BinaryFormatter();
            FileStream savefile = File.Open(filepath, FileMode.Open);
            int LoadedHigestScore = (int)binform.Deserialize(savefile);
            savefile.Close();
            return LoadedHigestScore;
        }
    }

    public void ResetHighestScore()
    {
        string filepath = Application.persistentDataPath + "/save.pexo";
        BinaryFormatter binform = new BinaryFormatter();
        FileStream savefile = File.Create(filepath);
        binform.Serialize(savefile, 0);
        savefile.Close();
    }

}
