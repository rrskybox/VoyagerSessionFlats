using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionFlats
{
    internal class VoyFileManager
    {
        const string PAfolderPath = "\\Documents\\AdvancedVoyager\\";

        public static IEnumerable<string> GetListRotatorPA()
        {
            List<string> listRotatorPA = new List<string>();
            string rpaDirPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + PAfolderPath;
            string rpaFilePath = rpaDirPath + " RotatorPAList.txt";
            if (!File.Exists(rpaFilePath))
                return listRotatorPA;
            StreamReader sfw = new StreamReader(rpaFilePath);
            while (!sfw.EndOfStream)
                listRotatorPA.Add(sfw.ReadLine());
            sfw.Close();
            IEnumerable<string> ilistRotatorPAlist = listRotatorPA;
            return ilistRotatorPAlist.Distinct();
        }

        public static void SetListRotatorPA(IEnumerable<string> paList)
        {
            string rpaDirPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + PAfolderPath;
            string rpaFilePath = rpaDirPath + " RotatorPAList.txt";
            //Create and/or open file for data -- will overwrite any previous contents
            StreamWriter sfw = File.CreateText(rpaFilePath);
            foreach (string rpa in paList)
                sfw.WriteLine(rpa);
            return;
        }

        public static void DeleteRotatorPAFile()
        {
            string rpaDirPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + PAfolderPath;
            string rpaFilePath = rpaDirPath + " RotatorPAList.txt";
            if (File.Exists(rpaFilePath))
                File.Delete(rpaFilePath);
        }

        public static void RemoveRotatorPA(string rotatorPA)
        {
            IEnumerable<string> iList = GetListRotatorPA().Where(pa => pa != rotatorPA);

        }

    }
}
