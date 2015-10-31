using System;
using SFML.System;
using SFML.Graphics;

namespace EquationInvasion
{
	public class Player
	{
		private CircleShape _shape;

		public Player (FloatRect screenRect)
		{
			// setup player
			_shape = new CircleShape(30, 3);
			_shape.Position = new Vector2f(screenRect.Width/2 - (_shape.Radius), screenRect.Height - (_shape.Radius*2));
			_shape.FillColor = Color.Green;
		}
		public void Draw(RenderWindow window)
		{
			window.Draw (_shape);
		}

		public void Move(Direction d, FloatRect screenRect)
		{
			float dist = 10.5f;

			// Move players position only if its inside the screen
			switch (d) {
			case Direction.LEFT:
				if (_shape.Position.X >= dist)
					_shape.Position = new Vector2f (_shape.Position.X - dist, _shape.Position.Y);
				break;
			case Direction.RIGHT:
				if (_shape.Position.X <= screenRect.Width - (_shape.Radius*2) - dist)
					_shape.Position = new Vector2f (_shape.Position.X + dist, _shape.Position.Y);
				break;
			}
		}
	}
}

