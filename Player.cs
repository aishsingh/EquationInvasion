using System;
using SFML.System;
using SFML.Graphics;
using System.Collections.Generic;

namespace EquationInvasion
{
	public class Player
	{
		private List<RectangleShape> _bullets;
		private CircleShape _shape;
		private const float BULLET_HEIGHT = 20.0f;
		private const float BULLET_SPEED = 20.0f;

		public Player (FloatRect screenRect)
		{
			// setup player
			_shape = new CircleShape(30, 3);
			_shape.Position = new Vector2f(screenRect.Width/2 - (_shape.Radius), screenRect.Height - (_shape.Radius*2));
			_shape.FillColor = Color.Green;

			_bullets = new List<RectangleShape> ();
		}

		public void Draw(RenderWindow window)
		{
			window.Draw (_shape);

			// Draw the bullets on the screen and move then up
			// Remove them once they leave the screen
			foreach (RectangleShape b in _bullets.ToArray()) {
				window.Draw (b);

				if (b.Position.Y > 0 + BULLET_HEIGHT)
					b.Position = new Vector2f (b.Position.X, b.Position.Y - BULLET_SPEED);
				else {
					_bullets.Remove (b);
					break;
				}
			}
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

		public void Shoot()
		{
			RectangleShape bullet = new RectangleShape (new Vector2f(10, BULLET_HEIGHT));
			bullet.Position = new Vector2f(_shape.Position.X + (_shape.Radius/2) + (bullet.Size.X), _shape.Position.Y - (bullet.Size.Y/2));
			bullet.FillColor = Color.Green;

			_bullets.Add (bullet);
		}

		public List<RectangleShape> bullets
		{
			get { return _bullets; }
		}

		public void RemoveBullet (RectangleShape b)
		{
			_bullets.Remove (b);
		}
	}
}

