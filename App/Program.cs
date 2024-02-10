// See https://aka.ms/new-console-template for more information
using ParserClasses;

class Program{
    public static void Main(string[] args){
        List<int> l= new List<int>{2,3,5,7};
        List<int> e = l.GetRange(2,0);
        Console.WriteLine($"Hello, World! {e.Count}");
        foreach( var el in e){
            Console.WriteLine(el);
        }
        //new Parser(@"./test/sample.yaml");
    }
}
