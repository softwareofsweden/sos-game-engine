using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// using Microsoft.Xna.Framework.Storage;
// using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using System.IO;

namespace SosEngine
{

    public class SaveDataManager
    {

        // protected StorageDevice device;

        /*
        protected void GetDevice()
        {
            IAsyncResult result = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
            result.AsyncWaitHandle.WaitOne();
            device = StorageDevice.EndShowSelector(result);
            result.AsyncWaitHandle.Close();
        }

        protected StorageContainer GetContainer(string displayName)
        {
            IAsyncResult result = device.BeginOpenContainer(displayName, null, null);
            result.AsyncWaitHandle.WaitOne();
            StorageContainer container = device.EndOpenContainer(result);
            result.AsyncWaitHandle.Close();
            return container;
        }
        */

        public SaveDataManager()
        {
            // GetDevice();
        }

        public bool Load<T>(out object data)
        {
            data = null;
            return false;

            /*
            StorageContainer container = GetContainer("Save");
            string filename = "savegame.sav";
            if (!container.FileExists(filename))
            {
                container.Dispose();
                data = null;
                return false;
            }
            Stream stream = container.OpenFile(filename, FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            data = serializer.Deserialize(stream);
            stream.Close();
            container.Dispose();
            data = null;
            return true;
            */
        }

        public void Save<T>(object gameData)
        {
            /*
            StorageContainer container = GetContainer("Save");
            string filename = "savegame.sav";
            if (container.FileExists(filename))
            {
                container.DeleteFile(filename);
            }
            Stream stream = container.CreateFile(filename);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, gameData);
            stream.Close();
            container.Dispose();
            */
        }

    }

}
