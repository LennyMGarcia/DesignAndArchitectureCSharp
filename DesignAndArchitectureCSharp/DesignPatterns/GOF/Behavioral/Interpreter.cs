using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * El patron Interpreter tiene como objetivo proporcionar una forma de definir una gramatica
 * para un lenguaje y evaluar expresiones escritas en ese lenguaje
 * Esto permite que el sistema sea mas flexible y escalable
 * ya que se puede agregar o modificar la gramatica sin necesidad de cambiar el codigo del sistema.
 */
namespace DesignAndArchitectureCSharp.DesignPatterns.GOF.Behavioral
{
    public abstract class Expression
    {
        public abstract double Evaluate();
    }

    public class NumberExpression(double value) : Expression
    {
        private double _value = value;

        public override double Evaluate()
        {
            return _value;
        }
    }

    public abstract class BinaryExpression(Expression left, Expression right) : Expression
    {
        protected Expression _left = left;
        protected Expression _right = right;
    }

    public class AddExpression(Expression left, Expression right) : BinaryExpression(left, right)
    {

        //se sumas los valores asignados
        public override double Evaluate()
        {
            return _left.Evaluate() + _right.Evaluate();
        }
    }

    public class SubtractExpression(Expression left, Expression right) : BinaryExpression(left, right)
    {
        public override double Evaluate()
        {
            return _left.Evaluate() - _right.Evaluate();
        }
    }

    public class MultiplyExpression(Expression left, Expression right) : BinaryExpression(left, right)
    {
        public override double Evaluate()
        {
            return _left.Evaluate() * _right.Evaluate();
        }
    }

    public class DivideExpression(Expression left, Expression right) : BinaryExpression(left, right)
    {
        public override double Evaluate()
        {
            if (_right.Evaluate() == 0)
            {
                throw new DivideByZeroException();
            }
            return _left.Evaluate() / _right.Evaluate();
        }
    }
    //El interpreter
    public class MathInterpreter
    {
        private Dictionary<string, Func<Expression, Expression, Expression>> _operators;

        public MathInterpreter()
        {
            //Se asignan los simbolos a interpretar y el func dique se acepta y que debe devolver
            _operators = new Dictionary<string, Func<Expression, Expression, Expression>>
        {
            { "+", (left, right) => new AddExpression(left, right) },
            { "-", (left, right) => new SubtractExpression(left, right) },
            { "*", (left, right) => new MultiplyExpression(left, right) },
            { "/", (left, right) => new DivideExpression(left, right) }
        };
        }

        public double Evaluate(string expression)
        {
            //El string se divide por espacios
            string[] tokens = expression.Split(' ');
            //Parse se encarga de construir el arbol a analizar, tendra muchas expresiones que se analizaran
            Expression ast = Parse(tokens);

            return ast.Evaluate();
        }

        private Expression Parse(string[] tokens)
        {
            Stack<Expression> stack = new Stack<Expression>();

            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];

                if (_operators.ContainsKey(token))
                {
                    // Tomar el operando izquierdo de la pila
                    Expression left = stack.Pop();

                    // Tomar el proximo token como operando derecho
                    Expression right = new NumberExpression(double.Parse(tokens[i + 1]));
                    // Construir la expresion binaria y pushearla en la pila
                    // asi es como se usa un diccionario que contiene FUnciones
                    stack.Push(_operators[token](left, right));

                    // Saltar el proximo token, recuerda que esta este i++ y el otro, no va al right sino al que va despues
                    i++;
                }
                else
                {
                    // Si no es un operador, es un numero, se convierte a double y se pusha en la pila
                    stack.Push(new NumberExpression(double.Parse(token)));
                }
            }

            return stack.Pop();
        }
    }
}
