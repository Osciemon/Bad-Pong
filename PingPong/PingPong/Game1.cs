using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq.Expressions;
using System.Transactions;
using static System.Net.Mime.MediaTypeNames;

namespace PingPong
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        int gameWaitTime = 60 * 2;

        public SpriteFont font;
        public int hits = 0;
        Texture2D Ball, Ball2, PlayerOne, NPC;
        KeyboardState key, LastKeyboard;
        Vector2 PlayerVelocity, PlayerPosition, BallVelocity, BallPosition, Ball2Velocity, Ball2Position;
        Vector2 NPCVelocity, NPCPosition;
        Rectangle PlayerHitBox, BallHitBox, Ball2HitBox, NPCHitbox;
        int PlayerScore, NPCScore;
        SpriteFont Text;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.ApplyChanges();

            BallPosition = new Vector2(960 - 50, 540 - 50);
            Ball2Position = new Vector2(960, 540);
            NPCPosition = new Vector2(1830, 440);
            NPCHitbox = new Rectangle(0, 0, 95, 214);
            PlayerHitBox = new Rectangle(0, 0, 65, 214);
            SetBallPosition();
            SetBall2Position();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            Ball = Content.Load<Texture2D>("Smile");
            Ball2 = Content.Load<Texture2D>("Ball2");
            PlayerOne = Content.Load<Texture2D>("sword");
            NPC = Content.Load<Texture2D>("sword");
            Text = Content.Load<SpriteFont>("PointCounter");
            // TODO: use this.Content to load your game content here


        }

        protected override void Update(GameTime gameTime)
        {
            if (gameWaitTime != 0)
            {
                gameWaitTime--;
                return;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            key = Keyboard.GetState();

            if (key.IsKeyDown(Keys.S))
            {
                PlayerVelocity.Y = 10;
            }
            else if (key.IsKeyDown(Keys.W))
            {
                PlayerVelocity.Y = -10;
            }
            else
            {
                PlayerVelocity.Y = 0;
            }
            if (BallPosition.Y == 1830)
            {

            }

            //BallVelocity.Y = 8;
            //BallVelocity.X = 8;


            PlayerPosition += PlayerVelocity;
            BallPosition += BallVelocity;
            Ball2Position += Ball2Velocity;

            SetNPCPosition();


            PlayerHitBox.Location = PlayerPosition.ToPoint() + new Point(10, 0);
            BallHitBox.Location = BallPosition.ToPoint();
            Ball2HitBox.Location = Ball2Position.ToPoint();
            NPCHitbox.Location = NPCPosition.ToPoint() - new Point(85, 0);

            if (PlayerHitBox.Intersects(BallHitBox))
            {
                BallVelocity.X *= -1;

            }
            else if (NPCHitbox.Intersects(BallHitBox))
            {
                BallVelocity.X *= -1;

            }
            else
            { }
            if (BallPosition.Y < 0)
            {
                BallVelocity.Y *= -1;
            }
            else if (BallPosition.Y >= 940)
            {
                BallVelocity.Y *= -1;
            }
            else { }

            if ( BallPosition.X >= 1920)
            {
                BallPosition.X = 960-50;
                BallPosition.Y = 540 - 50;
                PlayerScore ++;
                SetBallPosition();
            }
            else if(BallPosition.X <= 0)
            {
                BallPosition.X = 960 - 50;
                BallPosition.Y = 540 - 50;
                NPCScore++;
                SetBallPosition();
            }

            if (PlayerHitBox.Intersects(Ball2HitBox))
            {
                Ball2Velocity.X *= -1;

            }
            else if (NPCHitbox.Intersects(Ball2HitBox))
            {
                Ball2Velocity.X *= -1;
            }
            else
            { }
            if (Ball2Position.Y < 0)
            {
                Ball2Velocity.Y *= -1;
            }
            else if (Ball2Position.Y >= 940)
            {
                Ball2Velocity.Y *= -1;
            }
            else { }
            if (Ball2Position.X >= 1920)
            {
                Ball2Position.X = 960 - 50;
                Ball2Position.Y = 540 - 50;
                PlayerScore += 2;
                SetBall2Position();
            }
            else if (Ball2Position.X <= -1)
            {
                Ball2Position.X = 960 - 50;
                Ball2Position.Y = 540 - 50;
                NPCScore += 2;
                SetBall2Position();
            }
            if (PlayerPosition.Y < 1)
            {
                PlayerPosition.Y = 0;
            }
            if (PlayerPosition.Y > 870)
            {
                PlayerPosition.Y = 870;
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(Text, PlayerScore.ToString(), new Vector2(640, 60), Color.White);
            _spriteBatch.DrawString(Text, NPCScore.ToString(), new Vector2(1920 - 640, 60), Color.White);
            _spriteBatch.Draw(Ball, BallPosition, Color.White);
            _spriteBatch.Draw(Ball2, Ball2Position, Color.White);
            _spriteBatch.Draw(NPC, NPCPosition, Color.White);
            _spriteBatch.Draw(PlayerOne, PlayerPosition, Color.Azure);
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);


        }
        public void SetNPCPosition()
        {
            if (BallPosition.X > Ball2Position.X)
            {
                if (BallPosition.Y < NPCPosition.Y + 50)
                {
                    NPCPosition.Y -= 10;
                }
                else if (BallPosition.Y >= NPCPosition.Y+ 50)
                {
                    NPCPosition.Y += 10;
                }
            }
            else if (BallPosition.X < Ball2Position.X)
            {
                if (Ball2Position.Y < NPCPosition.Y+ 50)
                {
                    NPCPosition.Y -= 10;
                }
                else if (Ball2Position.Y > NPCPosition.Y + 50)
                {
                    NPCPosition.Y += 10;
                }
            }
        }

        public void SetBallPosition()
        {
            Random rnd = new Random();
            BallVelocity.X = rnd.Next (5, 10);
            BallVelocity.Y = rnd.Next (5, 10);
        }
        public void SetBall2Position()
        {
            Random rnd = new Random();
            Ball2Velocity.X = rnd.Next(-5, -2);
            Ball2Velocity.Y = rnd.Next(2, 5);
        }
    }
}
