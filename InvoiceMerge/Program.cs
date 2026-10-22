// See https://aka.ms/new-console-template for more information

using InvoiceMerge;

try
{
    if (args == null)
    {
        Console.WriteLine("Missing argument, please provide a directory to process");
    }
    else
    {
        string str1 = args[0];
        if (Directory.Exists(str1))
        {
            string str2 = "";
            List<string> stringList1 = new List<string>();
            List<string> stringList2 = new List<string>();
            string[] files = Directory.GetFiles(str1, "*.pdf");
            foreach (string str3 in files)
            {
                if (Path.GetFileName(str3).Contains("_"))
                {
                    if (str2 == "")
                        stringList1.Add(str3);
                    else if (PDFTool.FindPrefix(str3) == str2)
                    {
                        stringList1.Add(str3);
                    }
                    else
                    {
                        if (stringList1.Count > 1)
                        {
                            PDFTool.MergeFiles(stringList1, PDFTool.FindPrefix(stringList1[0]) + ".pdf");
                            stringList2.AddRange((IEnumerable<string>)stringList1);
                        }
                        stringList1 = new List<string>();
                        stringList1.Add(str3);
                        Console.WriteLine("");
                    }
                    str2 = PDFTool.FindPrefix(str3);
                }
            }
            if (stringList1.Count > 1)
            {
                PDFTool.MergeFiles(stringList1, PDFTool.FindPrefix(stringList1[0]) + ".pdf");
                stringList2.AddRange((IEnumerable<string>)stringList1);
            }
            Console.WriteLine("");
            List<string> inputFiles = new List<string>();
            foreach (string path in files)
            {
                if (Path.GetFileName(path).ToLower().Contains("_1.pdf"))
                    inputFiles.Add(path);
            }
            if (inputFiles.Count > 0)
            {
                PDFTool.MergeFiles(inputFiles, Path.Combine(str1, "elortnoc.pdf"));
                File.Copy(Path.Combine(str1, "elortnoc.pdf"), Path.Combine(str1, "controle.pdf"));
                File.Delete(Path.Combine(str1, "elortnoc.pdf"));
            }
            if (stringList2.Count > 0)
            {
                foreach (string path in stringList2)
                {
                    Console.WriteLine("Delete file " + path);
                    File.Delete(path);
                }
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine((object)ex);
}