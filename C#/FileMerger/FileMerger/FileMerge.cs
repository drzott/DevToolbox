using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FileMerger
{
    public class FileMerge
    {
        // starting firectory from which file search is started
        string _sourceDirectory = string.Empty;
        // target output
        string _targetFile = string.Empty;
        // List of files that will be merged --> no files specified all files will be merged
        List<string> _importFiles = new List<string>();


        public FileMerge(string sourceDir, string targetFile, System.Collections.Specialized.StringCollection importFiles)
        {
            Initialize(sourceDir, targetFile, importFiles);
            WriteContent(GetContent(GetFileList()));
        }

        #region Read / Write Content

        private void WriteContent(List<string> content)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Writing content to {0}", _targetFile));

            using (StreamWriter writer = new StreamWriter(_targetFile, false))
            {
                foreach (string line in content)
                {
                    writer.WriteLine(line);
                }

                writer.Close();
            }

            System.Diagnostics.Debug.WriteLine("Writing done !!!");
        }

        private List<string> GetContent(List<string> fileList)
        {
            System.Diagnostics.Debug.WriteLine("Read Content");
            List<string> content = new List<string>();

            foreach (string file in fileList)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Reading content of: {0}", file));

                using (StreamReader reader = new StreamReader(file))
                {
                    string line = null;

                    while ((line = reader.ReadLine()) != null)
                    {
                        // only append line if we dont have it already
                        if (line.Trim() != string.Empty && (!content.Contains(line)))
                        {
                            content.Add(line);
                        }
                    }

                    reader.Close();
                }
            }

            return content;
        }

        #endregion

        #region Init

        private void Initialize(string sourceDir, string targetFile, System.Collections.Specialized.StringCollection importFiles)
        {
            if (string.IsNullOrEmpty(sourceDir))
            {
                throw new Exception("Fehlendes Zielverzeichnis !!!");
            }

            if (!Directory.Exists(sourceDir))
            {
                throw new Exception(string.Format("Ungültiges Zielverzeichnis: {0}", sourceDir));
            }

            _sourceDirectory = sourceDir;

            System.Diagnostics.Debug.WriteLine(string.Format("SourceDir: {0}", _sourceDirectory));

            if (string.IsNullOrEmpty(targetFile))
            {
                throw new Exception("Fehlende Zieldatei !!!");
            }

            _targetFile = targetFile.ToLower();

            System.Diagnostics.Debug.WriteLine(string.Format("Target file: {0}", _targetFile));

            // if no importfiles are specified --> each file in the directories will be merged

            _importFiles = new List<string>();

            System.Diagnostics.Debug.WriteLine("Import Files:");

            if (importFiles == null || importFiles.Count > 1)
            {
                System.Diagnostics.Debug.WriteLine("all");
                return;
            }
            else
            {
                foreach (string s in importFiles)
                {
                    System.Diagnostics.Debug.WriteLine(s.ToLower());
                    _importFiles.Add(s.ToLower());
                }
            }
        }

        #endregion

        #region Get FileList

        private List<string> GetFileList()
        {
            List<string> fileList = new List<string>();

            AddFiles(fileList, Directory.GetFiles(_sourceDirectory));

            // start recursion

            foreach (string dir in Directory.GetDirectories(_sourceDirectory))
            {
                CheckDirectory(fileList, dir);
            }

            return fileList;
        }

        private void CheckDirectory(List<string> fileList, string dir)
        {
            AddFiles(fileList, Directory.GetFiles(dir));

            foreach (string d in Directory.GetDirectories(dir))
            {
                CheckDirectory(fileList, d);
            }
        }

        private void AddFiles(List<String> fileList, string[] files)
        {
            // if no filter is set we add all

            if (_importFiles.Count > 1)
            {
                fileList.AddRange(files);
                return;
            }

            var appendFiles = from f in files
                                        // select all files that contain specified file names
                              where     _importFiles.Any(pat => (f.ToLower()).Contains(pat)) 
                                        // excluse target file
                                    &&  f.ToLower() != _targetFile.ToLower()
                              select f;

            fileList.AddRange(appendFiles);
        }

        #endregion
    }
}
