using CsvHelper;
using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Collect
{
    class Program
    {
        static void Main(string[] args)
        {
            //create a new file with today's date everytime we run the program.
            string newFileName = "Collected_Files_employee.csv";
            string fullPath = Path.Combine("\\\\igxur\\igxsites\\cms101\\pb24\\lehi3-scraper\\src\\New Scraper c#\\Collect", newFileName);

            using (StreamWriter sw = new StreamWriter(fullPath))
            {
                //I made the comma separated values = | because files had , or . in the name
                sw.WriteLine("Url| Site| Type| Size| Date Created| Date Modified| Sub-Directories| Owner");
            }
            

            Console.WriteLine("File created successfully");
            Console.ReadLine();

            string errors = "";
            //the directory we want to find all files from.
            ListFilesInDirectory(@"\\lehi3\emp$\", fullPath, errors);
            Console.Write("Done");
            Console.Read();

            //finished searching
        }

        static void ListFilesInDirectory(string workingDirectory, string newFile = "", string errors = "")
        {
            string[] filePaths = Directory.GetFiles(workingDirectory);
            string[] directoryPaths = Directory.GetDirectories(workingDirectory);

            //put the file name, folder, and paths into the newFile
            try
            {
                foreach (string file in filePaths)
                {
                    //get the file extension ex: .jpeg
                    string type = Path.GetExtension(file);
                    
                    //if the file is not any of these values skip it and move on to the next file.
                    if (type == ".css" || type ==  ".js" || type == ".ini" || type == ".db" || type == ".ds_store")
                    {
                        continue;
                    }
                    
                    //get user who created file
                    IdentityReference sid = null;
                    string owner = "";
                    try
                    {

                        FileSecurity fileSecurity = File.GetAccessControl(file);
                        sid = fileSecurity.GetOwner(typeof(SecurityIdentifier));
                        NTAccount ntAccount = sid.Translate(typeof(NTAccount)) as NTAccount;
                        owner = ntAccount.Value;

                    }
                    //the owner was not specified
                    catch (IdentityNotMappedException ex)
                    {
                            if(sid != null)
                            {
                                owner = sid.ToString();
                            }
                    }
                    
                    //next line is kept as a reference only
                    //string user = File.GetAccessControl(file).GetOwner(typeof(NTAccount)).ToString();
                    
                    //get other info
                    DateTime creation = File.GetCreationTime(file);
                    string creationDate = creation.ToString("yyyy/MM/dd");
                    DateTime modification = File.GetLastWriteTime(file);
                    string modDate = modification.ToString("yyyy/MM/dd");
                    string[] siteSplit = file.Split('\\');
                    string site = siteSplit[4]; //the first directory in the file path
                    
                    //if the site (directory value) turns out to be a file itself make it \ 
                    if (site.Contains("."))
                    {
                        site = @"\";
                    }
                    long sizeBytes = new FileInfo(file).Length;
                    string size = sizeBytes.ToString();
                    size = String.Format("{0}B", size);
                    int subsCount = siteSplit.Length - 5; //how many sub directories
                    
                    // this happens because it is the file and root directory
                    if (site.Contains(@"\"))
                    {
                        subsCount = 0;
                    }
                    string subs = subsCount.ToString();
                    using (StreamWriter writeIntoFile = new StreamWriter(newFile, true))
                    {
                        //save the file info into the CSV file
                        string line = String.Format("{0}| {1}| {2}| {3}| {4}| {5}| {6}| {7}", file, site, type, size, creationDate, modDate, subs, owner);
                        writeIntoFile.WriteLine(line);
                    }
                    Console.WriteLine(Path.GetFullPath(file));
                }
                
                //run through each folder and sub folder with this foreach loop. This is a recursive loop so we scour each folder, subfolder, and file
                foreach (string folder in directoryPaths)
                {
                    try
                    {
                        ListFilesInDirectory(Path.GetFullPath(folder), newFile, errors);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        Console.WriteLine("Unautherized Access Exception: " + folder);
                        errors += folder + "\n";
                        continue;
                    }
                }
            }
            //make a list of all errors found while reading through the files
            catch (System.Exception)
            {
                Console.WriteLine("Error!!!");
            }
            Console.WriteLine("List of errors:" + errors + "\nEND ERRORS LIST");
        }
    }
}

