using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Parser
{
    public static List<List<List<string>>> DialogParse(TextAsset data)
    {
        List<List<List<string>>> returnList = new List<List<List<string>>>();
        StringReader sr = new StringReader(data.text);

        bool isEnd = false;
        while (isEnd == false)
        {
            bool readMode = false;

            while (readMode == false)       // ===가 나올때까지 읽는다.
            {
                string findStart = sr.ReadLine();
                if (findStart == "===")
                {
                    readMode = true;
                }
                if (findStart == "END")
                {
                    isEnd = true;
                    break;
                }
            }

            if (isEnd == true) break;

            string source = sr.ReadLine();  // 먼저 한줄을 읽는다. 
            string[] values;                // 구분된 데이터들을 저장할 배열 (values[0]이면 첫번째 데이터 )
            List<List<string>> dialog = new List<List<string>>();   // 하나의 대화를 저장하는 2차원 리스트

            while (source != "===")         //===를 만나면 그만둔다.
            {
                //Debug.Log(source);
                values = source.Split('/');  // Slash로 구분한다

                List<string> temp = new List<string>();
                for (int i = 0; i < values.Length; i++)
                {
                    temp.Add(values[i]);
                }
                dialog.Add(temp);

                source = sr.ReadLine();    // 한줄 읽는다.
            }

            returnList.Add(dialog);
        }

        return returnList;
    }
}