using SevenZip;
using SevenZipExtractor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _7zAssimilator.CommonFormFiles
{
    public static class MassFileZipConvert
    {
        static MassFileZipConvert()
        {
        }

        public static bool Convert(string directoryLocation, string extractLocation)
        {
            string logLocation = Directory.GetCurrentDirectory() + @"\log.txt";
            SevenZip.SevenZipExtractor.SetLibraryPath(Directory.GetCurrentDirectory() + @"\7z.dll");

            SevenZip.SevenZipCompressor.SetLibraryPath(Directory.GetCurrentDirectory() + @"\7z.dll");

            try
            {




                // From a directory walk through it and every sub directory
                // Check to see if a file type is zip/rar/ect, if so extract file to selected drive (presumable one with a lot of space)
                // Delete the original and Rezip it in 7z (the most compression I can find) back where it was found.
                // if there is an issue write to a bug log the issue and delete the newly created files

                //TODO: Make a log function that takes in a list of strings and writes to the given log path

                string[] directoryList = Directory.GetDirectories(directoryLocation);

                using (StreamWriter w = File.AppendText(logLocation))
                {
                    w.WriteLine(@"/^\/^\/^\/^\/^\/^\/^\/^\/^\/^\/^\/^\/^\/^\/^\");
                    w.WriteLine("Starting new compression at " + DateTime.Now);
                    w.WriteLine("Directory location: " + directoryLocation);
                    w.WriteLine("Unpacking Locataion: " + extractLocation);
                    w.WriteLine();
                    w.WriteLine();
                }

                WorkThroughDirectories(directoryLocation, extractLocation + @"\", true, logLocation);
                //foreach (string directory in directoryList)
                //{
                //    WorkThroughDirectories(directory, extractLocation, true, logLocation);
                //};

                using (StreamWriter w = File.AppendText(logLocation))
                {
                    w.WriteLine("All done.");
                    w.WriteLine("/////////////////////////////////////");
                }
                return true;
            }
            catch (Exception e)
            {
                using (StreamWriter w = File.AppendText(logLocation))
                {
                    w.WriteLine("Something Went Wrong.");
                    w.WriteLine("Error Message: " +  e.Message);
                    w.WriteLine("/////////////////////////////////////");
                }
                return false;
            }
            
        }

        public static void WorkThroughDirectories(string CurrentDirectory, string ExtractLocation, bool FirstCall, string LogLocation)
        {
            try
            {
                string[] directoryList = Directory.GetDirectories(CurrentDirectory);

                // Walk through folders until only files in directory
                foreach (string directory in directoryList)
                {
                    WorkThroughDirectories(directory, ExtractLocation, FirstCall, LogLocation);
                };

                // Check for .rar
                string[] rarFileList = Directory.GetFiles(CurrentDirectory, "*.rar");
                CopyAndReplace(rarFileList, ExtractLocation, FirstCall, LogLocation);
                string[] zipFileList = Directory.GetFiles(CurrentDirectory, "*.zip");
                CopyAndReplace(zipFileList, ExtractLocation, FirstCall, LogLocation);
            }
            catch (Exception ex)
            {
                using (StreamWriter w = File.AppendText(LogLocation))
                {
                    w.WriteLine();
                    w.WriteLine("/////////////////////////////////////");
                    w.WriteLine("EXCEPTION");
                    w.WriteLine(ex.Message);
                    w.WriteLine("/////////////////////////////////////");
                }
            }

        }

        public static void CopyAndReplace(string[] ZipFileLocations, string ExtractLocation, bool FirstCall, string LogLocation)
        {
            //Remove characters forbidden from directories
            ExtractLocation = Regex.Replace(ExtractLocation, @"!(.*)?\(", "");

            try
            {
                foreach (string location in ZipFileLocations)
                {
                    // the split removes the file extension.
                    string fileName = Path.GetFileName(location).Split('.').First();
                    string newExtractLocation = string.Empty;
                    if (FirstCall)
                    {
                        newExtractLocation = ExtractLocation + fileName + @"\";
                    }
                    else
                    {
                        newExtractLocation = Path.GetDirectoryName(location) + @"\" + fileName + @"\";
                    }

                    //if(Directory.Exists(newExtractLocation))
                    //    newExtractLocation = newExtractLocation + fileName + @"\";

                    using (ArchiveFile archiveFile = new ArchiveFile(location))
                    {
                        using (StreamWriter w = File.AppendText(LogLocation))
                        {
                            w.WriteLine("About to Extract: ");
                            w.WriteLine(location);
                            w.WriteLine("To the new location: ");
                            w.WriteLine(newExtractLocation);
                        }
                        // Extracts the file to a new subdirectory in the location with the same name as the zip file
                        archiveFile.Extract(newExtractLocation);
                        string[] directoryList = Directory.GetDirectories(newExtractLocation);
                        foreach (string directory in directoryList)
                        {
                            WorkThroughDirectories(directory, newExtractLocation, false, LogLocation);

                        };

                        string[] rarFileList = Directory.GetFiles(newExtractLocation, "*.rar");
                        CopyAndReplace(rarFileList, newExtractLocation, false, LogLocation);
                        string[] zipFileList = Directory.GetFiles(newExtractLocation, "*.zip");
                        CopyAndReplace(zipFileList, newExtractLocation, false, LogLocation);
                    }

                    SevenZipCompressor sevenZipCompressor = new SevenZipCompressor();
                    sevenZipCompressor.ScanOnlyWritable = true;

                    using (StreamWriter w = File.AppendText(LogLocation))
                    {
                        w.WriteLine("About to compress to: ");
                        w.WriteLine(Path.GetDirectoryName(location) + @"\" + fileName + ".7z");

                    }
                    sevenZipCompressor.CompressDirectory(newExtractLocation, Path.GetDirectoryName(location) + @"\" + fileName + ".7z");
                    File.Delete(location);
                    Directory.Delete(newExtractLocation, true);

                }
            }
            catch (Exception ex)
            {
                using (StreamWriter w = File.AppendText(LogLocation))
                {
                    w.WriteLine();
                    w.WriteLine("/////////////////////////////////////");
                    w.WriteLine("EXCEPTION");
                    w.WriteLine(ex.Message);
                    w.WriteLine("/////////////////////////////////////");
                }
            }

        }
    }
}
