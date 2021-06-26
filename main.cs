using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mytools
{
  class main
  {
    static string[] Virtual_operands = new string[3];
    static int[] positions = new int[6] { -1, -1, -1, -1, -1, -1 };
    static string[] reconstructed_operands = new string[6];
    static void swap(ref string a, ref string b)
    {
      string temp = a;
      a = b;
      b = temp;
    }

    static int Partition(string[] array, int low, int high)
    { // 1. Select a pivot point.
      string pivot = array[high];
      int lowIndex = (low - 1);
      for (int j = low; j < high; j++)
      { // 2. Reorder the collection.
        if (String.Compare(array[j], pivot) < 1)
        {
          lowIndex++;
          swap(ref array[lowIndex], ref array[j]);
        }
      }
      swap(ref array[lowIndex + 1], ref array[high]);
      return lowIndex + 1;
    }

    static void Sort(string[] array, int low, int high)
    {
      if (low < high)
      { // 3. Recursively continue sorting the array
        int partitionIndex = Partition(array, low, high);
        Sort(array, low, partitionIndex - 1);
        Sort(array, partitionIndex + 1, high);
      }
    }

    static void Main(string[] Lined_operands)
    {
      string[] array = Lined_operands;
      int subscript;
      string input_from = "";
      extract_virtual_operands(Lined_operands);
      Sort(Virtual_operands, 0, Virtual_operands.Length - 1);
      position_virtual_operands(Lined_operands);
      position_actual_operands(Lined_operands);
      Console.WriteLine("\nAfter positioning the actual operands: ");
      reconstruct_operands(Lined_operands);
      for (subscript = 0; subscript < reconstructed_operands.Length; subscript++)
      {
        for (int i = 0; i < Lined_operands.Length; i++)
        {
          if (reconstructed_operands[subscript] == "")
          {
            reconstructed_operands[subscript] = Lined_operands[i];
            input_from = Lined_operands[i];
          }
        }
        
        if (subscript < reconstructed_operands.Length - 1)
        {
          Console.Write("{0} {1}", reconstructed_operands[subscript], subscript);
          
          if (reconstructed_operands[subscript] == "-t")
            type_the_target(reconstructed_operands[subscript + 1], input_from);
          if (reconstructed_operands[subscript] == "-h")
            ReadMe();
          if (reconstructed_operands[subscript] == "-o")
            column_texts_out(input_from, reconstructed_operands[subscript + 1]);
        }
        else
          Console.WriteLine("{0}", reconstructed_operands[subscript]);
      }
    }

    static void reconstruct_operands(string[] Lined_operands)
    {
      for (int subscript = 0; subscript < (Virtual_operands.Length * 2); subscript++)
      {
        if (positions[subscript] != -1)
          reconstructed_operands[subscript] = Lined_operands[positions[subscript]];
        else
          reconstructed_operands[subscript] = "";
      }
    }

    static void position_actual_operands(string[] Lined_operands)
    {
      for (int counter = 1; counter < positions.Length; counter++)
        if (positions[counter] == -1)
        {
          if (positions[counter - 1] != -1)
          {
            if (positions[counter - 1] < Lined_operands.Length - 1)
              positions[counter] = positions[counter - 1] + 1;
          }
        }
    }

    static void position_virtual_operands(string[] Lined_operands)
    {
      for (int counter = 0; counter < Virtual_operands.Length; counter++)
        for (int subscript = 0; subscript < Lined_operands.Length; subscript++)
          if (Virtual_operands[counter] == Lined_operands[subscript])
            positions[counter * 2] = subscript;
    }
    static void extract_virtual_operands(string[] Input)
    {
      for (int subscript = 0, counter = 0; subscript < Input.Length; subscript++)
      {
        if (Input[subscript][0] == '-')
          Virtual_operands[counter++] = Input[subscript];
      } 
    }

    static void ReadMe()
    {
      StreamReader reader = new StreamReader("readme.txt");
      Console.WriteLine(reader.ReadToEnd());
      reader.Close();
    }

    static String StoreToDataRecord(string operandPath1)
    {
      string input = System.IO.File.ReadAllText(operandPath1);
      string inputLine = "";
      StringReader reader = new StringReader(input);
      RecType1 ModuleData1 = new RecType1();
      List<RecType1> Data1 = new List<RecType1>();
      int i=1;
      while ((inputLine = reader.ReadLine()) != null)
      {
        string[] inputArray = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        Data1.Add
         (
          new RecType1() 
          { 
            ID = i++, 
            TimeStamp = inputArray[0]+" "+inputArray[1], 
            Level = inputArray[2],
            PID = inputArray[3],
            ErorrMassage = inputArray[4]+" "+inputArray[5]+" "+inputArray[6]+" "+inputArray[7],
            ErorrDetail = string.Join(" ", inputArray.Skip(8)),
          }
         )
        ;                    
      }
      string jsonString = Data1.ToJSON();
      return jsonString;
    }  

    static string StoreToPlainText(string operandPath1)
    {
      string input = System.IO.File.ReadAllText(operandPath1);
      string inputLine = "";
      StringReader reader = new StringReader(input);
      string Data1 = "";
      int i=1;
      while((inputLine = reader.ReadLine()) != null)
      {
        string[] inputArray = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        Data1 +=    "ID = "+ i + "\n" +
                    "TimeStamp = " + inputArray[0] +" "+ inputArray[1] + "\n" +
                    "Level = "+inputArray[2] + "\n" +
                    "PID = "+inputArray[3] + "\n" +
                    "ErorrMassage = "+inputArray[4]+" "+inputArray[5]+" "+inputArray[6]+" "+inputArray[7] + "\n" +
                    "ErorrDetail = "+string.Join(" ", inputArray.Skip(8));
      }
             //string jsonString = Data1.ToJSON();
      return Data1;
    }

    static void type_the_target (string the_type, string filepath)
    {
      switch (the_type)
      {
        case "json": column_json(filepath); break;  // so-called convert to text
        case "text": column_texts(filepath); break;
        default: Console.WriteLine("Tuliskan sintaks program dengan benar. Ketik mytools -h untuk bantuan"); break;
      }
    }

    static void column_json(string operand1)
    {
      try
      {
        System.IO.StreamWriter SerializedFile;
        string path = operand1;
        string[] baris = File.ReadAllLines(path);
        int jumlah_baris = baris.Length;
        FileInfo metafile;
        FileInfo metafile2 = new FileInfo(path);
        
        if (metafile2.IsReadOnly == true)
          Console.WriteLine("File is Read-Only");        
        else
        {
          string FileExt = Path.ChangeExtension(path, ".json");
          metafile = new FileInfo(FileExt);
          string FileName = metafile.Name;

          SerializedFile = new StreamWriter(FileName);
          
          if (metafile2.Name == "error.log")
          {
            string DataResult = StoreToDataRecord(path);
            SerializedFile.WriteLine(DataResult);
            SerializedFile.Close();
          }
          else
          {
            for (int a = 0; a < jumlah_baris; a++)
            {
              SerializedFile.WriteLine(baris[a]);
            }
            SerializedFile.Close();
          }
        }
      }
      catch (IOException error)
      {
        if (error.Message.Contains("already exist"))
          Console.WriteLine("Nama file sudah ada. Silahkan rename file!");
        else
          Console.WriteLine(error.Message);
      }
      catch (UnauthorizedAccessException error_access) 
      {
        Console.WriteLine(error_access.Message);
      }
    }

    static void column_texts(string operand1)
    {
      try
      {
        System.IO.StreamWriter SerializedFile;

        string path = operand1;
        string[] baris = File.ReadAllLines(path);
        int jumlah_baris = baris.Length;
        FileInfo metafile;
        FileInfo metafile2 = new FileInfo(path);

        if (metafile2.IsReadOnly == true)
          Console.WriteLine("File is Read-Only");
        else
        {
          string FileExt = Path.ChangeExtension(path, ".txt");
          metafile = new FileInfo(FileExt);
          string FileName = metafile.Name;
          SerializedFile = new StreamWriter(FileName);
          if (metafile2.Name == "error.log")
          {
            string DataResult = StoreToPlainText(path);
            SerializedFile.WriteLine(DataResult);
            SerializedFile.Close();
          }
          else
          {
            for (int a = 0; a < jumlah_baris; a++)
              SerializedFile.WriteLine(baris[a]);
            SerializedFile.Close();
          }
        }
      }
      catch (IOException error)
      {
        if (error.Message.Contains("already exist"))
          Console.WriteLine("Nama file sudah ada. Silahkan rename file!");
        else
          Console.WriteLine(error.Message);
      }
      catch (UnauthorizedAccessException error_access) 
      {
        Console.WriteLine(error_access.Message);
      }
    }

    static void column_texts_out(string operand1, string operand2)
    {
      try
      {
        System.IO.StreamWriter SerializedFile;
        string path = operand1;
        string[] baris = File.ReadAllLines(path);
        int jumlah_baris = baris.Length;
        FileInfo metafile;
        FileInfo metafile2 = new FileInfo(path);
        if (metafile2.IsReadOnly == true)
          Console.WriteLine("File is Read-Only"); 
        else
        {
            metafile = new FileInfo(operand1);
            string FileName = metafile.FullName;
            SerializedFile = new StreamWriter(operand2);
            if (metafile2.Name == "error.log")
            {
              string DataResult = StoreToPlainText(path);
              SerializedFile.WriteLine(DataResult);
              SerializedFile.Close();
            }
            else
            {
              for (int a = 0; a < jumlah_baris; a++)
                SerializedFile.WriteLine(baris[a]);
            }
            SerializedFile.Close();
        }
      }
      catch (IOException error)
      {
        if (error.Message.Contains("already exist"))
          Console.WriteLine("Nama file sudah ada. Silahkan rename file!");
        else
          Console.WriteLine(error.Message);
      }
      catch (UnauthorizedAccessException error_access) 
      {
        Console.WriteLine(error_access.Message);
      }
    }
  }
}
