using NUnit.Framework;
using System;

namespace EquationInvasion
{
	[TestFixture ()]
	public class EquationTests
	{
		[Test ()]
		public void TestEquationAdditionSolution ()
		{
			Equation eq = new Equation ();
			eq.val1 = 5;
			eq.val2 = 5;
			eq.type = EquationType.ADDITION;

			// 5 + 5 = 10
			Assert.AreEqual(10, EquationTarget.CalcSolution(eq));
		}

		[Test ()]
		public void TestEquationSubtractionSolution ()
		{
			Equation eq = new Equation ();
			eq.val1 = 5;
			eq.val2 = 5;
			eq.type = EquationType.SUBTRACTION;

			// 5 - 5 = 0
			Assert.AreEqual(0, EquationTarget.CalcSolution(eq));
		}

		[Test ()]
		public void TestEquationMultiplicationSolution ()
		{
			Equation eq = new Equation ();
			eq.val1 = 5;
			eq.val2 = 5;
			eq.type = EquationType.MULTIPLICATION;

			// 5 x 5 = 25
			Assert.AreEqual(25, EquationTarget.CalcSolution(eq));
		}

		[Test ()]
		public void TestEquationDivisionSolution ()
		{
			Equation eq = new Equation ();
			eq.val1 = 5;
			eq.val2 = 5;
			eq.type = EquationType.DIVISISON;

			// 5 / 5 = 1
			Assert.AreEqual (1, EquationTarget.CalcSolution(eq));
		}
	}
}

