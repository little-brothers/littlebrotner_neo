
using System.Collections.Generic;

[AssetPath("ending")]
public struct EndingData : IDatabaseRow
{
    public int id { private set; get; }
	public string endingQuestion { private set; get; }
	public string endingTitle { private set; get; }
	public string endingDescription { private set; get; }
	public string endingHint { private set; get; }
    public string[] conditions { private set; get; }

    int IDatabaseRow.ID { get { return id; } }

    bool IDatabaseRow.Parse(List<string> row)
    {
        id = int.Parse(row[0].Substring(1));
        endingQuestion = row[3];
        endingHint = row[12];
        endingDescription = row[10];
        endingTitle = row[5];
        conditions = new string[]{row[6], row[7]};

        return true;
    }
}
