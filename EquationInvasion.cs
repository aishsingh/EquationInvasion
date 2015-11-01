using System;
using SFML.System;
using SFML.Graphics;
using SFML.Window;
using System.Collections.Generic;

namespace EquationInvasion
{
	class EquasionInvasion
	{
		RenderWindow _window;
		private FloatRect screenRect;
		private Direction _arrowKeyDown = Direction.NONE;
		private List<Target> _targets;
		private Player _player;
		private int _wave;
		private Direction _waveOscilatingDir = Direction.NONE;
		private bool _isSolving;
		private EquationTarget _eqToSolve;
		private String _eqCurrentInput;
		private Font _font;

		public EquasionInvasion() {
			SetupWindow();

			// setup player
			screenRect = new FloatRect (_window.Position.X, _window.Position.Y, _window.Size.X, _window.Size.Y);
			_player = new Player(screenRect);

			// initilise fiels
			_targets = new List<Target> ();
			_isSolving = false;
			_eqToSolve = new EquationTarget();
			_eqCurrentInput = "";

			// loaf font
			_font = new Font("nk57-monospace-no-bk.ttf");
		}
		public void SetupWindow()
		{
			// create window
			_window = new RenderWindow (new VideoMode (1000, 800), "Equation Invasion! - Tainted Mustard", Styles.Close);
			_window.SetVerticalSyncEnabled (true);
			_window.SetFramerateLimit(60);
	
			// setup SFML event handlers
			_window.Closed += new EventHandler(OnClosed);
			_window.KeyPressed += new EventHandler<KeyEventArgs>(OnKeyPressed);
			_window.KeyReleased += new EventHandler<KeyEventArgs> (OnKeyReleased);
			_window.MouseButtonPressed += new EventHandler<MouseButtonEventArgs>(OnMouseClicked);
			_window.TextEntered += new EventHandler<TextEventArgs>(OnTextEntered);
		}

		public void StartGameLoop()
		{
			while (_window.IsOpen)
			{
				// check wave progress
				if (_targets.Count == 0)
					NextWave ();

				_window.DispatchEvents();
				_window.Clear(Color.Black);

				// Move player
				if (_arrowKeyDown != Direction.NONE)
					_player.Move (_arrowKeyDown, screenRect);

				// draw player
				_player.Draw (_window);

				// draw all targets to the screen
				foreach (Target t in _targets) {
					if (!_isSolving)
						t.Oscillate (_waveOscilatingDir);
					t.Draw (_window);
				}

				// revert target direction when outer targets hit the screen edge
				if (_targets.Count > 0 && Target.HasOuterTargetHitScreenEdge (_targets)) {
					if (_waveOscilatingDir == Direction.LEFT)
						_waveOscilatingDir = Direction.RIGHT;
					else if (_waveOscilatingDir == Direction.RIGHT)
						_waveOscilatingDir = Direction.LEFT;
				}


				if (_isSolving)
					DrawEquationSolver ();
				else
					HandleTargetCollision ();
							
				// refresh screen
				_window.Display();
			}
		}
		void OnClosed(object sender, EventArgs e)
		{
			_window.Close();
		}
		void OnKeyPressed(object sender, EventArgs e)
		{
			KeyEventArgs pressed = (KeyEventArgs)e;

			// normal game controls
			if (!_isSolving) {
				if (pressed.Code == Keyboard.Key.Left)
					_arrowKeyDown = Direction.LEFT;
				else if (pressed.Code == Keyboard.Key.Right)
					_arrowKeyDown = Direction.RIGHT;
				else if (pressed.Code == Keyboard.Key.PageUp)  // This is a cheat only for testing
					NextWave ();
				else if (pressed.Code == Keyboard.Key.Space)
					_player.Shoot ();

			} else {  // solving eq controls

				if ((pressed.Code == Keyboard.Key.BackSpace || pressed.Code == Keyboard.Key.Delete) && _eqCurrentInput.Length > 0)
					_eqCurrentInput = _eqCurrentInput.Remove (_eqCurrentInput.Length - 1);
				else if (pressed.Code == Keyboard.Key.Return)
					SubmitAnswer ();
			}
		}

