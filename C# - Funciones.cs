using System;

namespace CodigosGuia
{
	public class Bucles
	{
		Int32 Contador = 0;
		Int32 Num;
		readonly String Mensaje = 
			"Hola Mundo!";
		readonly String[] Frutas = 
			{ "Manzana", "Uvas", "Pera", "Naranja" };
		/* While:
			Primero valida si la condición es verdadera, y despues ejecuta
			En este caso, si Contador es menor que 10
		*/
		void Bucle_While()
		{
			while (Contador < 10)
			{
				Console.WriteLine(Mensaje);
				Contador++;
			}
		}
		/* Do While:
			Primero ejecuta y despues verifica la condición
			En este caso pide ingresar una valor igual o menor que 10, si Num es mayor termina
		*/
		void Bucle_DoWhile()
		{
			do
			{
				Console.WriteLine("Escribe numero");
				Num = Int32.Parse(Console.ReadLine());
			}
			while(Num <= 10);
		}
		/* For:
			For requiere la variable Contador (i) de forma interna como se muestra
			Primero valida si la condición es verdadera, y despues ejecuta
		*/
		void Bucle_For()
		{
			for (int i = 0; i < 5; i++)
			{
				Console.WriteLine(Mensaje);
			}
			// Recorrer e imprimir valores del Array Frutas:
			for (int i = 0; i < Frutas.Length; i++)
			{
				Console.WriteLine(Frutas[i]);
			}
		}
		/* foreach:
			El foreach recorre y hace algo especifico con cada elemento según codifiquemos
			En es caso, imprimir valores del Array Frutas:
		*/
		void Bucle_Foreach()
		{
			foreach (String Fruta in Frutas)
			{
				Console.WriteLine(Fruta);
			}
		}
	}
}