using System;
using SFML.System;
using SFML.Graphics;
using System.Collections.Generic;

namespace EquationInvasion
{
	public class Target
	{
		private RectangleShape _shape;
		private static float _border = 50.0f;

		public Target () : this(0, 0) {}
		public Target (float x, float y)
		{
			_shape = new RectangleShape (new Vector2f (60.0f, 60.0f));
			_shape.Position = new Vector2f (x, y);
		}

		public void Draw(RenderWindow window)
		{
			window.Draw(_shape);
		}

		public void Oscillate(Direction dir)
		{
			if (dir == Direction.LEFT)
				_shape.Position = new Vector2f (_shape.Position.X - 1.5f, _shape.Position.Y);
			else if (dir == Direction.RIGHT)
				_shape.Position = new Vector2f (_shape.Position.X + 1.5f, _shape.Position.Y);
		}

		/// <summary>
		/// Gets A rectangle which represents the Targets size/pos.
		/// </summary>
		/// <value>The rect.</value>
		public FloatRect Rect
		{
			get { return new FloatRect(_shape.Position, _shape.Size);}
		}


		/// <summary>
		/// Generate a list of targets based in passed in demensions.
		/// </summary>
		/// <returns>The targets.</returns>
		/// <param name="w">Number of horizontal targets.</param>
		/// <param name="h">Number of vertical targets.</param>
		public static List<Target> GenTargets(int h, int v) {
			List<Target> grid = new List<Target>();

			float gap = 40.0f;
			float x = _border;
			float y = _border;
			for (int i = 0; i < h; i++) {
				for (int j = 0; j < v; j++) {
					grid.Add (new Target (x + ((60+gap)*i), y + ((60+gap)*j) + gap));
				}
			}

			return grid;
		}
		
		public static bool HasOscillatedToEdge(Target t)
		{
			if (t.Rect.Left + (t.Rect.Width/2) < _border)
				return true;
			else if (t.Rect.Left + (t.Rect.Width/2) > 1000 - _border)
				return true;
			else
				return false;
		}

		public static List<Target> GetOuterTargets(List<Target> targets)
		{
			// find outer left side
			Target outerLeft = targets[0];
			foreach (Target t in targets) {
				if (t.Rect.Left < outerLeft.Rect.Left)
					outerLeft = t;
			}

			// find outer right side
			Target outerRight = targets[0];
			foreach (Target t in targets) {
				if (t.Rect.Left > outerRight.Rect.Left)
					outerRight = t;
			}

			// return outer left and right targets
			return new List<Target> () {outerLeft, outerRight};
		}

		public static bool HasOuterTargetHitScreenEdge(List<Target> targets)
		{
			List<Target> outer = GetOuterTargets (targets);
			return (HasOscillatedToEdge(outer[0]) || HasOscillatedToEdge(outer[1]));
		}

		public static bool HasBulletHitTarget(RectangleShape b, Target t)
		{
			// check for a bounding box collision
			return !(b.Position.X > t.Rect.Left + t.Rect.Width
				|| b.Position.X + b.Size.X < t.Rect.Left
				|| b.Position.Y > t.Rect.Top + t.Rect.Height
				|| b.Position.Y + b.Size.Y < t.Rect.Top);
		}
	}
}

