using System.Linq.Expressions;
using ParserClasses;
using ParseUtils;

namespace CustomParsers
{

    class LL1Parser : Parser
    {

        LL1Parser(string filename) : base(filename)
        {
            this.Type = "LL(1)";
        }

        private void RemoveLeftRec()
        {
            /**
            algo:
            function remove_left_recursion(grammar):
                new_grammar = []
                for each production in grammar:
                    if production has left recursion:
                    split production.rhs into alpha and beta
                    create a new non-terminal A'
                    new_production_1 = {lhs: production.lhs, rhs: [beta, A']}
                    new_production_2 = {lhs: A', rhs: [alpha, A', epsilon]}
                    append new_production_1 and new_production_2 to new_grammar
                    else:
                    append production to new_grammar
                return new_grammar
            */
            
        }

        private List<List<Variable>?>[]? AlphaBetaSplit(Grammar prod){
            Variable lhs = prod.Lhs ?? throw new ArgumentNullException("lhs is null!");
            List<List<Variable>> rhs = prod.Rhs ?? throw new ArgumentNullException("rhs is null!");
            List<List<Variable>?> alphaList = new(),betaList = new();
            bool flag=false;
            foreach(List<Variable> expr in rhs){
                if(expr!=null && expr[0]==lhs){
                    List<Variable> newExpr = expr.GetRange(1,expr.Count-1);
                    alphaList.Add(expr);
                    //if left rec set flag true
                    flag=true;
                }else{
                    betaList.Add(expr);
                }
            }
            if(flag) return new List<List<Variable>?>[]{alphaList,betaList};
            return null;
        }

        private void LeftFactor()
        {
            throw new NotImplementedException();
        }

        private void First()
        {
            throw new NotImplementedException();
        }

        private void Follow()
        {
            throw new NotImplementedException();
        }

        protected override void ExtraCalls()
        {
            throw new NotImplementedException();
        }

        protected override string ParserAlgorithm()
        {
            throw new NotImplementedException();
            return "parse";
        }

        protected List<List<Variable>?>[]? TestAlphaBetaSplit(Grammar sampleGrammar)
        {
            var result = this.AlphaBetaSplit(sampleGrammar);
            return result;
        }

    }

}