using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SosEngine
{
    public class Replay
    {
        public enum Mode
        {
            Record,
            Play,
            Standby
        }

        private Mode mode;
        private long tick;
        private bool oldLeft;
        private bool oldRight;
        private bool oldUp;
        private bool oldDown;
        private bool oldButton1;
        private bool oldButton2;
        private string fileName;
        private StringBuilder recordedData;
        private Dictionary<long, string> loadedData;

        public Replay(Stream stream)
        {
            this.fileName = "";
            this.mode = Mode.Play;
            this.loadedData = new Dictionary<long, string>();
            using (TextReader tr = new StreamReader(stream))
            {
                string line;
                while ((line = tr.ReadLine()) != null)
                {
                    var arr = line.Split(',');
                    loadedData.Add(long.Parse(arr[0]), arr[1]);
                }
            }
        }

        public Replay(string fileName, Mode mode)
        {
            this.fileName = fileName;
            this.mode = mode;
            switch (mode)
            {
                case Mode.Record:
                    this.recordedData = new StringBuilder();
                    break;
                case Mode.Play:
                    this.loadedData = new Dictionary<long, string>();
                    foreach (var line in System.IO.File.ReadAllLines(fileName))
                    {
                        var arr = line.Split(',');
                        loadedData.Add(long.Parse(arr[0]), arr[1]);
                    }
                    break;
                case Mode.Standby:
                    break;
            }
        }

        protected void HandleRecording(ref bool ctrlLeft, ref bool ctrlRight, ref bool ctrlUp, ref bool ctrlDown, ref bool ctrlA, ref bool ctrlB)
        {
            if (oldLeft != ctrlLeft || oldRight != ctrlRight || oldUp != ctrlUp || oldDown != ctrlDown || oldButton1 != ctrlA || oldButton2 != ctrlB)
            {
                recordedData.AppendLine(string.Format("{0},{1}{2}{3}{4}{5}{6}",
                    tick,
                    ctrlLeft ? "1" : "0",
                    ctrlRight ? "1" : "0",
                    ctrlUp ? "1" : "0",
                    ctrlDown ? "1" : "0",
                    ctrlA ? "1" : "0",
                    ctrlB ? "1" : "0"
                    ));
            }
        }

        protected void HandlePlay(ref bool ctrlLeft, ref bool ctrlRight, ref bool ctrlUp, ref bool ctrlDown, ref bool ctrlA, ref bool ctrlB)
        {
            if (loadedData.ContainsKey(tick))
            {
                var data = loadedData[tick];
                ctrlLeft = data[0] == '1';
                ctrlRight = data[1] == '1';
                ctrlUp = data[2] == '1';
                ctrlDown = data[3] == '1';
                ctrlA = data[4] == '1';
                ctrlB = data[5] == '1';
            }
            else
            {
                ctrlLeft = oldLeft;
                ctrlRight = oldRight;
                ctrlUp = oldUp;
                ctrlDown = oldDown;
                ctrlA = oldButton1;
                ctrlB = oldButton2;
            }
        }

        public void Update(ref bool ctrlLeft, ref bool ctrlRight, ref bool ctrlUp, ref bool ctrlDown, ref bool ctrlA, ref bool ctrlB)
        {
            tick++;
            switch (mode)
            {
                case Mode.Record:
                    HandleRecording(ref ctrlLeft, ref ctrlRight, ref ctrlUp, ref ctrlDown, ref ctrlA, ref ctrlB);
                    break;
                case Mode.Play:
                    HandlePlay(ref ctrlLeft, ref ctrlRight, ref ctrlUp, ref ctrlDown, ref ctrlA, ref ctrlB);
                    break;
                case Mode.Standby:
                    break;
            }

            oldLeft = ctrlLeft;
            oldRight = ctrlRight;
            oldUp = ctrlUp;
            oldDown = ctrlDown;
            oldButton1 = ctrlA;
            oldButton2 = ctrlB;
        }

        public void Save()
        {
            if (mode == Mode.Record)
            {
                System.IO.File.WriteAllText(fileName, recordedData.ToString());
            }
        }

    }


}
