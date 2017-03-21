using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;

using Base;

namespace StaticData
{
    public enum DataFileType
    {
        Config,
        Plot,
    }
    public class BaseParser
    {
        public static string CONFIG_PATH = Application.streamingAssetsPath + "/Configs/";
		public static string PLOT_PATH = Application.streamingAssetsPath + "/Plots/";

        private List<string[]> dataStrList = new List<string[]>();

        protected void LoadFile(string path)
		{
			if (File.Exists(path))
			{
                StreamReader sr = new StreamReader(path, System.Text.UTF8Encoding.UTF8);
                bool isNameLine = true;
                bool isTypeLine = true;
				while (!sr.EndOfStream)
				{
					string line = sr.ReadLine();
					if (isNameLine)
					{
						isNameLine = false;
						continue;
					}
                    if (isTypeLine)
                    {
                        isTypeLine = false;
                        continue;
                    }

					char firstChar = line[0];
					if (firstChar == ',')
					{
						continue;
					}
					string[] strArray = line.Split(',');
					dataStrList.Add(strArray);
				}
				sr.Close();

				RowIndex = 0;
			}
			else
			{
				BaseLogger.LogFormat("Can't find config file: {0}", path);
			}

		}
		protected void UnloadFile()
		{
            dataStrList.Clear();
		}

        protected int RowIndex { get; private set; }
		protected bool EndOfRow
		{
			get
			{
				return RowIndex == dataStrList.Count;
			}
		}
		protected void NextLine()
		{
			RowIndex++;
		}
		
        protected string GetContent(int col)
        {
            return dataStrList[RowIndex][col];
        }
    }
}

