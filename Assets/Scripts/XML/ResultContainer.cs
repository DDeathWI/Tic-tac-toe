using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

[XmlRoot("ResultContainet")]
public class ResultContainer
{
    [XmlArray("Results"), XmlArrayItem("Result")]
    public List<Result> Results;

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(ResultContainer));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static ResultContainer Load(string path)
    {
        var serializer = new XmlSerializer(typeof(ResultContainer));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as ResultContainer;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static ResultContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(ResultContainer));
        return serializer.Deserialize(new StringReader(text)) as ResultContainer;
    }

    public ResultContainer()
    {

    }
}
