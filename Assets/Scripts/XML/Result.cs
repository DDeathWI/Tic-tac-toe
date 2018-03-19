using System.Xml;
using System.Xml.Serialization;

public class Result
{
    [XmlAttribute("Result")]
    public string result;

    public int SpendTime;

    public Result()
    {
        
    }

    public Result(string str, int time)
    {
        result = str;
        SpendTime = time;
    }
}
