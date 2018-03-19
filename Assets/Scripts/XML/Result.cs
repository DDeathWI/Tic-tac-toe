using System.Xml;
using System.Xml.Serialization;

public class Result
{
    [XmlAttribute("Result")]
    public string PlayerName;
    public string EnemyName;
    public string result;
    public int SpendTime;

    public Result()
    {
        
    }

    public Result(string playerName, string enemyName, string str, int time)
    {
        PlayerName = playerName;
        EnemyName = enemyName;
        result = str;
        SpendTime = time;
    }
}
