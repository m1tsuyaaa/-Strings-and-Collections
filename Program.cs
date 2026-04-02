using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

class Program
{
  static Dictionary<string, List<string>> mistakeDict;
  static string directoryPath;
  static string[] files;
  static int fileIndex;
  static string filePath;
  static string content;
  static string correctedContent;
  static string finalContent;
  static string[] words;
  static int wordIndex;
  static string word;
  static string cleanWord;
  static string correctWord;
  static List<string> mistakes;
  static int mistakeIndex;
  static string mistake;
  static string result;
  static Match digitsMatch;
  static string allDigits;
  static string newPhone;
  static string[] keys;
  static int keyIndex;
  static string phone;
  static int phoneLength1;
  static int phoneLength2;
  static int startIndex1;
  static int length1;
  static int startIndex2;
  static int length2;
  static int startIndex3;
  static int length3;
  static int startIndex4;
  static int length4;

  static void Main()
  {
    mistakeDict = new Dictionary<string, List<string>>();

    InitializeMistakeDictionary();

    Console.Write("Введите путь к папке с текстовыми файлами: ");
    directoryPath = Console.ReadLine();

    phoneLength1 = 11;
    phoneLength2 = 10;
    startIndex1 = 1;
    length1 = 2;
    startIndex2 = 3;
    length2 = 3;
    startIndex3 = 6;
    length3 = 2;
    startIndex4 = 8;
    length4 = 2;

    if (Directory.Exists(directoryPath) == false)
    {
      Console.WriteLine("Директория не найдена!");
      Console.ReadKey();
      return;
    }

    files = Directory.GetFiles(directoryPath, "*.txt");

    fileIndex = 0;
    while (fileIndex < files.Length)
    {
      filePath = files[fileIndex];
      Console.WriteLine("\nОбработка файла: " + Path.GetFileName(filePath));
      ProcessFile();
      fileIndex += 1;
    }

    Console.WriteLine("\nГотово!");
    Console.ReadKey();
  }

  static void InitializeMistakeDictionary()
  {
    List<string> mistakes1 = new List<string>();
    mistakes1.Add("првиет");
    mistakes1.Add("пирвет");
    mistakes1.Add("превет");
    mistakes1.Add("привт");
    mistakeDict.Add("привет", mistakes1);

    List<string> mistakes2 = new List<string>();
    mistakes2.Add("поке");
    mistakes2.Add("пака");
    mistakes2.Add("поко");
    mistakeDict.Add("пока", mistakes2);

    List<string> mistakes3 = new List<string>();
    mistakes3.Add("магазен");
    mistakes3.Add("магазинн");
    mistakes3.Add("магозин");
    mistakeDict.Add("магазин", mistakes3);

    List<string> mistakes4 = new List<string>();
    mistakes4.Add("кампютер");
    mistakes4.Add("компютер");
    mistakes4.Add("компьютерр");
    mistakeDict.Add("компьютер", mistakes4);

    List<string> mistakes5 = new List<string>();
    mistakes5.Add("работаа");
    mistakes5.Add("робота");
    mistakes5.Add("работо");
    mistakeDict.Add("работа", mistakes5);
  }

  static void ProcessFile()
  {
    content = File.ReadAllText(filePath, Encoding.UTF8);

    correctedContent = FixMistakes();
    finalContent = FixPhoneNumbers();

    if (content != finalContent)
    {
      File.WriteAllText(filePath, finalContent, Encoding.UTF8);
      Console.WriteLine("  Изменения сохранены.");
    }
    else
    {
      Console.WriteLine("  Изменений не потребовалось.");
    }
  }

  static string FixMistakes()
  {
    words = content.Split(' ');
    keys = new string[mistakeDict.Count];
    mistakeDict.Keys.CopyTo(keys, 0);

    wordIndex = 0;
    while (wordIndex < words.Length)
    {
      word = words[wordIndex];
      cleanWord = Regex.Replace(word, @"[^\w\-]", "");

      keyIndex = 0;
      while (keyIndex < keys.Length)
      {
        correctWord = keys[keyIndex];
        mistakes = mistakeDict[correctWord];

        mistakeIndex = 0;
        while (mistakeIndex < mistakes.Count)
        {
          mistake = mistakes[mistakeIndex];

          if (cleanWord.Equals(mistake, StringComparison.OrdinalIgnoreCase))
          {
            words[wordIndex] = word.Replace(cleanWord, correctWord);
            Console.WriteLine("    Исправлено: " + mistake + " -> " + correctWord);
          }
          mistakeIndex += 1;
        }
        keyIndex += 1;
      }
      wordIndex += 1;
    }

    return string.Join(" ", words);
  }

  static string FixPhoneNumbers()
  {
    result = Regex.Replace(correctedContent, @"\(\d{3}\)\s*\d{3}-\d{2}-\d{2}", delegate (System.Text.RegularExpressions.Match match)
    {
      phone = match.Value;
      digitsMatch = Regex.Match(phone, @"\d+");

      if (digitsMatch.Success)
      {
        allDigits = Regex.Replace(phone, @"\D", "");

        if ((allDigits.Length == phoneLength1) || (allDigits.Length == phoneLength2))
        {
          newPhone = "+380 " + allDigits.Substring(startIndex1, length1) + " " + allDigits.Substring(startIndex2, length2) + " " + allDigits.Substring(startIndex3, length3) + " " + allDigits.Substring(startIndex4, length4);
          Console.WriteLine("    Телефон: " + phone + " -> " + newPhone);
          return newPhone;
        }
      }

      return phone;
    });

    return result;
  }
}