using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Parser
{
    public static List<List<List<string>>> DialogParse(TextAsset data)
    {
        List<List<List<string>>> returnList = new List<List<List<string>>>();

        StringReader sr = new StringReader(data.text);
        string source = sr.ReadLine();              // 먼저 한줄을 읽는다. 
        string[] values;                            // 구분된 데이터들을 저장할 배열 (values[0]이면 첫번째 데이터 )

        while (source != null)
        {
            values = source.Split('\t');        // tab으로 구분한다

            int num = int.Parse(values[0]);
            List<string> dialog = new List<string>();

            for (int i = 1; i < values.Length; i++)
            {
                dialog.Add(values[i]);
            }

            if (returnList.Count <= num) returnList.Add(new List<List<string>>());

            returnList[num].Add(dialog);

            source = sr.ReadLine();             // 한줄 읽는다.
        }

        return returnList;
    }

    public static List<ItemInfo> ItemParse(TextAsset data)
    {
        List<ItemInfo> returnList = new List<ItemInfo>();

        StringReader sr = new StringReader(data.text);
        string source = sr.ReadLine();  // 먼저 한줄을 읽는다. 
        string[] values;                // 구분된 데이터들을 저장할 배열 (values[0]이면 첫번째 데이터 )

        while (source != null)
        {
            values = source.Split('/'); // Slash로 구분

            int indexNum = int.Parse(values[0]);
            Sprite image = Resources.Load<Sprite>("ItemImage/" + values[2]);
            int hpChange = indexNum != 0 ? int.Parse(values[3]) : 0;
            int mpChange = indexNum != 0 ? int.Parse(values[4]) : 0;

            returnList.Add(new ItemInfo(indexNum, values[1], image, hpChange, mpChange, values[5]));

            source = sr.ReadLine();    // 한줄 읽는다.
        }

        return returnList;
    }

    public static List<Problem> CrammedParse(TextAsset data)
    {
        List<Problem> ret = new List<Problem>();

        StringReader sr = new StringReader(data.text);
        string source = sr.ReadLine();  // 먼저 한줄을 읽는다. 
        string[] values;                // 구분된 데이터들을 저장할 배열 (values[0]이면 첫번째 데이터 )

        while (source != null)
        {
            string question = "";
            List<string> examples = new List<string>();
            int answer;

            question = source;

            source = sr.ReadLine();
            values = source.Split('\t'); // tab으로 구분
            foreach (string example in values)
            {
                examples.Add(example);
            }

            source = sr.ReadLine();
            answer = int.Parse(source);

            Problem problem = new Problem(question, examples, answer);
            ret.Add(problem);

            source = sr.ReadLine();    // 한줄 읽는다.
        }

        return ret;
    }

    public static List<CarrotTalkDialogue> SkippingParse(TextAsset data)
    {
        List<CarrotTalkDialogue> ret = new List<CarrotTalkDialogue>();

        StringReader sr = new StringReader(data.text);
        string source = sr.ReadLine();  // 먼저 한줄을 읽는다. 
        string[] values;                // 구분된 데이터들을 저장할 배열 (values[0]이면 첫번째 데이터 )

        while (source != null)
        {
            string question = "";
            List<string> answers = new List<string>();
            List<int> points = new List<int>();
            List<string> reactions = new List<string>();

            values = source.Split('\t');
            question = values[1];

            for (int i = 0; i < 4; i++)
            {
                source = sr.ReadLine();
                values = source.Split('\t');
                answers.Add(values[1]);
                points.Add(int.Parse(values[2]));
            }

            for (int i = 0; i < 4; i++)
            {
                source = sr.ReadLine();
                values = source.Split('\t');
                reactions.Add(values[1]);
            }

            CarrotTalkDialogue dialogue = new CarrotTalkDialogue(question, answers, points, reactions);
            ret.Add(dialogue);

            source = sr.ReadLine();
        }

        return ret;
    }

    public static List<Collection> CollectionParse(TextAsset data)
    {
        List<Collection> returnList = new List<Collection>();

        StringReader sr = new StringReader(data.text);
        string source = sr.ReadLine();  // 먼저 한줄을 읽는다. 
        string[] values;                // 구분된 데이터들을 저장할 배열 (values[0]이면 첫번째 데이터 )

        while (source != null)
        {
            values = source.Split('\t'); // tab으로 구분
            if (values.Length != 3) Debug.Log("Collection Parsing Warning at " + values[0] + " : " + data.name + "를 읽은 과정에서 길이가 3가 아닌 줄을 발견했습니다.");

            string name = values[0];
            string desc = values[1];
            Sprite img = Resources.Load<Sprite>("CollectionImage/" + values[2]);

            returnList.Add(new Collection(false, name, desc, img));

            source = sr.ReadLine();    // 한줄 읽는다.
        }

        return returnList;
    }
}