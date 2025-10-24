using CsvHelper.Configuration;

namespace Aftertime.StorylineEngine
{
    public class TextElementMap : ClassMap<TextElement>
    {
        public TextElementMap()
        {
            Map(m => m.Index).Name("Index");
            Map(m => m.EpisodeName).Name("EpisodeName");
            Map(m => m.Uid).Name("Uid");
            Map(m => m.Text).Name("Text");
        
            Map(m => m.Characters)
                .Name("Characters")
                .TypeConverter(new CsvArrayConverter<string>());
        
            Map(m => m.FunctionName).Name("FunctionName");
            Map(m => m.FunctionType).Name("FunctionType");
        
            Map(m => m.FunctionValuesRaw)
                .Name("FunctionValues")
                .TypeConverter(new CsvArrayConverter<string>());
        
            Map(m => m.SelectElements)
                .Name("SelectElements")
                .TypeConverter(new CsvArrayConverter<string>());
        
            Map(m => m.Conversation).Name("Conversation");
        
            Map(m => m.VoicePath)
                .Name("VoicePath")
                .TypeConverter(new CsvArrayConverter<string>());
        }
    }   
}
