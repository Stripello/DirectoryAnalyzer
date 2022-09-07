﻿using DirectoryAnalyzer.Models;

namespace DirectoryAnalyzer
{
    public static class TableOperator
    {
        private static int[] LongestElement(string[,] input)
        {
            var amountOfColumns = input.GetLength(1);
            var amountOfRows = input.GetLength(0);
            var setOfWidth = new int[amountOfColumns];

            for (int i = 0; i < amountOfColumns; i++)
            {
                int maxLength = 0;
                for (int j = 0; j < amountOfRows; j++)
                {
                    if ((input[j, i] ?? "").Length > maxLength)
                    {
                        maxLength = input[j, i].Length;
                    }
                }
                setOfWidth[i] = maxLength;
            }
            return setOfWidth;
        }

        internal static IList<string> BuildTable(string[,] inputTable, string head = "")
        {
            head ??= "";
            var amountOfRows = inputTable.GetLength(0);
            var amountOfColumns = inputTable.GetLength(1);
            var setOfColumnsWidth = LongestElement(inputTable);
            int summarizedWidth = setOfColumnsWidth.Sum() + amountOfColumns*3 +1;
            var tempHead = head;
            if (summarizedWidth >= tempHead.Length + 2)
            {
                tempHead = "|" + new string(' ', (summarizedWidth- tempHead.Length-2)/2) + tempHead + new string (' ', summarizedWidth-(summarizedWidth- tempHead.Length-2)/2-2- tempHead.Length)+ "|";
            }
            else
            {
                tempHead = "|" + tempHead + "|";
                var alternativeLength = tempHead.Length;
                for (int i = 0; i < amountOfColumns - 1; i++)
                {
                    setOfColumnsWidth[i] += (alternativeLength - summarizedWidth) / amountOfColumns;
                }
                setOfColumnsWidth[amountOfColumns-1] += (alternativeLength - summarizedWidth) / amountOfColumns + (alternativeLength - summarizedWidth)%amountOfColumns;
                summarizedWidth = alternativeLength;
            }

            //amountOfColumns*3+1 is amount of additional symbols, generated by whitespaces before and after text
            //in cell plus vertical "lines" of table
           
            var horizontalLine = new string ('-', summarizedWidth);
            var answer = Enumerable.Repeat("", amountOfRows * 2 + 1).ToList(); 
            //additional rows in table was generated by horizontal lines
            for (int i = 0; i < amountOfRows * 2 + 1; i++)
            {
                if (i % 2 == 0)
                {
                    answer[i] = horizontalLine;
                }
                else
                {
                    answer[i] += "|";
                    for (int j = 0; j < amountOfColumns; j++)
                    {
                        answer[i] += " ";
                        answer[i] += inputTable[i / 2, j];
                        int deltaLength = setOfColumnsWidth[j] - (inputTable[i / 2, j] ?? "").Length;
                        for (uint k = 0; k < deltaLength; k++)
                        {
                            answer[i] +=" ";
                        }
                        answer[i] += " |";
                    }
                }
            }
            if (head != "")
            {
                answer.Reverse();
                answer.Add(tempHead);
                answer.Add(horizontalLine);
                answer.Reverse();
            }
            return answer;
        }

        public static IList<string> BuildTable(IList<MyFileInfo> incomingFileInfos,
            bool showName = true, bool showExtension = true, bool showSize = true, bool showChangedate = true, 
            string headOfTable = "")
            //pretty shure there is a better way to pass to the mehod set of bools
        {
            var answerColumns = 0;
            answerColumns = showName ? answerColumns + 1 : answerColumns;
            answerColumns = showExtension ? answerColumns + 1 : answerColumns;
            answerColumns = showSize ? answerColumns + 1 : answerColumns;
            answerColumns = showChangedate ? answerColumns + 1 : answerColumns;
            if (answerColumns == 0)
            {
                return new List<string>() { string.Empty };
            }
            var auxArray = new string[incomingFileInfos.Count+1, answerColumns];
            var currentColumn = 0;
            if (showName)
            {
                auxArray[0, currentColumn] = "file name";
                for (var i = 0; i < incomingFileInfos.Count; i++)
                {
                    auxArray[i + 1, currentColumn] = incomingFileInfos[i].Name;
                }
                currentColumn++;
            }
            if (showExtension)
            {
                auxArray[0, currentColumn] = "extension";
                for (var i = 0; i < incomingFileInfos.Count; i++)
                {
                    auxArray[i + 1, currentColumn] = incomingFileInfos[i].Extension;
                }
                currentColumn++;
            }
            if (showSize)
            {
                auxArray[0, currentColumn] = "size"; //rework
                for (var i = 0; i < incomingFileInfos.Count; i++)
                {
                    switch (incomingFileInfos[i].Size)
                    {
                        case > 1099511627776: //terabyte size 
                            auxArray[i + 1, currentColumn] = (incomingFileInfos[i].Size / 1099511627776).ToString() + " Tb";
                            break;
                        case > 1073741824: //gigabyte size 
                            auxArray[i + 1, currentColumn] = (incomingFileInfos[i].Size / 1073741824).ToString() + " Gb";
                            break;
                        case > 1048576: //megabyte size 
                            auxArray[i + 1, currentColumn] = (incomingFileInfos[i].Size / 1048576).ToString() + " Kb";
                            break;
                        case > 1024: //kilobyte size 
                            auxArray[i + 1, currentColumn] = (incomingFileInfos[i].Size / 1024).ToString() + " Kb";
                            break;

                        default:
                            auxArray[i + 1, currentColumn] = (incomingFileInfos[i].Size).ToString() + " bytes";// (1+...)shift for headder
                            break;

                    }
                }
                currentColumn++;
            }
            if (showChangedate)
            {
                auxArray[0, currentColumn] = "date of last change";
                for (var i = 0; i < incomingFileInfos.Count; i++)
                {
                    auxArray[i + 1, currentColumn] = incomingFileInfos[i].Changedate.ToString();
                }
            }

            return BuildTable((auxArray), headOfTable);
        }

        internal static IList<string> BuildTable<T1,T2>(IList<(T1, T2)> input, string head = "", 
            string firstColumnName="", string secondColumnName = "")
        {
            var columnNamesExists = (!string.IsNullOrEmpty(firstColumnName)) && (!string.IsNullOrEmpty(secondColumnName));
            var amountOfRows = input.Count();
            amountOfRows = columnNamesExists ? amountOfRows + 1: amountOfRows;
            var subArray = new string[amountOfRows,2];
            if (columnNamesExists)
            {
                subArray[0, 0] = firstColumnName;
                subArray[0,1] = secondColumnName;
            }
            var shift = columnNamesExists ? 1 : 0;
            for (int i = 0 ; i < amountOfRows-shift; i++)
            {
                subArray[i + shift, 0] = input[i].Item1.ToString();
                subArray[i+shift,1] = input[i].Item2.ToString();
            }
            return BuildTable(subArray, head);
        }
    }
}