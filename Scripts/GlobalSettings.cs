using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GlobalSettings : MonoBehaviour {
    public static int GraphicsQuality = 0;
    public static float MusicVolume = 1f;

    private void Awake()
    {
        LoadSettings();
    }

    public static void SaveSettings()
    {
            string filepath = Application.persistentDataPath + "/Settings.pexo";
            SavingSettings tmp = new SavingSettings();
            tmp.MusicVolume = MusicVolume;
            tmp.GraphicsQuality = GraphicsQuality;
            BinaryFormatter binform = new BinaryFormatter();
            FileStream savefile = File.Create(filepath);
            binform.Serialize(savefile, tmp);
            savefile.Close();
            AudioListener.volume = MusicVolume;
    }

    public static void LoadSettings()
    {
        string filepath = Application.persistentDataPath + "/Settings.pexo";
        BinaryFormatter binform = new BinaryFormatter();
        if (!File.Exists(filepath))
        {
            GraphicsQuality = 0;
            MusicVolume = 1f;
        }
        else
        {
            FileStream savefile = File.Open(filepath, FileMode.Open);
            SavingSettings tmp = (SavingSettings)binform.Deserialize(savefile);
            savefile.Close();

            GraphicsQuality = tmp.GraphicsQuality;
            MusicVolume = tmp.MusicVolume;
            AudioListener.volume = MusicVolume;
        }
    }

    public static void ResetSettings()
    {
        BinaryFormatter binform = new BinaryFormatter();
        string filepath = Application.persistentDataPath + "/Settings.pexo";
        SavingSettings tmp = new SavingSettings();
        tmp.MusicVolume = 1f;
        tmp.GraphicsQuality = 0;
        FileStream savefile = File.Create(filepath);
        binform.Serialize(savefile, tmp);
        savefile.Close();
        AudioListener.volume = MusicVolume;
    }

}
