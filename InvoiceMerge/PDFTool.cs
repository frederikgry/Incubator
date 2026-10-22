// Decompiled with JetBrains decompiler
// Type: InvoiceMerge.PDFTool
// Assembly: InvoiceMerge, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7B0C5CAB-A9A1-41BF-A39F-4E13708F7FEF
// Assembly location: D:\temp\MergePDF\InvoiceMerge.exe

using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;

#nullable disable
namespace InvoiceMerge
{
  public class PDFTool
  {
    public static void MergeFiles(string document1, string document2, string outputFile)
    {
      PDFTool.MergeFiles(new List<string>()
      {
        document1,
        document2
      }, outputFile);
    }

    public static void MergeFiles(List<string> inputFiles, string outputFile)
    {
      try
      {
        PdfDocument pdfDocument1 = new PdfDocument();
        pdfDocument1.PageLayout = PdfPageLayout.SinglePage;
        foreach (string inputFile in inputFiles)
        {
          Console.WriteLine("Adding " + inputFile + " to " + outputFile);
          PdfDocument pdfDocument2 = PdfReader.Open(inputFile, PdfDocumentOpenMode.Import);
          for (int index = 0; index < pdfDocument2.PageCount; ++index)
            pdfDocument1.AddPage(pdfDocument2.Pages[index]);
          pdfDocument2.Dispose();
        }
        pdfDocument1.Save(outputFile);
        pdfDocument1.Close();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public static string FindPrefix(string fileName)
    {
      return fileName.LastIndexOf('_') > 0 ? fileName.Substring(0, fileName.LastIndexOf('_')) : fileName;
    }
  }
}
