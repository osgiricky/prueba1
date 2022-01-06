using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mountain
{

    class Program
    {
        public static int n = 1000;
        public static Calculated Calculo = new Calculated();
        static void Main(string[] args)
        {
            var matrix = ReadMatrixFromFile("C:\\Personal\\EASE SOLUTION\\map.txt");
            List<int> ListaAux = new List<int>();
            for (var x = 0; x < n; x++)
            {
                for (var y = 0; y < n; y++)
                {
                    ListaAux.Clear();
                    ListaAux.Add(matrix[x][y]);
                    Calculo = traverse(x, y, 1, matrix[x][y], ref matrix, ref ListaAux);                    
                }
            }
            Console.WriteLine("Length of calculated path: " + Calculo.Length);
            Console.WriteLine("Drop of calculated path: " + Calculo.Drop);
            //Console.WriteLine("Calculated path : " + Calculo.PathCalculated);
            Console.Read();
        }

        static int[][] ReadMatrixFromFile(string inputFile)
        {
            var lines = GetLines(inputFile);
            int[][] matrix = new int[lines.Count -1][];
            for (int i = 1; i < lines.Count; i++)
            {
                var line = lines[i];
                matrix[i-1] = line.Split(' ').Select(x => x.Trim()).Select(x => int.Parse(x)).ToArray();
            }
            return matrix;
        }

        static List<string> GetLines(string inputFile)
        {
            List<string> lines = new List<string>();
            using (var reader = new StreamReader(inputFile))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        lines.Add(line);
                    }
                }
            }
            return lines;
        }

        static Calculated traverse(int x, int y, int length, int start, ref int[][] matrix, ref List<int> ListaAux)
        {
            //consider as x y axis, instead of doing 4 if block. we can think about it as
            //current point [x,y]
            //go up [x, y + 1], go right [x + 1, y], go down [x, y - 1], go left [x - 1, y]
            int[] xAxis = { 0, 1, 0, -1 };
            int[] yAxis = { 1, 0, -1, 0 };
            

            for (var k = 0; k < 4; k++)
            {
                //check if the moving is still inside the graph
                bool isInsideGraph = x + xAxis[k] >= 0 && x + xAxis[k] < n && y + yAxis[k] >= 0 && y + yAxis[k] < n;
                if (isInsideGraph && (matrix[x][y] > matrix[x + xAxis[k]][y + yAxis[k]]))
                {

                    //if can traverse and the current value is bigger the the next traverse point.
                    //set the length and keep the start point. to calculate the maxlength and drop later.  
                    ListaAux.Add(matrix[x + xAxis[k]][y + yAxis[k]]);
                    traverse(x + xAxis[k], y + yAxis[k], length + 1, start, ref matrix, ref ListaAux);
                    
                }
            }

            //current drop
            var drop = start - matrix[x][y];
            if (length > Calculo.Length)
            {
                Calculo.Length = length;
                Calculo.Drop = drop;
                Calculo.PathCalculated = ListaAux.ToArray();
            }
            else if (length == Calculo.Length && Calculo.Drop < drop)
            {
                Calculo.Length = length;
                Calculo.Drop = drop;
                Calculo.PathCalculated = ListaAux.ToArray();
            }

            return Calculo;
        }

    }
}