		void OnKeyReleased(object sender, EventArgs e)
		{
			KeyEventArgs pressed = (KeyEventArgs)e;

			if ((pressed.Code == Keyboard.Key.Left && _arrowKeyDown == Direction.LEFT) || 
				(pressed.Code == Keyboard.Key.Right && _arrowKeyDown == Direction.RIGHT))
				_arrowKeyDown = Direction.NONE;
		}

		void OnMouseClicked(object sender, EventArgs e)
		{
			// any mouse click input will be recieved here
			MouseButtonEventArgs pressed = (MouseButtonEventArgs)e;
		}

		void OnTextEntered(object sender, EventArgs e)
		{
			TextEventArgs input = (TextEventArgs)e;

			// make sure val is not to big for int type
			if (_eqCurrentInput.Length < 8) {
				int n;
				// add to current input string if a num is entered
				bool isNum = int.TryParse (input.Unicode, out n);
				if (isNum) {
					_eqCurrentInput += n;
					_eqCurrentInput = int.Parse (_eqCurrentInput).ToString ();
				}
				else if (input.Unicode == "-" && _eqCurrentInput.Length > 0)
					_eqCurrentInput = (int.Parse(_eqCurrentInput) *-1).ToString();
			}
		}

		private void NextWave()
		{
			EquationDifficulty diff = new EquationDifficulty();
			if (_wave >= 10)
				diff = EquationDifficulty.HARD;
			else if (_wave >= 4)
				diff = EquationDifficulty.INTERMEDIATE;
			else
				diff = EquationDifficulty.EASY;

			_targets = Target.GenTargets (_wave * 2, _wave * 1, diff);
			_wave++;
			_waveOscilatingDir = Direction.RIGHT;
		}

		private void HandleTargetCollision ()
		{
			foreach (Target t in _targets.ToArray()) {
				foreach (RectangleShape b in _player.bullets.ToArray()) {
					if (Target.HasBulletHitTarget (b, t)) {
						if (t is EquationTarget) {
							EquationTarget eqTarget = (EquationTarget)t;
							eqTarget.Selected ();
							Console.WriteLine ("{0} {1} {2} = {3}", eqTarget.GetEquation.val1, eqTarget.GetEquation.type.ToString (), eqTarget.GetEquation.val2, eqTarget.GetEquation.solution);
							_isSolving = true;
							_eqToSolve = eqTarget;

						} else {
							// only remove now if normal target
							// equation targets get removed after being solved
							_targets.Remove (t);
						}

						_player.RemoveBullet (b);
						break;
					}
				}
			}

		}

		private void DrawEquationSolver()
		{
			// load font
			String eqText = _eqToSolve.GetEquation.val1 + " " + Equation.GetString(_eqToSolve.GetEquation.type) + " " + _eqToSolve.GetEquation.val2 + "\n= " + _eqCurrentInput;
			Text eq = new Text (eqText, _font);

			// align backgroud with text
			RectangleShape background = new RectangleShape (new Vector2f (eq.GetGlobalBounds().Width + 90, eq.GetGlobalBounds().Height + 45));
			background.FillColor = Color.Red;

			Vector2f pos = new Vector2f (_window.Size.X/2 - (background.Size.X/2), _window.Size.Y/2 - (background.Size.Y/2));
			background.Position = pos;
			eq.Position = new Vector2f (pos.X + 40, pos.Y + 20);

			_window.Draw (background);
			_window.Draw (eq);
		}

		private void SubmitAnswer()
		{
			if (int.Parse (_eqCurrentInput) == _eqToSolve.GetEquation.solution) {
				// correct answer
				// now we can destroy the target
				_targets.Remove (_eqToSolve);

				// reset input
				_eqCurrentInput = "";
				_isSolving = false;
			}
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			EquasionInvasion app = new EquasionInvasion();
			app.StartGameLoop();
		}
	}
}
