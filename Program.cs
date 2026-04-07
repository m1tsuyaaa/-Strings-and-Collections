using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

class Program
{
  static void Main()
  {
    int PhoneLengthNew;
    int PhoneLengthOld;
    int OperatorStart;
    int OperatorLength;
    int FirstGroupStart;
    int FirstGroupLength;
    int SecondGroupStart;
    int SecondGroupLength;
    int ThirdGroupStart;
    int ThirdGroupLength;
    int StartIndex;
    int ZeroValue;

    PhoneLengthNew = 11;
    PhoneLengthOld = 10;
    OperatorStart = 1;
    OperatorLength = 2;
    FirstGroupStart = 3;
    FirstGroupLength = 3;
    SecondGroupStart = 6;
    SecondGroupLength = 2;
    ThirdGroupStart = 8;
    ThirdGroupLength = 2;
    StartIndex = 1;
    ZeroValue = 0;

    Dictionary<string, List<string>> mistakeDict;
    mistakeDict = new Dictionary<string, List<string>>();

    List<string> mistakesForHello;
    mistakesForHello = new List<string>();
    mistakesForHello.Add("првиет");
    mistakesForHello.Add("пирвет");
    mistakesForHello.Add("превет");
    mistakesForHello.Add("привт");
    mistakeDict.Add("привет", mistakesForHello);

    List<string> mistakesForBye;
    mistakesForBye = new List<string>();
    mistakesForBye.Add("поке");
    mistakesForBye.Add("пака");
    mistakesForBye.Add("поко");
    mistakeDict.Add("пока", mistakesForBye);

    List<string> mistakesForShop;
    mistakesForShop = new List<string>();
    mistakesForShop.Add("магазен");
    mistakesForShop.Add("магазинн");
    mistakesForShop.Add("магозин");
    mistakeDict.Add("магазин", mistakesForShop);

    List<string> mistakesForComputer;
    mistakesForComputer = new List<string>();
    mistakesForComputer.Add("кампютер");
    mistakesForComputer.Add("компютер");
    mistakesForComputer.Add("компьютерр");
    mistakeDict.Add("компьютер", mistakesForComputer);

    List<string> mistakesForWork;
    mistakesForWork = new List<string>();
    mistakesForWork.Add("работаа");
    mistakesForWork.Add("робота");
    mistakesForWork.Add("работо");
    mistakeDict.Add("работа", mistakesForWork);

    Console.Write("Enter path to folder with text files: ");
    string directoryPath;
    directoryPath = Console.ReadLine();

    if (Directory.Exists(directoryPath) == false)
    {
      Console.WriteLine("Directory not found!");
      Console.ReadKey();
      return;
    }

    string[] files;
    files = Directory.GetFiles(directoryPath, "*.txt");

    int fileIndex;
    fileIndex = ZeroValue;

    string filePath;
    string content;
    string[] words;
    string[] dictionaryKeys;
    int wordIndex;
    string currentWord;
    string cleanedWord;
    int keyIndex;
    string correctWord;
    List<string> mistakeList;
    int mistakeIndex;
    string currentMistake;
    string correctedContent;
    string phonePattern;
    string finalContent;
    string phoneNumber;
    string onlyDigits;
    string convertedPhone;

    while (fileIndex < files.Length)
    {
      filePath = files[fileIndex];
      Console.WriteLine("\nProcessing file: " + Path.GetFileName(filePath));

      content = File.ReadAllText(filePath, Encoding.UTF8);

      words = content.Split(' ');
      dictionaryKeys = new string[mistakeDict.Count];
      mistakeDict.Keys.CopyTo(dictionaryKeys, ZeroValue);

      wordIndex = ZeroValue;

      while (wordIndex < words.Length)
      {
        currentWord = words[wordIndex];
        cleanedWord = Regex.Replace(currentWord, @"[^\w\-]", "");

        keyIndex = ZeroValue;

        while (keyIndex < dictionaryKeys.Length)
        {
          correctWord = dictionaryKeys[keyIndex];
          mistakeList = mistakeDict[correctWord];

          mistakeIndex = ZeroValue;

          while (mistakeIndex < mistakeList.Count)
          {
            currentMistake = mistakeList[mistakeIndex];

            if (cleanedWord.Equals(currentMistake, StringComparison.OrdinalIgnoreCase))
            {
              words[wordIndex] = currentWord.Replace(cleanedWord, correctWord);
              Console.WriteLine("    Fixed: " + currentMistake + " -> " + correctWord);
            }
            mistakeIndex += StartIndex;
          }
          keyIndex += StartIndex;
        }
        wordIndex += StartIndex;
      }

      correctedContent = string.Join(" ", words);

      phonePattern = @"\(\d{3}\)\s*\d{3}-\d{2}-\d{2}";

      finalContent = Regex.Replace(correctedContent, phonePattern, delegate (System.Text.RegularExpressions.Match match)
      {
        phoneNumber = match.Value;
        onlyDigits = Regex.Replace(phoneNumber, @"\D", "");

        if ((onlyDigits.Length == PhoneLengthNew) || (onlyDigits.Length == PhoneLengthOld))
        {
          convertedPhone = "+380 "
            + onlyDigits.Substring(OperatorStart, OperatorLength) + " "
            + onlyDigits.Substring(FirstGroupStart, FirstGroupLength) + " "
            + onlyDigits.Substring(SecondGroupStart, SecondGroupLength) + " "
            + onlyDigits.Substring(ThirdGroupStart, ThirdGroupLength);
          Console.WriteLine("    Phone: " + phoneNumber + " -> " + convertedPhone);
          return convertedPhone;
        }

        return phoneNumber;
      });

      if (content != finalContent)
      {
        File.WriteAllText(filePath, finalContent, Encoding.UTF8);
        Console.WriteLine("  Changes saved.");
      }
      else
      {
        Console.WriteLine("  No changes needed.");
      }

      fileIndex += StartIndex;
    }

    Console.WriteLine("\nDone!!");
    Console.ReadKey();
  }
}