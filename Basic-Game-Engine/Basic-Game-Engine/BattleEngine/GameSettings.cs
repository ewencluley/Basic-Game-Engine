using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BattleEngine
{
    public static class GameSettings
    {
        public static Settings settings = new Settings();

        public static void LoadSettings()
        {
            try
            {
                Stream stream = File.Open("settings.dat", FileMode.Open); ;
                try
                {
                    //Open the file written above and read values from it.

                    BinaryFormatter bformatter = new BinaryFormatter();

                    Console.WriteLine("Reading Employee Information");
                    settings = (Settings)bformatter.Deserialize(stream);
                    stream.Close();
                }
                catch
                {
                    stream.Close();
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                //No settings file found, use defaults & save
                SaveSettings();
            }

        }

        public static void SaveSettings()
        {
            Stream stream = File.Open("settings.dat", FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();

            Console.WriteLine("Writing Employee Information");
            bformatter.Serialize(stream, settings);
            stream.Close();
        }

        [Serializable()]
        public struct Settings: ISerializable
        {

            //Video Settings
            public static int resolutionWidth = 1024;
            public static int resolutionHeight = 768;
            public static bool fullScreen = false;
            //Audio Settings
            public static int musicVolume = 100;
            public static int effectsVolume = 100;

            public Settings(SerializationInfo info, StreamingContext ctxt)
            {
                resolutionWidth = (int)info.GetValue("ResolutionWidth", typeof(int)); 
                resolutionHeight = (int)info.GetValue("ResolutionHeight", typeof(int));
                musicVolume = (int)info.GetValue("MusicVolume", typeof(int));
                effectsVolume = (int)info.GetValue("EffectsVolume", typeof(int));
                fullScreen = (bool)info.GetValue("FullScreen", typeof(bool)); 
            }

           public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
           {
               info.AddValue("ResolutionWidth", resolutionWidth);
               info.AddValue("ResolutionHeight", resolutionHeight);
               info.AddValue("MusicVolume", musicVolume);
               info.AddValue("EffectsVolume", effectsVolume);
               info.AddValue("FullScreen", fullScreen);
           }
        }
    }
}
