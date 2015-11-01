using System;
using SFML.System;
using SFML.Graphics;

namespace EquationInvasion
{
	public enum EquationType
	{
		ADDITION,
		SUBTRACTION,
		MULTIPLICATION,
		DIVISISON
	}

	public enum EquationDifficulty
	{
		EASY,
		INTERMEDIATE,
		HARD
	}

	public struct Equation
	{
		public int val1;
		public EquationType type;
		public int val2;
		public float solution;

		public static String GetString(EquationType type)
		{
			switch (type) {
			default:
				return "+";
			case EquationType.SUBTRACTION:
				return "-";
			case EquationType.MULTIPLICATION:
				return "x";
			case EquationType.DIVISISON:
				return "/";
			}
		}
	}

	public class EquationTarget : Target
	{

		private Equation _eq;
		public EquationTarget () : this (0.0f, 0.0f, EquationDifficulty.EASY, new Random()) {}
		public EquationTarget (float x, float y, EquationDifficulty diff, Random r) : base(x, y, Color.Red)
		{
			_eq = GenEquation (diff, r);
		}

		public Equation GetEquation
		{
			get { return _eq; }
		}

		public void Selected()
		{
			float offset = 5.0f;
			_shape.FillColor = Color.Black;
			_shape.Size = new Vector2f(_shape.Size.X - (offset*2), _shape.Size.Y - (offset*2));
			_shape.Position = new Vector2f (_shape.Position.X + offset, _shape.Position.Y + offset);
			_shape.OutlineThickness = offset;
			_shape.OutlineColor = Color.Red;
		}

		/// <summary>
		/// Generates a random equation.
		/// </summary>
		/// <returns>The equation.</returns>
		/// <param name="diff">Diff.</param>
		/// <param name="r">Random generator. This is needed to get unique rand values each time</param>
		public static Equation GenEquation(EquationDifficulty diff, Random r) {
			Equation eq = new Equation ();

			if (diff == EquationDifficulty.EASY) {
				eq.val1 = r.Next (0, 10);
				eq.val2 = r.Next (0, 10);
				eq.type = (EquationType)r.Next (0, 2);
			} 
			else if (diff == EquationDifficulty.INTERMEDIATE) {
				eq.val1 = r.Next (0, 15);
				eq.val2 = r.Next (0, 15);
				eq.type = (EquationType)r.Next (0, 3);
			} 
			else {
				// hard
				eq.val1 = r.Next (0, 50);
				eq.val2 = r.Next (0, 50);
				eq.type = (EquationType)r.Next (0, 4);
			}

			eq.solution = CalcSolution(eq);	

			return eq;
		}

		public static float CalcSolution(Equation eq)
		{
			switch (eq.type) {
				case EquationType.ADDITION:
					return eq.val1 + eq.val2;

				case EquationType.SUBTRACTION:
					return eq.val1 - eq.val2;

				case EquationType.MULTIPLICATION:
					return eq.val1 * eq.val2;

				case EquationType.DIVISISON:
				return eq.val1 / eq.val2;
			}

			// it shouldn't reach this code
			return 0.0f;
		}
	}
}

