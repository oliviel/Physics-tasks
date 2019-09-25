using System;
using System.Collections.Generic;

namespace fisica_2
{
    class Program
    {
        const decimal KE = 9E9m;
        static List<Particula> cargas = new List<Particula>();
        static void Main(string[] args)
        {
            bool isRunning = true;

            while (isRunning)
            {
                short opc = GetMenu();
                switch (opc)
                {
                    case 0: //Salir
                        isRunning = false;
                        break;

                    case 1: //Entrar cargas 
                        cargas = GetCharges();
                        break;

                    case 2: //Calcular Fuerzas
                        Console.Write($"Elija una carga [{cargas.Count - 1}]: ");
                        CalcularFuerzaResultante(Convert.ToInt32(Console.ReadLine()));
                        break;

                    case 3: //Calcular Campos Electricos
                        Console.WriteLine("Entre posicion del punto P");
                        Console.Write("X: ");
                        decimal x = Convert.ToDecimal(Console.ReadLine());
                        Console.Write("Y: ");
                        decimal y = Convert.ToDecimal(Console.ReadLine());
                        Console.Write("Z: ");
                        decimal z = Convert.ToDecimal(Console.ReadLine());
                        CalcularCampoElectrico(new Vector(x, y, z));
                        break;

                    case 4: //Calcular Potencial Electrico
                        Console.WriteLine("Entre posicion del punto P");
                        Console.Write("X: ");
                        x = Convert.ToDecimal(Console.ReadLine());
                        Console.Write("Y: ");
                        y = Convert.ToDecimal(Console.ReadLine());
                        Console.Write("Z: ");
                        z = Convert.ToDecimal(Console.ReadLine());
                        CalcularPotencialElectrico(new Vector(x, y, z));
                        break;

                    case 5: //Calcular Energia Potencial
                        CalcularEnergiaPotencial();
                        break;

                    default:
                        break;
                }
            }
        }

        private static void CalcularPotencialElectrico(Vector punto)
        {
            decimal magnitudResultante = 0m;
            for (int i = 0; i < cargas.Count; i++)
            {

                var distance = punto.DistanceTo(cargas[i].Position);
                var magnitude = KE * ((cargas[i].Carga) / distance);

                magnitudResultante += magnitude;

            }
            Console.WriteLine($"Potencial Electrico en P: {magnitudResultante.ToString("E")}");
        }

        private static void CalcularEnergiaPotencial()
        {
            decimal magnitudResultante = 0m;
            for (int i = 0; i < cargas.Count; i++)
            {

                for (int j = 0; j < cargas.Count; j++)
                {
                    if (i < j)
                    {
                        var distance = cargas[i].Position.DistanceTo(cargas[j].Position);
                        var magnitude = KE * ((cargas[i].Carga * cargas[j].Carga) / distance);

                        magnitudResultante += magnitude;

                        Console.WriteLine(magnitudResultante.ToString("E"));
                    }
                }
            }
            Console.WriteLine($"Energia Potencial: {magnitudResultante.ToString("E")}");
        }

        private static void CalcularCampoElectrico(Vector punto)
        {
            Vector campoResultante = new Vector();
            for (int i = 0; i < cargas.Count; i++)
            {

                var distance = punto.DistanceTo(cargas[i].Position);
                var magnitude = KE * ((cargas[i].Carga) / (decimal)Math.Pow(decimal.ToDouble(distance), 3));

                campoResultante += magnitude * (punto - cargas[i].Position);
            }
            Console.WriteLine($"Fuerza Resultante Campo Eletrico: {campoResultante.Length().ToString("E")}");
            Console.WriteLine($"Sus componentes son: {campoResultante}");
        }

        private static void CalcularFuerzaResultante(int index)
        {
            Vector fuerzaTotal = new Vector();
            for (int i = 0; i < cargas.Count; i++)
            {
                if (index != i)
                {
                    var distance = cargas[index].Position.DistanceTo(cargas[i].Position);
                    var magnitude = KE * ((cargas[index].Carga * cargas[i].Carga) / (decimal)Math.Pow(decimal.ToDouble(distance), 3));

                    fuerzaTotal += magnitude * (cargas[index].Position - cargas[i].Position);
                }
            }
            Console.WriteLine($"Su Fuerza Resultante es: {fuerzaTotal.Length().ToString("E")}");
            Console.WriteLine($"Sus componentes son: {fuerzaTotal}");
        }

        private static List<Particula> GetCharges()
        {
            List<Particula> particulas = new List<Particula>();
            while (true)
            {
                var particula = new Particula();

                Console.WriteLine("Inserte el numero 0 para salir");
                Console.Write("Carga: ");
                particula.Carga = Convert.ToDecimal(Console.ReadLine());

                if (particula.Carga == 0)
                    break;

                Console.Write("x: ");
                particula.Position.X = Convert.ToDecimal(Console.ReadLine());
                Console.Write("y: ");
                particula.Position.Y = Convert.ToDecimal(Console.ReadLine());
                Console.Write("z: ");
                particula.Position.Z = Convert.ToDecimal(Console.ReadLine());

                particulas.Add(particula);
            }

            return particulas;
        }

        static short GetMenu()
        {
            Console.WriteLine("1 = Insertar las cargas");
            Console.WriteLine("2 = Calcular Fuerza Electrica");
            Console.WriteLine("3 = Calcular Campos Electricos");
            Console.WriteLine("4 = Calcular Potencial Electrico");
            Console.WriteLine("5 = Calcular Energia Potencial");
            Console.WriteLine("0 = Salir");

            return Convert.ToInt16(Console.ReadLine());
        }
    }

    public struct Particula
    {
        public Vector Position;
        public decimal Carga;

        public Particula(decimal carga, Vector position)
        {
            this.Position = position;
            this.Carga = carga;
        }
    }

    public struct Vector
    {
        public decimal X;
        public decimal Y;
        public decimal Z;

        public Vector(decimal x, decimal y, decimal z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vector operator *(Vector a, decimal scalar)
        {
            return new Vector(scalar * a.X, scalar * a.Y, scalar * a.Z);
        }

        public static Vector operator *(decimal scalar, Vector a)
        {
            return a * scalar;
        }

        public decimal DotProduct(Vector other)
        {
            return X * other.X + Y * other.Y + Z * other.Z;
        }

        public decimal Length()
        {
            return (decimal)Math.Sqrt(decimal.ToDouble(DotProduct(this)));
        }

        public decimal DistanceTo(Vector other)
        {
            return (this - other).Length();
        }

        public override string ToString()
        {
            return $"{this.X.ToString("E")}i + {this.Y.ToString("E")}j + {this.Z.ToString("E")}k";
        }
    }
}