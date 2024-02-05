using YamlDotNet.Core.Tokens;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using ParserClasses;
using System.Text.RegularExpressions;
namespace ParseUtils
{
    abstract class Parser
    {
        List<Variable>? Variables { get; set; }
        List<Grammar>? Grammars { get; set; }

        public string? Type { get; set; }

        protected Dictionary<string, string> Messages;
        private readonly string? parserText = null;
        abstract protected void ExtraCalls();
        abstract protected string ParserAlgorithm();
        public Parser(string filename)
        {
            this.Messages = new Dictionary<string, string>{
                {"read_file","Reading Grammar File"},
                {"deserialize","Deserializing Grammar file"},
                {"parse","Parsing Grammar"}
            };
            this.parserText = File.ReadAllText(filename);
            this.Message("read_file");
        }

        protected void Message(string code)
        {
            Console.WriteLine(Messages[code] + "....");
        }

        private string Deserialize()
        {
            /**
                Deserialize yaml file and convert to Grammar and  Variable objects
            */
            if (this.parserText == null) throw new ArgumentNullException("ParserText is null");

            IDeserializer? deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build() ?? throw new ArgumentNullException("Deserializer is null!");

            Root? root = deserializer.Deserialize<Root>(this.parserText);
            this.Variables = root.Variables ?? throw new ArgumentNullException("Variables not found!");
            this.Grammars = root.GetGrammars();
            return "deserialize";
        }

        public void Parse()
        {
            this.Message(this.Deserialize());
            this.ExtraCalls();
            this.Message(this.ParserAlgorithm());
        }

    }

}