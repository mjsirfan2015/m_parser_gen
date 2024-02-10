using System.Data;
using System.Text.RegularExpressions;
using YamlDotNet.Core.Tokens;

namespace ParserClasses
{
    public class Variable
    {
        private string? _type;
        public string? Name { get; set; }
        public string? Type { get{
            return _type ?? "(T)";
        } set{
            _type=value;
        } }

        public override string ToString(){
            return $"Variable(name={this.Name},type={this.Type})";
        }

        public static bool operator ==(Variable? x, Variable? y){
            if(x is null){
                return y is null;
            }
            if(y is null)return false;
            return x.Name == y.Name && x.Type == y.Type;
        }

        public static bool operator !=(Variable x, Variable y){
            return x.Name != y.Name || x.Type != y.Type;
        }

        public override bool Equals(object? obj){
            return this.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class Grammar
    {
        public Variable? Lhs { get; set; }
        public List<List<Variable>>? Rhs { get; set; }

        public string GetLhs(bool showType = false)
        {
            if (this.Lhs == null) throw new ArgumentNullException($"{nameof(Lhs)} is null");
            string type = showType ? (":" + this.Lhs.Type) : "";
            return $"<{this.Lhs.Name}{type}>";
        }

        public string GetRhs(bool showType = false)
        {
            if (this.Rhs == null) throw new ArgumentNullException("RHS is null");
            string rhs = "";
            for (int i = 0; i < this.Rhs.Count; i++)
            {
                List<Variable>? statement = this.Rhs[i];
                foreach (Variable token in statement)
                {
                    string type = showType ? (":" + token.Type) : "";
                    rhs += $"<{token.Name}>";
                }
                if (i != this.Rhs.Count - 1)
                {
                    rhs += "|";
                }
            }
            return rhs;
        }

        public override string ToString()
        {
            string? lhs = this.GetLhs();
            string? rhs = this.GetRhs();
            return $"Grammar({lhs} := {rhs})";
        }

    }

    public class RuleSet
    {
        public string? Rule { get; set; }
    }

    public class Root
    {
        public List<Variable>? Variables { get; set; }
        public List<RuleSet>? Grammars { get; set; }
        public Dictionary<string, Variable>? VarMap;

        private void GenerateMap()
        {
            VarMap = new Dictionary<string, Variable>();
            if (Variables != null)
            {
                foreach (Variable variable in Variables)
                {
                    if (variable.Name != null) VarMap.Add(variable.Name, variable);
                    else throw new ArgumentNullException("Variable Name is null");
                }
            }
            else
            {
                throw new ArgumentNullException("Variables is null");
            }
        }

        private List<List<Variable>> MatchList(string input)
        {
            List<List<Variable>> varList = new();
            bool flag;
            flag = true;
            string tok = "";
            if (VarMap == null) throw new ArgumentNullException("Variable Map not generated");
            string[] statements = input.Split("|");

            foreach (string statement in statements)
            {
                List<Variable> lst = new();
                foreach (char c in statement)
                {
                    if (c == '<')
                    {
                        flag = false;
                    }
                    else if (c == '>')
                    {
                        flag = true; //set flag back to true
                                     //get appropriate variable from varmap
                        try
                        {
                            Variable variable = VarMap[tok];
                            lst.Add(variable);
                        }
                        catch (KeyNotFoundException)
                        {
                            throw;
                        }
                        tok = "";//clear token
                    }
                    else
                    {
                        //scan token if flag is false
                        if (flag == true)
                        {
                            throw new InvalidDataException("Missing '<' ");
                        }
                        tok += c;
                    }
                }
                varList.Add(lst);
            }
            return varList;

        }

        public List<Grammar> GetGrammars()
        {
            this.GenerateMap();
            if (Grammars == null) throw new ArgumentNullException("Grammars is null");
            List<Grammar> grammars = new();
            foreach (RuleSet grammar in Grammars)
            {
                string? rule = grammar.Rule ?? throw new ArgumentNullException("rule is null");
                string[] split = rule.Split(":=");
                if (split.Length != 2)
                {
                    throw new InvalidExpressionException($"Invalid Rule! Got {split.Length} items");
                }
                Grammar grm = new Grammar();
                string lhs = split[0].Trim(), rhs = split[1].Trim();

                if (lhs.StartsWith("<") && lhs.EndsWith(">"))
                {
                    try
                    {
                        if (VarMap != null) grm.Lhs = VarMap[lhs.Substring(1, lhs.Length-2)];
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Console.WriteLine($"lhs.Substring(1, lhs.Length) is out of range.Invalid key is \"{lhs.Substring(1, lhs.Length)}\"");
                        Environment.Exit(0);
                    }
                    catch (KeyNotFoundException)
                    {
                        Console.WriteLine($"Invalid Key: \"{lhs.Substring(1, lhs.Length)}\"");
                        Environment.Exit(0);
                    }
                }

                grm.Rhs = MatchList(rhs);
                grammars.Add(grm);
            }
            return grammars;
        }
    }

}